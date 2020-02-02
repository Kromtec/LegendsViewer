using System.ComponentModel;

namespace LegendsViewer.Legends.Enums
{
    public enum VagueRelationshipType
    {
        Unknown,
        [Description("War Buddy")]
        WarBuddy,
        [Description("Athlete Buddy")]
        AthleteBuddy,
        [Description("Childhood Friend")]
        ChildhoodFriend,
        [Description("Persecution Grudge")]
        PersecutionGrudge,
        [Description("Supernatural Grudge")]
        SupernaturalGrudge,
        [Description("Religious Persecution Grudge")]
        ReligiousPersecutionGrudge,
        [Description("Artistic Buddy")]
        ArtisticBuddy,
        [Description("Jealous Obsession")]
        JealousObsession,
        Grudge
    }
}
