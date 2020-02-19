using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using LegendsViewer.Controls.HTML;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.Map
{
    public class MapMenu
    {
        public readonly MapPanel Map;
        public readonly List<MapMenuOption> Options = new List<MapMenuOption>();

        public const string FontFamily = "Segoe UI Symbol";
        public const int FontSize = 9;
        private const int XBuffer = 5;
        protected const int OptionHeight = 18;

        public Color MenuColor = Color.LightGray;
        protected Color FontColor = Color.Black;
        protected Color ToggleColor = Color.FromArgb(150, 150, 150);
        public Rectangle MenuBox = new Rectangle(0, 0, 0, 0);
        public bool Open;
        public MapMenuOption SelectedOption;

        public MapMenu(MapPanel map)
        {
            Map = map;
        }

        public void AddOptions(List<object> addOptions)
        {
            Options.Clear();
            addOptions = addOptions.Distinct().ToList(); //remove duplicates
            foreach (Entity civ in addOptions.OfType<Entity>())
            {
                Options.Add(new MapMenuOption(this, civ));
            }

            foreach (Site site in addOptions.OfType<Site>())
            {
                Options.Add(new MapMenuOption(this, site));
            }

            foreach (Battle battle in addOptions.OfType<Battle>())
            {
                Options.Add(new MapMenuOption(this, battle));
            }

            foreach (List<Battle> battles in addOptions.OfType<List<Battle>>())
            {
                Options.Add(new MapMenuOption(this, battles));
            }

            foreach (string text in addOptions.OfType<string>())
            {
                Options.Add(new MapMenuOption(this, text));
            }

            CalculateSize();
        }

        public void AddOption(object option)
        {
            Options.Add(new MapMenuOption(this, option));
            CalculateSize();
        }

        protected virtual void CalculateSize()
        {
            MenuBox.Width = 0;
            foreach (MapMenuOption option in Options)
            {
                SizeF fontSize;
                using (Bitmap measure = new Bitmap(1, 1))
                using (Graphics g = Graphics.FromImage(measure))
                using (Font menuFont = new Font(FontFamily, FontSize))
                {
                    fontSize = g.MeasureString(option.Text, menuFont);
                }

                if (fontSize.Width > MenuBox.Width)
                {
                    MenuBox.Width = Convert.ToInt32(fontSize.Width) + XBuffer * 2;
                }
            }
            MenuBox.Height = Options.Count * OptionHeight;
        }

        public virtual void Draw(Graphics g)
        {
            if (MenuBox.Right > Map.Right)
            {
                MenuBox.X -= MenuBox.Width;
            }

            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(100, Color.Black)))
            {
                g.FillRectangle(shadowBrush, MenuBox.X + 3, MenuBox.Y + 3, MenuBox.Width, MenuBox.Height);
            }

            using (SolidBrush menuBrush = new SolidBrush(MenuColor))
            {
                g.FillRectangle(menuBrush, MenuBox);
            }

            using (Pen shadowPen = new Pen(Color.FromArgb(100, Color.Black)))
            {
                g.DrawLine(shadowPen, MenuBox.Left, MenuBox.Top, MenuBox.Left, MenuBox.Bottom);
                g.DrawLine(shadowPen, MenuBox.Left, MenuBox.Top, MenuBox.Right, MenuBox.Top);
            }

            foreach (MapMenuOption option in Options)
            {
                int optionYPosition = MenuBox.Y + OptionHeight * Options.IndexOf(option);
                if (option == SelectedOption && option.Selectable)
                {
                    using (SolidBrush invertMenu = new SolidBrush(FontColor))
                    {
                        g.FillRectangle(invertMenu, MenuBox.X, optionYPosition, MenuBox.Width, OptionHeight);
                    }

                    using (Font menuFont = new Font(FontFamily, FontSize))
                    using (SolidBrush invertFont = new SolidBrush(MenuColor))
                    {
                        g.DrawString(option.Text, menuFont, invertFont, MenuBox.X + XBuffer, optionYPosition);
                    }
                }
                else
                {
                    if (option.Toggled)
                    {
                        using (SolidBrush toggleBrush = new SolidBrush(ToggleColor))
                        {
                            g.FillRectangle(toggleBrush, MenuBox.X, optionYPosition, MenuBox.Width, OptionHeight);
                        }
                    }

                    using (Font menuFont = new Font(FontFamily, FontSize))
                    using (SolidBrush fontBrush = new SolidBrush(FontColor))
                    {
                        g.DrawString(option.Text, menuFont, fontBrush, MenuBox.X + XBuffer, optionYPosition);
                    }
                }

                if (option.SubMenu != null)
                {
                    using (AdjustableArrowCap subArrow = new AdjustableArrowCap(OptionHeight - 12, 3))
                    using (Pen arrowPen = new Pen(FontColor))
                    {
                        arrowPen.CustomEndCap = subArrow;
                        if (option == SelectedOption)
                        {
                            arrowPen.Color = MenuColor;
                        }

                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLine(arrowPen, MenuBox.Right - subArrow.Height - 2, optionYPosition + OptionHeight / 2, MenuBox.Right - 2, optionYPosition + OptionHeight / 2);
                    }
                }
            }

            MapMenuOption autoShowSubMenu = null;
            if (Options.Count(option => option.OptionObject.GetType() == typeof(Battle)) == 1)
            {
                autoShowSubMenu = Options.First(option => option.OptionObject.GetType() == typeof(Battle));
            }
            else if (Options.Count(option => option.OptionObject.GetType() == typeof(List<Battle>)) > 0)
            {
                autoShowSubMenu = Options.First(option => option.OptionObject.GetType() == typeof(List<Battle>));
            }
            else if (Options.Count(option1 => option1.OptionObject.GetType() == typeof(Site)) == 1)
            {
                autoShowSubMenu = Options.First(option1 => option1.OptionObject.GetType() == typeof(Site));
            }
            else if (Options.Count(option1 => option1.OptionObject.GetType() == typeof(War)) == 1)
            {
                autoShowSubMenu = Options.First(option1 => option1.OptionObject.GetType() == typeof(War));
            }

            foreach (MapMenuOption option in Options)
            {
                int optionYPosition = MenuBox.Y + OptionHeight * Options.IndexOf(option);

                if (option.SubMenu != null)
                {
                    if (option == SelectedOption ||
                       !Open && (option == autoShowSubMenu || Options.Count == 1))
                    {
                        option.SubMenu.MenuBox.Location = new Point(MenuBox.Right, optionYPosition);
                        if (option.SubMenu.MenuBox.Right > Map.Right)
                        {
                            option.SubMenu.MenuBox.X = MenuBox.X - option.SubMenu.MenuBox.Width;
                        }

                        option.SubMenu.Draw(g);
                    }
                }
            }
        }

        public virtual bool HighlightOption(int x, int y)
        {
            if (SelectedOption?.SubMenu != null && SelectedOption.SubMenu.HighlightOption(x, y)) { }
            else if (x >= MenuBox.Left && x <= MenuBox.Right && y >= MenuBox.Top && y <= MenuBox.Bottom && Options.Count > 0)
            {
                SelectedOption = Options[(y - MenuBox.Top - 1) / OptionHeight];
            }
            else
            {
                SelectedOption = null;
            }

            Map.Invalidate();
            if (SelectedOption != null)
            {
                return true;
            }

            return false;
        }

        public void Click(int x, int y)
        {
            if (x >= MenuBox.Left && x <= MenuBox.Right && y >= MenuBox.Top && y <= MenuBox.Bottom)
            {
                SelectedOption.Click();
            }
            else if (SelectedOption != null && SelectedOption.SubMenu != null)
            {
                SelectedOption.SubMenu.Click(x, y);
            }
        }
    }

    public class MapMenuOption
    {
        public MapMenu Parent;
        public string Text;
        public Object OptionObject;

        //OnClick
        //OnHover
        public bool Toggled = false, Selectable = true;

        public MapMenu SubMenu;
        public int Width;

        public MapMenuOption(MapMenu parent, object itemObject = null)
        {
            Parent = parent;
            OptionObject = itemObject;
            if (OptionObject != null)
            {
                if (OptionObject is string s)
                {
                    Text = s;
                }

                if (OptionObject is Entity entity)
                {
                    Text = entity.Name + " (" + entity.Race.NamePlural + ")";
                }

                if (OptionObject is List<Battle> battles)
                {
                    Text = "Battles: " + battles.Count;
                    SubMenu = new MapMenu(Parent.Map);
                    SubMenu.AddOption("Total Deaths: " + battles.Sum(b => b.AttackerDeathCount + b.DefenderDeathCount));
                    SubMenu.Options.Last().Selectable = false;
                    SubMenu.AddOption("Notable: " + battles.Count(b => b.Notable));
                    SubMenu.Options.Last().Selectable = false;
                    if (battles.Count <= 10)
                    {
                        foreach (Battle b in battles)
                        {
                            SubMenu.AddOption(b);
                        }
                    }
                    else
                    {
                        SubMenu.AddOption("Load Battles");
                        SubMenu.Options.Last().OptionObject = battles;
                    }
                }
                if (OptionObject is Battle battle)
                {
                    Text = battle.Name + " (Battle)";
                    SubMenu = new MapMenu(Parent.Map);
                    SubMenu.AddOption("Year: " + battle.StartYear);
                    SubMenu.AddOption(battle.Attacker.Name + " (A)");
                    SubMenu.Options.Last().OptionObject = battle.Attacker;
                    SubMenu.AddOption(battle.Defender.Name + " (D)");
                    SubMenu.Options.Last().OptionObject = battle.Defender;
                    SubMenu.AddOption("Combatants: " + (battle.NotableAttackers.Count + battle.AttackerSquads.Sum(squad => squad.Numbers)) + " / " + (battle.NotableDefenders.Count + battle.DefenderSquads.Sum(squad => squad.Numbers)) + "    Kills: " + battle.DefenderDeathCount + " / " + battle.AttackerDeathCount);
                    SubMenu.Options.ForEach(option => option.Selectable = false);
                    if (battle.ParentCollection != null)
                    {
                        SubMenu.AddOption(battle.ParentCollection);
                    }
                    //SubMenu.AddOption("Deaths: " + (battle.AttackerDeathCount + battle.DefenderDeathCount));
                }
                if (OptionObject is War war)
                {
                    Text = war.Name + " (War)";
                    SubMenu = new MapMenu(Parent.Map);
                    string warYear = war.StartYear + " to ";
                    if (war.EndYear == -1)
                    {
                        warYear += "Present";
                    }
                    else
                    {
                        warYear += war.EndYear;
                    }

                    SubMenu.AddOption(warYear + " | Total Deaths: " + war.Collections.OfType<Battle>().Sum(b => b.AttackerDeathCount + b.DefenderDeathCount));
                    SubMenu.AddOption(war.Attacker + " (A)");
                    SubMenu.AddOption(war.Defender + " (D)");
                    SubMenu.AddOption("Victories: " + war.AttackerVictories.OfType<Battle>().Count() + " / " + war.DefenderVictories.OfType<Battle>().Count() + "    Kills: " + war.DefenderDeathCount + " / " + war.AttackerDeathCount);
                    SubMenu.Options.ForEach(option => option.Selectable = false);
                }
                if (OptionObject is Site site)
                {
                    Text = site.Name + " (" + site.Type + ")";
                    List<object> details = new List<object>
                    {
                        site.UntranslatedName
                    };
                    if (site.Populations.Count > 0)
                    {
                        details.Add("Populations: ");
                        foreach (Population population in site.Populations)
                        {
                            if (population.Count == 1)
                            {
                                details.Add("     " + population.Count + " " + population.Race.NameSingular);
                            }
                            else
                            {
                                details.Add("     " + population.Count + " " + population.Race.NamePlural);
                            }
                        }
                    }
                    SubMenu = new MapMenu(Parent.Map);
                    SubMenu.AddOptions(details);
                    foreach (MapMenuOption option in SubMenu.Options)
                    {
                        option.Selectable = false;
                    }
                }
            }

            SizeF stringSize;
            using (Bitmap temp = new Bitmap(1, 1))
            using (Graphics measure = Graphics.FromImage(temp))
            using (Font font = new Font(MapMenu.FontFamily, MapMenu.FontSize))
            {
                stringSize = measure.MeasureString(Text, font);
            }

            Width = Convert.ToInt32(stringSize.Width) + 5;
        }

        public void Click()
        {
            if (OptionObject is List<Battle> battles)
            {
                (Parent.Map.TabControl.Parent as FrmLegendsViewer)?.ChangeBattleBaseList(battles, "Map Battles");
            }
            else if (!(OptionObject is string))
            {
                Parent.Map.TabControl.Navigate(ControlOption.Html, OptionObject);
            }
            else if ((string) OptionObject != Text)
            {
                if ((string) OptionObject == "Overlay")
                {
                    Parent.Map.MakeOverlay(Text);
                }
            }
            else
            {
                switch (Text)
                {
                    case "Toggle Civs": Parent.Map.ToggleCivs(); break;
                    case "Toggle Sites": Parent.Map.ToggleSites(); break;
                    case "Toggle Battles": Parent.Map.ToggleBattles(); break;
                    case "Toggle Wars": Parent.Map.ToggleWars(); break;
                    case "Toggle Overlay": Parent.Map.ToggleOverlay(); break;
                    case "Toggle Alt Map": Parent.Map.ToggleAlternateMap(); break;
                    case "Load Alternate Map...": Parent.Map.ChangeMap(); break;
                    case "Export Map...": Parent.Map.ExportMap(); break;
                    case "Zoom In": Parent.Map.ZoomIn(); break;
                    case "Zoom Out": Parent.Map.ZoomOut(); break;
                    case "+1000": Parent.Map.ChangeYear(1000); break;
                    case "+100": Parent.Map.ChangeYear(100); break;
                    case "+10": Parent.Map.ChangeYear(10); break;
                    case "+1": Parent.Map.ChangeYear(1); break;
                    case "-1": Parent.Map.ChangeYear(-1); break;
                    case "-10": Parent.Map.ChangeYear(-10); break;
                    case "-100": Parent.Map.ChangeYear(-100); break;
                    case "-1000": Parent.Map.ChangeYear(-1000); break;
                }
            }
        }
    }

    public class MapMenuHorizontal : MapMenu
    {
        public MapMenuHorizontal(MapPanel map) : base(map)
        {
        }

        protected override void CalculateSize()
        {
            foreach (MapMenuOption option in Options)
            {
                SizeF fontSize;
                using (Bitmap measure = new Bitmap(1, 1))
                using (Graphics g = Graphics.FromImage(measure))
                using (Font menuFont = new Font(FontFamily, FontSize))
                {
                    fontSize = g.MeasureString(option.Text, menuFont);
                }

                MenuBox.Width += Convert.ToInt32(fontSize.Width) + 5;
            }

            MenuBox.Height = OptionHeight;
        }

        public override bool HighlightOption(int x, int y)
        {
            if (x >= MenuBox.Left && x <= MenuBox.Right && y >= MenuBox.Top && y <= MenuBox.Bottom)
            {
                int xPosition = MenuBox.X;
                SelectedOption = null;
                foreach (MapMenuOption option in Options)
                {
                    if (x > xPosition && x < xPosition + option.Width)
                    {
                        SelectedOption = option;
                        break;
                    }
                    xPosition += option.Width;
                }
            }
            else if (SelectedOption?.SubMenu != null)
            {
                SelectedOption.SubMenu.HighlightOption(x, y);
            }
            else
            {
                SelectedOption = null;
            }

            Map.Invalidate();
            if (SelectedOption != null)
            {
                return true;
            }

            return false;
        }

        public override void Draw(Graphics g)
        {
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(100, Color.Black)))
            {
                g.FillRectangle(shadowBrush, MenuBox.X + 3, MenuBox.Y + 3, MenuBox.Width, MenuBox.Height);
            }

            using (SolidBrush menuBrush = new SolidBrush(MenuColor))
            {
                g.FillRectangle(menuBrush, MenuBox);
            }

            int optionXPosition = MenuBox.X;
            foreach (MapMenuOption option in Options)
            {
                SizeF stringSize;
                using (Font menuFont = new Font(FontFamily, FontSize))
                {
                    stringSize = g.MeasureString(option.Text, menuFont);
                }

                int optionWidth = Convert.ToInt32(stringSize.Width) + 5;
                if (option == SelectedOption)
                {
                    using (SolidBrush invertMenu = new SolidBrush(FontColor))
                    {
                        g.FillRectangle(invertMenu, optionXPosition, MenuBox.Y, optionWidth, OptionHeight);
                    }

                    using (Font menuFont = new Font(FontFamily, FontSize))
                    using (SolidBrush invertFont = new SolidBrush(MenuColor))
                    {
                        g.DrawString(option.Text, menuFont, invertFont, optionXPosition + 5, MenuBox.Y);
                    }

                    if (option.SubMenu != null)
                    {
                        option.SubMenu.MenuBox.Location = new Point(optionXPosition, MenuBox.Bottom);
                        option.SubMenu.Draw(g);
                    }
                }
                else
                {
                    if (option.Toggled)
                    {
                        using (SolidBrush toggleBrush = new SolidBrush(ToggleColor))
                        {
                            g.FillRectangle(toggleBrush, optionXPosition, MenuBox.Y, optionWidth, OptionHeight);
                        }
                    }

                    using (Font menuFont = new Font(FontFamily, FontSize))
                    using (SolidBrush fontBrush = new SolidBrush(FontColor))
                    {
                        g.DrawString(option.Text, menuFont, fontBrush, optionXPosition + 5, MenuBox.Y);
                    }
                }

                optionXPosition += optionWidth;
            }
        }
    }
}