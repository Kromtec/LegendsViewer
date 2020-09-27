using System;
using System.Collections.Generic;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public abstract class WorldObject : DwarfObject
    {
        public List<WorldEvent> Events { get; set; }
        public abstract List<WorldEvent> FilteredEvents { get; }
        public List<EventCollection> EventCollectons { get; set; }
        public int EventCount { get { return Events.Count; } set { } }
        public int Id { get; set; }

        protected WorldObject() { 
            Id = -1; 
            Events = new List<WorldEvent>();
            EventCollectons = new List<EventCollection>();
        }

        protected WorldObject(List<Property> properties, World world) : this()
        {
            foreach(Property property in properties)
            {
                switch (property.Name)
                {
                    case "id": Id = Convert.ToInt32(property.Value); break;
                }
            }
        }
        

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            return "";
        }
    }
}
