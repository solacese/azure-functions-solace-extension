using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Triggers;

namespace WebJobs.Extension.Solace
{
    /// <summary>
    /// Binding provider
    /// </summary>
    public class SolaceTriggerBindingProvider: ITriggerBindingProvider
    {
        /// <summary>
        /// SolaceExtensionConfigProvider instance variable. Used to create the
        /// context
        /// </summary>
        private SolaceExtensionConfigProvider _provider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="provider"><c>SolaceExtensionConfigProvider</c> instance</param>
        public SolaceTriggerBindingProvider(SolaceExtensionConfigProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Create the trigger binding
        /// </summary>
        /// <param name="context"><c>TriggerBindingProviderContext</c> context</param>
        /// <returns>A Task that has the trigger binding</returns>
        public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
        {
            var parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<SolaceTriggerAttribute>(false);

            if (attribute == null) return Task.FromResult<ITriggerBinding>(null);
            if (parameter.ParameterType != typeof(string)) throw new InvalidOperationException("Invalid parameter type");

            var triggerBinding = new SolaceTriggerBinding(_provider.CreateContext(attribute));

            return Task.FromResult<ITriggerBinding>(triggerBinding);
        }
    }
}   
