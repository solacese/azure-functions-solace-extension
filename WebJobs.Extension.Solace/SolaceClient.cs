using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Extensions.Logging;
using SolaceSystems.Solclient.Messaging;

namespace WebJobs.Extension.Solace
{
    /// <summary>
    /// Solace Clinet utility class. This is wrapper class around the
    /// MySolaceClient library.
    /// </summary>
    public class SolaceClient : IDisposable
    {
        private ILogger<SolaceClient> logger;
        private IContext context;
        private ISession session;
        private IQueue queue;
        private IFlow flow;

        static SolaceClient()
        {
            var cfp = new ContextFactoryProperties();
            ContextFactory.Instance.Init(cfp);
        }

        public SolaceClient(ILogger<SolaceClient> logger)
        {
            this.logger = logger;
            this.context = ContextFactory.Instance.CreateContext(new ContextProperties(), null);
        }

        /// <summary>
        /// Connect to Solace server
        /// </summary>
        /// <param name="connstring">Solace Connection string</param>
        public void Connect(SolaceTriggerAttribute attribute)
        {
            this.logger.LogInformation("About to connect!");

            var sessionProps = new SessionProperties()
            {
                Host = attribute.Host,
                VPNName = attribute.VpnName,
                UserName = attribute.UserName,
                Password = attribute.Password,
                SubscribeBlocking = true,
            };

            this.session = context.CreateSession(sessionProps, null, null);
            this.session.Connect();
        }

        /// <summary>
        /// Subscribe to Solace channel
        /// </summary>
        /// <param name="subject">Channel string</param>
        /// <param name="subscription">Subscription lambda</param>
        /// <param name="queueGroup">QueueGroup name</param>
        /// <returns>Returns a Task that completes when the subscription ends</returns>
        public Task Subscribe(string queueName, Func<MessageEventArgs,Task<FunctionResult>> handler)
        {
            var flowProperties = new FlowProperties()
            {
                AckMode = MessageAckMode.ClientAck
            };

            this.queue = ContextFactory.Instance.CreateQueue(queueName);
            this.flow = this.session.CreateFlow(flowProperties, queue, null, async (s, e) => 
            {
                var result = await handler.Invoke(e);
                
                if(result.Succeeded)
                {
                    this.flow.Ack(e.Message.ADMessageId);
                }
                
            },
            (s,e) => { }); // TODO: what if the flow goes down? Disconnect? how to propogate out events to Function?

            this.flow.Start();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Disconnect from server
        /// </summary>
        public void Disconnect()
        {
            this.flow.Stop();
            this.session.Disconnect();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.queue.Dispose();
            this.session.Dispose();
            this.context.Dispose();
        }
    }
}
    