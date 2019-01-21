namespace dbBus
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    using dbBus.Core;
    using dbBus.Core.Model;

    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using TheOne.OrmLite.Core;

    public class PullMessagesJob
    {
        private readonly IBusConfiguration cfg;

        private readonly ILogger<PullMessagesJob> log;

        public PullMessagesJob(IBusConfiguration cfg, ILogger<PullMessagesJob> log)
        {
            this.cfg = cfg;
            this.log = log;
        }

        public async Task Execute()
        {
            var pullingWatch = System.Diagnostics.Stopwatch.StartNew();

            this.log.LogDebug("Pulling messages ..");

            try
            {
                foreach (var ri in this.cfg.RegistrationInfo)
                {
                    using (var db = this.cfg.DbConnectionFactory.Open())
                    {
                        await this.ProcessMessages(db, ri);
                    }
                }
            }
            catch (Exception e)
            {
                this.log.LogError(e, $"Error processing messages");
            }

            pullingWatch.Stop();

            this.log.LogDebug($"Pulling finished,  execution time: {pullingWatch.ElapsedMilliseconds} ms");
        }

        private async Task ProcessMessages(IDbConnection db, RegistrationInfo ri)
        {
            const string Qry = @"select * 
                        from Messages m
                        where m.MessageType = @MessageTypeName
                        and m.ValidUntil >= @date
                        and not exists (
                            select 1 from MessageHandlers h 
                            where h.MessageId = m.id
                                and h.HandlerId = @HandlerTypeName
                                and h.Error is null
                                and h.Handled = 1)";

            var messages = db.SqlList<MessageRow>(Qry, new { ri.MessageTypeName, ri.HandlerTypeName, date = DateTime.Now })
                .Take(this.cfg.PullMaxMessages);

            var tasks = messages.Select(message => this.ProcessMessage(ri, db, message));

            await Task.WhenAll(tasks);
        }

        private async Task ProcessMessage(RegistrationInfo ri, IDbConnection db, MessageRow message)
        {
            var mh = new MessageHandlerRow
            {
                HandlerId = ri.HandlerTypeName,
                HandleDate = DateTime.Now,
                MessageId = message.Id
            };

            try
            {
                var handler = this.cfg.DependencyAdapter.GetService(ri.HandlerType);

                var messageCtx = (IMessage)JsonConvert.DeserializeObject(message.Content, ri.MessageType);
                messageCtx.InternalId = message.Id;
                await (Task)ri.HandlerType.GetMethod("Handle").Invoke(handler, new object[] { messageCtx });
                mh.Handled = true;
            }
            catch (Exception e)
            {
                var ie = DbBusUtils.GetMostInnerException(e);
                mh.Error = ie.Message;
                this.log.LogError(ie, $"Error handling message #{message.Id} of type [{ri.MessageTypeName}], handler: [{ri.HandlerTypeName}]");
            }

            await db.SaveAsync(mh);
        }
    }
}