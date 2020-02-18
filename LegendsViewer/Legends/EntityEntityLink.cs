using System;
using System.Collections.Generic;
using System.ComponentModel;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public enum EntityEntityLinkType // legends_plus.xml
    {
        [Description("Subgroup")]
        Child,
        [Description("Parentgroup")]
        Parent,
        Religious,
        Unknown
    }

    public class EntityEntityLink // legends_plus.xml
    {
        public EntityEntityLink(List<Property> properties, World world)
        {
            Type = EntityEntityLinkType.Unknown;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "type":
                        switch (property.Value)
                        {
                            case "CHILD":
                                Type = EntityEntityLinkType.Child;
                                break;
                            case "PARENT":
                                Type = EntityEntityLinkType.Parent;
                                break;
                            case "RELIGIOUS":
                                Type = EntityEntityLinkType.Religious;
                                break;
                            default:
                                world.ParsingErrors.Report("Unknown Entity Entity Link Type: " + property.Value);
                                break;
                        }
                        break;
                    case "target":
                        Target = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "strength":
                        Strength = Convert.ToInt32(property.Value);
                        break;
                }
            }
        }

        public EntityEntityLinkType Type { get; set; }
        public Entity Target { get; set; }
        public int Strength { get; set; }
    }
}