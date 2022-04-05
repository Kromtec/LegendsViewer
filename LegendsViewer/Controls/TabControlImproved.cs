using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LegendsViewer.Controls
{
    public class TabControlImproved : TabControl
    {
        private int _hotTabIndex = -1;

        public TabControlImproved()
        {
            AllowDrop = true;
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);
        }

        #region Properties

        private Point _dragStartPosition = Point.Empty;

        private int CloseButtonHeight => FontHeight;

        private int HotTabIndex
        {
            get => _hotTabIndex;
            set
            {
                if (_hotTabIndex != value)
                {
                    _hotTabIndex = value;
                    Invalidate();
                }
            }
        }

        #endregion Properties

        #region Overridden Methods

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            OnFontChanged(EventArgs.Empty);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            IntPtr hFont = Font.ToHfont();
            SendMessage(Handle, WmSetfont, hFont, new IntPtr(-1));
            SendMessage(Handle, WmFontchange, IntPtr.Zero, IntPtr.Zero);
            UpdateStyles();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _dragStartPosition = new Point(e.X, e.Y);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Tchittestinfo hti = new Tchittestinfo(e.X, e.Y);
            HotTabIndex = SendMessage(Handle, TcmHittest, IntPtr.Zero, ref hti);

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Rectangle r = new Rectangle(_dragStartPosition, Size.Empty);
            r.Inflate(SystemInformation.DragSize);

            TabPage tp = HoverTab();

            if (tp != null && !r.Contains(e.X, e.Y))
            {
                DoDragDrop(tp, DragDropEffects.All);
            }

            _dragStartPosition = Point.Empty;
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

            TabPage hoverTab = HoverTab();
            if (hoverTab == null)
            {
                e.Effect = DragDropEffects.None;
            }
            else if (e.Data.GetDataPresent(typeof(DwarfTabPage)))
            {
                e.Effect = DragDropEffects.Move;
                TabPage dragTab = (TabPage)e.Data.GetData(typeof(DwarfTabPage));

                if (hoverTab == dragTab)
                {
                    return;
                }

                Rectangle tabRect = GetTabRect(TabPages.IndexOf(hoverTab));
                tabRect.Inflate(-3, -3);
                if (tabRect.Contains(PointToClient(new Point(e.X, e.Y))))
                {
                    SwapTabPages(dragTab, hoverTab);
                    SelectedTab = dragTab;
                }
            }
        }

        private TabPage HoverTab()
        {
            for (int index = 0; index <= TabCount - 1; index++)
            {
                if (GetTabRect(index).Contains(PointToClient(Cursor.Position)))
                {
                    return TabPages[index];
                }
            }
            return null;
        }

        private void SwapTabPages(TabPage tp1, TabPage tp2)
        {
            int index1 = TabPages.IndexOf(tp1);
            int index2 = TabPages.IndexOf(tp2);
            TabPages[index1] = tp2;
            TabPages[index2] = tp1;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            HotTabIndex = -1;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            for (int id = 0; id < TabCount; id++)
            {
                DrawTabBackground(pevent.Graphics, id);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            for (int id = 0; id < TabCount; id++)
            {
                DrawTabContent(e.Graphics, id);
            }
        }

        protected virtual void CloseTab(int index)
        {
            TabPages.RemoveAt(index);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == TcmSetpadding)
            {
                m.LParam = Makelparam(Padding.X + CloseButtonHeight / 2, Padding.Y);
            }

            if (m.Msg == WmMousedown && !DesignMode && HotTabIndex >= 0)
            {
                Point pt = PointToClient(Cursor.Position);
                Rectangle closeRect = GetCloseButtonRect(HotTabIndex);
                if (closeRect.Contains(pt))
                {
                    CloseTab(HotTabIndex);
                    m.Msg = WmNull;
                }
            }
            base.WndProc(ref m);
        }

        #endregion Overridden Methods

        #region Private Methods

        private IntPtr Makelparam(int lo, int hi)
        {
            return new IntPtr((hi << 16) | (lo & 0xFFFF));
        }

        private void DrawTabBackground(Graphics graphics, int id)
        {
            Rectangle rc = GetTabRect(id);
            if (id == SelectedIndex)
            {
                graphics.FillRectangle(Brushes.DarkGray, rc);
            }
            else if (id == HotTabIndex)
            {
                rc.Width--;
                rc.Height--;
                graphics.DrawRectangle(Pens.DarkGray, rc);
            }
            else
            {
                graphics.DrawLine(Pens.DarkGray, rc.Left, rc.Bottom, rc.Right, rc.Bottom);
            }
        }

        private void DrawTabContent(Graphics graphics, int id)
        {
            bool selectedOrHot = id == SelectedIndex || id == HotTabIndex;
            bool vertical = Alignment >= TabAlignment.Left;

            Image tabImage = null;

            if (ImageList != null)
            {
                TabPage page = TabPages[id];
                if (page.ImageIndex > -1 && page.ImageIndex < ImageList.Images.Count)
                {
                    tabImage = ImageList.Images[page.ImageIndex];
                }

                if (page.ImageKey.Length > 0 && ImageList.Images.ContainsKey(page.ImageKey))
                {
                    tabImage = ImageList.Images[page.ImageKey];
                }
            }

            Rectangle tabRect = GetTabRect(id);
            Rectangle contentRect = vertical
                ? new Rectangle(0, 0, tabRect.Height, tabRect.Width)
                : new Rectangle(Point.Empty, tabRect.Size);
            Rectangle textrect = contentRect;
            textrect.Width -= FontHeight;

            if (tabImage != null)
            {
                textrect.Width -= tabImage.Width;
                textrect.X += tabImage.Width;
            }

            Color frColor = id == SelectedIndex ? Color.White : ForeColor;
            Color bkColor = id == SelectedIndex ? Color.DarkGray : BackColor;

            using (Bitmap bm = new Bitmap(contentRect.Width, contentRect.Height))
            {
                using (Graphics bmGraphics = Graphics.FromImage(bm))
                {
                    TextRenderer.DrawText(bmGraphics, TabPages[id].Text, Font, textrect, frColor, bkColor);
                    if (selectedOrHot)
                    {
                        Rectangle closeRect = new Rectangle(contentRect.Right - CloseButtonHeight, 0, CloseButtonHeight,
                            CloseButtonHeight);
                        closeRect.Offset(-2, (contentRect.Height - closeRect.Height) / 2);
                        DrawCloseButton(bmGraphics, closeRect);
                    }

                    if (tabImage != null)
                    {
                        Rectangle imageRect = new Rectangle(Padding.X, 0, tabImage.Width, tabImage.Height);
                        imageRect.Offset(0, (contentRect.Height - imageRect.Height) / 2);
                        bmGraphics.DrawImage(tabImage, imageRect);
                    }
                }

                if (vertical)
                {
                    if (Alignment == TabAlignment.Left)
                    {
                        bm.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    }
                    else
                    {
                        bm.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                }
                graphics.DrawImage(bm, tabRect);
            }
        }

        private void DrawCloseButton(Graphics graphics, Rectangle bounds)
        {
            graphics.FillRectangle(Brushes.Red, bounds);
            using (Font closeFont = new Font("Arial", Font.Size, FontStyle.Bold))
            {
                TextRenderer.DrawText(graphics, "X", closeFont, bounds, Color.White, Color.Red,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine |
                    TextFormatFlags.VerticalCenter);
            }
        }

        private Rectangle GetCloseButtonRect(int id)
        {
            Rectangle tabRect = GetTabRect(id);
            Rectangle closeRect = new Rectangle(tabRect.Left, tabRect.Top, CloseButtonHeight, CloseButtonHeight);

            switch (Alignment)
            {
                case TabAlignment.Left:
                    closeRect.Offset((tabRect.Width - closeRect.Width) / 2, 0);
                    break;
                case TabAlignment.Right:
                    closeRect.Offset((tabRect.Width - closeRect.Width) / 2, tabRect.Height - closeRect.Height);
                    break;
                default:
                    closeRect.Offset(tabRect.Width - closeRect.Width, (tabRect.Height - closeRect.Height) / 2);
                    break;
            }

            return closeRect;
        }

        #endregion Private Methods

        #region Interop

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam, ref Tchittestinfo lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct Tchittestinfo
        {
            public Point pt;
            public Tchittestflags flags;

            public Tchittestinfo(int x, int y)
            {
                pt = new Point(x, y);
                flags = Tchittestflags.TchtNowhere;
            }
        }

        [Flags]
        private enum Tchittestflags
        {
            TchtNowhere = 1,
            TchtOnitemicon = 2,
            TchtOnitemlabel = 4,
            TchtOnitem = TchtOnitemicon | TchtOnitemlabel
        }

        private const int WmNull = 0x0;
        private const int WmSetfont = 0x30;
        private const int WmFontchange = 0x1D;
        private const int WmMousedown = 0x201;

        private const int TcmFirst = 0x1300;
        private const int TcmHittest = TcmFirst + 13;
        private const int TcmSetpadding = TcmFirst + 43;

        #endregion Interop
    }
}