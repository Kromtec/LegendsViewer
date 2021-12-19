using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using LegendsViewer.Controls;
using LegendsViewer.Controls.Chart;
using LegendsViewer.Controls.HTML;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Controls.Tabs;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer
{
    public partial class FrmLegendsViewer : Form
    {
        private readonly string _version = "n/a";
        internal readonly DwarfTabControl Browser;
        internal bool DontRefreshBrowserPages = true;
        private readonly string _commandFile;

        internal World World { get; private set; }

        internal FileLoader FileLoader { get; }

        private LvCoordinator Coordinator { get; set; }

        public FrmLegendsViewer(string file = "")
        {
            InitializeComponent();

            // Start local http server
            LocalFileProvider.Run();

            Coordinator = new LvCoordinator(this);

            FileLoader = summaryTab1.CreateLoader();
            FileLoader.AfterLoad += (sender, args) => AfterLoad(args.Arg);

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            _version = fvi.FileVersion;
            var versionNumbers = _version.Split('.');
            if (versionNumbers.Length > 3)
            {
                _version = $"{versionNumbers[0]}.{versionNumbers[1]}.{versionNumbers[2]}";
            }

            Text = "Legends Viewer";
            lblVersion.Text = "v" + _version;
            lblVersion.Left = scWorld.Panel2.ClientSize.Width - lblVersion.Width - 3;
            tcWorld.Height = scWorld.Panel2.ClientSize.Height;

            Browser = new DwarfTabControl(World)
            {
                Location = new Point(0, btnBack.Bottom + 3)
            };
            Browser.Size = new Size(scWorld.Panel2.ClientSize.Width - Browser.Left, scWorld.Panel2.ClientSize.Height - Browser.Top);
            Browser.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            scWorld.Panel2.Controls.Add(Browser);
            foreach (TabPage tp in tcWorld.TabPages)
            {
                foreach (TabControl tabControl in tp.Controls.OfType<TabControl>())
                {
                    HideTabControlBorder(tabControl);
                }
            }

            if (file != "")
            {
                _commandFile = file;
            }

            foreach (var v in tcWorld.TabPages.OfType<TabPage>().SelectMany(x => x.Controls.OfType<BaseSearchTab>()))
            {
                v.Coordinator = Coordinator;
            }

            BrowserUtil.SetBrowserEmulationMode();
            Browser.Navigate(ControlOption.ReadMe);
        }

        private void FrmLegendsViewer_Shown(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_commandFile))
            {
                FileLoader.AttemptLoadFrom(_commandFile);
            }
        }

        private void HideTabControlBorder(TabControl tc)
        {
            Size tcSize = tc.Size;
            tc.Dock = DockStyle.None;
            tc.Size = tcSize;
            tc.Location = new Point(0, 0);
            tc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            tc.Left -= 3;
            tc.Top -= 2;
            tc.Width += 6;
            tc.Height += 4;
            foreach (TabPage tp in tc.TabPages)
            {
                foreach (var tabControl in tp.Controls.OfType<TabControl>())
                {
                    HideTabControlBorder(tabControl);
                }
            }
        }

        private void AfterLoad(World loadedWorld)
        {
            if (!FileLoader.Working && World != null)
            {
                World.Dispose();
                foreach (Entity entity in World.Entities)
                {
                    entity.Identicon?.Dispose();
                }
                World = null;
            }

            World = loadedWorld;

            ResetForm();
            Application.DoEvents();

            Browser.World = World;
            Text += " - " + World.Name;
            Browser.Navigate(ControlOption.Html, World);

            foreach (var v in tcWorld.TabPages.OfType<TabPage>().SelectMany(x => x.Controls.OfType<BaseSearchTab>()))
            {
                v.AfterLoad(loadedWorld);
            }

            dlgOpen.FileName = "";

            foreach (var v in tcWorld.TabPages.OfType<TabPage>().SelectMany(x => x.Controls.OfType<BaseSearchTab>()))
            {
                v.GenerateEventFilterCheckBoxes();
            }

            FileLoader.Working = false;
        }

        private void ResetForm()
        {
            Text = "Legends Viewer";
            foreach (var v in tcWorld.TabPages.OfType<TabPage>().SelectMany(x => x.Controls.OfType<BaseSearchTab>()))
            {
                v.ResetTab();
                v.ResetEvents();
            }
            Browser?.Reset();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Browser?.Back();
        }

        private void BtnForward_Click(object sender, EventArgs e)
        {
            Browser?.Forward();
        }

        private void BtnStats_Click(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                Browser?.Navigate(ControlOption.Html, World);
            }
        }

        private void BtnMap_Click(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                Browser?.Navigate(ControlOption.Map);
            }
        }

        private void BtnChart_Click(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                Browser?.Navigate(ControlOption.Chart, new Era(-1, World.Events.Last().Year, World));
            }
        }

        private void FrmLegendsViewer_ResizeEnd(object sender, EventArgs e)
        {
            foreach (var chart in Browser.TabPages.OfType<DwarfTabPage>().Select(x => x.Current).OfType<ChartControl>())
            {
                chart.DwarfChart.RefreshAllSeries();
            }
        }

        public void ChangeBattleBaseList(List<Battle> battles, string mapBattles)
        {
            var tab = tpWarfare.Controls.OfType<WarfareTab>().FirstOrDefault();
            tab?.ChangeBattleBaseList(battles, mapBattles);

            tcWorld.SelectedTab = tpWarfare;
        }

        private void Open_ReadMe(object sender, EventArgs e)
        {
            Browser.Navigate(ControlOption.ReadMe);
        }

        private void FrmLegendsViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            World?.Dispose();
            LocalFileProvider.Stop();
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            (sender as TabControl)?.SelectedTab?.Controls.OfType<BaseSearchTab>().FirstOrDefault()?.DoSearch();
        }

        private void btnAdvancedSearchClick(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                Browser.Navigate(ControlOption.Search);
            }
        }
    }
}
