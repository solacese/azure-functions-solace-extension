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
        private ILogger<SolaceClient> _logger;
        private IContext _context;
        private ISession _session;
        private IQueue _queue;
        private IFlow _flow;

        static SolaceClient()
        {
            var cfp = new ContextFactoryProperties();
            ContextFactory.Instance.Init(cfp);
        }

        public SolaceClient(ILogger<SolaceClient> logger)
        {
            _logger = logger;
            _context = ContextFactory.Instance.CreateContext(new ContextProperties(), null);
        }

        /// <summary>
        /// Connect to Solace server
        /// </summary>
        /// <param name="connstring">Solace Connection string</param>
        public void Connect(SolaceTriggerAttribute attribute)
        {
            _logger.LogInformation("About to connect!");

            var sessionProps = new SessionProperties()
            {
                Host = attribute.Host,
                VPNName = attribute.VpnName,
                UserName = attribute.UserName,
                Password = attribute.Password,
                SubscribeBlocking = true,
            };

            _session = _context.CreateSession(sessionProps, null, null);
            _session.Connect();
        }

        /// <summary>
        /// Subscribe to Solace channel
        /// </summary>
        /// <param name="queueName">QueueName string</param>
        /// <param name="handler">Subscription message handler</param>
        /// <returns>Returns a Task that completes when the subscription ends</returns>
        public Task Subscribe(string queueName, Func<MessageEventArgs,Task<FunctionResult>> handler)
        {
            var flowProperties = new FlowProperties()
            {
                AckMode = MessageAckMode.ClientAck
            };

            _queue = ContextFactory.Instance.CreateQueue(queueName);
            _flow = _session.CreateFlow(flowProperties, _queue, null, async (s, e) => 
            {
                var result = await handler.Invoke(e);
                
                if(result.Succeeded)
                {
                    _flow.Ack(e.Message.ADMessageId);
                }
                
            },
            (s,e) => { }); // TODO: what if the flow goes down? Disconnect? how to propogate out events to Function?

            _flow.Start();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Disconnect from server
        /// </summary>
        public void Disconnect()
        {
            _flow.Stop();
            _session.Disconnect();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _queue.Dispose();
            _session.Dispose();
            _context.Dispose();
        }
    }
}
    