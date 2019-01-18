using System;
using System.Threading;
using System.Threading.Tasks;
using dbBus.Core;
using dbBus.Core.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TheOne.OrmLite.Core;

namespace dbBus
{
    public class Bus : IBus
    {
        private readonly IBusConfiguration cfg;
        private readonly ILogger<Bus> log;
        private readonly PullMessagesJob job;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly IDependencyAdapter depCfg;

        public Bus(IBusConfiguration cfg, ILogger<Bus> log, PullMessagesJob job, IDependencyAdapter depCfg)
        {
            this.depCfg = depCfg;
            this.cfg = cfg;
            this.log = log;
            this.job = job;

            using (var db = this.cfg.DbConnectionFactory.Open())
            {
                db.CreateTableIfNotExists<MessageRow>();
                db.CreateTableIfNotExists<MessageHandlerRow>();
            }
        }

        public static IBusConfiguration Configure()
        {
            return new BusConfiguration();
        }

        public async Task<IMessage> Publish(IMessage message)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var msg = new MessageRow
            {
                Created = DateTime.Now,
                ValidUntil = DateTime.Now + this.cfg.MessageLifetime,
                MessageType = message.GetType().FullName,
                Content = JsonConvert.SerializeObject(message)
            };

            using (var db = this.cfg.DbConnectionFactory.Open())
            {
                await db.SaveAsync(msg);
                message.InternalId = msg.Id;
            }

            watch.Stop();

            this.log.LogDebug($"Message #{message.InternalId} of type [{message.GetType().FullName}] published in {watch.ElapsedMilliseconds} ms");
            return message;
        }

        public void Start()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await job.Execute();
                    await Task.Delay(this.cfg.PullInterval, this.cancellationTokenSource.Token);
                    if (this.cancellationTokenSource.IsCancellationRequested)
                    {
                        break;
                    }
                }
            });
        }

        public void Stop()
        {
            this.cancellationTokenSource.Cancel();
        }
    }
}