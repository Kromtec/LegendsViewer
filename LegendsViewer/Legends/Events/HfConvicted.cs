using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfConvicted : WorldEvent
    {
        public HistoricalFigure ConvictedHf { get; set; }
        public Entity ConvicterEntity { get; set; }
        public string Crime { get; set; }
        public HistoricalFigure FooledHf { get; set; }
        public HistoricalFigure FramerHf { get; set; }
        public int PrisonMonth { get; set; }
        public bool DeathPenalty { get; set; }
        public bool WrongfulConviction { get; set; }
        public HistoricalFigure CorruptConvictorHf { get; set; }
        public HistoricalFigure PlotterHf { get; set; }
        public bool Exiled { get; set; }

        public bool SurveiledConvicted { get; set; }
        public bool HeldFirmInInterrogation { get; set; }
        public HistoricalFigure CoConspiratorHf { get; set; }
        public bool SurveiledCoConspirator { get; set; }
        public bool ConvictIsContact { get; set; }
        public HistoricalFigure ImplicatedHf { get; set; }
        public Entity ConfessedAfterApbArrestEntity { get; set; }
        public HistoricalFigure InterrogatorHf { get; set; }
        public HistoricalFigure ContactHf { get; set; }
        public bool SurveiledContact { get; set; }

        public HfConvicted(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "convicted_hfid": ConvictedHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "convicter_enid": ConvicterEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "crime": Crime = property.Value; break;
                    case "prison_months": PrisonMonth = Convert.ToInt32(property.Value); break;
                    case "fooled_hfid": FooledHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "framer_hfid": FramerHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "death_penalty": property.Known = true; DeathPenalty = true; break;
                    case "wrongful_conviction": property.Known = true; WrongfulConviction = true; break;
                    case "corrupt_convicter_hfid": CorruptConvictorHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "plotter_hfid": PlotterHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "exiled": property.Known = true; Exiled = true; break;

                    case "held_firm_in_interrogation": property.Known = true; HeldFirmInInterrogation = true; break;
                    case "convict_is_contact": property.Known = true; ConvictIsContact = true; break;
                    case "surveiled_convicted": property.Known = true; SurveiledConvicted = true; break;
                    case "surveiled_coconspirator": property.Known = true; SurveiledCoConspirator = true; break;
                    case "surveiled_contact": property.Known = true; SurveiledContact = true; break;
                    case "confessed_after_apb_arrest_enid": ConfessedAfterApbArrestEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "coconspirator_hfid": CoConspiratorHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "implicated_hfid": ImplicatedHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "interrogator_hfid": InterrogatorHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "contact_hfid": ContactHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                }
            }
            ConvictedHf.AddEvent(this);
            ConvicterEntity.AddEvent(this);
            FooledHf.AddEvent(this);
            FramerHf.AddEvent(this);
            CorruptConvictorHf.AddEvent(this);
            if (PlotterHf != CorruptConvictorHf)
            {
                PlotterHf.AddEvent(this);
            }

            if (ConvicterEntity != ConfessedAfterApbArrestEntity)
            {
                ConfessedAfterApbArrestEntity.AddEvent(this);
            }
            CoConspiratorHf.AddEvent(this);
            ImplicatedHf.AddEvent(this);
            InterrogatorHf.AddEvent(this);
            if (ImplicatedHf != ContactHf)
            {
                ContactHf.AddEvent(this);
            }
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (HeldFirmInInterrogation)
            {
                eventString += "due to ongoing surveillance";
                if (SurveiledContact & ContactHf != null)
                {
                    eventString += " on the contact ";
                    eventString += ContactHf.ToLink(link, pov, this);
                }
                if (SurveiledCoConspirator & CoConspiratorHf != null)
                {
                    eventString += " on a coconspirator ";
                    eventString += CoConspiratorHf.ToLink(link, pov, this);
                }
                eventString += " as the plot unfolded, ";
            }
            eventString += ConvictedHf.ToLink(link, pov, this);
            eventString += $" was {(WrongfulConviction ? "wrongfully " : "")}convicted ";
            if (ConvictIsContact)
            {
                eventString += $"as a go-between in a conspiracy to commit {Crime} ";
            }
            else
            {
                eventString += $"of {Crime} ";
            }
            if (ConvicterEntity != null)
            {
                eventString += "by ";
                eventString += ConvicterEntity.ToLink(link, pov, this);
            }

            if (CorruptConvictorHf != null)
            {
                eventString += " and ";
                eventString += CorruptConvictorHf.ToLink(link, pov, this);
            }
            if (PlotterHf != null && PlotterHf != CorruptConvictorHf)
            {
                eventString += " plotted by ";
                eventString += PlotterHf.ToLink(link, pov, this);
            }
            if (FooledHf != null && FramerHf != null)
            {
                eventString += " after ";
                eventString += FramerHf.ToLink(link, pov, this);
                eventString += " fooled ";
                eventString += FooledHf.ToLink(link, pov, this);
                eventString += " with fabricated evidence";
            }
            if (PrisonMonth > 0)
            {
                eventString += $" and imprisoned for a term of {(PrisonMonth > 12 ? (PrisonMonth / 12) + " years" : PrisonMonth + " month")}";
            }
            else if (DeathPenalty)
            {
                eventString += " and sentenced to death";
            }
            if (Exiled)
            {
                eventString += " and exiled";
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ". ";
            if (ImplicatedHf != null)
            {
                eventString += ConvictedHf.ToLink(link, pov, this);
                eventString += " implicated ";
                eventString += ImplicatedHf.ToLink(link, pov, this);
                eventString += " during interrogation. ";
            }

            if (InterrogatorHf != null)
            {
                eventString += "Interrogation was led by ";
                eventString += InterrogatorHf.ToLink(link, pov, this);
                eventString += ". ";
            }
            return eventString;
        }
    }
}
