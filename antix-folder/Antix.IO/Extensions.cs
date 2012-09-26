using System;
using System.Text;
using Antix.IO.Entities;
using Antix.IO.Entities.Base;
using Antix.IO.Events.Base;

namespace Antix.IO
{
    public static class Extensions
    {
        public static IOFileEntityWriter CreateFile(
            this IIOSystem system, string identifier)
        {
            return system.CreateFile(identifier, Encoding.Default);
        }

        public static IObservable<IOEvent> Watch(
            this IIOSystem system,IOEntity entity)
        {
            return system.Watch(entity, IOWatchSettings.Default);
        }
    }
}