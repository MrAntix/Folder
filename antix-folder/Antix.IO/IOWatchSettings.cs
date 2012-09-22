using System;

namespace Antix.IO
{
    public class IOWatchSettings : IEquatable<IOWatchSettings>
    {
        // Defaults
        const int DefaultIntervalMilliseconds = 2000;
        public const bool DefaultIncludeSubdirectories = true;
        public static IOWatchSettings Default = new IOWatchSettings(new Parameters());

        // Properties
        readonly TimeSpan _interval;
        readonly bool _includeSubdirectories;

        public TimeSpan Interval
        {
            get { return _interval; }
        }

        public bool IncludeSubdirectories
        {
            get { return _includeSubdirectories; }
        }

        // Private Constructors
        IOWatchSettings(Parameters init)
        {
            _interval = init.Interval;
            _includeSubdirectories = init.IncludeSubdirectories;
        }

        // Create Functions
        public static IOWatchSettings Create()
        {
            return Default;
        }

        public static IOWatchSettings Create
            (Action<Parameters> assign)
        {
            var po = new Parameters();
            assign(po);

            if (po.Interval <= TimeSpan.FromSeconds(0))
                throw new ArgumentOutOfRangeException("Interval");

            return new IOWatchSettings(po);
        }

        // Parameters object
        public class Parameters
        {
            public Parameters()
            {
                Interval = TimeSpan.FromMilliseconds(DefaultIntervalMilliseconds);
                IncludeSubdirectories = DefaultIncludeSubdirectories;
            }

            public TimeSpan Interval { get; set; }
            public bool IncludeSubdirectories { get; set; }
        }

        #region equals

        bool IEquatable<IOWatchSettings>.Equals(IOWatchSettings other)
        {
            return Equals(other);
        }

        public override bool Equals(object obj)
        {
            var other = obj as IOWatchSettings;
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