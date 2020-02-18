using System.ComponentModel;

namespace LegendsViewer.Legends.Enums
{
    public enum EntityType // legends_plus.xml
    {
        Unknown,
        Civilization,
        [Description("Nomads")]
        NomadicGroup,
        [Description("Migrants")]
        MigratingGroup,
        [Description("Outcasts")]
        Outcast,
        [Description("Religion")]
        Religion,
        [Description("Government")]
        SiteGovernment,
        [Description("Performers")]
        PerformanceTroupe,
        [Description("Mercenary Company")]
        MercenaryCompany,
        Guild,
        [Description("Military Unit")]
        MilitaryUnit
    }
}