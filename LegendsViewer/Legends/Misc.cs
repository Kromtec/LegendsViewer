using System.Collections.Generic;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;

namespace LegendsViewer.Legends
{
    public static class Misc
    {
        public static void AddEvent(this WorldObject worldObject, WorldEvent worldEvent)
        {
            if (worldObject == null ||worldEvent == null)
            {
                return;
            }
#if DEBUG
            if (!worldObject.Events.Contains(worldEvent))
            {
#endif
                worldObject.Events.Add(worldEvent);
#if DEBUG
            }
            else
            {
                worldEvent.World.ParsingErrors.Report($"Already added event {worldEvent.Id} '{worldEvent.Type}' to object {worldObject.Id} '{worldObject.GetType()}'");
            }
#endif
        }

        public static void AddEventCollection(this WorldObject worldObject, EventCollection eventCollection)
        {
            if (worldObject == null || eventCollection == null)
            {
                return;
            }
            if (!worldObject.EventCollectons.Contains(eventCollection))
            {
                worldObject.EventCollectons.Add(eventCollection);
            }
#if DEBUG
            else
            {
                eventCollection.World.ParsingErrors.Report($"Already added eventCollection {eventCollection.Id} '{eventCollection.Type}' to object {worldObject.Id} '{worldObject.GetType()}'");
            }
#endif
        }

        public static T GetWorldObject<T>(this List<T> list, int id) where T : WorldObject
        {
            int min = 0;
            int max = list.Count - 1;
            while (min <= max)
            {
                int mid = min + (max - min) / 2;
                if (id > list[mid].Id)
                {
                    min = mid + 1;
                }
                else if (id < list[mid].Id)
                {
                    max = mid - 1;
                }
                else
                {
                    return list[mid];
                }
            }
            return null;
        }
    }
}