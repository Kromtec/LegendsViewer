using System.ComponentModel;

namespace LegendsViewer.Legends.Enums
{
    public enum SiteLinkType
    {
        Unknown,
        Lair,
        Hangout,
        [Description("Home - Site Building")]
        HomeSiteBuilding,
        [Description("Home - Site Underground")]
        HomeSiteUnderground,
        [Description("Home - Structure")]
        HomeStructure,
        [Description("Seat of Power")]
        SeatOfPower,
        Occupation,
        [Description("Home - Site Realization Building")]
        HomeSiteRealizationBuilding,
        [Description("Home - Site Abstract Building")]
        HomeSiteAbstractBuilding,
        [Description("Prison - Site Building")]
        PrisonSiteBuildingProfile
    }
}