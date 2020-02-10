using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class CreatedWorldConstruction : WorldEvent
    {
        public Entity Civ, SiteEntity;
        public Site Site1, Site2;
        public WorldConstruction WorldConstruction, MasterWorldConstruction;
        public CreatedWorldConstruction(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "civ_id": Civ = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_civ_id": SiteEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id1": Site1 = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "site_id2": Site2 = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "wcid": WorldConstruction = world.GetWorldConstruction(Convert.ToInt32(property.Value)); break;
                    case "master_wcid": MasterWorldConstruction = world.GetWorldConstruction(Convert.ToInt32(property.Value)); break;
                }
            }

            Civ.AddEvent(this);
            SiteEntity.AddEvent(this);

            WorldConstruction.AddEvent(this);
            MasterWorldConstruction.AddEvent(this);

            Site1.AddEvent(this);
            Site2.AddEvent(this);

            Site1.AddConnection(Site2);
            Site2.AddConnection(Site1);

            if (WorldConstruction != null)
            {
                WorldConstruction.Site1 = Site1;
                WorldConstruction.Site2 = Site2;
                if (MasterWorldConstruction != null)
                {
                    MasterWorldConstruction.Sections.Add(WorldConstruction);
                    WorldConstruction.MasterConstruction = MasterWorldConstruction;
                }
            }
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += SiteEntity != null ? SiteEntity.ToLink(link, pov, this) : "UNKNOWN ENTITY";
            eventString += " of ";
            eventString += Civ != null ? Civ.ToLink(link, pov, this) : "UNKNOWN CIV";
            eventString += " constructed ";
            eventString += WorldConstruction != null ? WorldConstruction.ToLink(link, pov, this) : "UNKNOWN CONSTRUCTION";
            if (MasterWorldConstruction != null)
            {
                eventString += " as part of ";
                eventString += MasterWorldConstruction.ToLink(link, pov, this);
            }
            eventString += " connecting ";
            eventString += Site1 != null ? Site1.ToLink(link, pov, this) : "UNKNOWN SITE";
            eventString += " and ";
            eventString += Site2 != null ? Site2.ToLink(link, pov, this) : "UNKNOWN SITE";
            eventString += ".";
            return eventString;
        }
    }
}