using System.ComponentModel;

namespace LegendsViewer.Legends.Enums
{
    public enum SiteType
    {
        Unknown,
        Cave,
        Fortress,
        [Description("Forest Retreat")]
        ForestRetreat,
        [Description("Dark Fortress")]
        DarkFortress,
        Town,
        Hamlet,
        Vault,
        [Description("Dark Pits")]
        DarkPits,
        Hillocks,
        Tomb,
        Tower,
        [Description("Mountain Halls")]
        MountainHalls,
        Camp,
        Lair,
        Labyrinth,
        Shrine,
        [Description("Important Location")]
        ImportantLocation,
        Fort,
        Monastery,
        Castle
    }
}
