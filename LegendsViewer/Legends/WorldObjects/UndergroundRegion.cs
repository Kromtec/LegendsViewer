using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Interfaces;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.WorldObjects
{
    public class UndergroundRegion : WorldObject, IHasCoordinates
    {
        public string Icon = "<i class=\"fa fa-fw fa-map\"></i>";

        public int Depth { get; set; }
        public string Type { get; set; }
        public List<Battle> Battles { get; set; }
        public List<Location> Coordinates { get; set; } // legends_plus.xml
        public int SquareTiles
        {
            get
            {
                return Coordinates.Count;
            }
        }
        public static List<string> Filters;
        public override List<WorldEvent> FilteredEvents
        {
            get { return Events.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }
        public UndergroundRegion() { Type = "INVALID UNDERGROUND REGION"; Depth = 0; Battles = new List<Battle>(); }
        public UndergroundRegion(List<Property> properties, World world)
            : base(properties, world)
        {
            Depth = 0;
            Type = "";
            Battles = new List<Battle>();
            Coordinates = new List<Location>();
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "depth": Depth = Convert.ToInt32(property.Value); break;
                    case "type": Type = Formatting.InitCaps(property.Value); break;
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
                }
            }
        }
        public override string ToString() { return Type; }
        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            string name;
            if (Type == "Cavern")
            {
                name = "the depths of the world";
            }
            else if (Type == "Underworld")
            {
                name = "the Underworld";
            }
            else
            {
                name = "an underground region (" + Type + ")";
            }

            if (link)
            {
                string title = Type;
                title += "&#13";
                title += "Events: " + Events.Count;

                if (pov != this)
                {
                    return Icon + "<a href = \"uregion#" + Id + "\" title=\"" + title + "\">" + name + "</a>";
                }

                return Icon + "<a title=\"" + title + "\">" + HtmlStyleUtil.CurrentDwarfObject(name) + "</a>";
            }
            return name;
        }

        public override string GetIcon()
        {
            return Icon;
        }
    }
}
