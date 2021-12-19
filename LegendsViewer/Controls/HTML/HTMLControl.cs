using System;
using System.Diagnostics;
using System.Windows.Forms;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    public class HtmlControl : PageControl
    {
        private readonly World _world;
        public WebBrowser HtmlBrowser;
        private readonly HtmlPrinter _printer;
        public int BrowserScrollPosition;
        public object HtmlObject;

        public HtmlControl(object htmlObject, DwarfTabControl tabControl, World world, ControlOption controlOption)
        {
            _world = world;
            HtmlObject = htmlObject;
            _printer = HtmlPrinter.GetPrinter(htmlObject, world, controlOption);
            Title = _printer.GetTitle();
            TabControl = tabControl;
        }

        ~HtmlControl()
        {
            Dispose(false);
        }

        public override Control GetControl()
        {
            if (HtmlBrowser?.IsDisposed != false)
            {
                _lastNav = DateTime.UtcNow;
                BrowserUtil.SetBrowserEmulationMode();
                HtmlBrowser = new WebBrowser
                {
                    Dock = DockStyle.Fill,
                    WebBrowserShortcutsEnabled = false,
                    ScriptErrorsSuppressed = true
                };
                HtmlBrowser.Navigate("about:blank");
                while (HtmlBrowser.Document?.Body == null)
                {
                    Application.DoEvents();
                }
                HtmlBrowser.DocumentText = _printer.GetHtmlPage();
                HtmlBrowser.DocumentCompleted += AfterPageLoad;
                HtmlBrowser.Navigating += BrowserNavigating;
                HtmlBrowser.Document.MouseMove += MouseOver;
                return HtmlBrowser;
            }
            return HtmlBrowser;
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    DisposePrinter();
                    if (HtmlBrowser?.Document != null)
                    {
                        int newerBrowsers = 0;
                        try
                        {
                            newerBrowsers = HtmlBrowser.Document.GetElementsByTagName("HTML")[0].ScrollTop;
                        }
                        catch (Exception)
                        {
                        }
                        BrowserScrollPosition = newerBrowsers > HtmlBrowser.Document.Body?.ScrollTop
                            ? newerBrowsers
                            : HtmlBrowser.Document.Body.ScrollTop;

                        HtmlBrowser.Dispose();
                        HtmlBrowser = null;
                    }
                }
                base.Dispose(disposing);
                Disposed = true;
            }
        }

        public override void Refresh()
        {
            HtmlBrowser.Navigate("about:blank");
            while (HtmlBrowser.Document == null || HtmlBrowser.Document.Body == null)
            {
                Application.DoEvents();
            }

            HtmlBrowser.DocumentText = _printer.GetHtmlPage();
        }

        private DateTime _lastNav;
        private void AfterPageLoad(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            (sender as WebBrowser).Document.Window.ScrollTo(0, BrowserScrollPosition);
            (sender as WebBrowser).Focus();
            GC.Collect();
        }

        private void MouseOver(object sender, HtmlElementEventArgs e)
        {
            if (TabControl.Parent.ContainsFocus)
            {
                HtmlBrowser.Document.Focus();
            }
        }

        private void BrowserNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.ToString() != "about:blank")
            {
                string url = e.Url.ToString();
                url = url.Substring(url.IndexOf(":") + 1, url.Length - url.IndexOf(":") - 1); //remove "about:" at the beginning of the url
                if (!NavigateToNewControl(url))
                {
                    if (!NavigateToNewObjectPage(url))
                    {
                        throw new Exception("Could not navigate with url: " + url);
                    }
                }

                e.Cancel = true; //Prevent the browser from actually navigating to a new page
            }
        }

        private bool NavigateToNewControl(string url)
        {
            if (Enum.TryParse(url, out LinkOption option))
            {
                switch (option)
                {
                    case LinkOption.LoadMap:
                        TabControl.Navigate(ControlOption.Map, HtmlObject);
                        break;
                    case LinkOption.LoadChart:
                        TabControl.Navigate(ControlOption.Chart, HtmlObject);
                        break;
                    case LinkOption.LoadEventOverview:
                        TabControl.Navigate(ControlOption.EventOverview, HtmlObject);
                        break;
                    case LinkOption.LoadSiteMap:
                        if (HtmlObject is Site currentSite)
                        {
                            Process.Start(currentSite.SiteMapPath);
                        }
                        break;
                }
                return true;
            }
            return false;
        }

        private bool NavigateToNewObjectPage(string url)
        {
            if (url.Contains("#"))
            {
                int id = Convert.ToInt32(url.Substring(url.IndexOf("#") + 1));
                string objectType = url.Substring(0, url.IndexOf("#"));
                object navigateObject = null;
                switch (objectType)
                {
                    case "hf":
                        navigateObject = _world.GetHistoricalFigure(id); break;
                    case "region":
                        navigateObject = _world.GetRegion(id); break;
                    case "uregion":
                        navigateObject = _world.GetUndergroundRegion(id); break;
                    case "site":
                        navigateObject = _world.GetSite(id); break;
                    case "entity":
                        navigateObject = _world.GetEntity(id); break;
                    case "collection":
                        navigateObject = _world.GetEventCollection(id); break;
                    case "era":
                        navigateObject = _world.GetEra(id); break;
                    case "artifact":
                        navigateObject = _world.GetArtifact(id); break;
                    case "worldconstruction":
                        navigateObject = _world.GetWorldConstruction(id); break;
                    case "writtencontent":
                        navigateObject = _world.GetWrittenContent(id); break;
                    case "danceform":
                        navigateObject = _world.GetDanceForm(id); break;
                    case "musicalform":
                        navigateObject = _world.GetMusicalForm(id); break;
                    case "poeticform":
                        navigateObject = _world.GetPoeticForm(id); break;
                    case "structure":
                        navigateObject = _world.GetStructure(id); break;
                    default: throw new Exception("Unhandled url type: " + objectType);
                }
                TabControl.Navigate(ControlOption.Html, navigateObject);
                return true;
            }
            return false;
        }

        //e.Cancel = true;
        private void DisposePrinter()
        {
            _printer.DeleteTemporaryFiles();
            _printer.Dispose();
        }
    }
}
