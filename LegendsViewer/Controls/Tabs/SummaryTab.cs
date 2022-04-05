using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LegendsViewer.Controls.HTML;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;

namespace LegendsViewer.Controls.Tabs
{
    public partial class SummaryTab : BaseSearchTab
    {
        public SummaryTab()
        {
            InitializeComponent();
        }

        public FileLoader CreateLoader()
        {
            return new FileLoader(
                  btnXML, txtXML
                , btnHistory, txtHistory
                , btnSitePops, txtSitePops
                , btnMap, txtMap
                , btnXMLPlus, txtXMLPlus
                , lblStatus, txtLog);
        }

        internal override void InitializeTab()
        {
        }

        internal override void AfterLoad(World world)
        {
            base.AfterLoad(world);
            txtLog.AppendText(World.Log.ToString());
            lblStatus.Text = "Done!";
            lblStatus.ForeColor = Color.Green;
        }

        internal override void ResetTab()
        {
            lblStatus.Text = "Setup...";
            lblStatus.ForeColor = Color.Blue;

            txtXML.Text = "Legends XML / Archive";
            txtHistory.Text = "World History Text";
            txtSitePops.Text = "Sites and Populations Text";
            txtMap.Text = "Map Image";
            txtLog.Clear();
        }

        private void btnShowMap_Click(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                Browser.Navigate(ControlOption.Map);
                ((Browser.SelectedTab as DwarfTabPage)?.Current.GetControl() as MapPanel)?.ToggleCivs();
            }
        }

        private void btnStats_Click(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                Browser.Navigate(ControlOption.Html, World);
            }
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                Browser.Navigate(ControlOption.Chart, new Era(-1, World.Events.Last().Year, World));
            }
        }
        private void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                Browser.Navigate(ControlOption.Search);
            }
        }

        private void txtLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (txtLog.SelectedText != "")
                {
                    Clipboard.SetText(txtLog.SelectedText);
                }
            }
        }
    }
}
