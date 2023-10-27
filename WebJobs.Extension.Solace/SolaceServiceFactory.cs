using Microsoft.Extensions.Logging;

namespace WebJobs.Extension.Solace
{
    /// <summary>
    /// Solace Service Factory, responsible for creating SolaceClient
    /// </summary>
    public class SolaceServiceFactory : ISolaceServiceFactory
    {
        private ILogger<SolaceClient> logger;

        public SolaceServiceFactory(ILogger<SolaceClient> logger)
        {
            this.logger = logger;
        }
        /// <summary>
        /// Create Solace Client from connection string
        /// </summary>
        /// <param name="connstring">Connection String</param>
        /// <returns>Returns SolaceClient</returns>
        public SolaceClient CreateSolaceClient(SolaceTriggerAttribute attribute)
        {
            SolaceClient client = new SolaceClient(logger);

            client.Connect(attribute);

            return client;
        }
    }
}
