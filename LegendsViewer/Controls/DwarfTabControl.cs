using System;
using System.Linq;
using System.Windows.Forms;
using LegendsViewer.Controls.Chart;
using LegendsViewer.Controls.HTML;
using LegendsViewer.Controls.Map;
using LegendsViewer.Controls.Query;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls
{
    public class DwarfTabControl : TabControlImproved
    {
        public World World { get; set; }
        private readonly bool _newTab = false;

        public DwarfTabControl(World world)
        {
            World = world;
        }

        public void Navigate(ControlOption controlOption, object navigateObject = null)
        {
            PageControl newControl = null;
            switch (controlOption)
            {
                case ControlOption.EventOverview:
                case ControlOption.Html:
                    if (navigateObject != null)
                    {
                        newControl = new HtmlControl(navigateObject, this, World, controlOption);
                    }
                    break;
                case ControlOption.Chart:
                    newControl = new ChartControl(World, navigateObject as DwarfObject, this);
                    break;
                case ControlOption.Map:
                    newControl = new MapControl(World, navigateObject, this);
                    break;
                case ControlOption.Search:
                    newControl = new SearchControl(this);
                    break;
                case ControlOption.ReadMe:
                    newControl = new ReadMeControl(this);
                    break;
            }

            if (newControl != null)
            {
                if (MakeNewTab())
                {
                    TabPages.Add(new DwarfTabPage(newControl));
                }
                else
                {
                    DwarfTabPage dwarfTabPage = SelectedTab as DwarfTabPage;
                    dwarfTabPage?.NewPageControl(newControl);
                }
            }
        }

        private bool MakeNewTab()
        {
            return TabPages.Count == 0 || ModifierKeys == Keys.Control || _newTab;
        }

        public void Back()
        {
            (SelectedTab as DwarfTabPage)?.Back();
        }

        public void Forward()
        {
            (SelectedTab as DwarfTabPage)?.Forward();
        }

        protected override void CloseTab(int index = -1)
        {
            if (SelectedIndex == -1)
            {
                return;
            }
            if (index == -1)
            {
                index = SelectedIndex;
            }
            if (TabPages.Count > 0)
            {
                var dwarfTabPage = TabPages[index];

                int newSelectedIndex = SelectedIndex;
                if (index <= SelectedIndex)
                {
                    newSelectedIndex = SelectedIndex - 1;
                }

                TabPages.Remove(dwarfTabPage);
                dwarfTabPage.Dispose();

                SelectedIndex = TabPages.Count > 0 && newSelectedIndex == -1 ? 0 : newSelectedIndex;
            }
        }

        public void RefreshAll(Type refreshType)
        {
            foreach (DwarfTabPage page in TabPages.OfType<DwarfTabPage>())
            {
                if (page.Current.GetType() == typeof(HtmlControl) && ((HtmlControl)page.Current).HtmlObject.GetType() == refreshType)
                {
                    page.Current.Refresh();
                }
                else if (page.Current.GetType() == typeof(ChartControl) && ((ChartControl)page.Current).FocusObject.GetType() == refreshType)
                {
                    page.Current.Refresh();
                }
            }
        }

        public void Reset()
        {
            while (TabPages.Count > 0)
            {
                CloseTab();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg > 512 && m.Msg <= 528)
            {
                switch (m.WParam.ToInt64())
                {
                    case 66059:
                    case (int)MouseButtons.XButton1:
                        Back();
                        break;
                    case 131595:
                    case (int)MouseButtons.XButton2:
                        Forward();
                        break;
                }
            }
            base.WndProc(ref m);
        }
    }
}