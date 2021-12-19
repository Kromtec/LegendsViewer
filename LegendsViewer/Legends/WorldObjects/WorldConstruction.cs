using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Controls;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Interfaces;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.WorldObjects
{
    public class WorldConstruction : WorldObject, IHasCoordinates
    {
        public string Name { get; set; } // legends_plus.xml
        public WorldConstructionType Type { get; set; } // legends_plus.xml
        public string TypeAsString { get => Type.GetDescription(); set { } }
        public List<Location> Coordinates { get; set; } // legends_plus.xml
        public Site Site1 { get; set; } // legends_plus.xml
        public Site Site2 { get; set; } // legends_plus.xml
        public List<WorldConstruction> Sections { get; set; } // legends_plus.xml
        public WorldConstruction MasterConstruction { get; set; } // legends_plus.xml

        public string Icon = "<i class=\"fa fa-fw fa-puzzle-piece\"></i>";

        public static List<string> Filters;
        public override List<WorldEvent> FilteredEvents => Events.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList();

        public WorldConstruction(List<Property> properties, World world)
            : base(properties, world)
        {
            Name = "Untitled";
            Coordinates = new List<Location>();
            Sections = new List<WorldConstruction>();
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "name": Name = Formatting.InitCaps(property.Value); break;
                    case "type":
                        switch (property.Value)
                        {
                            case "road":
                                Type = WorldConstructionType.Road;
                                Icon = "<i class=\"fa fa-fw fa-road\"></i>";
                                break;
                            case "bridge":
                                Type = WorldConstructionType.Bridge;
                                Icon = "<i class=\"glyphicon fa-fw glyphicon-menu-up\"></i>";
                                break;
                            case "tunnel":
                                Type = WorldConstructionType.Tunnel;
                                Icon = "<i class=\"glyphicon fa-fw glyphicon-oil\"></i>";
                                break;
                            default:
                                Type = WorldConstructionType.Unknown;
                                property.Known = false;
                                break;
                        }
                        break;
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

        public override string ToString() { return Name; }

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            if (link)
            {
                string linkedString = "";
                if (pov != this)
                {
                    string title = "";
                    title += "World Construction";
                    title += Type != WorldConstructionType.Unknown ? "" : ", " + Type;
                    title += "&#13";
                    title += "Events: " + Events.Count;

                    linkedString = Icon + "<a href = \"worldconstruction#" + Id + "\" title=\"" + title + "\">" + Name + "</a>";
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
