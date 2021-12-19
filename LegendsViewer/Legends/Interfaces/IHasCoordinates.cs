using System.Collections.Generic;

namespace LegendsViewer.Legends.Interfaces
{
    internal interface IHasCoordinates
    {
        List<Location> Coordinates { get; set; } // legends_plus.xml
    }
}
