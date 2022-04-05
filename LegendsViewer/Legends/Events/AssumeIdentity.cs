using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class AssumeIdentity : WorldEvent
    {
        public HistoricalFigure Trickster { get; set; }
        public int IdentityId { get; set; }
        public Entity Target { get; set; }

        public Identity Identity { get; set; }

        public string IdentityName { get; set; }
        public CreatureInfo IdentityRace { get; set; }
        public string IdentityCaste { get; set; }

        public AssumeIdentity(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "identity_id":
                        IdentityId = Convert.ToInt32(property.Value);
                        break;
                    case "target_enid":
                        Target = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "identity_histfig_id":
                    case "identity_nemesis_id":
                    case "trickster_hfid":
                    case "trickster":
                        if (Trickster == null)
                        {
                            Trickster = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        }
                        else
                        {
                            property.Known = true;
                        }
                        break;
                    case "target":
                        if (Target == null)
                        {
                            Target = world.GetEntity(Convert.ToInt32(property.Value));
                        }
                        else
                        {
                            property.Known = true;
                        }
                        break;
                    case "identity_name":
                        IdentityName = property.Value;
                        break;
                    case "identity_race":
                        IdentityRace = world.GetCreatureInfo(property.Value);
                        break;
                    case "identity_caste":
                        IdentityCaste = Formatting.InitCaps(property.Value);
                        break;
                }
            }

            Trickster.AddEvent(this);
            Target.AddEvent(this);
            if (!string.IsNullOrEmpty(IdentityName))
            {
                Identity = new Identity(IdentityName, IdentityRace, IdentityCaste);
            }
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Trickster?.ToLink(link, pov, this) ?? "an unknown creature";
            if (Target != null)
            {
                eventString += " fooled ";
                eventString += Target?.ToLink(link, pov, this) ?? "an unknown civilization";
                eventString += " into believing ";
                eventString += Trickster?.ToLink(link, pov, this) ?? "an unknown creature";
                eventString += " was ";
            }
            else
            {
                eventString += " assumed the identity of ";
            }
            Identity identity = Trickster?.Identities.Find(i => i.Id == IdentityId) ?? Identity;
            if (identity != null)
            {
                eventString += identity.Print(link, pov, this);
            }
            else
            {
                eventString += "someone else";
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}