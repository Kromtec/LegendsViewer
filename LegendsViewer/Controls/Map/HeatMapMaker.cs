using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using LegendsViewer.Legends;

namespace LegendsViewer.Controls.Map
{
    public class HeatMapMaker : IDisposable
    {
        private readonly Bitmap _heatMap;
        private Bitmap _heatGradient;
        private Bitmap _occurence;
        private readonly int _occurenceIntensity = 25, _occurenceDiameter = 75, _maxOccurencesToDraw = 50;
        private bool _disposed;

        public static Bitmap Create(int width, int height, List<Location> occurences, List<int> occurencesCount = null)
        {
            HeatMapMaker heatmap = new HeatMapMaker(width, height, occurences, occurencesCount);
            return heatmap._heatMap;
        }

        private HeatMapMaker(int width, int height, List<Location> occurences, List<int> occurencesCount = null)
        {
            MakeHeatGradient();
            MakeOccurence();
            Bitmap alphaMap = new Bitmap(width, height);
            Graphics alphaMapGraphics = Graphics.FromImage(alphaMap);
            alphaMapGraphics.SmoothingMode = SmoothingMode.None;
            alphaMapGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            if (occurencesCount == null)
            {
                var groupedOccurences = from occurence in occurences
                                        group occurence by occurence into occurenceGroup
                                        select occurenceGroup.Count();
                occurencesCount = groupedOccurences.ToList();
            }
            occurences = occurences.GroupBy(occurence => occurence).Select(occurence => occurence.Key).ToList();
            int maxOccurences = occurencesCount.Count > 0 ? occurencesCount.Max() : 0;
            double drawNumRatio = Convert.ToDouble(_maxOccurencesToDraw) / maxOccurences;
            for (int i = 0; i < occurences.Count; i++)
            {
                int drawNum = Convert.ToInt32(drawNumRatio * occurencesCount[i]);
                if (drawNum == 0 && occurencesCount[i] > 0)
                {
                    drawNum = 1;
                }

                for (int h = 0; h < drawNum; h++)
                {
                    DrawOccurence(occurences[i], alphaMapGraphics);
                }
            }

            alphaMap = Blur(alphaMap);
            _heatMap = new Bitmap(width, height);
            ConvertAlphaMapToHeatMap(alphaMap);
            alphaMap.Dispose();
        }

        private void MakeHeatGradient()
        {
            _heatGradient = new Bitmap(256, 25);

            using (LinearGradientBrush heatGradient = new LinearGradientBrush(new Point(0, 0), new Point(_heatGradient.Width, _heatGradient.Height), Color.Red, Color.Yellow))
            {
                ColorBlend colorBlend = new ColorBlend
                {
                    //colorBlend.Colors = new Color[] { Color.Red, Color.Yellow, Color.GreenYellow, Color.Transparent };
                    //colorBlend.Colors = new Color[] { Color.FromArgb(200, Color.Red), Color.FromArgb(200, Color.Yellow), Color.FromArgb(200, Color.GreenYellow), Color.Transparent };
                    //colorBlend.Positions = new float[] { 0.00f, 0.33f, 0.66f, 1.00f };
                    Colors = new[] { Color.FromArgb(200, Color.Red), Color.FromArgb(200, Color.Yellow), Color.FromArgb(200, Color.Green), Color.FromArgb(200, Color.Cyan), Color.FromArgb(200, Color.Blue), Color.Transparent },
                    Positions = new[] { 0.00f, 0.20f, 0.40f, 0.60f, 0.80f, 1.00f }
                };
                //colorBlend.Colors = new Color[] { Color.FromArgb(200, Color.White), Color.FromArgb(200, Color.Red), Color.FromArgb(200, Color.Yellow), Color.FromArgb(200, Color.Green), Color.FromArgb(200, Color.Cyan), Color.FromArgb(200, Color.Blue), Color.Transparent };
                //colorBlend.Positions = new float[] { 0.00f, 0.166f, 0.332f, 0.498f, 0.664f, .083f, 1f };
                //colorBlend.Colors = new Color[] { Color.FromArgb(200, Color.Red), Color.FromArgb(200, Color.Yellow), Color.FromArgb(200, Color.Green), Color.FromArgb(125, Color.Cyan), Color.FromArgb(50, Color.Blue)};
                //colorBlend.Positions = new float[] { 0.00f, 0.25f, 0.50f, 0.75f, 1.00f };
                heatGradient.InterpolationColors = colorBlend;
                Graphics fillGradient = Graphics.FromImage(_heatGradient);
                fillGradient.FillRectangle(heatGradient, 0, 0, _heatGradient.Width - 1, _heatGradient.Height);
                fillGradient.Dispose();
            }
        }

        private void MakeOccurence()
        {
            using (GraphicsPath gp = new GraphicsPath())
            {
                int x = _occurenceDiameter / 2;
                int y = _occurenceDiameter / 2;
                gp.AddEllipse(0, 0, _occurenceDiameter, _occurenceDiameter);
                using (PathGradientBrush gpBrush = new PathGradientBrush(gp))
                {
                    gpBrush.CenterPoint = new PointF(_occurenceDiameter / 2, _occurenceDiameter / 2);
                    gpBrush.CenterColor = Color.FromArgb(_occurenceIntensity, Color.Black);
                    gpBrush.SurroundColors = new[] { Color.Transparent };

                    _occurence = new Bitmap(_occurenceDiameter, _occurenceDiameter);
                    Graphics drawMap = Graphics.FromImage(_occurence);
                    drawMap.FillPath(gpBrush, gp);
                    drawMap.Dispose();
                }
            }
        }

        private void DrawOccurence(Location occurence, Graphics g)
        {
            int x = occurence.X - _occurenceDiameter / 2;
            int y = occurence.Y - _occurenceDiameter / 2;
            g.DrawImage(_occurence, new Point(x, y));
        }

        private void ConvertAlphaMapToHeatMap(Bitmap alphaMap)
        {
            using (ImageAttributes attributes = new ImageAttributes())
            {
                ColorMap[] newColorMap = new ColorMap[256];

                for (int x = 0; x < 256; x++)
                {
                    newColorMap[x] = new ColorMap
                    {
                        OldColor = Color.FromArgb(x, Color.Black),
                        NewColor = _heatGradient.GetPixel(255 - x, 0)
                    };
                }

                _ = Graphics.FromImage(_heatMap);
                attributes.SetRemapTable(newColorMap);
                Graphics remap = Graphics.FromImage(_heatMap);
                remap.DrawImage(alphaMap, new Rectangle(0, 0, _heatMap.Width, _heatMap.Height), 0, 0, _heatMap.Width, _heatMap.Height, GraphicsUnit.Pixel, attributes);
                remap.Dispose();
            }
        }

        private static Bitmap Blur(Bitmap source)
        {
            Bitmap blurred = new Bitmap(source.Width, source.Height);
            BitmapData bmd = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, source.PixelFormat);
            BitmapData bmdBlurred = blurred.LockBits(new Rectangle(0, 0, blurred.Width, blurred.Height), ImageLockMode.ReadOnly, blurred.PixelFormat);
            int pixelSize = 4;
            int radius = 8;
            int totalMax = (radius * 2 + 1) * 256;
            long sourceStart = (long)bmd.Scan0;
            long blurredStart = (long)bmdBlurred.Scan0;
            int stride = bmd.Stride;
            int width = bmd.Width;
            int height = bmd.Height;
            int xOffset, yOffset;
            unsafe
            {
                yOffset = 0;
                for (int y = 0; y < height; y++)
                {
                    int total = 0;
                    byte* row = (byte*)sourceStart + yOffset;
                    for (int kx = -radius; kx <= radius; kx++)
                    {
                        if (kx >= 0 && kx < width)
                        {
                            total += row[kx * pixelSize + 3];
                        }
                    }

                    byte* rowSet = (byte*)blurredStart + yOffset;
                    rowSet[0] = (byte)(total / (radius * 2 + 1));

                    xOffset = 3;
                    int xOffsetRemove = (1 - radius - 1) * pixelSize + 3;
                    int xOffsetAdd = (1 + radius) * pixelSize + 3;
                    for (int x = 1; x < width; x++)
                    {
                        if (x - radius - 1 >= 0)
                        {
                            total -= row[xOffsetRemove];
                        }

                        if (x + radius < width)
                        {
                            total += row[xOffsetAdd];
                        }

                        rowSet = (byte*)blurredStart + yOffset;
                        rowSet[xOffset] = total == 0 ? (byte)0 : (byte)(total / (radius * 2 + 1));

                        xOffset += pixelSize;
                        xOffsetRemove += pixelSize;
                        xOffsetAdd += pixelSize;
                    }
                    yOffset += stride;
                }

                source.UnlockBits(bmd);
                blurred.UnlockBits(bmdBlurred);
                source.Dispose();
                source = new Bitmap(blurred);
                bmd = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, source.PixelFormat);
                bmdBlurred = blurred.LockBits(new Rectangle(0, 0, blurred.Width, blurred.Height), ImageLockMode.ReadOnly, blurred.PixelFormat);
                sourceStart = (long)bmd.Scan0;

                xOffset = 3;
                for (int x = 0; x < width; x++)
                {
                    int total = 0;
                    byte* row;
                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        row = (byte*)sourceStart + ky * stride;
                        if (ky >= 0 && ky < height)
                        {
                            total += row[xOffset];
                        }
                    }

                    byte* rowSet = (byte*)blurredStart;
                    rowSet[xOffset] = (byte)(total / (radius * 2 + 1));

                    yOffset = 0;
                    int yOffsetRemove = (1 - radius - 1) * stride;
                    int yOffsetAdd = (1 + radius) * stride;
                    for (int y = 1; y < height; y++)
                    {
                        if (y - radius - 1 >= 0)
                        {
                            row = (byte*)sourceStart + yOffsetRemove;
                            total -= row[xOffset];
                        }
                        if (y + radius < height)
                        {
                            row = (byte*)sourceStart + yOffsetAdd;
                            total += row[xOffset];
                        }

                        rowSet = (byte*)blurredStart + yOffset;
                        if (total == 0)
                        {
                            rowSet[xOffset] = 0;
                        }

                        rowSet[xOffset] = (byte)(total / (radius * 2 + 1));
                        yOffset += stride;
                        yOffsetRemove += stride;
                        yOffsetAdd += stride;
                    }
                    xOffset += pixelSize;
                }
            }
            source.UnlockBits(bmd);
            source.Dispose();
            blurred.UnlockBits(bmdBlurred);
            return blurred;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _heatMap.Dispose();
                    _heatGradient.Dispose();
                    _occurence.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
