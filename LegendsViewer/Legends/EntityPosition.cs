using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends
{
    public class EntityPosition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameFemale { get; set; }
        public string NameMale { get; set; }
        public string Spouse { get; set; }
        public string SpouseFemale { get; set; }
        public string SpouseMale { get; set; }

        public EntityPosition(List<Property> properties, World world)
        {
            Id = -1;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "id": Id = Convert.ToInt32(property.Value); break;
                    case "name": Name = Formatting.InitCaps(property.Value); break;
                    case "name_male": NameMale = Formatting.InitCaps(property.Value); break;
                    case "name_female": NameFemale = Formatting.InitCaps(property.Value); break;
                    case "spouse": Spouse = Formatting.InitCaps(property.Value); break;
                    case "spouse_male": SpouseMale = Formatting.InitCaps(property.Value); break;
                    case "spouse_female": SpouseFemale = Formatting.InitCaps(property.Value); break;
                }
            }
        }

        public string GetTitleByCaste(string caste, bool isSpouse = false)
        {
            string positionName;
            if (isSpouse)
            {
                if (caste == "Female" && !string.IsNullOrEmpty(SpouseFemale))
                {
                    positionName = SpouseFemale;
                }
                else
                {
                    positionName = caste == "Male" && !string.IsNullOrEmpty(SpouseMale) ? SpouseMale : Spouse;
                }
            }
            else if (caste == "Female" && !string.IsNullOrEmpty(NameFemale))
            {
                positionName = NameFemale;
            }
            else
            {
                positionName = caste == "Male" && !string.IsNullOrEmpty(NameMale) ? NameMale : Name;
            }
            return positionName;
        }
    }
}
