using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class ModifiedBuilding : WorldEvent
    {
        public HistoricalFigure ModifierHf { get; set; }
        public Site Site { get; set; }
        public int StructureId { get; set; }
        public Structure Structure { get; set; }
        public string Modification { get; set; }

        public ModifiedBuilding(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "modifier_hfid": ModifierHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "structure_id": StructureId = Convert.ToInt32(property.Value); break;
                    case "modification": Modification = property.Value; break;
                }
            }

            if (Site != null)
            {
                Structure = Site.Structures.Find(structure => structure.Id == StructureId);
            }
            ModifierHf.AddEvent(this);
            Site.AddEvent(this);
            Structure.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += ModifierHf.ToLink(link, pov, this);
            eventString += " had a ";
            eventString += Modification;
            eventString += " added to ";
            eventString += Structure.ToLink(link, pov, this);
            if (Site != null)
            {
                eventString += " in ";
                eventString += Site.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}