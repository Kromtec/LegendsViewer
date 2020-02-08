using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfPreach : WorldEvent
    {
        public HistoricalFigure SpeakerHf { get; set; }
        public Site Site { get; set; }
        public PreachTopic Topic { get; set; }
        public Entity Entity1 { get; set; }
        public Entity Entity2 { get; set; }

        public HfPreach(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "speaker_hfid": SpeakerHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_hfid": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "topic":
                        switch (property.Value)
                        {
                            case "entity 1 should love entity 2":
                                Topic = PreachTopic.Entity1ShouldLoveEntity2;
                                break;
                            case "set entity 1 against entity 2":
                                Topic = PreachTopic.SetEntity1AgainstEntity2;
                                break;
                            default:
                                property.Known = false;
                                break;
                        }
                        break;
                    case "entity_1": Entity1 = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "entity_2": Entity2 = world.GetEntity(Convert.ToInt32(property.Value)); break;
                }
            }

            SpeakerHf.AddEvent(this);
            Site.AddEvent(this);
            Entity1.AddEvent(this);
            Entity2.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += SpeakerHf.ToLink(link, pov, this);
            eventString += " preached to ";
            eventString += Entity1.ToLink(link, pov, this);
            switch (Topic)
            {
                case PreachTopic.SetEntity1AgainstEntity2:
                    eventString += ", inveighing against ";
                    break;
                case PreachTopic.Entity1ShouldLoveEntity2:
                    eventString += ", urging love to be shown to ";
                    break;
            }
            eventString += Entity2.ToLink(link, pov, this);
            if (Site != null)
            {
                eventString += " at ";
                eventString += Site.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}