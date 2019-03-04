using System;

namespace check_yo_self_api.Server.Entities.Config
{
    public class ApplicationInsights
    {
        public bool TelemetryEnabled
        {
            get
            {
                return this.InstrumentationKey != Guid.Empty;
            }
        }
        public Guid InstrumentationKey { get; set; }
    }
}