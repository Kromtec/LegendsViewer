using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public class EntityOccasion : WorldObject
    {
        public string Name { get; set; } // legends_plus.xml

        public string Icon { get; set; }
        public int EventId { get; set; }
        public List<Schedule> Schedules { get; set; }
        public Entity Entity { get; set; }

        //public int GlobalID { get; set; }

        public static List<string> Filters;
        public override List<WorldEvent> FilteredEvents
        {
            get { return Events.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }

        public EntityOccasion(List<Property> properties, World world, Entity entity)
            : base(properties, world)
        {
            Name = "UNKNOWN OCCASION";
            Schedules = new List<Schedule>();

            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "name": Name = Formatting.InitCaps(property.Value); break;
                    case "event": EventId = Convert.ToInt32(property.Value); break;
                    case "schedule":
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            Schedules.Add(new Schedule(property.SubProperties, world));
                        }
                        break;
                }
            }

            Entity = entity;

            //GlobalID = world.Structures.Count;
            //world.Structures.Add(this);
        }

        public override string ToString() { return Name; }

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            //if (link)
            //{
            //    string title = Type.GetDescription();
            //    title += "&#13";
            //    title += "Events: " + Events.Count;

            //    string linkedString = "";
            //    if (pov != this)
            //    {
            //        linkedString = Icon + "<a href = \"structure#" + GlobalID + "\" title=\"" + title + "\">" + Name + "</a>";
            //    }
            //    else
            //    {
            //        linkedString = Icon + "<a title=\"" + title + "\">" + HTMLStyleUtil.CurrentDwarfObject(Name) + "</a>";
            //    }
            //    return linkedString;
            //}
            //else
            //{
            //    return Icon + Name;
            //}
            return Name;
        }
    }
}
