using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Interfaces
{
    interface IFeatured
    {
        string PrintFeature(bool link = true, DwarfObject pov = null);
    }
}
