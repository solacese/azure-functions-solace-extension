namespace WebJobs.Extension.Solace
{
    /// <summary>
    /// Solace Service factory. Create Solace Client and return
    /// </summary>
    public interface ISolaceServiceFactory
    {
        /// <summary>
        /// Create Solace Client from connection string
        /// </summary>
        /// <param name="connstring">Solace Connection string</param>
        /// <returns>Returns SolaceClient instance</returns>
        public SolaceClient CreateSolaceClient(SolaceTriggerAttribute attribute);
    }
}
