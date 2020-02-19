using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public class Identity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public int BirthYear { get; set; }
        public int BirthSeconds72 { get; set; }
        public Entity Entity { get; set; }
        public CreatureInfo Race { get; set; }
        public string Caste { get; set; }


        public Identity(List<Property> properties, World world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "id": Id = Convert.ToInt32(property.Value); break;
                    case "name": Name = Formatting.InitCaps(property.Value.Replace("'", "`")); break;
                    case "histfig_id": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "birth_year": BirthYear = Convert.ToInt32(property.Value); break;
                    case "birth_second": BirthSeconds72 = Convert.ToInt32(property.Value); break;
                    case "entity_id": Entity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "race": Race = world.GetCreatureInfo(property.Value); break;
                    case "caste": Caste = string.Intern(Formatting.InitCaps(property.Value.ToLower().Replace('_', ' '))); break;
                }
            }

            HistoricalFigure?.Identities.Add(this);
        }
        public string Print(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            var identityString = "the ";
            if (Race != null && Race != CreatureInfo.Unknown)
            {
                identityString += Race.NameSingular.ToLower();
            }
            else
            {
                identityString += HistoricalFigure.GetRaceString();
            }
            var icon = !string.IsNullOrWhiteSpace(Caste) ? GetIcon() : HistoricalFigure.GetIcon();
            if (!string.IsNullOrWhiteSpace(icon))
            {
                identityString += " " + icon;
            }
            identityString += " " + Name;
            if (Entity != null)
            {
                identityString += " of ";
                identityString += Entity.ToLink(link, pov, worldEvent);
            }
            return identityString;
        }

        public string GetIcon()
        {
            if (Caste == "Female")
            {
                return HistoricalFigure.FemaleIcon;
            }
            if (Caste == "Male")
            {
                return HistoricalFigure.MaleIcon;
            }
            if (Caste == "Default")
            {
                return HistoricalFigure.NeuterIcon;
            }
            return "";
        }

    }
}
