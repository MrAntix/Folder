using System;
using Xunit;

namespace Antix.IO.Tests.unit
{
    public class using_watch_settings
    {
        [Fact]
        public void defaults_check()
        {
            var value = TimeSpan.FromSeconds(20);

            var watchSettings = new IOWatchSettings
                                    {
                                        Interval = TimeSpan.FromSeconds(20)
                                    };

            Assert.Equal(value, watchSettings.Interval);
            Assert.Equal(
                IOWatchSettings.Default.IncludeSubdirectories,
                watchSettings.IncludeSubdirectories);
        }

        [Fact]
        public void new_equals_default()
        {
            var watchSettings = new IOWatchSettings();

            Assert.Equal(IOWatchSettings.Default, watchSettings);
        }
    }
}