using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class EntityOverthrown : WorldEvent
    {
        public Entity Entity { get; set; }
        public int PositionProfileId { get; set; }
        public HistoricalFigure OverthrownHistoricalFigure { get; set; }
        public Site Site { get; set; }
        public HistoricalFigure PositionTaker { get; set; }
        public HistoricalFigure Instigator { get; set; }
        public List<HistoricalFigure> Conspirators { get; set; }

        public EntityOverthrown(List<Property> properties, World world) : base(properties, world)
        {
            Conspirators = new List<HistoricalFigure>();
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "entity_id": Entity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "position_profile_id": PositionProfileId = Convert.ToInt32(property.Value); break;
                    case "overthrown_hfid": OverthrownHistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "pos_taker_hfid": PositionTaker = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "instigator_hfid": Instigator = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "conspirator_hfid": Conspirators.Add(world.GetHistoricalFigure(Convert.ToInt32(property.Value))); break;
                }
            }

            Entity.AddEvent(this);
            OverthrownHistoricalFigure.AddEvent(this);
            Site.AddEvent(this);
            PositionTaker.AddEvent(this);
            if (Instigator != PositionTaker)
            {
                Instigator.AddEvent(this);
            }
            foreach (HistoricalFigure conspirator in Conspirators)
            {
                if (conspirator != PositionTaker && conspirator != Instigator)
                {
                    conspirator.AddEvent(this);
                }
            }
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Instigator.ToLink(link, pov, this);
            eventString += " toppled the government of ";
            try
            {
                eventString += OverthrownHistoricalFigure.ToLink(link, pov, this);
            }
            catch
            {
                OverthrownHistoricalFigure = new HistoricalFigure();
                eventString += OverthrownHistoricalFigure.ToLink(link, pov, this);
            }
            eventString += " of ";
            eventString += Entity.ToLink(link, pov, this);
            if (PositionTaker != Instigator)
            {
                eventString += " placed ";
                eventString += PositionTaker.ToLink(link, pov, this);
                eventString += " in power";
            }
            else
            {
                eventString += " and assumed control";
            }
            if (Site != null)
            {
                eventString += " in ";
                eventString += Site.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            if (Conspirators.Count > 0)
            {
                eventString += " The support of ";
                for (int i = 0; i < Conspirators.Count; i++)
                {
                    var conspirator = Conspirators[i];
                    eventString += conspirator.ToLink(link, pov, this);
                    if (Conspirators.Count - i > 2)
                    {
                        eventString += ", ";
                    }
                    else if (Conspirators.Count - i == 2)
                    {
                        eventString += " and ";
                    }
                }
                eventString += "was crucial to the coup.";
            }
            return eventString;
        }
    }
}