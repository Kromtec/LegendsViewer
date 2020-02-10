using System.Collections.Generic;
using LegendsViewer.Legends;

namespace LegendsViewer.Controls.HTML.Utilities
{
    public class SkillDescription
    {
        public string Token { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Rank { get; set; }
        public int Points { get; set; }

        public SkillDescription(string category, string token, string name, string desc)
        {
            Category = category;
            Token = token;
            Name = name;
            Description = desc;
            Rank = "None";
            Points = 0;
        }
    }

    public static class SkillDictionary
    {
        public static readonly Dictionary<string, SkillDescription> Dict = new Dictionary<string, SkillDescription>
        {
            {"None", new SkillDescription("non", "NONE", "None", "None")},
            {"Mining", new SkillDescription("min", "MINING", "Miner", "Mining")},
            {"Woodcutting", new SkillDescription("wod", "WOODCUTTING", "Wood cutter", "Wood Cutting")},
            {"Carpentry", new SkillDescription("wod", "CARPENTRY", "Carpenter", "Carpentry")},
            {"Detailstone", new SkillDescription("stn", "DETAILSTONE", "Engraver", "Engraving")},
            {"Masonry", new SkillDescription("stn", "MASONRY", "Mason", "Masonry")},
            {"Animaltrain", new SkillDescription("hnt", "ANIMALTRAIN", "Animal trainer", "Animal Training")},
            {"Animalcare", new SkillDescription("hnt", "ANIMALCARE", "Animal caretaker", "Animal Caretaking")},
            {"Dissect Fish", new SkillDescription("fsh", "DISSECT_FISH", "Fish dissector", "Fish Dissection")},
            {"Dissect Vermin", new SkillDescription("hnt", "DISSECT_VERMIN", "Animal dissector", "Animal Dissection")},
            {"Processfish", new SkillDescription("fsh", "PROCESSFISH", "Fish cleaner", "Fish Cleaning")},
            {"Butcher", new SkillDescription("but", "BUTCHER", "Butcher", "Butchery")},
            {"Trapping", new SkillDescription("hnt", "TRAPPING", "Trapper", "Trapping")},
            {"Tanner", new SkillDescription("but", "TANNER", "Tanner", "Tanning")},
            {"Weaving", new SkillDescription("crf", "WEAVING", "Weaver", "Weaving")},
            {"Brewing", new SkillDescription("cok", "BREWING", "Brewer", "Brewing")},
            {"Alchemy", new SkillDescription("msc", "ALCHEMY", "Alchemist", "Alchemy")},
            {"Clothesmaking", new SkillDescription("crf", "CLOTHESMAKING", "Clothier", "Clothes Making")},
            {"Milling", new SkillDescription("frm", "MILLING", "Miller", "Milling")},
            {"Processplants", new SkillDescription("plt", "PROCESSPLANTS", "Thresher", "Threshing")},
            {"Cheesemaking", new SkillDescription("mlk", "CHEESEMAKING", "Cheese maker", "Cheese Making")},
            {"Milk", new SkillDescription("mlk", "MILK", "Milker", "Milking")},
            {"Cook", new SkillDescription("cok", "COOK", "Cook", "Cooking")},
            {"Plant", new SkillDescription("frm", "PLANT", "Grower", "Growing")},
            {"Herbalism", new SkillDescription("plt", "HERBALISM", "Herbalist", "Herbalism")},
            {"Fish", new SkillDescription("fsh", "FISH", "Fisherman", "Fishing")},
            {"Smelt", new SkillDescription("mtl", "SMELT", "Furnace operator", "Furnace Operation")},
            {"Extract Strand", new SkillDescription("crf", "EXTRACT_STRAND", "Strand extractor", "Strand Extraction")},
            {"Forge Weapon", new SkillDescription("mtl", "FORGE_WEAPON", "Weaponsmith", "Weaponsmithing")},
            {"Forge Armor", new SkillDescription("mtl", "FORGE_ARMOR", "Armorsmith", "Armorsmithing")},
            {"Forge Furniture", new SkillDescription("mtl", "FORGE_FURNITURE", "Metalsmith", "Metalsmithing")},
            {"Cutgem", new SkillDescription("gem", "CUTGEM", "Gem cutter", "Gem Cutting")},
            {"Encrustgem", new SkillDescription("gem", "ENCRUSTGEM", "Gem setter", "Gem Setting")},
            {"Woodcraft", new SkillDescription("wod", "WOODCRAFT", "Wood crafter", "Wood Crafting")},
            {"Stonecraft", new SkillDescription("stn", "STONECRAFT", "Stone crafter", "Stone Crafting")},
            {"Metalcraft", new SkillDescription("mtl", "METALCRAFT", "Metal crafter", "Metal Crafting")},
            {"Glassmaker", new SkillDescription("gls", "GLASSMAKER", "Glassmaker", "Glassmaking")},
            {"Leatherwork", new SkillDescription("but", "LEATHERWORK", "Leatherworker", "Leatherworkering[sic]")},
            {"Bonecarve", new SkillDescription("but", "BONECARVE", "Bone carver", "Bone Carving")},
            {"Axe", new SkillDescription("mil", "AXE", "Axeman", "Axe")},
            {"Sword", new SkillDescription("mil", "SWORD", "Swordsman", "Sword")},
            {"Dagger", new SkillDescription("mil", "DAGGER", "Knife user", "Knife")},
            {"Mace", new SkillDescription("mil", "MACE", "Maceman", "Mace")},
            {"Hammer", new SkillDescription("mil", "HAMMER", "Hammerman", "Hammer")},
            {"Spear", new SkillDescription("mil", "SPEAR", "Spearman", "Spear")},
            {"Crossbow", new SkillDescription("mil", "CROSSBOW", "Crossbowman", "Crossbow")},
            {"Shield", new SkillDescription("mil", "SHIELD", "Shield user", "Shield")},
            {"Armor", new SkillDescription("mil", "ARMOR", "Armor user", "Armor")},
            {"Siegecraft", new SkillDescription("sig", "SIEGECRAFT", "Siege engineer", "Siege Engineering")},
            {"Siegeoperate", new SkillDescription("sig", "SIEGEOPERATE", "Siege operator", "Siege Operation")},
            {"Bowyer", new SkillDescription("wod", "BOWYER", "Bowyer", "Bowmaking")},
            {"Pike", new SkillDescription("mil", "PIKE", "Pikeman", "Pike")},
            {"Whip", new SkillDescription("mil", "WHIP", "Lasher", "Lash")},
            {"Bow", new SkillDescription("mil", "BOW", "Bowman", "Bow")},
            {"Blowgun", new SkillDescription("mil", "BLOWGUN", "Blowgunner", "Blowgun")},
            {"Throw", new SkillDescription("mil", "THROW", "Thrower", "Throwing")},
            {"Mechanics", new SkillDescription("eng", "MECHANICS", "Mechanic", "Machinery")},
            {"Magic Nature", new SkillDescription("msc", "MAGIC_NATURE", "Druid", "Nature")},
            {"Sneak", new SkillDescription("hnt", "SNEAK", "Ambusher", "Ambush")},
            {"Designbuilding", new SkillDescription("eng", "DESIGNBUILDING", "Building designer", "Building Design")},
            {"Dress Wounds", new SkillDescription("doc", "DRESS_WOUNDS", "Wound dresser", "Wound Dressing")},
            {"Diagnose", new SkillDescription("doc", "DIAGNOSE", "Diagnostician", "Diagnostics")},
            {"Surgery", new SkillDescription("doc", "SURGERY", "Surgeon", "Surgery")},
            {"Set Bone", new SkillDescription("doc", "SET_BONE", "Bone doctor", "Bone Setting")},
            {"Suture", new SkillDescription("doc", "SUTURE", "Suturer", "Suturing")},
            {"Crutch Walk", new SkillDescription("msc", "CRUTCH_WALK", "Crutch-walker", "Crutch-walking")},
            {"Wood Burning", new SkillDescription("cem", "WOOD_BURNING", "Wood burner", "Wood Burning")},
            {"Lye Making", new SkillDescription("cem", "LYE_MAKING", "Lye maker", "Lye Making")},
            {"Soap Making", new SkillDescription("cem", "SOAP_MAKING", "Soaper", "Soap Making")},
            {"Potash Making", new SkillDescription("cem", "POTASH_MAKING", "Potash maker", "Potash Making")},
            {"Dyer", new SkillDescription("crf", "DYER", "Dyer", "Dyeing")},
            {"Operate Pump", new SkillDescription("pmp", "OPERATE_PUMP", "Pump operator", "Pump Operation")},
            {"Swimming", new SkillDescription("msc", "SWIMMING", "Swimmer", "Swimming")},
            {"Persuasion", new SkillDescription("soc", "PERSUASION", "Persuader", "Persuasion")},
            {"Negotiation", new SkillDescription("soc", "NEGOTIATION", "Negotiator", "Negotiation")},
            {"Judging Intent", new SkillDescription("soc", "JUDGING_INTENT", "Judge of intent", "Judging Intent")},
            {"Appraisal", new SkillDescription("soc", "APPRAISAL", "Appraiser", "Appraisal")},
            {"Organization", new SkillDescription("soc", "ORGANIZATION", "Organizer", "Organization")},
            {"Record Keeping", new SkillDescription("soc", "RECORD_KEEPING", "Record keeper", "Record Keeping")},
            {"Lying", new SkillDescription("soc", "LYING", "Liar", "Lying")},
            {"Intimidation", new SkillDescription("soc", "INTIMIDATION", "Intimidator", "Intimidation")},
            {"Conversation", new SkillDescription("soc", "CONVERSATION", "Conversationalist", "Conversation")},
            {"Comedy", new SkillDescription("soc", "COMEDY", "Comedian", "Comedy")},
            {"Flattery", new SkillDescription("soc", "FLATTERY", "Flatterer", "Flattery")},
            {"Console", new SkillDescription("soc", "CONSOLE", "Consoler", "Consoling")},
            {"Pacify", new SkillDescription("soc", "PACIFY", "Pacifier", "Pacification")},
            {"Tracking", new SkillDescription("hnt", "TRACKING", "Tracker", "Tracking")},
            {"Knowledge Acquisition", new SkillDescription("soc", "KNOWLEDGE_ACQUISITION", "Student", "Studying")},
            {"Concentration", new SkillDescription("msc", "CONCENTRATION", "Concentration", "Concentration")},
            {"Discipline", new SkillDescription("mil", "DISCIPLINE", "Discipline", "Discipline")},
            {"Situational Awareness", new SkillDescription("msc", "SITUATIONAL_AWARENESS", "Observer", "Observation")},
            {"Writing", new SkillDescription("soc", "WRITING", "Wordsmith", "Writing")},
            {"Prose", new SkillDescription("soc", "PROSE", "Writer", "Prose")},
            {"Poetry", new SkillDescription("soc", "POETRY", "Poet", "Poetry")},
            {"Reading", new SkillDescription("soc", "READING", "Reader", "Reading")},
            {"Speaking", new SkillDescription("soc", "SPEAKING", "Speaker", "Speaking")},
            {"Coordination", new SkillDescription("msc", "COORDINATION", "Coordination", "Coordination")},
            {"Balance", new SkillDescription("msc", "BALANCE", "Balance", "Balance")},
            {"Leadership", new SkillDescription("soc", "LEADERSHIP", "Leader", "Leadership")},
            {"Teaching", new SkillDescription("soc", "TEACHING", "Teacher", "Teaching")},
            {"Melee Combat", new SkillDescription("mil", "MELEE_COMBAT", "Fighter", "Fighting")},
            {"Ranged Combat", new SkillDescription("mil", "RANGED_COMBAT", "Archer", "Archery")},
            {"Wrestling", new SkillDescription("mil", "WRESTLING", "Wrestler", "Wrestling")},
            {"Bite", new SkillDescription("mil", "BITE", "Biter", "Biting")},
            {"Grasp Strike", new SkillDescription("mil", "GRASP_STRIKE", "Striker", "Striking")},
            {"Stance Strike", new SkillDescription("mil", "STANCE_STRIKE", "Kicker", "Kicking")},
            {"Dodging", new SkillDescription("mil", "DODGING", "Dodger", "Dodging")},
            {"Misc Weapon", new SkillDescription("mil", "MISC_WEAPON", "Misc. object user", "Misc. Object")},
            {"Knapping", new SkillDescription("msc", "KNAPPING", "Knapper", "Knapping")},
            {"Military Tactics", new SkillDescription("mil", "MILITARY_TACTICS", "Military tactics", "Military Tactics")},
            {"Shearing", new SkillDescription("crf", "SHEARING", "Shearer", "Shearing")},
            {"Spinning", new SkillDescription("crf", "SPINNING", "Spinner", "Spinning")},
            {"Pottery", new SkillDescription("pot", "POTTERY", "Potter", "Pottery")},
            {"Glazing", new SkillDescription("pot", "GLAZING", "Glazer", "Glazing")},
            {"Pressing", new SkillDescription("frm", "PRESSING", "Presser", "Pressing")},
            {"Beekeeping", new SkillDescription("bee", "BEEKEEPING", "Beekeeper", "Beekeeping")},
            {"Wax Working", new SkillDescription("bee", "WAX_WORKING", "Wax worker", "Wax Working")},
            {"Climbing", new SkillDescription("msc", "CLIMBING", "Climber", "Climbing")},
            {"Geld", new SkillDescription("hnt", "GELD", "Gelder", "Gelding")},
            {"Dance", new SkillDescription("soc", "DANCE", "Dancer", "Dancing")},
            {"Make Music", new SkillDescription("soc", "MAKE_MUSIC", "Musician", "Music")},
            {"Sing", new SkillDescription("soc", "SING", "Singer", "Singing")},
            {"Play Keyboard Instrument", new SkillDescription("soc", "PLAY_KEYBOARD_INSTRUMENT", "Keyboard Instrumentalist", "Keyboard Instrumentalist")},
            {"Play Stringed Instrument", new SkillDescription("soc", "PLAY_STRINGED_INSTRUMENT", "String Instrumentalist", "String Instrumentalist")},
            {"Play Wind Instrument", new SkillDescription("soc", "PLAY_WIND_INSTRUMENT", "Wind Instrumentalist", "Wind Instrumentalist")},
            {"Play Percussion Instrument", new SkillDescription("soc", "PLAY_PERCUSSION_INSTRUMENT", "Drummer", "Percussionist")},
            {"Critical Thinking", new SkillDescription("msc", "CRITICAL_THINKING", "Critical Thinker", "Critical Thinking")},
            {"Logic", new SkillDescription("msc", "LOGIC", "Logician", "Logic")},
            {"Mathematics", new SkillDescription("msc", "MATHEMATICS", "Mathematician", "Mathematics")},
            {"Astronomy", new SkillDescription("msc", "ASTRONOMY", "Astronomer", "Astronomy")},
            {"Chemistry", new SkillDescription("msc", "CHEMISTRY", "Chemist", "Chemistry")},
            {"Geography", new SkillDescription("msc", "GEOGRAPHY", "Geographer", "Geography")},
            {"Optics Engineer", new SkillDescription("eng", "OPTICS_ENGINEER", "Optician", "Optics Engineer")},
            {"Fluid Engineer", new SkillDescription("eng", "FLUID_ENGINEER", "Fluids Engineer", "Fluid Engineering")},
            {"Papermaking", new SkillDescription("ppr", "PAPERMAKING", "Paper Maker", "Paper Making")},
            {"Bookbinding", new SkillDescription("ppr", "BOOKBINDING", "Book Binder", "Book Binding")},
            {"Intrigue", new SkillDescription("soc", "INTRIGUE", "Schemer", "Scheming")},
            {"Riding", new SkillDescription("msc", "RIDING", "Rider", "Riding")}
        };

        public static SkillDescription LookupSkill(Skill skill)
        {
            SkillDescription desc = new SkillDescription("unk", skill.Name, skill.Name, skill.Name);

            if (Dict.ContainsKey(skill.Name))
            {
                desc = Dict[skill.Name];
            }

            desc.Rank = skill.Rank;
            desc.Points = skill.Points;

            return desc;
        }

        public static bool IsKnownSkill(Skill skill)
        {
            return Dict.ContainsKey(skill.Name);
        }
    }
}
