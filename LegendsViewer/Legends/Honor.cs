using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<HistoricalFigure> HonoredHfs { get; set; }

        public Honor(List<Property> properties, World world, Entity entity)
        {
            HonoredHfs = new List<HistoricalFigure>();
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

        public string Print(bool withHonoredHfs = false)
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

            if (RequiredBattles > 0 || RequiredYears > 0 || GivesPrecedence > 0 || GrantedToEverybody)
            {
                html += "<br/>";
                html += "<ul>";
                if (GivesPrecedence > 0)
                {
                    html += "<li>";
                    html += "Gives Precedence: " + GivesPrecedence;
                    html += "</li>";
                }
                if (RequiredBattles > 0 )
                {
                    html += "<li>";
                    html += "Required Battles: " + RequiredBattles;
                    html += "</li>";
                }
                if (RequiredYears > 0)
                {
                    html += "<li>";
                    html += "Required Years: " + RequiredYears;
                    html += "</li>";
                }

                if (GrantedToEverybody)
                {
                    html += "<li>";
                    html += "Granted to everybody";
                    html += "</li>";
                }
                html += "</ul>";
            }

            if (withHonoredHfs)
            {
                if (HonoredHfs.Any())
                {
                    html += "<ul>";
                    html += "<li><b>Honored Historical Figures:</b></li>";
                    html += "<ul>";
                    foreach (HistoricalFigure honoredHf in HonoredHfs)
                    {
                        html += "<li>";
                        html += honoredHf.ToLink();
                        html += "</li>";
                    }
                    html += "</ul>";
                    html += "</ul>";
                }
            }
            return html;
        }
    }
}
