using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using WebJobs.Extension.Solace;

namespace SolaceTrigger.Sample
{
    public static class SolaceTriggerSample
    {
        [FunctionName("SolaceTriggerSample")]
        public static void Run(
            [SolaceTrigger(Queue = "Q/tutorial")] string message,
            ILogger log)
        {
            log.LogInformation($"Message Received from Queue 'Q/tutorial': {message}");
        }
    }
}
    