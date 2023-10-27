using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;

namespace WebJobs.Extension.Solace
{
    /// <summary>
    /// WebJobBuilder extension to add Solace extensions
    /// </summary>
    public static class SolaceWebJobsBuilderExtensions
    {
        /// <summary>
        /// Extension method to add our custom extensions
        /// </summary>
        /// <param name="builder"><c>IWebJobsBuilder</c> instance</param>
        /// <returns><c>IWebJobsBuilder</c> instance</returns>
        /// <exception>Throws ArgumentNullException if builder is null</exception>
        public static IWebJobsBuilder AddSolaceExtension(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddExtension<SolaceExtensionConfigProvider>();
            builder.Services.AddSingleton<ISolaceServiceFactory, SolaceServiceFactory>();

            return builder;
        }
    }
}
