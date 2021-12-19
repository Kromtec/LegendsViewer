using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
using LegendsViewer.Legends;

namespace LegendsViewer.Controls
{
    /// <summary>
    /// This class holds all the common bits of code that are used across multiple tabs.
    /// It also provides the tabs with access to form level elements, like status bar texts.
    /// </summary>
    public class LvCoordinator
    {
        public LvCoordinator(FrmLegendsViewer form)
        {
            Form = form;
        }

        internal FrmLegendsViewer Form { get; private set; }

        internal DwarfTabControl Browser => Form?.Browser;
        internal World World => Form?.World;
        internal FileLoader FileLoader => Form?.FileLoader;

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Legends Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ChangeHotItemStyle(ObjectListView olv, ComboBox cb)
        {
            olv.UseTranslucentHotItem = false;
            olv.UseHotItem = true;
            olv.UseExplorerTheme = false;

            switch (cb.SelectedIndex)
            {
                case 0:
                    olv.UseHotItem = false;
                    break;
                case 1:
                    HotItemStyle hotItemStyle = new HotItemStyle
                    {
                        ForeColor = Color.AliceBlue,
                        BackColor = Color.FromArgb(255, 64, 64, 64)
                    };
                    olv.HotItemStyle = hotItemStyle;
                    break;
                case 2:
                    RowBorderDecoration rbd = new RowBorderDecoration
                    {
                        BorderPen = new Pen(Color.SeaGreen, 2),
                        FillBrush = null,
                        CornerRounding = 4.0f
                    };
                    HotItemStyle hotItemStyle2 = new HotItemStyle
                    {
                        Decoration = rbd
                    };
                    olv.HotItemStyle = hotItemStyle2;
                    break;
                case 3:
                    olv.UseTranslucentHotItem = true;
                    break;
                case 4:
                    HotItemStyle hotItemStyle3 = new HotItemStyle
                    {
                        Decoration = new LightBoxDecoration()
                    };
                    olv.HotItemStyle = hotItemStyle3;
                    break;
                case 5:
                    olv.FullRowSelect = true;
                    olv.UseHotItem = false;
                    olv.UseExplorerTheme = true;
                    break;
            }
            olv.Invalidate();
        }

        public void HandleHotItemChanged(object sender, HotItemChangedEventArgs e)
        {
        }

        public void HandleSelectionChanged(ObjectListView listView)
        {
        }
    }
}
