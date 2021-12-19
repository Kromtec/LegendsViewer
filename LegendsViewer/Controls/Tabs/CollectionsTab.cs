using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Windows.Forms;
using LegendsViewer.Controls.HTML;
using LegendsViewer.Legends;

namespace LegendsViewer.Controls.Tabs
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class CollectionsTab : BaseSearchTab
    {
        public CollectionsTab()
        {
            InitializeComponent();
        }

        internal override void InitializeTab()
        {
            EventTabs = new[] { tpEraEvents };
            EventTabTypes = new[] { typeof(Era) };

            listEraSearch.ShowGroups = false;
        }

        internal override void AfterLoad(World world)
        {
            base.AfterLoad(world);

            listEraSearch.SetObjects(World.Eras.ToArray());

            numStart.Maximum = numEraEnd.Value = numEraEnd.Maximum = World.Events.Last().Year;

            Coordinator.Form.DontRefreshBrowserPages = true;
            foreach (CheckBox eraCheck in tpEraEvents.Controls.OfType<CheckBox>())
            {
                eraCheck.Checked = false;
            }

            Coordinator.Form.DontRefreshBrowserPages = false;

            var eventTypes = from eventType in World.Events
                             group eventType by eventType.Type into type
                             select type.Key;

            TabEvents.Clear();
            TabEvents.Add(eventTypes.ToList());
        }

        internal override void DoSearch()
        {
            base.DoSearch();
        }

        internal override void ResetTab()
        {
            Coordinator.Form.DontRefreshBrowserPages = true;
            foreach (CheckBox eraCheck in tpEraEvents.Controls.OfType<CheckBox>())
            {
                eraCheck.Checked = false;
            }

            Coordinator.Form.DontRefreshBrowserPages = false;

            listEraSearch.Items.Clear();
            numStart.Value = -1;
            numEraEnd.Value = 0;
        }

        private void UpdateCounts(Label label, int shown, int total)
        {
            label.Text = $"{shown} / {total}";
        }

        private void btnEraShow_Click(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                Browser.Navigate(ControlOption.Html, new Era(Convert.ToInt32(numStart.Value), Convert.ToInt32(numEraEnd.Value), World));
            }
        }

        private void listEras_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }
    }
}
