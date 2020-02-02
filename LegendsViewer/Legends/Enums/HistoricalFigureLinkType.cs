using System.ComponentModel;

namespace LegendsViewer.Legends.Enums
{
    public enum HistoricalFigureLinkType
    {
        Unknown,
        Apprentice,
        Master,
        [Description("Former Apprentice")]
        FormerApprentice,
        [Description("Former Master")]
        FormerMaster,
        Child,
        Deity,
        Father,
        Lover,
        Mother,
        Spouse,
        Imprisoner,
        Prisoner, //Not found in XML, used by AddHFHFLink event
        [Description("Ex-Spouse")]
        ExSpouse,
        [Description("Traveling Companion")]
        Companion,
        [Description("Pet Owner")]
        PetOwner,
        [Description("Former Spouse")]
        FormerSpouse,
        [Description("Deceased Spouse")]
        DeceasedSpouse
    }
}