using System;

namespace Antix.IO
{
    /// <summary>
    /// <para>Watch settings object</para>
    /// </summary>
    public class IOWatchSettings : IEquatable<IOWatchSettings>
    {
        // Defaults
        public const int DefaultIntervalMilliseconds = 2000;
        public const bool DefaultIncludeSubdirectories = true;
        public static IOWatchSettings Default = new IOWatchSettings(new Parameters());

        // Properties
        readonly TimeSpan _interval;
        readonly bool _includeSubCategories;

        /// <summary>
        /// <para>Time to group events with</para>
        /// </summary>
        public TimeSpan Interval
        {
            get { return _interval; }
        }

        /// <summary>
        /// <para>Watch sub-categories as well</para>
        /// </summary>
        public bool IncludeSubCategories
        {
            get { return _includeSubCategories; }
        }

        // Private Constructors
        IOWatchSettings(Parameters init)
        {
            _interval = init.Interval;
            _includeSubCategories = init.IncludeSubdirectories;
        }

        /// <summary>
        /// <para>Create watch settings</para>
        /// <para><see cref="IOWatchSettings.Default"/> for a default settings object</para>
        /// </summary>
        /// <param name="assign">Parameters assignment delegate</param>
        /// <returns>New settings object</returns>
        public static IOWatchSettings Create
            (Action<Parameters> assign)
        {
            var po = new Parameters();
            assign(po);

            if (po.Interval <= TimeSpan.FromSeconds(0))
                throw new ArgumentOutOfRangeException("Interval");

            return new IOWatchSettings(po);
        }

        /// <summary>
        /// <para>Parameters object</para>
        /// </summary>
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
                   && IncludeSubCategories.Equals(other.IncludeSubCategories);
        }

        public override int GetHashCode()
        {
            return Interval.GetHashCode()
                   ^ IncludeSubCategories.GetHashCode();
        }

        #endregion
    }
}