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

            var watchSettings = IOWatchSettings
                .Create(x =>
                            {
                                x.Interval = TimeSpan.FromSeconds(20);
                            });

            Assert.Equal(value, watchSettings.Interval);
            Assert.Equal(
                IOWatchSettings.Default.IncludeSubCategories,
                watchSettings.IncludeSubCategories);
        }

        [Fact]
        public void new_equals_default()
        {
            var watchSettings = IOWatchSettings.Create();

            Assert.Equal(IOWatchSettings.Default, watchSettings);
        }
    }
}