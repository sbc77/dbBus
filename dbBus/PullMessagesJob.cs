using System;
using dbBus.Core;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TheOne.OrmLite.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Concurrent;
using dbBus.Core.Model;

namespace dbBus
{
    public class PullMessagesJob
    {
        private readonly IBusConfiguration cfg;
        private readonly IDependencyAdapter depCfg;
        private readonly ILogger<PullMessagesJob> log;

        public PullMessagesJob(IBusConfiguration cfg, IDependencyAdapter depCfg, ILogger<PullMessagesJob> log)
        {
            this.cfg = cfg;
            this.depCfg = depCfg;
            this.log = log;
        }

        public async Task Execute()
        {
            var pullingWatch = System.Diagnostics.Stopwatch.StartNew();

            var messageCounter = 0;
            var failCounter = 0;

            this.log.LogDebug("Pulling messages ..");

            try
            {
                foreach (var ri in this.cfg.RegistrationInfo)
                {
                    var qry = @"select * 
                        from dbo.Messages m
                        where m.MessageType = @MessageTypeName
                        and m.ValidUntil >= @date
                        and not exists (
                            select 1 from dbo.MessageHandlers h 
                            where h.MessageId = m.id
                                and h.HandlerId = @HandlerTypeName
                                and h.Error is null
                                and h.Handled = 1)";

                    var bag = new ConcurrentBag<object>();

                    using (var db = this.cfg.DbConnectionFactory.Open())
                    {
                        var messages = db.SqlList<MessageRow>(qry, new { ri.MessageTypeName, ri.HandlerTypeName, date = DateTime.Now }).Take(this.cfg.PullMaxMessages);

                        var tasks = messages.Select(async message =>
                        {
                            messageCounter++;

                            var result = await ProcessMessage(ri, db, message);

                            if (result == false)
                            {
                                failCounter++;
                            }
                        });

                        await Task.WhenAll(tasks);
                    }
                }
            }
            catch (Exception e)
            {
                this.log.LogError(e, $"Error processing messages");
            }

            pullingWatch.Stop();

            this.log.LogDebug($"Pulling finished, {messageCounter} messages, {failCounter} fails,  execution time: {pullingWatch.ElapsedMilliseconds} ms");
        }

        private async Task<bool> ProcessMessage(RegistrationInfo ri, IDbConnection db, MessageRow message)
        {
            var mh = new MessageHandlerRow
            {
                HandlerId = ri.HandlerTypeName,
                HandleDate = DateTime.Now,
                MessageId = message.Id
            };

            try
            {
                var handler = this.depCfg.GetService(ri.HandlerType);
                var messageCtx = (IMessage)JsonConvert.DeserializeObject(message.Content, ri.MessageType);
                messageCtx.InternalId = message.Id;
                await (Task)ri.HandlerType.GetMethod("Handle").Invoke(handler, new[] { messageCtx });
                mh.Handled = true;
            }
            catch (Exception e)
            {
                var ie = GetMostInnerException(e);
                mh.Error = ie.Message;
                log.LogError(ie, $"Error handling message #{message.Id} of type [{ri.MessageTypeName}], handler: [{ri.HandlerTypeName}]");
            }

            await db.SaveAsync(mh);

            return mh.Handled;
        }

        private static Exception GetMostInnerException(Exception e)
        {
            if (e.InnerException != null)
            {
                return GetMostInnerException(e.InnerException);
            }

            return e;
        }
    }
}