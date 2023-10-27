using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;

namespace WebJobs.Extension.Solace
{
    /*
        The SolaceListner class
        Implements the <c>IListener</c> interface. Contains the code to connect
        to a Solace broker and connect to a queue.
     */
    public class SolaceListener : IListener
    {
        private readonly ITriggeredFunctionExecutor _executor;
        private readonly SolaceTriggerContext _context;

        /// <summary>
        /// SolaceListener constructor
        /// </summary>
        ///
        /// <param name="executor"><c>ITriggeredFunctionExecutor</c> instance</param>
        /// <param name="context"><c>SolaceTriggerContext</c> instance</param>
        ///
        public SolaceListener(ITriggeredFunctionExecutor executor, SolaceTriggerContext context)
        {
            _executor = executor;
            _context = context;
        }

        /// <summary>
        /// Cancel any pending operation
        /// </summary>
        public void Cancel()
        {
            if (_context == null || _context.Client == null) return;

            _context.Client.Disconnect();
        }

        /// <summary>
        ///  Dispose method
        /// </summary>
        public void Dispose()
        {
            _context.Client.Dispose();
        }

        /// <summary>
        /// Start the listener asynchronously. Connect to a Solace queue and
        /// wait for message. When a message is received, execute the function
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A Task returned from Subscribe method</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _context.Client.Subscribe(_context.TriggerAttribute.Queue, (e) =>
            {
                var triggerData = new TriggeredFunctionData
                {
                    TriggerValue = Encoding.UTF8.GetString(e.Message.BinaryAttachment)
                };
                return _executor.TryExecuteAsync(triggerData, CancellationToken.None);
            });
        }

        /// <summary>
        /// Stop current listening operation
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => {
                _context.Client.Disconnect();
            });
        }
    }
}