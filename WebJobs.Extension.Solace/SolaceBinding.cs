using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebJobs.Extension.Solace;

[assembly: WebJobsStartup(typeof(SolaceBinding.Startup))]
namespace WebJobs.Extension.Solace
{
    /// <summary>
    /// Starup object
    /// </summary>
    public class SolaceBinding
    {
        /// <summary>
        /// IWebJobsStartup startup class
        /// </summary>
        public class Startup : IWebJobsStartup
        {
            public void Configure(IWebJobsBuilder builder)
            {
                builder.Services.AddLogging();
                // Add Solace extensions
                builder.AddSolaceExtension();
            }
        }
    }
}
