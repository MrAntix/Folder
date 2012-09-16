using System;

namespace Antix.IO
{
    public class IOWatchSettings : IIOWatchSettings
    {
        public TimeSpan Interval { get; set; }
        public bool IncludeSubdirectories { get; set; }

        public IOWatchSettings()
        {
            Interval = TimeSpan.FromSeconds(2);
            IncludeSubdirectories = true;
        }

        public static IIOWatchSettings Default = new IOWatchSettings();

        #region equals

        bool IEquatable<IIOWatchSettings>.Equals(IIOWatchSettings other)
        {
            return Equals(other);
        }

        public override bool Equals(object obj)
        {
            var other = obj as IIOWatchSettings;
            return other != null &&
                Interval.Equals(other.Interval)
                   && IncludeSubdirectories.Equals(other.IncludeSubdirectories);
        }

        public override int GetHashCode()
        {
            return Interval.GetHashCode()
                   ^ IncludeSubdirectories.GetHashCode();
        }

        #endregion
    }
}