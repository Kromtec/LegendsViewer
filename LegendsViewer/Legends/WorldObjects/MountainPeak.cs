using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Interfaces;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.WorldObjects
{
    public class MountainPeak : WorldObject, IHasCoordinates
    {
        public string Name { get; set; } // legends_plus.xml
        public WorldRegion Region { get; set; }
        public List<Location> Coordinates { get; set; } // legends_plus.xml
        public int Height { get; set; } // legends_plus.xml
        public string HeightMeter { get { return Height * 3+" m"; } set { } } // legends_plus.xml

        public string Icon = "<i class=\"fa fa-fw fa-wifi fa-flip-vertical\"></i>";

        public static List<string> Filters;
        public override List<WorldEvent> FilteredEvents
        {
            get { return Events.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }

        public MountainPeak(List<Property> properties, World world)
            : base(properties, world)
        {
            Name = "Untitled";
            Coordinates = new List<Location>();

            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "name": Name = Formatting.InitCaps(property.Value); break;
                    case "coords":
                        string[] coordinateStrings = property.Value.Split(new[] { '|' },
                            StringSplitOptions.RemoveEmptyEntries);
                        foreach (var coordinateString in coordinateStrings)
                        {
                            string[] xYCoordinates = coordinateString.Split(',');
                            int x = Convert.ToInt32(xYCoordinates[0]);
                            int y = Convert.ToInt32(xYCoordinates[1]);
                            Coordinates.Add(new Location(x, y));
                        }
                        break;
                    case "height": Height = Convert.ToInt32(property.Value); break;
                }
            }
        }

        public override string ToString() { return Name; }

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            if (link)
            {
                string linkedString = "";
                if (pov != this)
                {
                    string title = "";
                    title += "MountainPeak";
                    title += "&#13";
                    title += "Events: " + Events.Count;

                    linkedString = Icon + "<a href = \"mountainpeak#" + Id + "\" title=\"" + title + "\">" + Name + "</a>";
                }
                else
                {
                    linkedString = Icon + HtmlStyleUtil.CurrentDwarfObject(Name);
                }

                return linkedString;
            }
            return Name;
        }

        public override string GetIcon()
        {
            return Icon;
        }
    }
}
