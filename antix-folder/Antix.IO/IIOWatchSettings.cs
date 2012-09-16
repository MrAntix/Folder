using System;

namespace Antix.IO
{
    public interface IIOWatchSettings : IEquatable<IIOWatchSettings>
    {
        TimeSpan Interval { get; set; }
        bool IncludeSubdirectories { get; set; }
    }
}