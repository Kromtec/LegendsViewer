using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Interfaces;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfDied : WorldEvent, IFeatured
    {
        public HistoricalFigure Slayer { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public DeathCause Cause { get; set; }
        private string UnknownCause { get; set; }
        public Site Site { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }
        public int SlayerItemId { get; set; }
        public int SlayerShooterItemId { get; set; }
        public string SlayerRace { get; set; }
        public string SlayerCaste { get; set; }

        public int ItemId { get; set; }
        public string ItemType { get; set; }
        public string ItemSubType { get; set; }
        public string ItemMaterial { get; set; }
        public Artifact Artifact { get; set; }

        public int ShooterItemId { get; set; }
        public string ShooterItemType { get; set; }
        public string ShooterItemSubType { get; set; }
        public string ShooterItemMaterial { get; set; }
        public Artifact ShooterArtifact { get; set; }

        public HfDied(List<Property> properties, World world)
            : base(properties, world)
        {
            ItemId = -1;
            ShooterItemId = -1;
            SlayerItemId = -1;
            SlayerShooterItemId = -1;
            SlayerRace = "UNKNOWN";
            SlayerCaste = "UNKNOWN";
            Cause = DeathCause.Unknown;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "slayer_item_id": SlayerItemId = Convert.ToInt32(property.Value); break;
                    case "slayer_shooter_item_id": SlayerShooterItemId = Convert.ToInt32(property.Value); break;
                    case "cause":
                        switch (property.Value)
                        {
                            case "hunger": Cause = DeathCause.Starved; break;
                            case "struck": Cause = DeathCause.Struck; break;
                            case "murdered": Cause = DeathCause.Murdered; break;
                            case "old age": Cause = DeathCause.OldAge; break;
                            case "dragonfire": Cause = DeathCause.DragonsFire; break;
                            case "shot": Cause = DeathCause.Shot; break;
                            case "fire": Cause = DeathCause.Burned; break;
                            case "thirst": Cause = DeathCause.Thirst; break;
                            case "air": Cause = DeathCause.Suffocated; break;
                            case "blood": Cause = DeathCause.Bled; break;
                            case "cold": Cause = DeathCause.Cold; break;
                            case "crushed bridge": Cause = DeathCause.CrushedByABridge; break;
                            case "drown": Cause = DeathCause.Drowned; break;
                            case "infection": Cause = DeathCause.Infection; break;
                            case "obstacle": Cause = DeathCause.CollidedWithAnObstacle; break;
                            case "put to rest": Cause = DeathCause.PutToRest; break;
                            case "quitdead": Cause = DeathCause.StarvedQuit; break;
                            case "trap": Cause = DeathCause.Trap; break;
                            case "crushed": Cause = DeathCause.CaveIn; break;
                            case "cage blasted": Cause = DeathCause.InACage; break;
                            case "freezing water": Cause = DeathCause.FrozenInWater; break;
                            case "exec generic": Cause = DeathCause.ExecutedGeneric; break;
                            case "exec fed to beasts": Cause = DeathCause.ExecutedFedToBeasts; break;
                            case "exec burned alive": Cause = DeathCause.ExecutedBurnedAlive; break;
                            case "exec crucified": Cause = DeathCause.ExecutedCrucified; break;
                            case "exec drowned": Cause = DeathCause.ExecutedDrowned; break;
                            case "exec hacked to pieces": Cause = DeathCause.ExecutedHackedToPieces; break;
                            case "exec buried alive": Cause = DeathCause.ExecutedBuriedAlive; break;
                            case "exec beheaded": Cause = DeathCause.ExecutedBeheaded; break;
                            case "blood drained": Cause = DeathCause.DrainedBlood; break;
                            case "collapsed": Cause = DeathCause.Collapsed; break;
                            case "scared to death": Cause = DeathCause.ScaredToDeath; break;
                            case "scuttled": Cause = DeathCause.Scuttled; break;
                            case "flying object": Cause = DeathCause.FlyingObject; break;
                            case "slaughtered": Cause = DeathCause.Slaughtered; break;
                            case "melt": Cause = DeathCause.Melted; break;
                            case "spikes": Cause = DeathCause.Spikes; break;
                            case "heat": Cause = DeathCause.Heat; break;
                            case "vanish": Cause = DeathCause.Vanish; break;
                            case "cooling magma": Cause = DeathCause.CoolingMagma; break;
                            case "vehicle": Cause = DeathCause.Vehicle; break;
                            case "suicide drowned": Cause = DeathCause.SuicideDrowned; break;
                            case "suicide leaping": Cause = DeathCause.SuicideLeaping; break;
                            case "chasm": Cause = DeathCause.Chasm; break;
                            default: Cause = DeathCause.Unknown; UnknownCause = property.Value; world.ParsingErrors.Report("|==> Events 'hf died'/ \nUnknown Death Cause: " + UnknownCause); break;
                        }
                        break;
                    case "slayer_race": SlayerRace = Formatting.FormatRace(property.Value); break;
                    case "slayer_caste": SlayerCaste = property.Value; break;
                    case "hfid": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "slayer_hfid": Slayer = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "victim_hf": if (HistoricalFigure == null) { HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "slayer_hf": if (Slayer == null) { Slayer = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "death_cause": property.Known = true; break;
                    case "item": ItemId = Convert.ToInt32(property.Value); break;
                    case "item_type": ItemType = property.Value; break;
                    case "item_subtype": ItemSubType = property.Value; break;
                    case "mat": ItemMaterial = property.Value; break;
                    case "artifact_id": Artifact = world.GetArtifact(Convert.ToInt32(property.Value)); break;
                    case "shooter_item": ShooterItemId = Convert.ToInt32(property.Value); break;
                    case "shooter_item_type": ShooterItemType = property.Value; break;
                    case "shooter_item_subtype": ShooterItemSubType = property.Value; break;
                    case "shooter_mat": ShooterItemMaterial = property.Value; break;
                    case "shooter_artifact_id": ShooterArtifact = world.GetArtifact(Convert.ToInt32(property.Value)); break;
                }
            }

            HistoricalFigure.AddEvent(this);
            if (HistoricalFigure.DeathCause == DeathCause.None)
            {
                HistoricalFigure.DeathCause = Cause;
            }

            if (Slayer != null)
            {
                if (HistoricalFigure != Slayer)
                {
                    Slayer.AddEvent(this);
                }
                Slayer.NotableKills.Add(this);
            }
            Site.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
            Artifact.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += GetDeathString(link, pov);
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
        
        public string PrintFeature(bool link = true, DwarfObject pov = null)
        {
            string eventString = "";
            eventString += GetDeathString(link, pov);
            eventString += " in ";
            eventString += Year;
            return eventString;
        }

        private string GetDeathString(bool link = true, DwarfObject pov = null)
        {
            string eventString = "";
            eventString += HistoricalFigure.ToLink(link, pov, this) + " ";
            string deathString = "";

            if (Slayer != null || SlayerRace != "UNKNOWN" && SlayerRace != "-1")
            {
                string slayerString;
                if (Slayer == null)
                {
                    slayerString = " a " + SlayerRace.ToLower();
                }
                else
                {
                    slayerString = Slayer.ToLink(link, pov, this);
                }

                switch (Cause)
                {
                    case DeathCause.DragonsFire:
                        deathString = "burned up in " + slayerString + "'s dragon fire";
                        break;
                    case DeathCause.Burned:
                        deathString = "was burned to death by " + slayerString + "'s fire";
                        break;
                    case DeathCause.Murdered:
                        deathString = "was murdered by " + slayerString;
                        break;
                    case DeathCause.Shot:
                        deathString = "was shot and killed by " + slayerString;
                        break;
                    case DeathCause.Struck:
                        deathString = "was struck down by " + slayerString;
                        break;
                    case DeathCause.ExecutedGeneric:
                        deathString = "was executed by " + slayerString;
                        break;
                    case DeathCause.ExecutedBuriedAlive:
                        deathString = "was buried alive by " + slayerString;
                        break;
                    case DeathCause.ExecutedBurnedAlive:
                        deathString = "was burned alive by " + slayerString;
                        break;
                    case DeathCause.ExecutedCrucified:
                        deathString = "was crucified by " + slayerString;
                        break;
                    case DeathCause.ExecutedDrowned:
                        deathString = "was drowned by " + slayerString;
                        break;
                    case DeathCause.ExecutedFedToBeasts:
                        deathString = "was fed to beasts by " + slayerString;
                        break;
                    case DeathCause.ExecutedHackedToPieces:
                        deathString = "was hacked to pieces by " + slayerString;
                        break;
                    case DeathCause.ExecutedBeheaded:
                        deathString = "was beheaded by " + slayerString;
                        break;
                    case DeathCause.DrainedBlood:
                        deathString = "was drained of blood by " + slayerString;
                        break;
                    case DeathCause.Collapsed:
                        deathString = "collapsed, struck down by " + slayerString;
                        break;
                    case DeathCause.ScaredToDeath:
                        deathString = " was scared to death by " + slayerString;
                        break;
                    case DeathCause.Bled:
                        deathString = " bled to death, slain by " + slayerString;
                        break;
                    case DeathCause.Spikes:
                        deathString = " was impaled by " + slayerString;
                        break;
                    default:
                        deathString += ", slain by " + slayerString;
                        break;
                }
            }
            else
            {
                switch (Cause)
                {
                    case DeathCause.Thirst:
                        deathString = "died of thirst";
                        break;
                    case DeathCause.OldAge:
                        deathString = "died of old age";
                        break;
                    case DeathCause.Suffocated:
                        deathString = "suffocated";
                        break;
                    case DeathCause.Bled:
                        deathString = "bled to death";
                        break;
                    case DeathCause.Cold:
                        deathString = "froze to death";
                        break;
                    case DeathCause.CrushedByABridge:
                        deathString = "was crushed by a drawbridge";
                        break;
                    case DeathCause.Drowned:
                        deathString = "drowned";
                        break;
                    case DeathCause.Starved:
                        deathString = "starved to death";
                        break;
                    case DeathCause.Infection:
                        deathString = "succumbed to infection";
                        break;
                    case DeathCause.CollidedWithAnObstacle:
                        deathString = "died after colliding with an obstacle";
                        break;
                    case DeathCause.PutToRest:
                        deathString = "was put to rest";
                        break;
                    case DeathCause.StarvedQuit:
                        deathString = "starved";
                        break;
                    case DeathCause.Trap:
                        deathString = "was killed by a trap";
                        break;
                    case DeathCause.CaveIn:
                        deathString = "was crushed under a collapsing ceiling";
                        break;
                    case DeathCause.InACage:
                        deathString = "died in a cage";
                        break;
                    case DeathCause.FrozenInWater:
                        deathString = "was incased in ice";
                        break;
                    case DeathCause.Scuttled:
                        deathString = "was scuttled";
                        break;
                    case DeathCause.Slaughtered:
                        deathString = "was slaughtered";
                        break;
                    case DeathCause.FlyingObject:
                        deathString = "was killed by a flying object";
                        break;
                    case DeathCause.ExecutedGeneric:
                        deathString = "was executed";
                        break;
                    case DeathCause.ExecutedBuriedAlive:
                        deathString = "was buried alive";
                        break;
                    case DeathCause.ExecutedBurnedAlive:
                        deathString = "was burned alive";
                        break;
                    case DeathCause.ExecutedCrucified:
                        deathString = "was crucified";
                        break;
                    case DeathCause.ExecutedDrowned:
                        deathString = "was drowned";
                        break;
                    case DeathCause.ExecutedFedToBeasts:
                        deathString = "was fed to beasts";
                        break;
                    case DeathCause.ExecutedHackedToPieces:
                        deathString = "was hacked to pieces";
                        break;
                    case DeathCause.ExecutedBeheaded:
                        deathString = "was beheaded";
                        break;
                    case DeathCause.Melted:
                        deathString = "melted";
                        break;
                    case DeathCause.Spikes:
                        deathString = "was impaled";
                        break;
                    case DeathCause.Heat:
                        deathString = "died in the heat";
                        break;
                    case DeathCause.Vanish:
                        deathString = "vanished";
                        break;
                    case DeathCause.CoolingMagma:
                        deathString = "was encased in cooling magma";
                        break;
                    case DeathCause.Vehicle:
                        deathString = "was killed by a vehicle";
                        break;
                    case DeathCause.SuicideDrowned:
                        deathString = "drowned ";
                        switch (HistoricalFigure.Caste)
                        {
                            case "Female":
                                deathString += "herself ";
                                break;
                            case "Male":
                                deathString += "himself ";
                                break;
                            default:
                                deathString += "itself ";
                                break;
                        }
                        break;
                    case DeathCause.SuicideLeaping:
                        deathString = "leapt from a great height";
                        break;
                    case DeathCause.Chasm:
                        deathString = "fell into a deep chasm";
                        break;
                    case DeathCause.Unknown:
                        deathString = "died (" + UnknownCause + ")";
                        break;
                }
            }

            eventString += deathString;

            if (ItemId >= 0)
            {
                if (Artifact != null)
                {
                    eventString += " with " + Artifact.ToLink(link, pov, this);
                }
                else if (!string.IsNullOrWhiteSpace(ItemType) || !string.IsNullOrWhiteSpace(ItemSubType))
                {
                    eventString += " with a ";
                    eventString += !string.IsNullOrWhiteSpace(ItemMaterial) ? ItemMaterial + " " : " ";
                    eventString += !string.IsNullOrWhiteSpace(ItemSubType) ? ItemSubType : ItemType;
                }
            }
            else if (ShooterItemId >= 0)
            {
                if (ShooterArtifact != null)
                {
                    eventString += " (shot) with " + ShooterArtifact.ToLink(link, pov, this);
                }
                else if (!string.IsNullOrWhiteSpace(ShooterItemType) || !string.IsNullOrWhiteSpace(ShooterItemSubType))
                {
                    eventString += " (shot) with a ";
                    eventString += !string.IsNullOrWhiteSpace(ShooterItemMaterial) ? ShooterItemMaterial + " " : " ";
                    eventString += !string.IsNullOrWhiteSpace(ShooterItemSubType) ? ShooterItemSubType : ShooterItemType;
                }
            }
            else if (SlayerItemId >= 0)
            {
                eventString += " with a (" + SlayerItemId + ")";
            }
            else if (SlayerShooterItemId >= 0)
            {
                eventString += " (shot) with a (" + SlayerShooterItemId + ")";
            }

            if (Site != null)
            {
                eventString += " in " + Site.ToLink(link, pov, this);
            }
            else if (Region != null)
            {
                eventString += " in " + Region.ToLink(link, pov, this);
            }
            else if (UndergroundRegion != null)
            {
                eventString += " in " + UndergroundRegion.ToLink(link, pov, this);
            }

            return eventString;
        }
    }
}