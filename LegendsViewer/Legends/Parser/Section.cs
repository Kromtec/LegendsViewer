using System.ComponentModel;

namespace LegendsViewer.Legends.Parser
{
    // !!! In order as they appear in the XML. !!!
    public enum Section
    {
        Unknown,
        Junk,
        Landmasses,
        [Description("Mountain Peaks")]
        MountainPeaks,
        Regions,
        [Description("Underground Regions")]
        UndergroundRegions,
        Rivers,
        CreatureRaw,
        Sites,
        [Description("World Constructions")]
        WorldConstructions,
        Artifacts,
        [Description("Historical Figures")]
        HistoricalFigures,
        Identities,
        [Description("Entity Populations")]
        EntityPopulations,
        Entities,
        Events,
        [Description("Event Collections")]
        EventCollections,
        [Description("Historical Event Relationships")]
        HistoricalEventRelationships,
        HistoricalEventRelationshipSupplement,
        Eras,
        [Description("Written Content")]
        WrittenContent,
        [Description("Poetic Forms")]
        PoeticForms,
        [Description("Musical Forms")]
        MusicalForms,
        [Description("Dance Forms")]
        DanceForms,
    }
    // !!! In order as they appear in the XML. !!!
}
