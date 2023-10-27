using System;
using Microsoft.Azure.WebJobs.Description;

namespace WebJobs.Extension.Solace
{
    /// <summary>
    /// <c>Attribute</c> class for Trigger
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class SolaceTriggerAttribute : Attribute
    {
        public string Host { get; set; } = Environment.GetEnvironmentVariable("SolaceHost");
        public string VpnName { get; set; } = Environment.GetEnvironmentVariable("SolaceVpnName");
        public string UserName { get; set; } = Environment.GetEnvironmentVariable("SolaceUserName");
        public string Password { get; set; } = Environment.GetEnvironmentVariable("SolacePassword");
        public string Queue { get; set; } = Environment.GetEnvironmentVariable("SolaceQueue");
    }
}