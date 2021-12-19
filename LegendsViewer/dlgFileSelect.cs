using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LegendsViewer
{
    public partial class DlgFileSelect : Form
    {
        public string SelectedFile = "";
        public DlgFileSelect(List<string> files)
        {
            InitializeComponent();
            List<MapFile> maps = new List<MapFile>();
            files.ForEach(file => maps.Add(new MapFile(file)));
            maps = maps.OrderBy(map => map.Order).ToList();
            maps.ForEach(map => listFiles.Items.Add(map));
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (listFiles.SelectedIndex >= 0)
            {
                SelectedFile = (listFiles.SelectedItem as MapFile).FileName;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            listFiles.SelectedIndex = -1;
            SelectedFile = "";
            Close();
        }
    }

    public class MapFile
    {
        public string FileName, MapName;
        public int Order;
        public MapFile(string file)
        {
            if (file.Contains("-el-")) { MapName = "Elevations including lake and ocean floors"; Order = 3; }
            else if (file.Contains("-elw-")) { MapName = "Elevations respecting water level"; Order = 4; }
            else if (file.Contains("-bm-")) { MapName = "Biome"; Order = 5; }
            else if (file.Contains("-hyd-")) { MapName = "Hydrosphere"; Order = 6; }
            else if (file.Contains("-tmp-")) { MapName = "Temperature"; Order = 7; }
            else if (file.Contains("-rain-")) { MapName = "Rainfall"; Order = 8; }
            else if (file.Contains("-drn-")) { MapName = "Drainage"; Order = 9; }
            else if (file.Contains("-sav-")) { MapName = "Savagery"; Order = 10; }
            else if (file.Contains("-vol-")) { MapName = "Volcanism"; Order = 11; }
            else if (file.Contains("-veg-")) { MapName = "Current vegatation"; Order = 12; }
            else if (file.Contains("-evil-")) { MapName = "Evil"; Order = 13; }
            else if (file.Contains("-sal-")) { MapName = "Salinity"; Order = 14; }
            else if (file.Contains("-str-")) { MapName = "Structures/fields/roads/etc."; Order = 15; }
            else if (file.Contains("-trd-")) { MapName = "Trade"; Order = 16; }
            else if (file.StartsWith("world_graphic-")) { MapName = "Standard biome+site map"; Order = 1; }
            else if (file.StartsWith("world_map-")) { MapName = "ASCII / Tile Map"; Order = 2; }
            else { MapName = file; Order = 17; }

            FileName = file;
        }

        public override string ToString()
        {
            return MapName;
        }
    }
}
