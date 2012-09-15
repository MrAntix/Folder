using System;

namespace Antix.IO
{
    public class IOWatchSettings
    {
        public IOWatchSettings()
        {
            Interval = TimeSpan.FromSeconds(2);
            IncludeSubdirectories = true;
        }

        public TimeSpan Interval { get; set; }
        public bool IncludeSubdirectories { get; set; }
    }
}