using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class BuildingProfileAcquired : WorldEvent
    {
        public Site Site { get; set; }
        public int BuildingProfileId { get; set; }
        public SiteProperty SiteProperty { get; set; }
        public HistoricalFigure AcquirerHf { get; set; }
        public Entity AcquirerEntity { get; set; }
        public HistoricalFigure LastOwnerHf { get; set; }
        public bool Inherited { get; set; }
        public bool RebuiltRuined { get; set; }
        public bool PurchasedUnowned { get; set; }

        // http://www.bay12games.com/dwarves/mantisbt/view.php?id=11346
        // 0011346: <acquirer_enid> is always -1 in "building profile acquired" event
        public BuildingProfileAcquired(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "acquirer_hfid": AcquirerHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "acquirer_enid": AcquirerEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "last_owner_hfid": LastOwnerHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "building_profile_id": BuildingProfileId = Convert.ToInt32(property.Value); break;
                    case "purchased_unowned": property.Known = true; PurchasedUnowned = true; break;
                    case "inherited": property.Known = true; Inherited = true; break;
                    case "rebuilt_ruined": property.Known = true; RebuiltRuined = true; break;
                }
            }

            if (Site != null)
            {
                SiteProperty = Site.SiteProperties.FirstOrDefault(siteProperty => siteProperty.Id == BuildingProfileId);
                SiteProperty?.Structure?.AddEvent(this);
            }

            Site.AddEvent(this);
            AcquirerHf.AddEvent(this);
            AcquirerEntity.AddEvent(this);
            LastOwnerHf.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (AcquirerHf != null)
            {
                eventString += AcquirerHf?.ToLink(link, pov, this);
                if (AcquirerEntity != null)
                {
                    eventString += " of ";
                }
            }
            else if (AcquirerEntity != null)
            {
                eventString += AcquirerEntity.ToLink(link, pov, this);
            }
            else
            {
                eventString += "Someone ";
            }
            if (PurchasedUnowned)
            {
                eventString += " purchased ";
            }
            else if (Inherited)
            {
                eventString += " inherited ";
            }
            else if (RebuiltRuined)
            {
                eventString += " rebuilt ";
            }
            else
            {
                eventString += " acquired ";
            }

            eventString += SiteProperty.Print(link, pov);
            if (Site != null)
            {
                eventString += " in ";
                eventString += Site.ToLink(link, pov, this);
            }

            if (LastOwnerHf != null)
            {
                eventString += " formerly owned by ";
                eventString += LastOwnerHf.ToLink(link, pov, this);
            }

            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
