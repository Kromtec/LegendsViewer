using LegendsViewer.Controls.Query.Attributes;

namespace LegendsViewer.Legends
{
    public class Population
    {
        public bool IsMainRace => World.MainRaces.ContainsKey(Race);

        public bool IsOutcasts => Race.NamePlural.Contains("Outcasts");

        public bool IsPrisoners => Race.NamePlural.Contains("Prisoners");

        public bool IsSlaves => Race.NamePlural.Contains("Slaves");

        public bool IsVisitors => Race.NamePlural.Contains("Visitors");

        public bool IsAnimalPeople => Race.NamePlural.Contains(" Men") && !IsSlaves && !IsPrisoners && !IsOutcasts && !IsVisitors;

        [AllowAdvancedSearch]
        public CreatureInfo Race { get; set; }
        [AllowAdvancedSearch]
        public int Count { get; set; }

        public Population(CreatureInfo type, int count)
        {
            Race = type;
            Count = count;
        }
    }
}
