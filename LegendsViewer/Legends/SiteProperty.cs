using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public class SiteProperty
    {
        public int Id { get; set; }
        public SitePropertyType Type { get; set; }
        private int OwnerId { get; set; }
        public HistoricalFigure Owner { get; set; }
        public Structure Structure { get; set; }
        public Site Site { get; set; }

        public SiteProperty(List<Property> properties, World world, Site site)
        {
            Id = -1;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "id": 
                        Id = Convert.ToInt32(property.Value); 
                        break;
                    case "type":
                        switch (property.Value)
                        {
                            case "house": Type = SitePropertyType.House; break;
                            default:
                                property.Known = false;
                                break;
                        }
                        break;
                    case "owner_hfid": 
                        OwnerId = Convert.ToInt32(property.Value); 
                        break;
                    case "structure_id": 
                        Structure = site.Structures.FirstOrDefault(structure => structure.Id == Convert.ToInt32(property.Value)); 
                        break;
                    default:
                        property.Known = false;
                        break;
                }
            }

            Site = site;
        }

        public void Resolve(World world)
        {
            Owner = world.GetHistoricalFigure(OwnerId);
            Owner?.SiteProperties.Add(this);
            if (Structure != null && Owner != null)
            {
                Structure.Owner = Owner;
            }
        }
    }
}
