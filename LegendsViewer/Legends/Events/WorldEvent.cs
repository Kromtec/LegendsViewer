using System;
using System.Collections.Generic;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class WorldEvent : IComparable<WorldEvent>
    {
        private static readonly string[] MonthNames = { "Granite", "Slate", "Felsite", "Hematite", "Malachite", "Galena", "Limestone", "Sandstone", "Timber", "Moonstone", "Opal", "Obsidian" };
        private int _seconds72;

        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string MonthName
        {
            get
            {
                return MonthNames[Month - 1];
            }
        }

        public string Date
        {
            get
            {
                if (Year < 0)
                {
                    return "-";
                }
                return $"{Year:0000}-{Month:00}-{Day:00}";
            }
        }

        public int Seconds72
        {
            get => _seconds72;
            set
            {
                _seconds72 = value;
                Month = 1 + _seconds72 / (28 * 1200);
                Day = 1 + _seconds72 % (28 * 1200) / 1200;
            }
        }

        public string Type { get; set; }
        public EventCollection ParentCollection { get; set; }
        public World World { get; set; }

        public WorldEvent() { Id = -1; Year = -1; Seconds72 = -1; Type = "INVALID"; }

        public WorldEvent(List<Property> properties, World world)
        {
            World = world;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "id": Id = Convert.ToInt32(property.Value); property.Known = true; break;
                    case "year": Year = Convert.ToInt32(property.Value); property.Known = true; break;
                    case "seconds72": Seconds72 = Convert.ToInt32(property.Value); property.Known = true; break;
                    case "type": Type = string.Intern(property.Value); property.Known = true; break;
                }
            }
        }

        public virtual string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime() + Type;
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }

        public virtual string GetYearTime()
        {
            if (Year == -1)
            {
                return "In a time before time, ";
            }

            string yearTime = "In " + Year + ", ";
            if (Seconds72 == -1)
            {
                return yearTime;
            }

            int month = Seconds72 % 100800;
            if (month <= 33600)
            {
                yearTime += "early ";
            }
            else if (month <= 67200)
            {
                yearTime += "mid";
            }
            else if (month <= 100800)
            {
                yearTime += "late ";
            }

            int season = Seconds72 % 403200;
            if (season < 100800)
            {
                yearTime += "spring, ";
            }
            else if (season < 201600)
            {
                yearTime += "summer, ";
            }
            else if (season < 302400)
            {
                yearTime += "autumn, ";
            }
            else if (season < 403200)
            {
                yearTime += "winter, ";
            }

            return yearTime + " (" + Formatting.AddOrdinal(Day) + " of " + MonthName + ") ";
        }

        public string PrintParentCollection(bool link = true, DwarfObject pov = null)
        {
            if (ParentCollection == null)
            {
                return "";
            }
            EventCollection parent = ParentCollection;
            string collectionString = "";
            while (parent != null)
            {
                if (collectionString.Length > 0)
                {
                    collectionString += " as part of ";
                }
                collectionString += parent.ToLink(link, pov, this);
                parent = parent.ParentCollection;
            }
            return " during " + collectionString;
        }

        public int Compare(WorldEvent worldEvent)
        {
            return Id.CompareTo(worldEvent.Id);
        }

        public int CompareTo(object obj)
        {
            return Id.CompareTo(obj);
        }

        public int CompareTo(WorldEvent other)
        {
            return Id.CompareTo(other.Id);
        }
    }
}