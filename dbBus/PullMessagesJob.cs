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
        private readonly IErrorHandler errorHandler;

        public PullMessagesJob(IBusConfiguration cfg, ILogger<PullMessagesJob> log, IErrorHandler errorHandler)
        {
            this.cfg = cfg;
            this.log = log;
            this.errorHandler = errorHandler;
        }

        public async Task Execute()
        {
            var debugBus = Environment.GetEnvironmentVariable("ShowBusDebug")?.ToLower() == "true";

            var pullingWatch = System.Diagnostics.Stopwatch.StartNew();

            if (debugBus)
            {
                this.log.LogDebug("Pulling messages ..");
            }

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

            if (debugBus)
            {
                this.log.LogDebug($"Pulling finished,  execution time: {pullingWatch.ElapsedMilliseconds} ms");
            }
        }

        private async Task ProcessMessages(IDbConnection db, RegistrationInfo ri)
        {
            const string Qry = @"select * 
                        from Messages m
                        where m.MessageType = @MessageTypeName
                        and m.ValidUntil >= @date
                        and m.RetryNo between 0 and @MaxRetry
                        and not exists (
                            select 1 from MessageHandlers h 
                            where h.MessageId = m.id
                                and h.HandlerId = @HandlerTypeName
                                and h.Handled = 1)";

            var messages = db.SqlList<MessageRow>(Qry, new { ri.MessageTypeName, ri.HandlerTypeName, date = DateTime.Now, this.cfg.MaxRetry })
                .Take(this.cfg.PullMaxMessages);

            var tasks = messages.Select(message => this.ProcessMessage(ri, db, message));

            await Task.WhenAll(tasks);
        }

        private async Task ProcessMessage(RegistrationInfo ri, IDbConnection db, MessageRow dbMessage)
        {
            var mh = new MessageHandlerRow
            {
                HandlerId = ri.HandlerTypeName,
                HandleDate = DateTime.Now,
                MessageId = dbMessage.Id
            };

            try
            {
                var handler = this.cfg.DependencyAdapter.GetService(ri.HandlerType);

                var message = (IMessage)JsonConvert.DeserializeObject(dbMessage.Content, ri.MessageType);
                message.InternalId = dbMessage.Id;

                try
                {
                    await (Task)ri.HandlerType.GetMethod("Handle").Invoke(handler, new object[] { message });
                    mh.Handled = true;
                }
                catch (Exception e)
                {
                    dbMessage.RetryNo++;

                    mh.Error = e.Message;

                    var result = await this.errorHandler.OnError(handler, e, message, dbMessage.RetryNo);

                    if (result == false) // do not retry
                    {
                        dbMessage.RetryNo = -1;
                    }

                    db.Update(dbMessage);
                }
            }
            catch (Exception e)
            {
                var ie = DbBusUtils.GetMostInnerException(e);
                mh.Error = ie.Message;
                this.log.LogError(ie, $"Error handling message #{dbMessage.Id} of type [{ri.MessageTypeName}], handler: [{ri.HandlerTypeName}]");
            }

            await db.SaveAsync(mh);
        }
    }
}