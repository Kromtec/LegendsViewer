using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class EntityPersecuted : WorldEvent
    {
        public HistoricalFigure PersecutorHf { get; set; }
        public Entity PersecutorEntity { get; set; }
        public Entity TargetEntity { get; set; }
        public Site Site { get; set; }
        public int DestroyedStructureId { get; set; }
        public Structure DestroyedStructure { get; set; }
        public int ShrineAmountDestroyed { get; set; }
        public List<HistoricalFigure> ExpelledHfs { get; set; }
        public List<HistoricalFigure> PropertyConfiscatedFromHfs { get; set; }
        public List<int> ExpelledCreatures { get; set; }
        public List<int> ExpelledPopIds { get; set; }
        public List<int> ExpelledNumbers { get; set; }

        public EntityPersecuted(List<Property> properties, World world) : base(properties, world)
        {
            ExpelledHfs = new List<HistoricalFigure>();
            ExpelledCreatures = new List<int>();
            ExpelledPopIds = new List<int>();
            ExpelledNumbers = new List<int>();
            PropertyConfiscatedFromHfs = new List<HistoricalFigure>();

            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "persecutor_hfid": PersecutorHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "persecutor_enid": PersecutorEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "target_enid": TargetEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "shrine_amount_destroyed": ShrineAmountDestroyed = Convert.ToInt32(property.Value); break;
                    case "destroyed_structure_id": DestroyedStructureId = Convert.ToInt32(property.Value); break;
                    case "property_confiscated_from_hfid": PropertyConfiscatedFromHfs.Add(world.GetHistoricalFigure(Convert.ToInt32(property.Value))); break;
                    case "expelled_hfid": ExpelledHfs.Add(world.GetHistoricalFigure(Convert.ToInt32(property.Value))); break;
                    case "expelled_creature": ExpelledCreatures.Add(Convert.ToInt32(property.Value)); break;
                    case "expelled_pop_id": ExpelledPopIds.Add(Convert.ToInt32(property.Value)); break;
                    case "expelled_number": ExpelledNumbers.Add(Convert.ToInt32(property.Value)); break;
                }
            }
            if (Site != null)
            {
                DestroyedStructure = Site.Structures.FirstOrDefault(structure => structure.Id == DestroyedStructureId);
                DestroyedStructure.AddEvent(this);
            }
            PersecutorHf.AddEvent(this);
            PersecutorEntity.AddEvent(this);
            TargetEntity.AddEvent(this);
            Site.AddEvent(this);
            foreach (HistoricalFigure expelledHf in ExpelledHfs)
            {
                expelledHf.AddEvent(this);
            }
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += PersecutorHf.ToLink(link, pov, this);
            eventString += " of ";
            eventString += PersecutorEntity.ToLink(link, pov, this);
            eventString += " persecuted ";
            eventString += TargetEntity.ToLink(link, pov, this);
            eventString += " in ";
            eventString += Site.ToLink(link, pov, this);
            if (ExpelledHfs.Any())
            {
                eventString += ". ";
                if (ExpelledHfs.Count == 1)
                {
                    eventString += ExpelledHfs[0].ToLink(link, pov, this).ToUpperFirstLetter();
                    eventString += " was";
                }
                else
                {
                    eventString += ExpelledHfs[0].ToLink(link, pov, this).ToUpperFirstLetter();
                    for (int i = 1; i < ExpelledHfs.Count; i++)
                    {
                        if (i == ExpelledHfs.Count - 1)
                        {
                            eventString += " and ";
                        }
                        else
                        {
                            eventString += ", ";
                        }
                        eventString += ExpelledHfs[i].ToLink(link, pov, this);
                    }
                    eventString += " were";
                }
                eventString += " expelled";
                if (ShrineAmountDestroyed > 0 || DestroyedStructure != null)
                {
                    eventString += " and";
                }
            }
            else
            {
                eventString += " and";
            }

            if (DestroyedStructure != null)
            {
                eventString += " ";
                eventString += DestroyedStructure.ToLink(link, pov, this);
                eventString += " was destroyed";
                if (ShrineAmountDestroyed > 0)
                {
                    eventString += " along with several smaller sacred sites";
                }
            }
            else if(ShrineAmountDestroyed > 0)
            {
                eventString += " and some sacred sites were desecrated";
            }
            eventString += ".";
            return eventString;
        }
    }
}
