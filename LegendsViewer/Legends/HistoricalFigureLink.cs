using System;
using System.Collections.Generic;
using LegendsViewer.Controls.Query.Attributes;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public class HistoricalFigureLink
    {
        [AllowAdvancedSearch("Historical Figure")]
        public HistoricalFigure HistoricalFigure { get; set; }
        [AllowAdvancedSearch]
        public HistoricalFigureLinkType Type { get; set; }
        [AllowAdvancedSearch]
        public int Strength { get; set; }

        public HistoricalFigureLink(List<Property> properties, World world)
        {
            Strength = 0;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "hfid": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "link_strength": Strength = Convert.ToInt32(property.Value); break;
                    case "link_type":
                        HistoricalFigureLinkType linkType;
                        if (Enum.TryParse(Formatting.InitCaps(property.Value).Replace(" ", ""), out linkType))
                        {
                            Type = linkType;
                        }
                        else
                        {
                            Type = HistoricalFigureLinkType.Unknown;
                            world.ParsingErrors.Report("Unknown HF HF Link Type: " + property.Value);
                        }
                        break;
                }
            }

            //XML states that deity links, that should be 100, are 0.
            if (Type == HistoricalFigureLinkType.Deity && Strength == 0)
            {
                Strength = 100;
            }
        }

        public HistoricalFigureLink(HistoricalFigure historicalFigureTarget, HistoricalFigureLinkType type, int strength = 0)
        {
            HistoricalFigure = historicalFigureTarget;
            Type = type;
            Strength = strength;
        }
    }
}