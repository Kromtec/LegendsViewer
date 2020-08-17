using System.ComponentModel;

namespace LegendsViewer.Legends.Enums
{
    public enum PileType
    {
        [Description("pile")]
        Unknown,
        [Description("grisly mound")]
        GrislyMound,
        [Description("grotesque pillar")]
        GrotesquePillar,
        [Description("gruesome sculpture")]
        GruesomeSculpture
    }
}
