using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public class OwnerPeriod
    {
        public readonly Site Site;
        public HistoricalFigure Founder;
        public Entity Owner;
        public Entity Ender;
        public HistoricalFigure Destroyer;
        public readonly int StartYear;
        public int EndYear;
        public string StartCause;
        public string EndCause;

        public OwnerPeriod(Site site, Entity owner, int startYear, string startCause, HistoricalFigure founder = null)
        {
            Site = site;
            Owner = owner;
            StartYear = startYear;
            StartCause = startCause;
            EndYear = -1;

            Founder = founder;

            Owner?.AddOwnedSite(this);
        }
    }
}
