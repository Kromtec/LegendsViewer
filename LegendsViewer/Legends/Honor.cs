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
        public int RequiredKills { get; set; }
        public int ExemptEpId { get; set; }
        public int ExemptFormerEpId { get; set; }
        public bool GrantedToEverybody { get; set; }
        public bool RequiresAnyMeleeOrRangedSkill { get; set; }

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
                    case "required_kills": RequiredKills = Convert.ToInt32(property.Value); break;
                    case "exempt_epid": ExemptEpId = Convert.ToInt32(property.Value); break;
                    case "exempt_former_epid": ExemptFormerEpId = Convert.ToInt32(property.Value); break;
                    case "granted_to_everybody": property.Known = true; GrantedToEverybody = true; break;
                    case "requires_any_melee_or_ranged_skill": property.Known = true; RequiresAnyMeleeOrRangedSkill = true; break;
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

            if (RequiredBattles > 0 || RequiredYears > 0 || RequiredKills > 0 || GivesPrecedence > 0 || GrantedToEverybody || RequiresAnyMeleeOrRangedSkill)
            {
                html += "<br/>";
                html += "<ul>";
                if (GivesPrecedence > 0)
                {
                    html += "<li>";
                    html += "Gives Precedence: " + GivesPrecedence;
                    html += "</li>";
                }
                if (RequiredBattles > 0)
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
                if (RequiredKills > 0)
                {
                    html += "<li>";
                    html += "Required Kills: " + RequiredKills;
                    html += "</li>";
                }

                if (GrantedToEverybody)
                {
                    html += "<li>";
                    html += "Granted to everybody";
                    html += "</li>";
                }
                if (RequiresAnyMeleeOrRangedSkill)
                {
                    html += "<li>";
                    html += "Requires any melee or ranged skill";
                    html += "</li>";
                }
                html += "</ul>";
            }

            if (withHonoredHfs)
            {
                if (HonoredHfs.Count > 0)
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

        public string PrintRequirementsAsString()
        {
            string requirementsString = "";
            List<string> requirements = new List<string>();
            if (RequiredSkill != null)
            {
                requirements.Add($"attaining sufficient skill with the {RequiredSkill.Name.ToLower()}");
            }
            else if (RequiresAnyMeleeOrRangedSkill)
            {
                requirements.Add("attaining sufficient skill with a weapon or technique");
            }

            if (RequiredBattles == 1)
            {
                requirements.Add("serving in combat");
            }
            else if (RequiredBattles > 1)
            {
                requirements.Add($"serving in {RequiredBattles} battles");
            }
            if (RequiredKills == 1)
            {
                requirements.Add("killing an enemy");
            }
            else if (RequiredKills > 1)
            {
                requirements.Add($"killing {RequiredKills} enemies");
            }
            if (RequiredYears == 1)
            {
                requirements.Add("being enlisted for a year");
            }
            else if (RequiredYears > 1)
            {
                requirements.Add($"being enlisted for {RequiredYears} years");
            }

            if (requirements.Count == 1)
            {
                return requirements[0];
            }
            for (int i = 0; i < requirements.Count; i++)
            {
                requirementsString += requirements[i];
                if (i == requirements.Count - 2)
                {
                    requirementsString += " and ";
                }
                else if (i < requirements.Count - 2)
                {
                    requirementsString += ", ";
                }
            }

            return requirementsString;
        }
    }
}
