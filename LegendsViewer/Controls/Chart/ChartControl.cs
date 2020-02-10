using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.Chart
{
    public class ChartControl : PageControl, IDisposable
    {
        public ChartPanel DwarfChart;
        public DwarfObject FocusObject;
        public World World;
        public List<ChartOption> SeriesOptions;
        public Boolean OtherChart;
        public ChartControl(World world, DwarfObject focusObject, DwarfTabControl dwarfTabControl)
        {
            World = world; FocusObject = focusObject; TabControl = dwarfTabControl;
            Title = "Chart";
            if (FocusObject != null)
            {
                Title += " - " + FocusObject.ToLink(false, FocusObject);
            }
        }
        public override Control GetControl()
        {
            if (DwarfChart == null || DwarfChart.IsDisposed)
            {
                if (SeriesOptions != null)
                {
                    DwarfChart = new ChartPanel(World, FocusObject, SeriesOptions)
                    {
                        OtherChart = OtherChart
                    };
                }
                else
                {
                    DwarfChart = new ChartPanel(World, FocusObject);
                }
            }
            return DwarfChart;
        }

        public override void Refresh()
        {
            DwarfChart.RefreshFiltered();
            //DwarfChart.RefreshAllSeries();
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    if (DwarfChart != null)
                    {
                        SeriesOptions =
                            DwarfChart.SeriesOptions.GroupBy(option => option).Select(option => option.Key).ToList();
                        OtherChart = DwarfChart.OtherChart;
                        DwarfChart.Dispose();
                        DwarfChart = null;
                    }
                }
                base.Dispose(disposing);
                Disposed = true;
            }
        }
    }
}
