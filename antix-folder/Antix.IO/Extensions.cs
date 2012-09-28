using System;
using System.Text;
using Antix.IO.Entities;
using Antix.IO.Entities.Base;
using Antix.IO.Events.Base;

namespace Antix.IO
{
    public static class Extensions
    {
        public static IOFileEntity GetFileEntity(
            this IIOSystem system, string identifier)
        {
            return system.GetEntity<IOFileEntity>(identifier);
        }

        public static IOCategoryEntity GetCategoryEntity(
            this IIOSystem system, string identifier)
        {
            return system.GetEntity<IOCategoryEntity>(identifier);
        }

        public static IOFileEntityWriter CreateFile(
          this IIOSystem system, string identifier)
        {
            return system.CreateFile(identifier, Encoding.Default);
        }

        public static void DeleteFile(
            this IIOSystem system, string identifier)
        {
            system.DeleteFile(
                system.GetFileEntity(identifier));
        }

        public static IObservable<IOEvent> Watch(
            this IIOSystem system,IOEntity entity)
        {
            return system.Watch(entity, IOWatchSettings.Default);
        }
    }
}