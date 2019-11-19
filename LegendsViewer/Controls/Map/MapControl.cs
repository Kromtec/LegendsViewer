using System;
using System.Drawing;
using System.Windows.Forms;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.Map
{
    public class MapControl : PageControl, IDisposable
    {
        public MapPanel MapPanel;
        public double MapScale;
        public int CurrentYear;
        public Point Center;
        public bool CivsToggled, SitesToggled, WarsToggled, BattlesToggled;
        public object FocusObject;
        public World World;

        public MapControl(World world, object focusObject, DwarfTabControl dwarfTabControl)
        {
            Title = "Map";
            if (focusObject != null && focusObject is DwarfObject)
            {
                Title += " - " + (focusObject as DwarfObject).ToLink(false, focusObject as DwarfObject);
            }

            World = world; FocusObject = focusObject; TabControl = dwarfTabControl;
        }
        public override Control GetControl()
        {
            if (MapPanel == null || MapPanel.IsDisposed)
            {
                MapPanel = new MapPanel(World.Map, World, TabControl, FocusObject);
                if (MapScale > 0)
                {
                    MapPanel.ZoomCurrent = MapScale;
                    MapPanel.Center = Center;
                    MapPanel.ZoomToBoundsOnFirstPaint = false;
                    MapPanel.CurrentYear = CurrentYear;
                    if (CivsToggled)
                    {
                        MapPanel.ToggleCivs();
                    }

                    if (SitesToggled)
                    {
                        MapPanel.ToggleSites();
                    }

                    if (WarsToggled)
                    {
                        MapPanel.ToggleWars();
                    }

                    if (BattlesToggled)
                    {
                        MapPanel.ToggleBattles();
                    }

                    Refresh();
                }
                return MapPanel;
            }
            return MapPanel;
        }

        public override void Refresh()
        {
            MapPanel.Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    if (MapPanel != null)
                    {
                        MapScale = MapPanel.ZoomCurrent;
                        Center = MapPanel.Center;
                        CivsToggled = MapPanel.CivsToggled;
                        SitesToggled = MapPanel.SitesToggled;
                        WarsToggled = MapPanel.WarsToggled;
                        BattlesToggled = MapPanel.BattlesToggled;
                        CurrentYear = MapPanel.CurrentYear;
                        if (MapPanel.Overlay != null)
                        {
                            MapPanel.Overlay.Dispose();
                        }

                        MapPanel.Dispose();
                        MapPanel = null;
                    }

                }
                base.Dispose(disposing);
                Disposed = true;
            }
        }
    }
}
