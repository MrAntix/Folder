using System;

namespace Antix.IO
{
    public class WatchSettings
    {
        public WatchSettings()
        {
            Interval = TimeSpan.FromSeconds(2);
            IncludeSubdirectories = true;
        }

        public TimeSpan Interval { get; set; }
        public bool IncludeSubdirectories { get; set; }
    }
}