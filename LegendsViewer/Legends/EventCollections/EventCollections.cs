using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.EventCollections
{
    public abstract class EventCollection : DwarfObject
    {
        public int Id { get; set; }
        public int StartYear { get; set; }
        public int StartSeconds72 { get; set; }
        public int EndYear { get; set; }
        public int EndSeconds72 { get; set; }
        public string Type { get; set; }
        public EventCollection ParentCollection { get; set; }
        public List<WorldEvent> Collection { get; set; }
        public List<EventCollection> Collections { get; set; }
        public List<int> CollectionIDs { get; set; }
        public bool Notable { get; set; }
        public List<WorldEvent> AllEvents { get { return GetSubEvents(); } set { } }
        public abstract List<WorldEvent> FilteredEvents { get; }
        protected World World;
        protected EventCollection(List<Property> properties, World world)
        {
            Initialize();
            World = world;
            foreach(Property property in properties)
            {
                switch (property.Name)
                {
                    case "id": Id = Convert.ToInt32(property.Value); property.Known = true; break;
                    case "start_year": StartYear = Convert.ToInt32(property.Value); property.Known = true; break;
                    case "start_seconds72": StartSeconds72 = Convert.ToInt32(property.Value); property.Known = true; break;
                    case "end_year": EndYear = Convert.ToInt32(property.Value); property.Known = true; break;
                    case "end_seconds72": EndSeconds72 = Convert.ToInt32(property.Value); property.Known = true; break;
                    case "type": Type = Formatting.InitCaps(String.Intern(property.Value)); property.Known = true; break;
                    case "event":
                        WorldEvent collectionEvent = world.GetEvent(Convert.ToInt32(property.Value));
                        //Some Events don't exist in the XML now with 34.01? 
                        ///TODO: Investigate EventCollection Events that don't exist in the XML, check if they exist in game or if this is just errors.
                        if (collectionEvent != null)
                        {
                            collectionEvent.ParentCollection = this;
                            Collection.Add(collectionEvent); property.Known = true;
                        }
                        break;
                    case "eventcol": CollectionIDs.Add(Convert.ToInt32(property.Value)); property.Known = true; break;
                    default: break;
                }
            }
        }
        public EventCollection()
        {
            Initialize();
        }

        private void Initialize()
        {
            Id = StartYear = StartSeconds72 = EndYear = EndSeconds72 = -1; 
            Type = "INVALID";
            Collection = new List<WorldEvent>();
            Collections = new List<EventCollection>();
            CollectionIDs = new List<int>();
            Notable = true;
        }

        public string GetYearTime(bool start = true)
        {
            int year, seconds72;
            if (start) { year = StartYear; seconds72 = StartSeconds72; }
            else { year = EndYear; seconds72 = EndSeconds72; }
            if (year == -1)
            {
                return "In a time before time, ";
            }

            string yearTime = "In " + year + ", ";
            if (seconds72 == -1)
            {
                return yearTime;
            }

            int month = seconds72 % 100800;
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

            int season = seconds72 % 403200;
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

            return yearTime;
        }
        public string GetOrdinal(int ordinal)
        {
            if (ordinal <= 1)
            {
                return "";
            }

            string suffix = "";
            string numeral = ordinal.ToString();
            if (numeral.EndsWith("11") || numeral.EndsWith("12") || numeral.EndsWith("13"))
            {
                suffix = "th";
            }
            else if (numeral.EndsWith("1"))
            {
                suffix = "st";
            }
            else if (numeral.EndsWith("2"))
            {
                suffix = "nd";
            }
            else if (numeral.EndsWith("3"))
            {
                suffix = "rd";
            }
            else
            {
                suffix = "th";
            }

            return numeral + suffix + " ";
        }

        public List<WorldEvent> GetSubEvents()
        {
            List<WorldEvent> events = new List<WorldEvent>();
            foreach (EventCollection subCollection in Collections)
            {
                events.AddRange(subCollection.GetSubEvents());
            }

            events.AddRange(Collection);
            return events.OrderBy(collectionEvent => collectionEvent.Id).ToList();
        }
    }
}
