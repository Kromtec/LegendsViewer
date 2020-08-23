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
        Grudge,
        [Description("Jealous Relationship Grudge")]
        JealousRelationshipGrudge,
        [Description("Scholar Buddy")]
        ScholarBuddy,
        [Description("Business Rival")]
        BusinessRival,
        [Description("Athletic Rival")]
        AthleticRival,
        Lover,
        [Description("Former Lover")]
        FormerLover,
        Lieutenant
    }
}
