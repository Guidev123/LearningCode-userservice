using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Infrastructure.MessageBus.Configuration
{
    public class BusSettingsConfiguration
    {
        public string Queue { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public string RoutingKey { get; set; } = string.Empty;
        public string QueueType {  get; set; } = string.Empty;
        public string ClientProvidedName { get; set; } = string.Empty;
        public string Hostname { get; set; } = string.Empty;
    }
}
