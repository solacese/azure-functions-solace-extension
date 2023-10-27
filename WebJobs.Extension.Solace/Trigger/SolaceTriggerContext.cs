namespace WebJobs.Extension.Solace
{
    /// <summary>
    /// Trigger context class
    /// </summary>
    public class SolaceTriggerContext
    {
        /// <summary>
        /// <c>Attribute</c> instance
        /// </summary>
        public SolaceTriggerAttribute TriggerAttribute;
        /// <summary>
        /// <c>SolaceClient</c> instance to connect and bind to Solace
        /// </summary>
        public SolaceClient Client;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="attribute">Attribute instnace</param>
        /// <param name="client">SolaceClient instance</param>
        public SolaceTriggerContext(SolaceTriggerAttribute attribute, SolaceClient client)
        {
            this.TriggerAttribute = attribute;
            this.Client = client;
        }
    }
}