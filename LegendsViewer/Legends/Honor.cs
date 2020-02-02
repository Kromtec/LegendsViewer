using System;
using System.Collections.Generic;
using LegendsViewer.Controls.HTML;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public class Honor
    {
        public Entity Entity { get; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int GivesPrecedence { get; set; }
        public string RequiredSkillName { get; set; }
        public int RequiredSkillIpTotal { get; set; }
        public int RequiredBattles { get; set; }
        public int RequiredYears { get; set; }
        public int ExemptEpId { get; set; }
        public int ExemptFormerEpId { get; set; }
        public bool GrantedToEverybody { get; set; }
        public Skill RequiredSkill { get; set; }

        public Honor(List<Property> properties, World world, Entity entity)
        {
            Entity = entity;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "id": Id = Convert.ToInt32(property.Value); break;
                    case "name": Name = property.Value; break;
                    case "gives_precedence": GivesPrecedence = Convert.ToInt32(property.Value); break;
                    case "required_skill": RequiredSkillName = property.Value; break;
                    case "required_skill_ip_total": RequiredSkillIpTotal = Convert.ToInt32(property.Value); break;
                    case "required_battles": RequiredBattles = Convert.ToInt32(property.Value); break;
                    case "required_years": RequiredYears = Convert.ToInt32(property.Value); break;
                    case "exempt_epid": ExemptEpId = Convert.ToInt32(property.Value); break;
                    case "exempt_former_epid": ExemptFormerEpId = Convert.ToInt32(property.Value); break;
                    case "granted_to_everybody":
                        property.Known = true;
                        GrantedToEverybody = true; 
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(RequiredSkillName))
            {
                RequiredSkill = new Skill(RequiredSkillName, RequiredSkillIpTotal);
            }
        }

        public string Print()
        {
            string html = "";
            html += Name;
            if (RequiredSkill != null)
            {
                html += "<br/>";
                html += "<ol class='skills'>";
                html += HtmlPrinter.SkillToString(SkillDictionary.LookupSkill(RequiredSkill));
                html += "</ol>";
            }
            return html;
        }
    }
}
