using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Interfaces;
using LegendsViewer.Legends.WorldObjects;
using SevenZip;

namespace LegendsViewer.Controls.Map
{
    public class MapPanel : Panel
    {
        public DwarfTabControl TabControl;
        Bitmap _map, _minimap;
        public Bitmap AlternateMap, Overlay;
        //Size MinimapSize;
        public object FocusObject;
        List<object> _focusObjects;
        World _world;
        List<object> _displayObjects;
        public Point MousePanStart, Center, MouseClickLocation, MouseLocation;
        public Rectangle Source;
        Rectangle _zoomBounds;
        public double ZoomCurrent = 1;
        public double PixelWidth, PixelHeight, ZoomChange = 0.15, ZoomMax = 10.0, ZoomMin = 0.2;
        MapMenu _hoverMenu, _controlMenu, _yearMenu;
        MapMenuHorizontal _optionsMenu;
        public bool ZoomToBoundsOnFirstPaint, CivsToggled, SitesToggled, WarsToggled, BattlesToggled, OverlayToggled, AlternateMapToggled;
        public int TileSize = 16, MinYear, MaxYear, CurrentYear, MiniMapAreaSideLength = 200;
        List<CivPaths> _allCivPaths = new List<CivPaths>();
        List<Entity> _warEntities = new List<Entity>();
        List<Battle> _battles = new List<Battle>();
        List<Location> _battleLocations = new List<Location>();
        TrackBar _altMapTransparency = new TrackBar();
        float _altMapAlpha = 1.0f;

        public MapPanel(Bitmap map, World world, DwarfTabControl dwarfTabControl, object focusObject)
        {
            TabControl = dwarfTabControl;
            _map = map;
            _world = world;
            FocusObject = focusObject;
            if (FocusObject != null && FocusObject.GetType() == typeof(World))
            {
                FocusObject = null;
            }

            _displayObjects = new List<object>();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            Source = new Rectangle(new Point(Center.X - Width / 2, Center.Y - Height / 2), new Size(Width, Height));

            _hoverMenu = new MapMenu(this);
            _controlMenu = new MapMenu(this);
            _controlMenu.AddOptions(new List<object> { "Zoom In", "Zoom Out", "Toggle Civs", "Toggle Sites", "Toggle Wars", "Toggle Battles", "Toggle Overlay", "Toggle Alt Map" });
            _controlMenu.Open = true;
            _yearMenu = new MapMenu(this);
            _yearMenu.AddOptions(new List<object> { "+1000", "+100", "+10", "+1", "-1", "-10", "-100", "-1000" });
            _yearMenu.Open = true;
            _optionsMenu = new MapMenuHorizontal(this)
            {
                Open = true
            };
            _optionsMenu.AddOptions(new List<object> { "Load Alternate Map...", "Export Map...", "Overlays" });
            MapMenu overlayOptions = new MapMenu(this);
            overlayOptions.AddOptions(new List<object> { "Battles", "Battles (Notable)", "Battle Deaths", "Beast Attacks", "Site Population...", "Site Events", "Site Events (Filtered)" });
            overlayOptions.Options.ForEach(option => option.OptionObject = "Overlay");
            _optionsMenu.Options.Last().SubMenu = overlayOptions;

            _altMapTransparency.Minimum = 0;
            _altMapTransparency.Maximum = 100;
            _altMapTransparency.AutoSize = false;
            _altMapTransparency.Size = new Size(150, 25);
            _altMapTransparency.TickFrequency = 1;
            _altMapTransparency.TickStyle = TickStyle.None;
            _altMapTransparency.BackColor = _yearMenu.MenuColor;
            _altMapTransparency.Visible = false;
            _altMapTransparency.Scroll += ChangeAltMapTransparency;
            _altMapTransparency.Value = 100;
            Controls.Add(_altMapTransparency);
            _altMapTransparency.Location = new Point(MiniMapAreaSideLength + _yearMenu.MenuBox.Width, Height - _altMapTransparency.Height);


            MinYear = _world.Eras.First().StartYear;
            if (MinYear == -1)
            {
                MinYear = 0;
            }

            MaxYear = CurrentYear = _world.Eras.Last().EndYear;
            //Set Map Year if Entity has no active sites so they show on map
            if (FocusObject != null && FocusObject.GetType() == typeof(Entity) && ((Entity)FocusObject).SiteHistory.Count(sitePeriod => sitePeriod.EndYear == -1) == 0)
            {
                CurrentYear = (FocusObject as Entity).SiteHistory.Max(sitePeriod => sitePeriod.EndYear) - 1;
            }
            else if (FocusObject != null && FocusObject.GetType() == typeof(Battle))
            {
                CurrentYear = MinYear = MaxYear = ((Battle)focusObject).StartYear;
            }
            else if (FocusObject != null && FocusObject.GetType() == typeof(SiteConquered))
            {
                CurrentYear = MinYear = MaxYear = ((SiteConquered)FocusObject).StartYear;
            }

            if (focusObject != null && focusObject.GetType() == typeof(List<object>))
            {
                _focusObjects = ((List<object>)focusObject).GroupBy(item => item).Select(item => item.Key).ToList();
                if (_focusObjects.First().GetType() == typeof(Battle))
                {
                    _battles.AddRange(_focusObjects.Cast<Battle>());
                }
                else
                {
                    _displayObjects.AddRange(_focusObjects);
                }
            }
            else
            {
                _focusObjects = new List<object>();
            }

            if (focusObject != null && focusObject.GetType() != typeof(Battle))
            {
                _displayObjects.Add(focusObject);
            }

            if (focusObject != null && focusObject.GetType() == typeof(Battle))
            {
                _battles.Add(focusObject as Battle);
            }

            if (FocusObject != null && FocusObject.GetType() == typeof(War))
            {
                if (FocusObject is War war)
                {
                    MinYear = CurrentYear = war.StartYear;
                    if (war.EndYear != -1)
                    {
                        MaxYear = war.EndYear;
                    }

                    UpdateWarDisplay();
                    foreach (Battle battle in war.Collections.OfType<Battle>())
                    {
                        _battles.Add(battle);
                    }
                }
            }

            //Center and zoom map on focusObject of the map
            if (FocusObject == null || _focusObjects.Count > 0)
            {
                Center = new Point(_map.Width / 2, _map.Height / 2);
            }
            else if (FocusObject.GetType() == typeof(Site))
            {
                Site site = focusObject as Site;
                Center = new Point(site.Coordinates.X * TileSize + TileSize / 2, site.Coordinates.Y * TileSize + TileSize / 2);
                ZoomCurrent = 0.85;
            }
            else if (FocusObject.GetType() == typeof(Artifact))
            {
                Artifact artifact = focusObject as Artifact;
                Center = new Point(artifact.Coordinates.X * TileSize + TileSize / 2, artifact.Coordinates.Y * TileSize + TileSize / 2);
                ZoomCurrent = 0.85;
            }
            else if (FocusObject.GetType() == typeof(Entity) || FocusObject.GetType() == typeof(War)
                     || FocusObject.GetType() == typeof(Battle) || FocusObject.GetType() == typeof(SiteConquered)
                     || FocusObject.GetType() == typeof(WorldRegion) || FocusObject.GetType() == typeof(UndergroundRegion)
                     || FocusObject.GetType() == typeof(WorldConstruction) || FocusObject.GetType() == typeof(Landmass) ||
                     FocusObject.GetType() == typeof(MountainPeak))
            {
                List<Entity> entities = new List<Entity>();
                if (FocusObject.GetType() == typeof(Entity))
                {
                    entities.Add(FocusObject as Entity);
                }
                else if (FocusObject.GetType() == typeof(War))
                {
                    entities.Add((FocusObject as War).Attacker);
                    entities.Add((FocusObject as War).Defender);
                }
                _zoomBounds = new Rectangle(-1, -1, -1, -1);

                foreach (Entity displayEntity in entities)
                {
                    foreach (OwnerPeriod sitePeriod in displayEntity.SiteHistory.Where(ownerPeriod => ownerPeriod.StartYear <= CurrentYear && ownerPeriod.EndYear >= CurrentYear || ownerPeriod.EndYear == -1))
                    {
                        if (_zoomBounds.Top == -1)
                        {
                            _zoomBounds.Y = _zoomBounds.Height = sitePeriod.Site.Coordinates.Y;
                            _zoomBounds.X = _zoomBounds.Width = sitePeriod.Site.Coordinates.X;
                        }
                        if (sitePeriod.Site.Coordinates.Y < _zoomBounds.Y)
                        {
                            _zoomBounds.Y = sitePeriod.Site.Coordinates.Y;
                        }

                        if (sitePeriod.Site.Coordinates.X < _zoomBounds.X)
                        {
                            _zoomBounds.X = sitePeriod.Site.Coordinates.X;
                        }

                        if (sitePeriod.Site.Coordinates.Y > _zoomBounds.Height)
                        {
                            _zoomBounds.Height = sitePeriod.Site.Coordinates.Y;
                        }

                        if (sitePeriod.Site.Coordinates.X > _zoomBounds.Width)
                        {
                            _zoomBounds.Width = sitePeriod.Site.Coordinates.X;
                        }
                    }
                }

                if (FocusObject.GetType() == typeof(War))
                {
                    War war = FocusObject as War;

                    foreach (Battle battle in war.Collections.OfType<Battle>())
                    {
                        if (_zoomBounds.Top == -1)
                        {
                            _zoomBounds.Y = _zoomBounds.Height = battle.Coordinates.Y;
                            _zoomBounds.X = _zoomBounds.Width = battle.Coordinates.X;
                        }
                        if (battle.Coordinates.Y < _zoomBounds.Y)
                        {
                            _zoomBounds.Y = battle.Coordinates.Y;
                        }

                        if (battle.Coordinates.X < _zoomBounds.X)
                        {
                            _zoomBounds.X = battle.Coordinates.X;
                        }

                        if (battle.Coordinates.Y > _zoomBounds.Height)
                        {
                            _zoomBounds.Height = battle.Coordinates.Y;
                        }

                        if (battle.Coordinates.X > _zoomBounds.Width)
                        {
                            _zoomBounds.Width = battle.Coordinates.X;
                        }
                    }
                }

                if (FocusObject.GetType() == typeof(Battle) || FocusObject.GetType() == typeof(SiteConquered))
                {
                    Battle battle;
                    if (FocusObject.GetType() == typeof(Battle))
                    {
                        battle = FocusObject as Battle;
                    }
                    else
                    {
                        battle = (FocusObject as SiteConquered).Battle;
                    }

                    Center = new Point(battle.Coordinates.X * TileSize + TileSize / 2, battle.Coordinates.Y * TileSize + TileSize / 2);
                    _zoomBounds.Y = _zoomBounds.Height = battle.Coordinates.Y;
                    _zoomBounds.X = _zoomBounds.Width = battle.Coordinates.X;
                    Site attackerSite = GetClosestSite(battle.Attacker, battle.Coordinates);
                    if (attackerSite.Coordinates.Y < _zoomBounds.Y)
                    {
                        _zoomBounds.Y = attackerSite.Coordinates.Y;
                    }

                    if (attackerSite.Coordinates.X < _zoomBounds.X)
                    {
                        _zoomBounds.X = attackerSite.Coordinates.X;
                    }

                    if (attackerSite.Coordinates.Y > _zoomBounds.Height)
                    {
                        _zoomBounds.Height = attackerSite.Coordinates.Y;
                    }

                    if (attackerSite.Coordinates.X > _zoomBounds.Width)
                    {
                        _zoomBounds.Width = attackerSite.Coordinates.X;
                    }

                    Site defenderSite = GetClosestSite(battle.Defender, battle.Coordinates);
                    if (defenderSite.Coordinates.Y < _zoomBounds.Y)
                    {
                        _zoomBounds.Y = defenderSite.Coordinates.Y;
                    }

                    if (defenderSite.Coordinates.X < _zoomBounds.X)
                    {
                        _zoomBounds.X = defenderSite.Coordinates.X;
                    }

                    if (defenderSite.Coordinates.Y > _zoomBounds.Height)
                    {
                        _zoomBounds.Height = defenderSite.Coordinates.Y;
                    }

                    if (defenderSite.Coordinates.X > _zoomBounds.Width)
                    {
                        _zoomBounds.Width = defenderSite.Coordinates.X;
                    }
                }

                if (FocusObject is IHasCoordinates)
                {
                    _zoomBounds.X = (FocusObject as IHasCoordinates).Coordinates.Min(coord => coord.X);
                    _zoomBounds.Y = (FocusObject as IHasCoordinates).Coordinates.Min(coord => coord.Y);
                    _zoomBounds.Width = (FocusObject as IHasCoordinates).Coordinates.Max(coord => coord.X);
                    _zoomBounds.Height = (FocusObject as IHasCoordinates).Coordinates.Max(coord => coord.Y);
                }

                _zoomBounds.X = _zoomBounds.X * TileSize - TileSize / 2 - TileSize;
                _zoomBounds.Width = _zoomBounds.Width * TileSize + TileSize / 2 + TileSize;
                _zoomBounds.Y = _zoomBounds.Y * TileSize - TileSize / 2 - TileSize;
                _zoomBounds.Height = _zoomBounds.Height * TileSize + TileSize / 2 + TileSize;
                Center.X = (_zoomBounds.Left + _zoomBounds.Width) / 2;
                Center.Y = (_zoomBounds.Top + _zoomBounds.Height) / 2;
                ZoomToBoundsOnFirstPaint = true;
            }
            else
            {
                Center = new Point(0, 0);
            }

            _battleLocations = _battles.GroupBy(battle => battle.Coordinates).Select(battle => battle.Key).ToList();

            GenerateCivPaths();
            _minimap = world.MiniMap;
            UpdateWarDisplay();
            Invalidate();

            if (MapUtil.AlternateMap != null)
            {
                AlternateMap = MapUtil.AlternateMap;
                _altMapAlpha = MapUtil.AltMapAlpha;
                _altMapTransparency.Value = (int)(_altMapAlpha * 100);
                AlternateMapToggled = false;
                ToggleAlternateMap();
            }
        }

        private class CivPaths
        {
            public Entity Civ;
            public List<List<Site>> SitePaths;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (ZoomToBoundsOnFirstPaint)
            {
                ZoomToBounds();
            }

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            DrawMap(e.Graphics);
            DrawDisplayObjects(e.Graphics);
            DrawOverlay(e.Graphics);
            _controlMenu.Draw(e.Graphics);
            _yearMenu.Draw(e.Graphics);
            _optionsMenu.Draw(e.Graphics);
            DrawInfo(e.Graphics);
            DrawMiniMap(e.Graphics);
            if (_hoverMenu.Options.Count > 0)
            {
                _hoverMenu.Draw(e.Graphics);
            }
        }

        private void DrawMap(Graphics g, bool originalSize = false)
        {
            if (!originalSize)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 50)), new Rectangle(Location, Size));
            }

            if (AlternateMapToggled && _altMapAlpha > 0)
            {
                if (_altMapAlpha < 1)
                {
                    g.DrawImage(_map, originalSize ? getOriginalSize(_map) : ClientRectangle, originalSize ? getOriginalSize(_map) : Source, GraphicsUnit.Pixel);
                    using (ImageAttributes adjustAlpha = new ImageAttributes())
                    {
                        ColorMatrix adjustAlphaMatrix = new ColorMatrix();
                        adjustAlphaMatrix.Matrix00 = adjustAlphaMatrix.Matrix11 = adjustAlphaMatrix.Matrix22 = adjustAlphaMatrix.Matrix44 = 1.0f;
                        adjustAlphaMatrix.Matrix33 = _altMapAlpha;
                        adjustAlpha.SetColorMatrix(adjustAlphaMatrix);
                        if (originalSize)
                        {
                            g.DrawImage(AlternateMap, getOriginalSize(AlternateMap), 0, 0, AlternateMap.Width, AlternateMap.Height, GraphicsUnit.Pixel, adjustAlpha);
                        }
                        else
                        {
                            g.DrawImage(AlternateMap, ClientRectangle, Source.X, Source.Y, Source.Width, Source.Height, GraphicsUnit.Pixel, adjustAlpha);
                        }
                    }

                }
                else
                {
                    g.DrawImage(AlternateMap, originalSize ? getOriginalSize(AlternateMap) : ClientRectangle, originalSize ? getOriginalSize(AlternateMap) : Source, GraphicsUnit.Pixel);
                }
            }
            else
            {
                g.DrawImage(_map, originalSize ? getOriginalSize(_map) : ClientRectangle, originalSize ? getOriginalSize(_map) : Source, GraphicsUnit.Pixel);
            }
        }

        private Rectangle getOriginalSize(Bitmap map)
        {
            return new Rectangle(0, 0, map.Width, map.Height);
        }

        private void DrawMiniMap(Graphics g)
        {
            double minimapRatio;
            if (_map.Width > _map.Height)
            {
                minimapRatio = Convert.ToDouble(_minimap.Width) / Convert.ToDouble(_map.Width);
            }
            else
            {
                minimapRatio = Convert.ToDouble(_minimap.Height) / Convert.ToDouble(_map.Height);
            }

            Rectangle minimapArea = new Rectangle(0, Height - MiniMapAreaSideLength, MiniMapAreaSideLength, MiniMapAreaSideLength);
            Point miniMapDrawLocation = new Point(minimapArea.X + MiniMapAreaSideLength / 2 - _minimap.Width / 2, minimapArea.Y + MiniMapAreaSideLength / 2 - _minimap.Height / 2);
            g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 50)), minimapArea);
            g.DrawImage(_minimap, new Rectangle(miniMapDrawLocation.X, miniMapDrawLocation.Y, _minimap.Width, _minimap.Height), new Rectangle(0, 0, _minimap.Width, _minimap.Height), GraphicsUnit.Pixel);

            Point miniSourceLocation = new Point(miniMapDrawLocation.X + Convert.ToInt32(Source.X * minimapRatio), miniMapDrawLocation.Y + Convert.ToInt32(Source.Y * minimapRatio));
            Size miniSourceSize = new Size(Convert.ToInt32(Source.Width * minimapRatio), Convert.ToInt32(Source.Height * minimapRatio));
            Rectangle miniSource = new Rectangle(miniSourceLocation, miniSourceSize);

            if (miniSource.Left < minimapArea.Left) { miniSource.Width -= minimapArea.Left - miniSource.Left; miniSource.X = minimapArea.Left; }
            if (miniSource.Top < minimapArea.Top) { miniSource.Height -= minimapArea.Top - miniSource.Top; miniSource.Y = minimapArea.Top; }

            if (miniSource.Right > minimapArea.Right)
            {
                miniSource.Width = minimapArea.X + _minimap.Width - miniSource.X;
            }

            if (miniSource.Bottom > minimapArea.Bottom)
            {
                miniSource.Height = minimapArea.Y + _minimap.Height - miniSource.Y;
            }

            g.SmoothingMode = SmoothingMode.None;
            g.PixelOffsetMode = PixelOffsetMode.Default;
            using (Pen minimapPen = new Pen(Color.White))
            {
                g.DrawRectangle(minimapPen, miniSource);
            }
        }

        public void DrawDisplayObjects(Graphics g, bool originalSize = false)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Half;

            DrawEntities(g, _displayObjects.OfType<Entity>().ToList(), originalSize);
            DrawCivPaths(g, originalSize);
            
            Rectangle rectangle = originalSize ? getOriginalSize(_map) : Source;
            SizeF scaleTileSize = new SizeF((float)(PixelWidth * TileSize), (float)(PixelHeight * TileSize));
            foreach (Site site in _displayObjects.OfType<Site>())
            {
                PointF siteLocation = new PointF
                {
                    X = (float)((site.Coordinates.X * TileSize - rectangle.X) * PixelWidth),
                    Y = (float)((site.Coordinates.Y * TileSize - rectangle.Y) * PixelHeight)
                };
                using (Pen sitePen = new Pen(Color.White))
                {
                    g.DrawRectangle(sitePen, siteLocation.X, siteLocation.Y, scaleTileSize.Width, scaleTileSize.Height);
                }
            }
            foreach (var battle in _battleLocations)
            {
                PointF battleLocation = new PointF
                {
                    X = (float)((battle.X * TileSize - rectangle.X) * PixelWidth),
                    Y = (float)((battle.Y * TileSize - rectangle.Y) * PixelHeight)
                };
                using (Pen battlePen = new Pen(Color.FromArgb(175, Color.White), 2))
                {
                    g.DrawEllipse(battlePen, battleLocation.X, battleLocation.Y, scaleTileSize.Width, scaleTileSize.Height);
                }
            }

            foreach (War war in _displayObjects.OfType<War>().Where(war => war.StartYear <= CurrentYear && (war.EndYear >= CurrentYear || war.EndYear == -1)))
            {
                foreach (Battle battle in war.Collections.OfType<Battle>().Where(battle => battle.StartYear == CurrentYear))
                {
                    DrawBattlePaths(g, battle, originalSize);
                }
            }

            if (FocusObject is IHasCoordinates coordinates)
            {
                float margin = (float)(4 * PixelWidth);
                if (coordinates.GetType() == typeof(Landmass))
                {
                    PointF startloc = new PointF
                    {
                        X = (float)((coordinates.Coordinates[0].X * TileSize - rectangle.X) * PixelWidth),
                        Y = (float)((coordinates.Coordinates[0].Y * TileSize - rectangle.Y) * PixelHeight)
                    };
                    PointF endloc = new PointF
                    {
                        X = (float)((coordinates.Coordinates[1].X * TileSize - rectangle.X) * PixelWidth),
                        Y = (float)((coordinates.Coordinates[1].Y * TileSize - rectangle.Y) * PixelHeight)
                    };

                    using (Pen pen = new Pen(Color.FromArgb(255, Color.White), 2))
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), startloc.X, startloc.Y, endloc.X - startloc.X + scaleTileSize.Width, endloc.Y - startloc.Y + scaleTileSize.Height);
                        g.DrawRectangle(pen, startloc.X, startloc.Y, endloc.X - startloc.X + scaleTileSize.Width, endloc.Y - startloc.Y + scaleTileSize.Height);
                    }
                }
                else
                {
                    foreach (Location coord in coordinates.Coordinates)
                    {
                        PointF loc = new PointF
                        {
                            X = (float)((coord.X * TileSize - rectangle.X) * PixelWidth),
                            Y = (float)((coord.Y * TileSize - rectangle.Y) * PixelHeight)
                        };

                        using (Pen pen = new Pen(Color.FromArgb(255, Color.White), 2))
                        {
                            if (coordinates.GetType() == typeof(WorldRegion))
                            {
                                g.FillRectangle(new SolidBrush(Color.LightGreen), loc.X + margin, loc.Y + margin,
                                    scaleTileSize.Width - 2 * margin, scaleTileSize.Height - 2 * margin);
                            }
                            else if (coordinates.GetType() == typeof(UndergroundRegion))
                            {
                                g.FillRectangle(new SolidBrush(Color.SandyBrown), loc.X + margin, loc.Y + margin,
                                    scaleTileSize.Width - 2 * margin, scaleTileSize.Height - 2 * margin);
                            }
                            else if (coordinates.GetType() == typeof(WorldConstruction))
                            {
                                g.FillRectangle(new SolidBrush(Color.Gold), loc.X + margin, loc.Y + margin,
                                    scaleTileSize.Width - 2 * margin, scaleTileSize.Height - 2 * margin);
                            }
                            else if (coordinates.GetType() == typeof(MountainPeak))
                            {
                                g.FillRectangle(new SolidBrush(Color.CornflowerBlue), loc.X + margin, loc.Y + margin,
                                    scaleTileSize.Width - 2 * margin, scaleTileSize.Height - 2 * margin);
                            }
                            g.DrawRectangle(pen, loc.X + margin, loc.Y + margin,
                                scaleTileSize.Width - 2 * margin, scaleTileSize.Height - 2 * margin);
                        }
                    }
                }
            }

            if (FocusObject != null && FocusObject.GetType() == typeof(Battle))
            {
                DrawBattlePaths(g, FocusObject as Battle);
            }

            if (FocusObject != null && FocusObject.GetType() == typeof(SiteConquered))
            {
                DrawBattlePaths(g, ((SiteConquered)FocusObject).Battle);
            }
        }

        private void DrawOverlay(Graphics g, bool originalSize = false)
        {
            g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            if (OverlayToggled)
            {
                g.DrawImage(Overlay, originalSize ? getOriginalSize(_map) : ClientRectangle, originalSize ? getOriginalSize(_map) : Source, GraphicsUnit.Pixel);
            }
        }

        private void DrawEntities(Graphics g, List<Entity> entities, bool originalSize = false)
        {
            var rectangle = originalSize ? getOriginalSize(_map) : Source;
            SizeF scaleTileSize = new SizeF((float)PixelWidth * TileSize, (float)PixelHeight * TileSize);
            g.InterpolationMode = InterpolationMode.Default;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            foreach (Site site in entities.SelectMany(entity => entity.Sites).Distinct())
            {
                PointF siteLocation = new PointF
                {
                    X = (float)((site.Coordinates.X * TileSize - rectangle.X) * PixelWidth),
                    Y = (float)((site.Coordinates.Y * TileSize - rectangle.Y) * PixelHeight)
                };
                OwnerPeriod ownerPeriod = site.OwnerHistory.LastOrDefault(op => op.StartYear <= CurrentYear && op.EndYear >= CurrentYear || op.EndYear == -1);
                Entity entity = ownerPeriod?.Owner as Entity;
                if (entity == null)
                {
                    continue;
                }
                if (ownerPeriod.EndYear != -1 && ownerPeriod.EndYear <= CurrentYear)
                {
                    ImageAttributes imageAttributes = new ImageAttributes();
                    imageAttributes.SetColorMatrix(new ColorMatrix
                    {
                        Matrix00 = 0.66f,
                        Matrix11 = 0.66f,
                        Matrix22 = 0.66f,
                        Matrix33 = 1.00f,
                        Matrix44 = 0.66f
                    });
                    using (Bitmap lostSiteIdenticon = new Bitmap(entity.Identicon.Width, entity.Identicon.Height))
                    {
                        using (Graphics drawLostSite = Graphics.FromImage(lostSiteIdenticon))
                        {
                            drawLostSite.DrawImage(entity.Identicon, new Rectangle(0, 0, entity.Identicon.Width, entity.Identicon.Height), 0, 0, entity.Identicon.Width, entity.Identicon.Height, GraphicsUnit.Pixel, imageAttributes);
                        }
                        g.DrawImage(lostSiteIdenticon, new RectangleF(siteLocation.X, siteLocation.Y, scaleTileSize.Width + 1, scaleTileSize.Height + 1));
                    }
                }
                else
                {
                    g.DrawImage(entity.Identicon, new RectangleF(siteLocation.X, siteLocation.Y, scaleTileSize.Width + 1, scaleTileSize.Height + 1));
                }
            }
        }

        private void DrawBattlePaths(Graphics g, Battle battle, bool originalSize = false)
        {
            using (Pen attackerLine = new Pen(Color.Black, 3))
            using (Pen defenderLine = new Pen(Color.Black, 3))
            using (AdjustableArrowCap victorCap = new AdjustableArrowCap(4, 6))
            using (AdjustableArrowCap loserCap = new AdjustableArrowCap(5, 6))
            {
                attackerLine.DashStyle = DashStyle.Dot;
                defenderLine.DashStyle = DashStyle.Dot;

                loserCap.Filled = false;

                Site attackerSite = GetClosestSite(battle.Attacker, battle.Coordinates);
                Site defenderSite = GetClosestSite(battle.Defender, battle.Coordinates);

                attackerLine.Color = battle.Attacker.LineColor;
                defenderLine.Color = battle.Defender.LineColor;
                if (battle.Victor == battle.Attacker)
                {
                    attackerLine.CustomEndCap = victorCap;
                    defenderLine.CustomEndCap = loserCap;
                }
                else if (battle.Victor == battle.Defender)
                {
                    attackerLine.CustomEndCap = loserCap;
                    defenderLine.CustomEndCap = victorCap;
                }

                g.DrawLine(attackerLine, SiteToScreen(attackerSite.Coordinates, originalSize), SiteToScreen(battle.Coordinates, originalSize));
                if (defenderSite != null && defenderSite.Coordinates != battle.Coordinates)
                {
                    g.DrawLine(defenderLine, SiteToScreen(defenderSite.Coordinates, originalSize), SiteToScreen(battle.Coordinates, originalSize));
                }
            }
        }

        private void DrawCivPaths(Graphics g, bool originalSize = false)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            foreach (CivPaths civPaths in _allCivPaths)
            {
                foreach (List<Site> path in civPaths.SitePaths)
                {
                    Site site1 = path.First();
                    Site site2 = path.Last();
                    Size tileDistance = new Size(site1.Coordinates.X - site2.Coordinates.X, site1.Coordinates.Y - site2.Coordinates.Y);
                    if (tileDistance.Width < 0)
                    {
                        tileDistance.Width *= -1;
                    }

                    if (tileDistance.Height < 0)
                    {
                        tileDistance.Height *= -1;
                    }

                    if (tileDistance.Width > 1 || tileDistance.Height > 1)
                    {
                        using (Pen pathPen = new Pen(civPaths.Civ.LineColor, 2))
                        {
                            g.DrawLine(pathPen, SiteToScreen(site1.Coordinates, originalSize), SiteToScreen(site2.Coordinates, originalSize));
                        }
                    }
                }
            }
        }

        private void DrawInfo(Graphics g)
        {
            Point tileLocation = WindowToTilePoint(MouseLocation);
            Font font = new Font("Arial", 10);
            Brush fontBrush = new SolidBrush(Color.Gray);
            Brush boxBrush = new SolidBrush(Color.FromArgb(200, Color.Black));

            SizeF tileInfoSize = g.MeasureString(tileLocation.X + ", " + tileLocation.Y, font);
            Rectangle tileInfoBox = new Rectangle(_controlMenu.MenuBox.Width, Height - _minimap.Height - Convert.ToInt32(tileInfoSize.Height), 70, Convert.ToInt32(tileInfoSize.Height));
            g.FillRectangle(boxBrush, tileInfoBox);
            g.DrawString(tileLocation.X + ", " + tileLocation.Y, font, fontBrush, new Point(tileInfoBox.X + 3, tileInfoBox.Y));

            SizeF yearBoxSize = g.MeasureString(CurrentYear.ToString(), font);
            Rectangle yearBox = new Rectangle(_minimap.Width, Height - _yearMenu.MenuBox.Height - Convert.ToInt32(yearBoxSize.Height), _yearMenu.MenuBox.Width, Convert.ToInt32(yearBoxSize.Height));
            g.FillRectangle(boxBrush, yearBox);
            g.DrawString(CurrentYear.ToString(), font, fontBrush, new Point(yearBox.X + 3, yearBox.Y));

            font.Dispose();
            fontBrush.Dispose();
            boxBrush.Dispose();

        }

        public void ToggleCivs()
        {
            bool reToggleWars = false;
            if (WarsToggled)
            {
                ToggleWars();
                reToggleWars = true;
            }

            if (!CivsToggled)
            {
                _displayObjects.AddRange(_world.Entities.Where(entity => entity.IsCiv && entity != FocusObject && !_displayObjects.Contains(entity)));
                CivsToggled = true;
            }
            else
            {
                _displayObjects.RemoveAll(dwarfObject => dwarfObject.GetType() == typeof(Entity) && dwarfObject != FocusObject && !_warEntities.Contains(dwarfObject));
                CivsToggled = false;
            }

            _controlMenu.Options.Single(option => option.Text == "Toggle Civs").Toggled = CivsToggled;
            GenerateCivPaths();
            if (reToggleWars)
            {
                ToggleWars();
            }

            Invalidate();

        }

        public void ToggleSites()
        {
            if (!SitesToggled)
            {
                _displayObjects.AddRange(_world.Sites.Where(site => site != FocusObject));
                SitesToggled = true;
            }
            else
            {
                _displayObjects.RemoveAll(dwarfObject => dwarfObject.GetType() == typeof(Site) && dwarfObject != FocusObject && !_focusObjects.Contains(dwarfObject));
                SitesToggled = false;
            }

            _controlMenu.Options.Single(option => option.Text == "Toggle Sites").Toggled = SitesToggled;
            Invalidate();
        }

        public void ToggleWars()
        {
            if (!WarsToggled)
            {
                List<War> displayWars = new List<War>();
                foreach (Entity entity in _displayObjects.OfType<Entity>())
                {
                    foreach (War war in entity.Wars)
                    {
                        displayWars.Add(war);
                    }
                }

                foreach (War war in displayWars)
                {
                    if (!_displayObjects.Contains(war) && war != FocusObject)
                    {
                        _displayObjects.Add(war);
                    }
                }

                WarsToggled = true;
            }
            else
            {
                _displayObjects.RemoveAll(displayObject => displayObject.GetType() == typeof(War) && displayObject != FocusObject);
                WarsToggled = false;
            }
            UpdateWarDisplay();
            _controlMenu.Options.Single(option => option.Text == "Toggle Wars").Toggled = WarsToggled;
            Invalidate();
        }

        public void ToggleBattles()
        {
            if (!BattlesToggled)
            {
                //DisplayObjects.AddRange(World.EventCollections.OfType<Battle>().Where(battle => !DisplayObjects.Contains(battle) && battle != FocusObject));
                _battles.AddRange(_world.EventCollections.OfType<Battle>().Where(battle => !_displayObjects.Contains(battle) && battle != FocusObject && !_focusObjects.Contains(battle)));
                _battleLocations = _battles.GroupBy(battle => battle.Coordinates).Select(battle => battle.Key).ToList();
                BattlesToggled = true;
            }
            else
            {
                //DisplayObjects.RemoveAll(displayObject => displayObject.GetType() == typeof(Battle) && displayObject != FocusObject);
                _battles.Clear();
                if (_focusObjects.Count > 0 && _focusObjects.First().GetType() == typeof(Battle))
                {
                    _battles.AddRange(_focusObjects.Cast<Battle>());
                }

                if (FocusObject != null && FocusObject.GetType() == typeof(Battle))
                {
                    _battles.Add(FocusObject as Battle);
                }

                if (FocusObject != null && FocusObject.GetType() == typeof(War))
                {
                    //DisplayObjects.AddRange((FocusObject as War).Collections.OfType<Battle>());
                    _battles.AddRange(((War)FocusObject).Collections.OfType<Battle>());

                }
                //BattleLocations = DisplayObjects.OfType<Battle>().GroupBy(battle => battle.Coordinates).Select(battle => battle.Key).ToList();
                _battleLocations = _battles.GroupBy(battle => battle.Coordinates).Select(battle => battle.Key).ToList();
                BattlesToggled = false;
            }
            _controlMenu.Options.Single(option => option.Text == "Toggle Battles").Toggled = BattlesToggled;
            Invalidate();
        }

        public void ToggleOverlay()
        {
            if (Overlay != null)
            {
                OverlayToggled = !OverlayToggled;
                Invalidate();
            }
            _controlMenu.Options.Single(option => option.Text == "Toggle Overlay").Toggled = OverlayToggled;
        }

        public void ToggleAlternateMap()
        {
            if (AlternateMap != null)
            {
                AlternateMapToggled = !AlternateMapToggled;
                Invalidate();
            }
            _controlMenu.Options.Single(option => option.Text == "Toggle Alt Map").Toggled = AlternateMapToggled;
            _altMapTransparency.Visible = AlternateMapToggled;
        }

        public void GenerateCivPaths()
        {
            _allCivPaths = new List<CivPaths>();
            foreach (Entity civ in _displayObjects.OfType<Entity>())
            {
                CivPaths civPaths = new CivPaths
                {
                    Civ = civ,
                    SitePaths = PathMaker.Create(civ, CurrentYear)
                };
                _allCivPaths.Add(civPaths);
            }
        }

        public void ChangeYear(int amount)
        {
            CurrentYear += amount;
            if (CurrentYear > MaxYear)
            {
                CurrentYear = MaxYear;
            }

            if (CurrentYear < MinYear)
            {
                CurrentYear = MinYear;
            }

            UpdateWarDisplay();
            GenerateCivPaths();
            Invalidate();
        }

        public void UpdateWarDisplay()
        {
            _displayObjects.RemoveAll(displayObject => _warEntities.Contains(displayObject));
            _warEntities.Clear();
            foreach (War war in _displayObjects.OfType<War>().Where(war => war.StartYear <= CurrentYear && (war.EndYear >= CurrentYear || war.EndYear == -1)))
            {
                if (war.Attacker.Parent != null)
                {
                    if (!_displayObjects.Contains(war.Attacker.Parent))
                    {
                        _warEntities.Add(war.Attacker.Parent);
                    }
                }
                else if (!_displayObjects.Contains(war.Attacker))
                {
                    _warEntities.Add(war.Attacker);
                }

                if (war.Defender.Parent != null)
                {
                    if (!_displayObjects.Contains(war.Defender.Parent))
                    {
                        _warEntities.Add(war.Defender.Parent);
                    }
                }
                else if (!_displayObjects.Contains(war.Defender))
                {
                    _warEntities.Add(war.Defender);
                }
            }
            if (FocusObject != null && (FocusObject.GetType() == typeof(Battle) || FocusObject.GetType() == typeof(SiteConquered)))
            {
                Battle battle;
                if (FocusObject.GetType() == typeof(Battle))
                {
                    battle = FocusObject as Battle;
                }
                else
                {
                    battle = ((SiteConquered)FocusObject).Battle;
                }

                if (battle != null)
                {
                    if (battle.Attacker.Parent != null)
                    {
                        if (!_displayObjects.Contains(battle.Attacker.Parent))
                        {
                            _warEntities.Add(battle.Attacker.Parent);
                        }
                    }
                    else if (!_displayObjects.Contains(battle.Attacker))
                    {
                        _warEntities.Add(battle.Attacker);
                    }

                    if (battle.Defender.Parent != null)
                    {
                        if (!_displayObjects.Contains(battle.Defender.Parent))
                        {
                            _warEntities.Add(battle.Defender.Parent);
                        }
                    }
                    else if (!_displayObjects.Contains(battle.Defender))
                    {
                        _warEntities.Add(battle.Defender);
                    }
                }
            }
            _displayObjects.AddRange(_warEntities);
            GenerateCivPaths();
        }

        public void MakeOverlay(string overlay)
        {
            List<Location> coordinatesList = new List<Location>();
            List<int> occurences = new List<int>();
            _optionsMenu.Options.Single(option => option.Text == "Overlays").SubMenu.Options.ForEach(option => option.Toggled = false);
            _optionsMenu.Options.Single(option => option.Text == "Overlays").SubMenu.Options.Single(option => option.Text == overlay).Toggled = true;
            switch (overlay)
            {
                case "Battles":
                    _world.EventCollections.OfType<Battle>().Select(battle => battle.Coordinates).ToList().ForEach(coordinates => coordinatesList.Add(coordinates));
                    break;
                case "Battles (Notable)":
                    _world.EventCollections.OfType<Battle>().Where(battle => battle.Notable).Select(battle => battle.Coordinates).ToList().ForEach(coordinates => coordinatesList.Add(coordinates));
                    break;
                case "Battle Deaths":
                    foreach (Location coordinates in _world.EventCollections.OfType<Battle>().GroupBy(battle => battle.Coordinates).Select(battle => battle.Key).ToList())
                    {
                        coordinatesList.Add(coordinates);
                        occurences.Add(_world.EventCollections.OfType<Battle>().Where(battle => battle.Coordinates == coordinates).Sum(battle => battle.AttackerDeathCount + battle.DefenderDeathCount));
                    }
                    break;
                case "Site Population...":
                    DlgPopulation selectPopulations = new DlgPopulation(_world);
                    selectPopulations.ShowDialog();
                    if (selectPopulations.SelectedPopulations.Count == 0)
                    {
                        return;
                    }

                    foreach (Location coordinates in _world.Sites.GroupBy(site => site.Coordinates).Select(site => site.Key).ToList())
                    {
                        coordinatesList.Add(coordinates);
                        occurences.Add(_world.Sites.Where(site => site.Coordinates == coordinates).Sum(site => site.Populations.Where(population => selectPopulations.SelectedPopulations.Contains(population.Race)).Sum(population => population.Count)));
                    }
                    break;
                case "Site Events":
                    foreach (Location coordinates in _world.Sites.GroupBy(site => site.Coordinates).Select(site => site.Key).ToList())
                    {
                        coordinatesList.Add(coordinates);
                        occurences.Add(_world.Sites.Where(site => site.Coordinates == coordinates).Sum(site => site.Events.Count()));
                    }
                    break;
                case "Site Events (Filtered)":
                    foreach (Location coordinates in _world.Sites.GroupBy(site => site.Coordinates).Select(site => site.Key).ToList())
                    {
                        coordinatesList.Add(coordinates);
                        occurences.Add(_world.Sites.Where(site => site.Coordinates == coordinates).Sum(site => site.Events.Count(dEvent => !Legends.WorldObjects.Site.Filters.Contains(dEvent.Type))));
                    }
                    break;
                case "Beast Attacks":
                    _world.EventCollections.OfType<BeastAttack>().Select(attack => attack.Coordinates).ToList().ForEach(coordinates => coordinatesList.Add(coordinates));
                    break;
            }

            for (int i = 0; i < coordinatesList.Count; i++)
            {
                coordinatesList[i] = new Location(coordinatesList[i].X * TileSize + TileSize / 2, coordinatesList[i].Y * TileSize + TileSize / 2);
            }

            if (Overlay != null)
            {
                Overlay.Dispose();
            }

            if (occurences.Count > 0)
            {
                Overlay = HeatMapMaker.Create(_map.Width, _map.Height, coordinatesList, occurences);
            }
            else
            {
                Overlay = HeatMapMaker.Create(_map.Width, _map.Height, coordinatesList);
            }

            OverlayToggled = false;
            ToggleOverlay();
            GC.Collect();
        }

        public void ExportMap()
        {
            SaveFileDialog exportMapDialog =
                new SaveFileDialog
                {
                    Filter = "Png Image (.png)|*.png|JPEG Image (.jpeg)|*.jpeg|Gif Image (.gif)|*.gif|Bitmap Image (.bmp)|*.bmp|Tiff Image (.tiff)|*.tiff|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

            if (exportMapDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap map = new Bitmap(_world.Map.Height, _world.Map.Width);

                using (Graphics mapGraphics = Graphics.FromImage(map))
                {
                    mapGraphics.PixelOffsetMode = PixelOffsetMode.Half;
                    DrawMap(mapGraphics, true);
                    DrawDisplayObjects(mapGraphics, true);
                    DrawOverlay(mapGraphics, true);
                    using (Stream exportStream = exportMapDialog.OpenFile())
                    {
                        switch (exportMapDialog.FilterIndex)
                        {
                            case 1:
                                map.Save(exportStream, ImageFormat.Png);
                                break;
                            case 2:
                                map.Save(exportStream, ImageFormat.Jpeg);
                                break;
                            case 3:
                                map.Save(exportStream, ImageFormat.Gif);
                                break;
                            case 4:
                                map.Save(exportStream, ImageFormat.Bmp);
                                break;
                            case 5:
                                map.Save(exportStream, ImageFormat.Tiff);
                                break;
                            default:
                                map.Save(exportStream, ImageFormat.Png);
                                break;
                        }
                        exportStream.Close();
                    }
                }
            }
        }

        public void ChangeMap()
        {
            OpenFileDialog openMap = new OpenFileDialog();
            openMap.ShowDialog();
            string fileName;
            bool deleteFile = false;
            if (openMap.FileName != "" && !openMap.FileName.EndsWith(".bmp") && !openMap.FileName.EndsWith(".png") && !openMap.FileName.EndsWith(".jpeg") && !openMap.FileName.EndsWith(".jpg"))
            {
                deleteFile = true;
                using (SevenZipExtractor extractor = new SevenZipExtractor(openMap.FileName))
                {
                    if (extractor.ArchiveFileNames.Count(file => file.EndsWith(".bmp") || file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".jpeg")) == 0)
                    {
                        MessageBox.Show("No Image Files Found");
                        return;
                    }
                    if (extractor.ArchiveFileNames.Count(file => file.EndsWith(".bmp") || file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".jpeg")) == 1)
                    {
                        fileName = extractor.ArchiveFileNames.Single(file => file.EndsWith(".bmp") || file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".jpeg"));
                    }
                    else
                    {
                        DlgFileSelect fileSelect = new DlgFileSelect(extractor.ArchiveFileNames.Where(file => file.EndsWith(".bmp") || file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".jpeg")).ToList());
                        fileSelect.ShowDialog();
                        if (fileSelect.SelectedFile == "")
                        {
                            return;
                        }

                        if (File.Exists(fileSelect.SelectedFile)) { MessageBox.Show(fileSelect.SelectedFile + " already exists."); return; }
                        fileName = fileSelect.SelectedFile;
                    }
                    extractor.ExtractFiles(Directory.GetCurrentDirectory(), fileName);
                }
            }
            else
            {
                fileName = openMap.FileName;
            }

            if (fileName != "")
            {
                using (FileStream mapStream = new FileStream(fileName, FileMode.Open))
                {
                    AlternateMap = new Bitmap(mapStream);
                }

                if (AlternateMap.Size != _map.Size)
                {
                    Bitmap resized = new Bitmap(_map.Width, _map.Height);
                    using (Graphics resize = Graphics.FromImage(resized))
                    {
                        resize.SmoothingMode = SmoothingMode.AntiAlias;
                        resize.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        resize.DrawImage(AlternateMap, new Rectangle(0, 0, _map.Width, _map.Height));
                        AlternateMap.Dispose();
                        AlternateMap = resized;
                    }
                }
                MapUtil.AlternateMap = AlternateMap;
                if (MapUtil.AltMapAlpha == 0)
                {
                    MapUtil.AltMapAlpha = 1;
                }
                AlternateMapToggled = false;
                ToggleAlternateMap();
            }

            if (deleteFile)
            {
                File.Delete(fileName);
            }
        }

        private void ChangeAltMapTransparency(object sender, EventArgs e)
        {
            _altMapAlpha = _altMapTransparency.Value / 100.0f;
            MapUtil.AltMapAlpha = _altMapAlpha;
            Invalidate();
        }

        private PointF SiteToScreen(Location siteCoordinates, bool originalSize = false)
        {
            var rectangle = originalSize ? getOriginalSize(_map) : Source;
            PointF screenCoordinates = new PointF
            {
                X = (float)((siteCoordinates.X * TileSize - rectangle.X + TileSize / 2) * PixelWidth),
                Y = (float)((siteCoordinates.Y * TileSize - rectangle.Y + TileSize / 2) * PixelHeight)
            };
            return screenCoordinates;
        }

        private void Pan(int xChange, int yChange)
        {
            Source.Offset(xChange, yChange);
            Center.Offset(xChange, yChange);
            _hoverMenu.Open = false;
            if (xChange > 2 || xChange < -2 || yChange > 2 || yChange < -2)
            {
                _optionsMenu.SelectedOption = null;
            }

            Invalidate();
        }

        public void ZoomIn()
        {
            if (ZoomCurrent > ZoomMin)
            {
                ZoomCurrent -= ZoomCurrent * ZoomChange;
            }

            UpdateScales();
            Invalidate();
        }

        public void ZoomOut()
        {
            if (ZoomCurrent < ZoomMax)
            {
                ZoomCurrent += ZoomCurrent * ZoomChange;
            }

            UpdateScales();
            Invalidate();
        }

        private void ZoomToBounds()
        {
            ZoomCurrent = 1.0;
            while (ZoomCurrent > 0.85)
            {
                ZoomCurrent -= ZoomCurrent * ZoomChange; //Zoom in to set zoom scale
            }

            ZoomCurrent += ZoomCurrent * ZoomChange;
            UpdateScales();
            while (true) //Zoom out till bounds are within the map source
            {
                if (Source.X <= _zoomBounds.X && Source.Right >= _zoomBounds.Width && Source.Y <= _zoomBounds.Y && Source.Bottom >= _zoomBounds.Height)
                {
                    break;
                }

                ZoomCurrent += ZoomCurrent * ZoomChange;
                if (ZoomCurrent > ZoomMax)
                {
                    break;
                }

                UpdateScales();
            }
            ZoomToBoundsOnFirstPaint = false;
        }

        private void UpdateScales()
        {
            Source.Location = new Point(Center.X - Convert.ToInt32(Width / 2 * ZoomCurrent), Center.Y - Convert.ToInt32(Height / 2 * ZoomCurrent));
            Source.Size = new Size(Convert.ToInt32(Width * ZoomCurrent), Convert.ToInt32(Height * ZoomCurrent));
            PixelWidth = Convert.ToDouble(Width) / Source.Width;
            PixelHeight = Convert.ToDouble(Height) / Source.Height;
            _hoverMenu.Open = false;
            _controlMenu.MenuBox.Location = new Point(0, Height - _minimap.Height - _controlMenu.MenuBox.Height);
            _yearMenu.MenuBox.Location = new Point(_minimap.Width, Height - _yearMenu.MenuBox.Height);
            _altMapTransparency.Location = new Point(MiniMapAreaSideLength + _yearMenu.MenuBox.Width, Height - _altMapTransparency.Height);
        }

        private Point WindowToWorldPoint(Point window)
        {
            int x = Source.X + Convert.ToInt32(window.X * ZoomCurrent);
            int y = Source.Y + Convert.ToInt32(window.Y * ZoomCurrent);
            return new Point(x, y);
        }

        public Point WindowToTilePoint(Point window)
        {
            Point tilePoint = WindowToWorldPoint(window);
            tilePoint.X = tilePoint.X / TileSize;
            tilePoint.Y = tilePoint.Y / TileSize;
            return tilePoint;
        }

        public Location WindowToWorldTile(Point window)
        {
            Point location = WindowToTilePoint(window);
            return new Location(location.X, location.Y);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            UpdateScales();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseClickLocation = e.Location;
            }

            if (e.Button == MouseButtons.Left)
            {
                MousePanStart = e.Location;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Location.X > MouseClickLocation.X - 5 && e.Location.X < MouseClickLocation.X + 5
                && e.Location.Y > MouseClickLocation.Y - 5 && e.Location.Y < MouseClickLocation.Y + 5)//	e.Location == MouseClickLocation)
            {
                if (_controlMenu.SelectedOption != null)
                {
                    _controlMenu.Click(e.X, e.Y);
                }
                else if (_yearMenu.SelectedOption != null)
                {
                    _yearMenu.Click(e.X, e.Y);
                }
                else if (_optionsMenu.SelectedOption != null)
                {
                    _optionsMenu.Click(e.X, e.Y);
                }
                else if (_hoverMenu.Options.Count == 1 && _hoverMenu.Options.Count(option => option.SubMenu.Options.Count(option2 => option2.SubMenu != null) > 0) == 0)
                {
                    _hoverMenu.Options.First().Click();
                }
                else if (!_hoverMenu.Open && _hoverMenu.Options.Count > 0)
                {
                    _hoverMenu.Open = true;
                }
                else if (_hoverMenu.Open)
                {
                    _hoverMenu.Click(e.X, e.Y);
                    _hoverMenu.Open = false;
                }
                else
                {
                    _hoverMenu.Open = false;
                }

                _optionsMenu.SelectedOption = null;
            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!_controlMenu.HighlightOption(e.X, e.Y) && !_yearMenu.HighlightOption(e.X, e.Y) && !_optionsMenu.HighlightOption(e.X, e.Y) && !_hoverMenu.Open)
            {
                List<object> addOptions = new List<object>();
                Location tile = WindowToWorldTile(e.Location);
                foreach (Site site in _displayObjects.OfType<Entity>().SelectMany(entity => entity.Sites).Where(site => site.Coordinates == tile).Distinct())
                {
                    OwnerPeriod ownerPeriod = site.OwnerHistory.LastOrDefault(op => op.StartYear <= CurrentYear && op.EndYear >= CurrentYear || op.EndYear == -1);
                    if(ownerPeriod != null && ownerPeriod.Owner is Entity entity)
                    {
                        addOptions.Add(entity);
                        addOptions.Add(ownerPeriod.Site);
                    }
                }

                foreach (Site site in _displayObjects.OfType<Site>().Where(site => site.Coordinates == tile && !addOptions.Contains(site)))
                {
                    addOptions.Add(site);
                }

                if (FocusObject != null && FocusObject.GetType() == typeof(Battle) && ((Battle)FocusObject).Coordinates == tile)
                {
                    addOptions.Add(FocusObject as Battle);
                }

                foreach (Battle battle in _displayObjects.OfType<War>().SelectMany(war => war.Collections).OfType<Battle>().Where(battle => battle != FocusObject && battle.StartYear == CurrentYear && battle.Coordinates == tile))
                {
                    addOptions.Add(battle);
                }

                if (BattlesToggled || _battles.Count > 0)
                {
                    if (_battleLocations.Any(location => location == tile))
                    {
                        List<Battle> battles = _battles.Where(battle => battle.Coordinates == tile).ToList();
                        if (battles.Count > 0)
                        {
                            addOptions.Add(battles);
                        }
                    }
                }

                _hoverMenu.AddOptions(addOptions);
                if (_hoverMenu.Options.Count > 0)
                {
                    _hoverMenu.MenuBox.Location = WindowToTilePoint(e.Location);
                    _hoverMenu.MenuBox.X = Convert.ToInt32(((_hoverMenu.MenuBox.X + 1) * TileSize - Source.X) * PixelWidth);
                    _hoverMenu.MenuBox.Y = Convert.ToInt32(((_hoverMenu.MenuBox.Y + 1) * TileSize - Source.Y) * PixelHeight);
                    Invalidate();
                }
            }
            else
            {
                _hoverMenu.HighlightOption(e.X, e.Y);
            }

            if (_optionsMenu.SelectedOption != null || _controlMenu.SelectedOption != null || _yearMenu.SelectedOption != null)
            {
                _hoverMenu.Options.Clear();
            }

            if (e.Button == MouseButtons.Left)
            {
                Point worldStart = WindowToWorldPoint(new Point(MousePanStart.X, MousePanStart.Y));
                Point worldEnd = WindowToWorldPoint(new Point(e.X, e.Y));
                int xChange = worldStart.X - worldEnd.X;
                int yChange = worldStart.Y - worldEnd.Y;

                Pan(xChange, yChange);
                MousePanStart = e.Location;
            }

            MouseLocation = e.Location;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Focus();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {

            if (e.Delta > 0)
            {
                if (ModifierKeys == (Keys.Control | Keys.Shift))
                {
                    ChangeYear(100);
                }
                else if (ModifierKeys == Keys.Control)
                {
                    ChangeYear(1);
                }
                else if (ModifierKeys == Keys.Shift)
                {
                    ChangeYear(10);
                }
                else
                {
                    ZoomIn();
                }
            }
            else if (e.Delta < 0)
            {
                if (ModifierKeys == (Keys.Control | Keys.Shift))
                {
                    ChangeYear(-100);
                }
                else if (ModifierKeys == Keys.Control)
                {
                    ChangeYear(-1);
                }
                else if (ModifierKeys == Keys.Shift)
                {
                    ChangeYear(-10);
                }
                else
                {
                    ZoomOut();
                }
            }
        }

        public static List<Bitmap> CreateBitmaps(World world, DwarfObject dwarfObject)
        {
            int maxSize = world.PageMiniMap.Height > world.PageMiniMap.Width ? world.PageMiniMap.Height : world.PageMiniMap.Width;
            MapPanel createBitmaps = new MapPanel(world.Map, world, null, dwarfObject)
            {
                Height = maxSize,
                Width = maxSize
            };
            if (createBitmaps.ZoomToBoundsOnFirstPaint)
            {
                createBitmaps.ZoomToBounds();
            }

            createBitmaps.MiniMapAreaSideLength = maxSize;
            createBitmaps._minimap = world.PageMiniMap;

            Bitmap miniMap = new Bitmap(maxSize, maxSize);

            using (Graphics miniMapGraphics = Graphics.FromImage(miniMap))
            {
                createBitmaps.DrawMiniMap(miniMapGraphics);
            }

            Bitmap map = new Bitmap(maxSize, maxSize);

            using (Graphics mapGraphics = Graphics.FromImage(map))
            {
                mapGraphics.PixelOffsetMode = PixelOffsetMode.Half;
                createBitmaps.DrawMap(mapGraphics);
                createBitmaps.DrawDisplayObjects(mapGraphics);
                createBitmaps.Dispose();
                return new List<Bitmap> { map, miniMap };
            }
        }

        private Site GetClosestSite(Entity civ, Location coordinates)
        {
            double closestDistance = double.MaxValue;
            Site closestSite = null;
            foreach (OwnerPeriod period in civ.SiteHistory.Where(ownerPeriod => ownerPeriod.StartYear <= CurrentYear && ownerPeriod.EndYear >= CurrentYear || ownerPeriod.EndYear == -1))
            {
                int rise = period.Site.Coordinates.Y - coordinates.Y;
                int run = period.Site.Coordinates.X - coordinates.X;
                double distance = Math.Sqrt(Math.Pow(rise, 2) + Math.Pow(run, 2));
                if (distance < closestDistance)
                {
                    closestSite = period.Site;
                    closestDistance = distance;
                }
            }
            return closestSite;
        }
    }
}