using System.Collections.Concurrent;
using Microsoft.Azure.WebJobs.Host.Config;
//using WebJobs.Extension.Solace.Bindings;

namespace WebJobs.Extension.Solace
{
    /// <summary>
    /// Extension Config Provider class
    /// </summary>
    public class SolaceExtensionConfigProvider : IExtensionConfigProvider
    {
        /// <summary>
        /// Solace Service Factory, used to create context
        /// </summary>
        public ISolaceServiceFactory _serviceFactory;
        /// <summary>
        /// A ConcurrentDictionary to cache the clients. The clients are chached
        /// based on the connection string
        /// </summary>
        private ConcurrentDictionary<string, SolaceClient> ClientCache { get; } = new ConcurrentDictionary<string, SolaceClient>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceFactory">Solace Service Factory instance</param>
        public SolaceExtensionConfigProvider(ISolaceServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        /// <summary>
        /// Initialize the extensions
        /// </summary>
        /// <param name="context">Extension config context</param>
        public void Initialize(ExtensionConfigContext context)
        {
            // Add trigger first
            var triggerRule = context.AddBindingRule<SolaceTriggerAttribute>();
            triggerRule.BindToTrigger(new SolaceTriggerBindingProvider(this));

        }

        /// <summary>
        /// Create Trigger context from a new SolaceClient and the attribute
        /// supplied
        /// </summary>
        /// <param name="attribute">SolaceTriggerAttribute instance</param>
        /// <returns>SolaceTriggerContext instance</returns>
        public SolaceTriggerContext CreateContext(SolaceTriggerAttribute attribute)
        {
            return new SolaceTriggerContext(attribute, _serviceFactory.CreateSolaceClient(attribute));
        }
    }
}
