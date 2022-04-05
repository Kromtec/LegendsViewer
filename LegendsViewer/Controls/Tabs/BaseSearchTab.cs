using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using LegendsViewer.Controls.HTML;
using LegendsViewer.Legends;

namespace LegendsViewer.Controls.Tabs
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class BaseSearchTab : UserControl
    {
        private LvCoordinator _coordinator;

        protected TabPage[] EventTabs = new TabPage[0];
        protected Type[] EventTabTypes = new Type[0];
        protected List<List<string>> TabEvents = new List<List<string>>();

        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LvCoordinator Coordinator
        {
            get => _coordinator;
            set
            {
                _coordinator = value;
                if (value != null)
                {
                    InitializeTab();
                    SetupGeneralListViewEvents();
                }
            }
        }

        internal DwarfTabControl Browser => Coordinator?.Browser;
        internal World World => Coordinator?.World;
        internal FileLoader FileLoader => Coordinator?.FileLoader;

        public BaseSearchTab()
        {
            InitializeComponent();
        }

        internal virtual void InitializeTab() { }

        internal virtual void AfterLoad(World world) { }

        internal virtual void ResetTab() { }

        internal virtual void DoSearch() { }

        internal virtual void ResetEvents()
        {
            RemoveEventFilterCheckBoxes();
            TabEvents?.Clear();
        }

        internal ObjectListView ListView { get; set; }
        internal static List<LinkLabel> MaxResultsLabels = new List<LinkLabel>();

        private void SetupGeneralListViewEvents()
        {
            if (ListView == null || Coordinator == null)
            {
                return;
            }

            ListView.SelectionChanged += delegate
            {
                Coordinator.HandleSelectionChanged(ListView);
            };

            ListView.HotItemChanged += delegate (object sender, HotItemChangedEventArgs args)
            {
                Coordinator.HandleHotItemChanged(sender, args);
            };

            ListView.GroupTaskClicked += delegate (object sender, GroupTaskClickedEventArgs args)
            {
                Coordinator.ShowMessage("Clicked on group task: " + args.Group.Name);
            };

            ListView.GroupStateChanged += delegate (object sender, GroupStateChangedEventArgs e)
            {
                Debug.WriteLine("Group '{0}' was {1}{2}{3}{4}{5}{6}", e.Group.Header, e.Selected ? "Selected" : "", e.Focused ? "Focused" : "", e.Collapsed ? "Collapsed" : "", e.Unselected ? "Unselected" : "", e.Unfocused ? "Unfocused" : "", e.Uncollapsed ? "Uncollapsed" : "");
            };
        }

        protected void ListSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ObjectListView view)
            {
                Browser.Navigate(ControlOption.Html, view.SelectedObject);
            }

            if (sender is ListBox box)
            {
                Browser.Navigate(ControlOption.Html, box.SelectedItem);
            }
        }

        protected internal void GenerateEventFilterCheckBoxes()
        {
            Array.Sort(AppHelpers.EventInfo, delegate (string[] a, string[] b)
            {
                return string.Compare(a[1], b[1]);
            });
            for (int eventTab = 0; eventTab < EventTabs.Length; eventTab++)
            {
                int count = 0;
                TabEvents[eventTab].Sort((a, b) => AppHelpers.EventInfo[Array.IndexOf(AppHelpers.EventInfo, AppHelpers.EventInfo.Single(eventInfo => eventInfo[0] == a))][1].CompareTo(AppHelpers.EventInfo[Array.IndexOf(AppHelpers.EventInfo, AppHelpers.EventInfo.Single(eventInfo => eventInfo[0] == b))][1]));
                foreach (string eventType in TabEvents[eventTab])
                {
                    CheckBox eventCheck = new CheckBox();
                    EventTabs[eventTab].Controls.Add(eventCheck);
                    string[] eventInfo = AppHelpers.EventInfo.Single(a => a[0] == eventType);
                    eventCheck.Text = eventInfo[1];
                    eventCheck.Checked = EventTabs[eventTab].Parent.Name != "tcEras";
                    eventCheck.CheckedChanged += OnEventFilterCheck;
                    hint.SetToolTip(eventCheck, eventInfo[2]);
                    eventCheck.Location = new Point(10, 23 * count);
                    eventCheck.Width = 235;
                    count++;
                }
                Button btnAll = new Button
                {
                    Text = "Select All",
                    Location = new Point(10, 23 * count)
                };
                btnAll.Click += SelectAllEventCheckBoxes;
                EventTabs[eventTab].Controls.Add(btnAll);
                Button btnNone = new Button
                {
                    Text = "Deselect All",
                    Location = new Point(90, 23 * count)
                };
                btnNone.Click += SelectAllEventCheckBoxes;
                EventTabs[eventTab].Controls.Add(btnNone);
            }
        }

        protected void RemoveEventFilterCheckBoxes()
        {
            foreach (TabPage eventTab in EventTabs)
            {
                eventTab.Controls.Clear();
            }
        }

        private void OnEventFilterCheck(object sender, EventArgs e)
        {
            CheckBox eventCheck = sender as CheckBox;
            if (!FileLoader.Working && World != null)
            {
                foreach (string[] eventInfo in AppHelpers.EventInfo.Where(a => a[1] == eventCheck.Text))
                {
                    int eventPageIndex = Array.IndexOf(EventTabs, eventCheck.Parent);
                    List<string> eventFilter = EventTabTypes[eventPageIndex].GetField("Filters").GetValue(null) as List<string>;
                    if (eventCheck.Checked)
                    {
                        eventFilter.Remove(eventInfo[0]);
                    }
                    else
                    {
                        eventFilter.Add(eventInfo[0]);
                    }

                    if (!Coordinator.Form.DontRefreshBrowserPages)
                    {
                        Browser.RefreshAll(EventTabTypes[eventPageIndex]);
                    }
                }
            }
            else
            {
                eventCheck.CheckedChanged -= OnEventFilterCheck;
                eventCheck.Checked = true;
                eventCheck.CheckedChanged += OnEventFilterCheck;
            }
        }

        private void SelectAllEventCheckBoxes(object sender, EventArgs e)
        {
            Button selectButton = sender as Button;
            Coordinator.Form.DontRefreshBrowserPages = true;
            foreach (CheckBox checkEvent in selectButton.Parent.Controls.OfType<CheckBox>())
            {
                if (selectButton.Text == "Select All")
                {
                    checkEvent.Checked = true;
                }

                if (selectButton.Text == "Deselect All")
                {
                    checkEvent.Checked = false;
                }
            }
            Browser.RefreshAll(EventTabTypes[Array.IndexOf(EventTabs, selectButton.Parent)]);
            Coordinator.Form.DontRefreshBrowserPages = false;
        }
    }
}
