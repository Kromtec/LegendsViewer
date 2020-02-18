namespace LegendsViewer.Legends
{
    public class Population
    {
        public bool IsMainRace
        {
            get
            {
                return World.MainRaces.ContainsKey(Race);
            }
        }

        public bool IsOutcasts
        {
            get
            {
                return Race.NamePlural.Contains("Outcasts");
            }
        }

        public bool IsPrisoners
        {
            get
            {
                return Race.NamePlural.Contains("Prisoners");
            }
        }

        public bool IsSlaves
        {
            get
            {
                return Race.NamePlural.Contains("Slaves");
            }
        }

        public bool IsVisitors
        {
            get
            {
                return Race.NamePlural.Contains("Visitors");
            }
        }

        public bool IsAnimalPeople
        {
            get
            {
                return Race.NamePlural.Contains(" Men") && !IsSlaves && !IsPrisoners && !IsOutcasts && !IsVisitors;
            }
        }

        public CreatureInfo Race { get; set; }
        public int Count { get; set; }

        public Population(CreatureInfo type, int count)
        {
            Race = type;
            Count = count;
        }
    }
}
