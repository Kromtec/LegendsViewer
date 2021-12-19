using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.Chart
{
    public class ChartPanel : Panel
    {
        private readonly System.Windows.Forms.DataVisualization.Charting.Chart _dwarfChart;
        private readonly DwarfObject _focusObject;
        private readonly World _world;
        private readonly List<Series> _series = new List<Series>();
        public bool OtherChart;
        private ToolStripMenuItem _timelineMenu;
        private string _aliveHfRace = "";
        public List<ChartOption> SeriesOptions = new List<ChartOption>();
        public ChartPanel(World world, DwarfObject focusObject, List<ChartOption> options = null)
        {
            Dock = DockStyle.Fill;

            _focusObject = focusObject;
            _world = world;
            _dwarfChart = new System.Windows.Forms.DataVisualization.Charting.Chart { Dock = DockStyle.Fill };
            Controls.Add(_dwarfChart);
            Refresh();

            _dwarfChart.BackColor = Color.White;
            _dwarfChart.ChartAreas.Add(new ChartArea());
            _dwarfChart.ChartAreas.Last().AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            _dwarfChart.ChartAreas.Last().BackColor = Color.White;
            _dwarfChart.Legends.Add(new Legend());
            _dwarfChart.Legends.Last().LegendStyle = LegendStyle.Row;
            _dwarfChart.Legends.Last().Position.Width = 100;
            _dwarfChart.Legends.Last().Position.Y = 100;
            _dwarfChart.Legends.Last().Position.Height = 4;
            _dwarfChart.Titles.Add(new Title());
            _dwarfChart.Titles.Last().Position.Auto = false;
            _dwarfChart.Titles.Last().Position.X = 50;
            _dwarfChart.Titles.Last().Position.Y = 1.5f;
            if (options != null)
            {
                SeriesOptions = options;
            }
            else if (_focusObject.GetType() == typeof(Battle))
            {
                SeriesOptions.Add(ChartOption.OtherBattleRemaining);
            }
            else
            {
                SeriesOptions.Add(ChartOption.TimelineEvents);
                SeriesOptions.Add(ChartOption.TimelineEventsFiltered);
            }
            SetupMenu();
            //GenerateSeries();
        }

        public void RefreshFiltered()
        {
            if (SeriesOptions.Contains(ChartOption.TimelineEventsFiltered))
            {
                _series.RemoveAt(SeriesOptions.IndexOf(ChartOption.TimelineEventsFiltered));
                SeriesOptions.Remove(ChartOption.TimelineEventsFiltered);
                SeriesOptions.Add(ChartOption.TimelineEventsFiltered);
                GenerateSeries();
            }
        }

        public void RefreshAllSeries()
        {
            SeriesOptions = SeriesOptions.Distinct().ToList();
            _series.Clear();
            _dwarfChart.Series.Clear();
            GenerateSeries();
        }

        public void GenerateSeries()
        {
            for (int i = 0; i < _timelineMenu.DropDownItems.Count; i++)
            {
                ChartMenuItem item = _timelineMenu.DropDownItems[i] as ChartMenuItem;
                item.Checked = SeriesOptions.Contains(item.Option);
            }

            int newSeries = SeriesOptions.Count - _series.Count;
            if (newSeries < 0) //Some Error prevention, sometimes Series don't get remade properly during Refresh and will have too man, so force it to remake the series, obsolete?
            {
                _series.Clear();
                newSeries = SeriesOptions.Count;
            }
            if (newSeries > 0)
            {
                _dwarfChart.Series.Clear();
                int seriesCount = SeriesOptions.Count;
                for (int i = SeriesOptions.Count - newSeries; i < SeriesOptions.Count; i++)
                {
                    List<Series> addSeries = GenerateSeries(SeriesOptions[i]);
                    foreach (Series series in addSeries)
                    {
                        _series.Add(series);
                        if (SeriesOptions.Count(option => option == SeriesOptions[i]) != addSeries.Count)
                        {
                            SeriesOptions.Insert(i, SeriesOptions[i++]);
                        }
                    }
                }

                if (SeriesOptions.Contains(ChartOption.TimelineEventsFiltered))
                {
                    int i = SeriesOptions.IndexOf(ChartOption.TimelineEventsFiltered);
                    SeriesOptions.Remove(ChartOption.TimelineEventsFiltered);
                    SeriesOptions.Insert(0, ChartOption.TimelineEventsFiltered);
                    Series temp = _series[i];
                    _series.RemoveAt(i);
                    _series.Insert(0, temp);
                }
                if (SeriesOptions.Contains(ChartOption.TimelineEvents))
                {
                    int i = SeriesOptions.IndexOf(ChartOption.TimelineEvents);
                    SeriesOptions.Remove(ChartOption.TimelineEvents);
                    SeriesOptions.Insert(0, ChartOption.TimelineEvents);
                    Series temp = _series[i];
                    _series.RemoveAt(i);
                    _series.Insert(0, temp);
                }

                foreach (Series series in _series)
                {
                    _dwarfChart.Series.Add(series);
                }
            }

            if (_dwarfChart.Series.Count == 0)
            {
                return;
            }

            if (!SeriesOptions.Contains(ChartOption.WorldHfRemaining) && !SeriesOptions.Contains(ChartOption.OtherBattleRemaining))
            {
                _dwarfChart.ChartAreas.First().AxisX.Minimum = _dwarfChart.Series.SelectMany(series => series.Points).Min(point => point.XValue);
                _dwarfChart.ChartAreas.First().AxisX.Maximum = _dwarfChart.Series.SelectMany(series => series.Points).Max(point => point.XValue);
            }
            else
            {
                _dwarfChart.ChartAreas.First().AxisX.Minimum = _dwarfChart.Series.SelectMany(series => series.Points).Min(point => point.XValue) - 1;
                _dwarfChart.ChartAreas.First().AxisX.Maximum = _dwarfChart.Series.SelectMany(series => series.Points).Max(point => point.XValue) + 1;
            }

            double maxYAxis = _dwarfChart.Series.SelectMany(series => series.Points).Max(point => point.YValues[0]);
            int minYAxis = 20;
            _dwarfChart.ChartAreas.First().AxisY.Maximum = maxYAxis < minYAxis ? minYAxis : maxYAxis * 1.05;
        }

        private List<Series> GenerateSeries(ChartOption option, string command = "")
        {
            List<Series> series = new List<Series>();
            switch (option) //Series Setup
            {
                case ChartOption.TimelineEvents:
                    series.Add(new Series("Events"));
                    series.First().Color = Color.FromArgb(65, 140, 240); break;
                case ChartOption.TimelineEventsFiltered:
                    series.Add(new Series("Events (Filtered)"));
                    series.First().Color = Color.FromArgb(252, 180, 64); break;
                case ChartOption.TimelineActiveSites:
                    series.Add(new Series("Sites"));
                    series.First().Color = Color.FromArgb(145, 70, 170); break;
                case ChartOption.TimelineActiveSitesByRace:
                    List<CreatureInfo> races = _world.Entities.Where(entity => entity.IsCiv).GroupBy(entity => entity.Race).Select(entity => entity.Key).ToList();
                    foreach (CreatureInfo race in races)
                    {
                        Series raceSeries = new Series(race.NamePlural)
                        {
                            Color = _world.Entities.First(entity => entity.IsCiv && entity.Race == race).LineColor,
                            BorderWidth = 3,
                            IsVisibleInLegend = false
                        };
                        series.Add(raceSeries);
                    }
                    break;
                case ChartOption.TimelineActiveWars:
                    series.Add(new Series("Wars"));
                    series.First().Color = Color.FromArgb(202, 107, 75); break;
                case ChartOption.TimelineAliveHFs:
                    series.Add(new Series("Historical Figures"));
                    series.First().Color = Color.FromArgb(224, 64, 10); break;
                case ChartOption.TimeLineAliveHfSpecific:
                    series.Add(new Series(_aliveHfRace));
                    series.First().Color = Color.FromArgb(224, 64, 10); break;
                case ChartOption.TimelineBattles:
                    series.Add(new Series("Battles"));
                    series.First().Color = Color.FromArgb(26, 59, 105); break;
                case ChartOption.TimelineBeastAttacks:
                    series.Add(new Series("Beast Attacks"));
                    series.First().Color = Color.FromArgb(105, 170, 60); break;
                case ChartOption.TimelineBattleDeaths:
                    series.Add(new Series("Battle Deaths"));
                    series.First().Color = Color.FromArgb(130, 160, 210); break;
                case ChartOption.WorldHfAlive: series.Add(new Series("Historical Figures - Alive")); break;
                case ChartOption.WorldHfRemaining:
                    series.Add(new Series("Historical Figures - Totals"));
                    series.Add(new Series("Historical Figures - Remaining"));
                    break;
                //case ChartOption.WorldHFDead: series.Insert(new Series("Historical Figures - Dead");break;
                case ChartOption.WorldHfRaces: series.Add(new Series("Historical Figures")); break;
                case ChartOption.WorldOutdoorPopulations: series.Add(new Series("Outdoor Populations (Not Including Unnumbered)")); break;
                case ChartOption.WorldRegionTypes: series.Add(new Series("Regions")); break;
                case ChartOption.WorldSitePopulations: series.Add(new Series("Site Populations")); break;
                case ChartOption.WorldDeaths: series.Add(new Series("Deaths")); break;
                case ChartOption.WorldSiteTypes: series.Add(new Series("Sites")); break;
                case ChartOption.WorldUndergroundPopulations: series.Add(new Series("Underground Populations (Not Including Unnumbered)")); break;
                case ChartOption.OtherEventTypes: series.Add(new Series("Event Types")); break;
                case ChartOption.OtherEntityPopulations: series.Add(new Series("Entity Populations")); break;
                case ChartOption.OtherKillsByRace: series.Add(new Series("Kills by Race")); break;
                case ChartOption.OtherDeaths: series.Add(new Series("Deaths")); break;
                case ChartOption.OtherSitePopulations: series.Add(new Series("Site Populations")); break;
                case ChartOption.OtherWarLosses: series.Add(new Series("War Losses")); break;
                case ChartOption.OtherBattleRemaining:
                    series.Add(new Series("Forces - Total"));
                    series.Add(new Series("Forces - Remaining"));
                    break;
            }

            switch (option) //Chart Setup
            {
                case ChartOption.TimelineEvents:
                case ChartOption.TimelineEventsFiltered:
                case ChartOption.TimelineActiveSites:
                case ChartOption.TimelineActiveSitesByRace:
                case ChartOption.TimelineActiveWars:
                case ChartOption.TimelineAliveHFs:
                case ChartOption.TimeLineAliveHfSpecific:
                case ChartOption.TimelineBattles:
                case ChartOption.TimelineBeastAttacks:
                case ChartOption.TimelineBattleDeaths:
                    foreach (Series setup in series)
                    {
                        setup.ChartType = option == ChartOption.TimelineEvents || option == ChartOption.TimelineEventsFiltered
                            ? SeriesChartType.Area
                            : SeriesChartType.Line;
                        //setup.XValueType = ChartValueType.Int32;
                    }

                    _dwarfChart.Legends.Last().LegendStyle = LegendStyle.Row;
                    _dwarfChart.Legends.Last().Position.Auto = false;
                    _dwarfChart.Legends.Last().Position.Width = 100;
                    _dwarfChart.Legends.Last().Position.Y = 100;
                    _dwarfChart.Legends.Last().Position.Height = 4;
                    _dwarfChart.Titles.Last().Text = "Timeline";
                    _dwarfChart.ChartAreas.Last().Area3DStyle.Enable3D = false;
                    _dwarfChart.ChartAreas.First().AxisX.MajorGrid.Enabled = true;
                    _dwarfChart.ChartAreas.First().AxisX.Interval = 0;
                    _dwarfChart.ChartAreas.First().AxisX.LabelStyle.Angle = 0;
                    _dwarfChart.Legends.Last().Enabled = true;
                    break;
                case ChartOption.WorldHfAlive:
                //case ChartOption.WorldHFDead:
                case ChartOption.WorldHfRaces:
                case ChartOption.WorldOutdoorPopulations:
                case ChartOption.WorldRegionTypes:
                case ChartOption.WorldSitePopulations:
                case ChartOption.WorldDeaths:
                case ChartOption.WorldSiteTypes:
                case ChartOption.WorldUndergroundPopulations:
                case ChartOption.OtherEventTypes:
                case ChartOption.OtherEntityPopulations:
                case ChartOption.OtherKillsByRace:
                //case ChartOption.OtherEntityWarDeaths:
                //case ChartOption.OtherEntityWarKills:
                //case ChartOption.OtherEntityWarLosses:
                case ChartOption.OtherDeaths:
                case ChartOption.OtherSitePopulations:
                case ChartOption.OtherWarLosses:
                    series.First().ChartType = SeriesChartType.Pie;
                    series.First().CustomProperties = option == ChartOption.OtherWarLosses
                        ? "PieLabelStyle=Outside,PieStartAngle=270"
                        : "CollectedThreshold=0.75, PieLabelStyle=Outside,PieStartAngle=270";

                    series.First().IsValueShownAsLabel = true;
                    series.First().Label = "#LEGENDTEXT\n#VAL (#PERCENT)";
                    _dwarfChart.ChartAreas.Last().Area3DStyle.Enable3D = false;
                    _dwarfChart.ChartAreas.First().AxisX.MajorGrid.Enabled = true;
                    _dwarfChart.Legends.Last().Enabled = false;
                    _dwarfChart.Titles.Last().Text = series.First().Name;
                    break;
                //DwarfChart.LegendsViewer.Last().LegendStyle = LegendStyle.Column;
                //DwarfChart.LegendsViewer.Last().Position.Auto = true; break;
                case ChartOption.WorldHfRemaining:
                case ChartOption.OtherBattleRemaining:
                    foreach (Series setup in series)
                    {
                        setup.ChartType = SeriesChartType.Column;
                        setup.IsValueShownAsLabel = true;
                        setup.CustomProperties = "DrawSideBySide=False";
                    }
                    //series.Last().CustomProperties += ",LabelStyle=Bottom";
                    _dwarfChart.ChartAreas.Last().Area3DStyle.Enable3D = false;
                    _dwarfChart.ChartAreas.Last().AxisX.IntervalOffset = 0;
                    _dwarfChart.ChartAreas.First().AxisX.MajorGrid.Enabled = false;
                    _dwarfChart.ChartAreas.First().AxisX.Interval = 1;
                    _dwarfChart.ChartAreas.First().AxisX.LabelStyle.Angle = -30;
                    _dwarfChart.Legends.Last().Enabled = false;
                    _dwarfChart.Titles.Last().Text = series.Last().Name;
                    break;
            }

            switch (option) //Generate Series
            {
                case ChartOption.TimelineActiveSites:
                case ChartOption.TimelineActiveSitesByRace:
                case ChartOption.TimelineActiveWars:
                case ChartOption.TimelineAliveHFs:
                case ChartOption.TimeLineAliveHfSpecific:
                case ChartOption.TimelineBattles:
                case ChartOption.TimelineBeastAttacks:
                case ChartOption.TimelineEvents:
                case ChartOption.TimelineEventsFiltered:
                case ChartOption.TimelineBattleDeaths:
                    int startYear = 0, endYear = 0;
                    List<WorldEvent> eventsList = null;
                    List<BeastAttack> beastAttacks;
                    List<Battle> battles;
                    List<War> wars;
                    List<HistoricalFigure> aliveHFs = null;
                    List<HistoricalFigure> hfs = null;

                    eventsList = _focusObject is EventCollection ? (_focusObject as EventCollection).GetSubEvents() : (_focusObject as WorldObject).Events;

                    if (_focusObject.GetType() == typeof(Entity))
                    {
                        wars = (_focusObject as Entity).Wars;
                    }
                    else
                    {
                        wars = _focusObject.GetType() == typeof(War) ? new List<War> { _focusObject as War } : _world.Wars;
                    }

                    if (_focusObject.GetType() == typeof(HistoricalFigure))
                    {
                        battles = (_focusObject as HistoricalFigure).Battles;
                    }
                    else if (_focusObject.GetType() == typeof(Site))
                    {
                        battles = (_focusObject as Site).Warfare.OfType<Battle>().ToList();
                    }
                    else if (_focusObject.GetType() == typeof(Entity))
                    {
                        battles = (_focusObject as Entity).Wars.SelectMany(war => war.Collections.OfType<Battle>()).ToList();
                    }
                    else if (_focusObject.GetType() == typeof(War))
                    {
                        battles = (_focusObject as War).Collections.OfType<Battle>().ToList();
                    }
                    else
                    {
                        battles = _focusObject.GetType() == typeof(WorldRegion) ? (_focusObject as WorldRegion).Battles : _world.Battles;
                    }

                    if (_focusObject.GetType() == typeof(HistoricalFigure))
                    {
                        beastAttacks = (_focusObject as HistoricalFigure).BeastAttacks;
                    }
                    else
                    {
                        beastAttacks = _focusObject.GetType() == typeof(Site) ? (_focusObject as Site).BeastAttacks : _world.BeastAttacks;
                    }

                    eventsList = eventsList.OrderBy(events => events.Year).ToList();
                    if (eventsList.Count > 0)
                    {
                        startYear = eventsList.First().Year;
                        endYear = eventsList.Last().Year;
                    }

                    int hfIndex = 0;
                    if (option == ChartOption.TimelineAliveHFs || option == ChartOption.TimeLineAliveHfSpecific)
                    {
                        if (option == ChartOption.TimelineAliveHFs)
                        {
                            hfs = _world.HistoricalFigures.OrderBy(hf => hf.BirthYear).ToList();
                        }
                        //aliveHFs = World.HistoricalFigures.Where(hf => hf.BirthYear <= startYear).ToList();
                        else
                        {
                            hfs = _world.HistoricalFigures.Where(hf => hf.Race.NameSingular == _aliveHfRace).OrderBy(hf => hf.BirthYear).ToList(); //====================================================
                        }

                        aliveHFs = hfs.Where(hf => hf.BirthYear <= startYear).ToList();
                        //hfs = World.HistoricalFigures.OrderBy(hf => hf.BirthYear).ToList();
                        HistoricalFigure firstHfIndex = hfs.Find(hf => hf.BirthYear > startYear);
                        hfIndex = firstHfIndex == null ? 0 : hfs.IndexOf(firstHfIndex);
                    }

                    int offset = 0;
                    if (startYear == -1)
                    {
                        offset = 1;
                    }

                    _dwarfChart.ChartAreas.Last().AxisX.IntervalOffset = offset;

                    //uses an event index so the loop doesn't go through every event for each year, only the eventsList for that year
                    int eventIndex = 0;
                    if (eventsList.Count > 0)
                    {
                        eventIndex = eventsList.IndexOf(eventsList.First(ev => ev.Year == startYear));
                    }

                    int beastAttackStartYear = 0;
                    int beastAttackIndex = 0;
                    if (beastAttacks?.Count(ba => ba.StartYear >= startYear) > 0)
                    {
                        beastAttackStartYear = beastAttacks.FindIndex(ba => ba.StartYear == startYear) >= 0
                            ? startYear
                            : beastAttacks.First(ba => ba.StartYear > startYear).StartYear;

                        beastAttackIndex = beastAttacks.IndexOf(beastAttacks.First(ba => ba.StartYear == beastAttackStartYear));
                    }
                    int battleStartYear = 0;
                    int battleIndex = 0;
                    if (battles?.Count > 0)
                    {
                        battleStartYear = battles.FindIndex(battle => battle.StartYear == startYear) >= 0
                            ? startYear
                            : battles.First(battle => battle.StartYear > startYear).StartYear;

                        battleIndex = battles.IndexOf(battles.First(battle => battle.StartYear == battleStartYear));
                    }
                    for (int year = startYear; year <= endYear; year++)
                    {
                        int count = 0;
                        switch (option)
                        {
                            case ChartOption.TimelineEvents:
                            case ChartOption.TimelineEventsFiltered:
                                while (eventIndex < eventsList.Count && eventsList[eventIndex].Year == year)
                                {
                                    if (option == ChartOption.TimelineEvents)
                                    {
                                        count++;
                                    }
                                    else if (!(_focusObject.GetType().GetField("Filters").GetValue(null) as List<string>).Contains(eventsList[eventIndex].Type))
                                    {
                                        count++;
                                    }

                                    eventIndex++;
                                }
                                break;
                            case ChartOption.TimelineActiveSites:
                                count = _focusObject.GetType() == typeof(Era)
                                    ? _world.Entities.Where(entity => entity.IsCiv).Sum(entity => entity.SiteHistory.Count(site => year >= site.StartYear && (year <= site.EndYear || site.EndYear == -1)))
                                    : (_focusObject as Entity).SiteHistory.Count(site => year >= site.StartYear && (year <= site.EndYear || site.EndYear == -1));

                                break;
                            case ChartOption.TimelineActiveSitesByRace:
                                foreach (Series race in series)
                                {
                                    count = _world.Entities.Where(entity => entity.IsCiv && entity.Race.NamePlural == race.Name).Sum(entity => entity.SiteHistory.Count(site => year >= site.StartYear && (year <= site.EndYear || site.EndYear == -1)));
                                    race.Points.AddXY(year, count);
                                }
                                break;
                            case ChartOption.TimelineActiveWars: count = wars.Count(war => year >= war.StartYear && (year <= war.EndYear || war.EndYear == -1)); break;
                            case ChartOption.TimelineAliveHFs:
                            case ChartOption.TimeLineAliveHfSpecific:
                                aliveHFs.RemoveAll(hf => hf.DeathYear <= year && hf.DeathYear != -1); //Removes Dead HFs
                                while (hfIndex < hfs.Count && hfs[hfIndex].BirthYear == year) //Adds HFs born in current year;
                                {
                                    aliveHFs.Add(hfs[hfIndex]);
                                    hfIndex++;
                                }
                                count = aliveHFs.Count;
                                break;
                            //count = World.HistoricalFigures.Count(hf => year >= hf.BirthYear && (year <= hf.DeathYear || hf.DeathYear == -1)); break;
                            case ChartOption.TimelineBattles:
                            case ChartOption.TimelineBattleDeaths:
                                //count = battles.Count(battle => battle.StartYear == year);
                                while (battleIndex < battles.Count && battles[battleIndex].StartYear == year)
                                {
                                    switch (option)
                                    {
                                        case ChartOption.TimelineBattles: count++; break;
                                        case ChartOption.TimelineBattleDeaths:
                                            Battle yearBattle = battles[battleIndex];
                                            count += yearBattle.AttackerDeathCount + yearBattle.DefenderDeathCount;
                                            break;
                                    }
                                    battleIndex++;
                                }
                                break;
                            case ChartOption.TimelineBeastAttacks:
                                while (beastAttackIndex < beastAttacks.Count && beastAttacks[beastAttackIndex].StartYear == year)
                                {
                                    count++;
                                    beastAttackIndex++;
                                }
                                break;

                                //case ChartOption.TimelineBattleDeaths: count = battles.Where(battle => battle.StartYear == year).Sum(battle => battle.AttackerDeathCount + battle.DefenderDeathCount); break;
                        }
                        if (series.Count == 1)
                        {
                            series.First().Points.AddXY(year, count);
                        }
                    }
                    int maxPoints = Convert.ToInt32(_dwarfChart.ClientRectangle.Width * 0.9) / 3;
                    if (series.First().Points.Count > maxPoints)
                    {
                        List<double> averagedPoints = new List<double>();
                        int averageMaxCount = series.First().Points.Count / maxPoints;
                        int averageCount = 0;
                        double sum = 0;
                        for (int i = 0; i < series.First().Points.Count; i++)
                        {
                            sum += series.First().Points[i].YValues[0];
                            averageCount++;
                            if (averageCount == averageMaxCount)
                            {
                                averagedPoints.Add(sum / averageCount);
                                sum = 0;
                                averageCount = 0;
                            }
                        }
                        if (averageCount > 0)
                        {
                            averagedPoints.Add(sum / averageCount);
                        }

                        double yearXPoints = Convert.ToDouble(endYear) / averagedPoints.Count;
                        series.First().Points.Clear();
                        for (int i = 0; i < averagedPoints.Count; i++)
                        {
                            series.First().Points.AddXY(i * yearXPoints, averagedPoints[i]);
                        }
                    }
                    break;
                case ChartOption.WorldHfAlive:
                    _world.HistoricalFigures
                        .Where(hf => hf.DeathYear == -1)
                        .GroupBy(hf => hf.Race)
                        .Select(hf => new { Race = hf.Key, Count = hf.Count() })
                        .OrderByDescending(hf => hf.Count).ToList()
                        .ForEach(hf =>
                        {
                            series.First().Points.AddY(hf.Count);
                            series.First().Points.Last().LegendText = hf.Race.NamePlural;
                        });
                    break;
                case ChartOption.WorldHfRemaining:
                    var hfTotals = _world.HistoricalFigures
                        .GroupBy(hf => hf.Race)
                        .Select(hf => new { Race = hf.Key, Count = hf.Count() })
                        .OrderByDescending(hf => hf.Count)
                        .ToList();
                    var hfKilled = _world.Events.OfType<HfDied>()
                        .GroupBy(death => death.HistoricalFigure.Race)
                        .Select(hf => new { Race = hf.Key, Count = hf.Count() });
                    int otherLimit = Convert.ToInt32(hfTotals.Sum(hf => hf.Count) * 0.005);
                    var otherRaces = hfTotals.Where(hf => hf.Count < otherLimit).Select(hf => hf.Race);
                    int otherTotal = hfTotals.Where(hf => hf.Count < otherLimit).Sum(hf => hf.Count);
                    int otherKilled = hfKilled.Where(hf => otherRaces.Contains(hf.Race)).Sum(hf => hf.Count);
                    hfTotals.RemoveAll(hf => hf.Count < otherLimit);
                    for (int i = 0; i < hfTotals.Count; i++)
                    {
                        series.First().Points.AddXY(i, hfTotals[i].Count);
                        if (hfKilled.Count(hf => hf.Race == hfTotals[i].Race) > 0)
                        {
                            series.Last().Points.AddXY(i, hfTotals[i].Count - hfKilled.First(hf => hf.Race == hfTotals[i].Race).Count);
                        }
                        else
                        {
                            series.Last().Points.AddXY(i, hfTotals[i].Count);
                        }
                        series.Last().Points.Last().AxisLabel = hfTotals[i].Race.NamePlural;
                    }
                    series.First().Points.AddXY(hfTotals.Count(hf => hf.Count >= otherLimit), otherTotal);
                    series.Last().Points.AddXY(hfTotals.Count(hf => hf.Count >= otherLimit), otherTotal - otherKilled);
                    series.Last().Points.Last().AxisLabel = "Other";
                    break;
                //case ChartOption.WorldHFDead:
                //    World.HistoricalFigures.Where(hf => hf.DeathYear > 0).GroupBy(hf => hf.Race).Select(hf => new { Race = hf.Key, Count = hf.Count() }).OrderByDescending(hf => hf.Count).ToList().ForEach(hf => { series.First().Points.AddY(hf.Count); series.First().Points.Last().LegendText = hf.Race; }); break;
                case ChartOption.WorldHfRaces:
                    _world.HistoricalFigures.GroupBy(hf => hf.Race).Select(hf => new { Race = hf.Key, Count = hf.Count() }).OrderByDescending(hf => hf.Count).ToList().ForEach(hf => { series.First().Points.AddY(hf.Count); series.First().Points.Last().LegendText = hf.Race.NamePlural; }); break;
                case ChartOption.WorldRegionTypes:
                    _world.Regions.GroupBy(region => region.Type).Select(region => new { Type = region.Key, Count = region.Count() }).OrderByDescending(region => region.Count).ToList().ForEach(region => { series.First().Points.AddY(region.Count); series.First().Points.Last().LegendText = region.Type; }); break;
                case ChartOption.WorldSitePopulations:
                    _world.SitePopulations.GroupBy(pop => pop.Race).Select(pop => new { Type = pop.Key, Count = pop.Sum(population => population.Count) }).OrderByDescending(pop => pop.Count).ToList().ForEach(pop => { series.First().Points.AddY(pop.Count); series.First().Points.Last().LegendText = pop.Type.NamePlural; }); break;
                case ChartOption.WorldDeaths:
                case ChartOption.OtherDeaths:
                    List<HfDied> hfDeaths = new List<HfDied>();
                    List<Battle.Squad> squads = new List<Battle.Squad>();
                    List<string> deathRaces = new List<string>();
                    if (option == ChartOption.WorldDeaths)
                    {
                        hfDeaths = _world.Events.OfType<HfDied>().ToList();
                        //hfDeaths = World.Battles.SelectMany(battle => battle.GetSubEvents().OfType<HFDied>()).ToList();
                        squads = _world.Battles.SelectMany(battle => battle.AttackerSquads.Concat(battle.DefenderSquads)).ToList();
                    }
                    else if (_focusObject.GetType() == typeof(Site))
                    {
                        hfDeaths = (_focusObject as Site).Events.OfType<HfDied>().ToList();
                        squads = (_focusObject as Site).Warfare.OfType<Battle>().SelectMany(battle => battle.AttackerSquads.Concat(battle.DefenderSquads)).ToList();
                    }
                    else if (_focusObject.GetType() == typeof(Region))
                    {
                        hfDeaths = (_focusObject as WorldRegion).Events.OfType<HfDied>().ToList();
                        squads = (_focusObject as WorldRegion).Battles.SelectMany(battle => battle.AttackerSquads.Concat(battle.DefenderSquads)).ToList();
                    }
                    else if (_focusObject.GetType() == typeof(Era))
                    {
                        Era era = _focusObject as Era;
                        hfDeaths = era.Events.OfType<HfDied>().ToList();
                        //hfDeaths = World.Battles.Where(battle => battle.StartYear >= era.StartYear && battle.StartYear <= era.EndYear).SelectMany(battle => battle.GetSubEvents().OfType<HFDied>()).ToList();
                        squads = _world.Battles.Where(battle => battle.StartYear >= era.StartYear && battle.StartYear <= era.EndYear).SelectMany(battle => battle.AttackerSquads.Concat(battle.DefenderSquads)).ToList();
                    }

                    hfDeaths.ConvertAll(death => death.HistoricalFigure.Race).ForEach(death => deathRaces.Add(death.NamePlural));
                    foreach (Battle.Squad squad in squads)
                    {
                        for (int i = 0; i < squad.Deaths; i++)
                        {
                            deathRaces.Add(squad.Race.NamePlural);
                        }
                    }
                    deathRaces.GroupBy(race => race).Select(race => new { Type = race.Key, Count = race.Count() }).OrderByDescending(race => race.Count).ToList().ForEach(race => { series.First().Points.AddY(race.Count); series.First().Points.Last().LegendText = race.Type; });
                    break;
                case ChartOption.WorldSiteTypes:
                    _world.Sites.GroupBy(region => region.Type).Select(site => new { Type = site.Key, Count = site.Count() }).OrderByDescending(site => site.Count).ToList().ForEach(site => { series.First().Points.AddY(site.Count); series.First().Points.Last().LegendText = site.Type; }); break;
                case ChartOption.WorldOutdoorPopulations:
                    _world.OutdoorPopulations.Where(pop => pop.Count != int.MaxValue).ToList().ForEach(pop => { series.First().Points.AddY(pop.Count); series.First().Points.Last().LegendText = pop.Race.NamePlural; }); break;
                case ChartOption.WorldUndergroundPopulations:
                    _world.UndergroundPopulations.Where(pop => pop.Count != int.MaxValue).ToList().ForEach(pop => { series.First().Points.AddY(pop.Count); series.First().Points.Last().LegendText = pop.Race.NamePlural; }); break;
                case ChartOption.OtherEventTypes:
                    if (_focusObject is EventCollection)
                    {
                        (_focusObject as EventCollection).GetSubEvents().GroupBy(events => events.Type).Select(events => new { Type = events.Key, Count = events.Count() }).OrderByDescending(events => events.Count).ToList().ForEach(events => { series.First().Points.AddY(events.Count); series.First().Points.Last().LegendText = AppHelpers.EventInfo.Single(eventInfo => eventInfo[0] == events.Type)[1]; });
                    }
                    else
                    {
                        (_focusObject as WorldObject).Events.GroupBy(events => events.Type).Select(events => new { Type = events.Key, Count = events.Count() }).OrderByDescending(events => events.Count).ToList().ForEach(events => { series.First().Points.AddY(events.Count); series.First().Points.Last().LegendText = AppHelpers.EventInfo.Single(eventInfo => eventInfo[0] == events.Type)[1]; });
                    }

                    break;
                case ChartOption.OtherKillsByRace:
                    (_focusObject as HistoricalFigure).NotableKills.GroupBy(death => death.HistoricalFigure.Race).Select(death => new { Race = death.Key, Count = death.Count() }).OrderByDescending(death => death.Count).ToList().ForEach(death => { series.First().Points.AddY(death.Count); series.First().Points.Last().LegendText = death.Race.NamePlural; }); break;
                case ChartOption.OtherEntityPopulations:
                    (_focusObject as Entity).Populations.OrderByDescending(pop => pop.Count).ToList().ForEach(pop => { series.First().Points.AddY(pop.Count); series.First().Points.Last().LegendText = pop.Race.NamePlural; }); break;
                //case ChartOption.OtherDeaths:
                //    (FocusObject as WorldRegion).Events.OfType<HFDied>().GroupBy(death => death.HistoricalFigure.Race).Select(death => new { Race = death.Key, Count = death.Count() }).OrderByDescending(death => death.Count).ToList().ForEach(death => { series.First().Points.AddY(death.Count); series.First().Points.Last().LegendText = death.Race; }); break;
                //case ChartOption.OtherSiteDeaths:
                //    (FocusObject as Site).Events.OfType<HFDied>().GroupBy(death => death.HistoricalFigure.Race).Select(death => new { Race = death.Key, Count = death.Count() }).OrderByDescending(death => death.Count).ToList().ForEach(death => { series.First().Points.AddY(death.Count); series.First().Points.Last().LegendText = death.Race; }); break;
                case ChartOption.OtherSitePopulations:
                    (_focusObject as Site).Populations.OrderByDescending(pop => pop.Count).ToList().ForEach(pop => { series.First().Points.AddY(pop.Count); series.First().Points.Last().LegendText = pop.Race.NamePlural; }); break;
                case ChartOption.OtherWarLosses:
                    List<War> warsList = new List<War>();
                    if (_focusObject.GetType() == typeof(War))
                    {
                        warsList.Add(_focusObject as War);
                    }
                    else if (_focusObject.GetType() == typeof(Entity))
                    {
                        foreach (War addWar in (_focusObject as Entity).Wars)
                        {
                            warsList.Add(addWar);
                        }
                    }

                    List<Entity> entities = warsList.SelectMany(war => new List<Entity> { war.Attacker, war.Defender }).ToList();
                    entities = entities.GroupBy(entity => entity).Select(entity => entity.Key).ToList();
                    entities.RemoveAll(entity => entity.Parent != null && entities.Contains(entity.Parent));
                    foreach (Entity entity in entities)
                    {
                        List<Battle> battles1 = warsList.SelectMany(wars1 => wars1.Collections.OfType<Battle>()).ToList();
                        List<HfDied> hfDeathsList = new List<HfDied>();
                        List<Battle.Squad> squadsList = new List<Battle.Squad>();
                        List<string> deathRacesList = new List<string>();
                        hfDeathsList = battles1.Where(battle => battle.Attacker == entity || battle.Attacker.Parent == entity).SelectMany(battle => battle.GetSubEvents().OfType<HfDied>().Where(death => battle.NotableAttackers.Contains(death.HistoricalFigure))).ToList();
                        hfDeathsList = hfDeathsList.Concat(battles1.Where(battle => battle.Defender == entity || battle.Defender.Parent == entity).SelectMany(battle => battle.GetSubEvents().OfType<HfDied>().Where(death => battle.NotableDefenders.Contains(death.HistoricalFigure))).ToList()).ToList();
                        squadsList = battles1.Where(battle => battle.Attacker == entity || battle.Attacker.Parent == entity).SelectMany(battle => battle.AttackerSquads).ToList();
                        squadsList = squadsList.Concat(battles1.Where(battle => battle.Defender == entity || battle.Defender.Parent == entity).SelectMany(battle => battle.DefenderSquads).ToList()).ToList();
                        hfDeathsList.ConvertAll(death => death.HistoricalFigure.Race).ForEach(death => deathRacesList.Add(death.NamePlural));

                        //squadsList.GroupBy(squad => squad.Race).Select(squad => new { Race = squad.Key, Count = squad.Sum(race => race.Deaths) });
                        foreach (Battle.Squad squad in squadsList)
                        {
                            string plural = squad.Race.NamePlural;
                            for (int i = 0; i < squad.Deaths; i++)
                            {
                                deathRacesList.Add(plural);
                            }
                        }

                        var deathsList = deathRacesList.GroupBy(race => race).Select(race => new { Type = race.Key, Count = race.Count() }).OrderByDescending(race => race.Count).ToList();
                        int deathOtherLimit = Convert.ToInt32(deathsList.Sum(death => death.Count) * 0.02);
                        deathsList.Where(death => death.Count >= deathOtherLimit).ToList().ForEach(race => { series.First().Points.AddY(race.Count); series.First().Points.Last().LegendText = race.Type; series.First().Points.Last().Color = entity.LineColor; series.First().Points.Last().BorderColor = Color.Gray; series.First().Points.Last().LabelBackColor = Color.FromArgb(127, entity.LineColor); });
                        int deathOtherCount = deathsList.Where(death => death.Count < deathOtherLimit).Sum(death => death.Count);
                        if (deathOtherCount > 0)
                        {
                            series.First().Points.AddY(deathOtherCount);
                            series.First().Points.Last().LegendText = "Other";
                            series.First().Points.Last().Color = entity.LineColor;
                            series.First().Points.Last().BorderColor = Color.Gray;
                            series.First().Points.Last().LabelBackColor = Color.FromArgb(127, entity.LineColor);
                        }
                    }
                    break;
                case ChartOption.OtherBattleRemaining:
                    Battle battle1 = _focusObject as Battle;
                    List<string> attackers = battle1.NotableAttackers.ConvertAll(hf => hf.Race.NamePlural);
                    List<string> attackersKilled = battle1.NotableAttackers.Where(hf => battle1.GetSubEvents().OfType<HfDied>().Count(death => death.HistoricalFigure == hf) > 0).Select(hf => hf.Race.NamePlural).ToList();
                    foreach (Battle.Squad squad in battle1.AttackerSquads)
                    {
                        string plural = squad.Race.NamePlural;
                        for (int i = 0; i < squad.Numbers; i++)
                        {
                            attackers.Add(plural);
                        }

                        for (int i = 0; i < squad.Deaths; i++)
                        {
                            attackersKilled.Add(plural);
                        }
                    }
                    var attackerTotals = attackers.GroupBy(attacker => attacker).Select(result => new { Type = result.Key, Count = result.Count() }).OrderByDescending(attacker => attacker.Count).ToList();
                    var attackerTotalsKilled = attackersKilled.GroupBy(attacker => attacker).Select(result => new { Type = result.Key, Count = result.Count() }).OrderBy(attacker => attackerTotals.IndexOf(attackerTotals.First(race => race.Type == attacker.Type))).ToList();

                    for (int i = 0; i < attackerTotals.Count; i++)
                    {
                        series.First().Points.AddXY(i, attackerTotals[i].Count);
                        if (attackerTotalsKilled.Count(race => race.Type == attackerTotals[i].Type) > 0)
                        {
                            series.Last().Points.AddXY(i, attackerTotals[i].Count - attackerTotalsKilled.First(race => race.Type == attackerTotals[i].Type).Count);
                        }
                        else
                        {
                            series.Last().Points.AddXY(i, attackerTotals[i].Count);
                        }

                        series.First().Color = Color.LightGray;
                        series.Last().Points.Last().Color = battle1.Attacker.LineColor;
                        series.Last().Points.Last().AxisLabel = attackerTotals[i].Type;
                        //series.Last().Points.Last().LabelBackColor = Color.FromArgb(127, battle1.Attacker.LineColor);
                    }

                    series.First().Points.AddXY(attackerTotals.Count, 0);
                    series.First().Points.Last().IsEmpty = true;
                    series.First().Points.Last().Label = "";
                    series.Last().Points.AddXY(attackerTotals.Count, 0);
                    series.Last().Points.Last().IsEmpty = true;
                    series.Last().Points.Last().Label = "";
                    series.Last().Points.Last().AxisLabel = "VS.";

                    List<string> defenders = battle1.NotableDefenders.ConvertAll(hf => hf.Race.NamePlural);
                    List<string> defendersKilled = battle1.NotableDefenders.Where(hf => battle1.GetSubEvents().OfType<HfDied>().Count(death => death.HistoricalFigure == hf) > 0).Select(hf => hf.Race.NamePlural).ToList();
                    foreach (Battle.Squad squad in battle1.DefenderSquads)
                    {
                        string plural = squad.Race.NamePlural;
                        for (int i = 0; i < squad.Numbers; i++)
                        {
                            defenders.Add(plural);
                        }

                        for (int i = 0; i < squad.Deaths; i++)
                        {
                            defendersKilled.Add(plural);
                        }
                    }
                    var defenderTotals = defenders.GroupBy(defender => defender).Select(result => new { Type = result.Key, Count = result.Count() }).OrderByDescending(defender => defender.Count).ToList();
                    var defenderTotalsKilled = defendersKilled.GroupBy(defender => defender).Select(result => new { Type = result.Key, Count = result.Count() }).OrderBy(defender => defenderTotals.IndexOf(defenderTotals.First(race => race.Type == defender.Type))).ToList();

                    for (int i = 0; i < defenderTotals.Count; i++)
                    {
                        series.First().Points.AddXY(i + attackerTotals.Count + 1, defenderTotals[i].Count);
                        if (defenderTotalsKilled.Count(race => race.Type == defenderTotals[i].Type) > 0)
                        {
                            series.Last().Points.AddXY(i + attackerTotals.Count + 1, defenderTotals[i].Count - defenderTotalsKilled.First(race => race.Type == defenderTotals[i].Type).Count);
                        }
                        else
                        {
                            series.Last().Points.AddXY(i + attackerTotals.Count + 1, defenderTotals[i].Count);
                        }

                        series.First().Color = Color.LightGray;
                        series.Last().Points.Last().Color = battle1.Defender.LineColor;
                        series.Last().Points.Last().AxisLabel = defenderTotals[i].Type;
                    }
                    break;
            }
            return series;
        }

        private void RemoveSeries(ChartOption option)
        {
            while (SeriesOptions.Count(option1 => option1 == option) > 0)
            {
                int i = SeriesOptions.IndexOf(option);
                SeriesOptions.Remove(option);
                if (_series.Count > i)
                {
                    _series.RemoveAt(i);
                }
                if (_dwarfChart.Series.Count > i)
                {
                    _dwarfChart.Series.RemoveAt(i);
                }
            }
        }

        private void SetupMenu()
        {
            MenuStrip menu = new MenuStrip();
            ToolStripMenuItem timeline = new ToolStripMenuItem();
            _timelineMenu = timeline;
            ToolStripMenuItem other = new ToolStripMenuItem();
            ToolStripMenuItem world = new ToolStripMenuItem();

            timeline.Text = "Timeline";
            other.Text = "Other";
            world.Text = "World";

            if (_focusObject is WorldObject && (_focusObject as WorldObject).Events.Count > 0
                || _focusObject.GetType() == typeof(War) && (_focusObject as EventCollection).GetSubEvents().Count > 0)
            {
                timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineEvents });
                timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineEventsFiltered });
                other.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.OtherEventTypes });
            }

            if (_focusObject.GetType() == typeof(Era))
            {
                timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineAliveHFs });
                timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimeLineAliveHfSpecific });
                timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineActiveSites });
                timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineActiveSitesByRace });
                timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineActiveWars });
                timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineBattles });
                timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineBattleDeaths });
                timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineBeastAttacks });
                other.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.OtherDeaths });
            }
            else if (_focusObject.GetType() == typeof(HistoricalFigure))
            {
                HistoricalFigure hf = _focusObject as HistoricalFigure;
                if (hf.NotableKills.Count > 0)
                {
                    other.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.OtherKillsByRace });
                }

                if (hf.Battles.Count > 0)
                {
                    timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineBattles });
                }

                if (hf.BeastAttacks != null)
                {
                    timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineBeastAttacks });
                }
            }
            else if (_focusObject.GetType() == typeof(Entity))
            {
                Entity entity = _focusObject as Entity;
                if (entity.SiteHistory.Count > 0)
                {
                    timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineActiveSites });
                }

                if (entity.Populations.Count > 0)
                {
                    other.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.OtherEntityPopulations });
                }

                if (entity.Wars.Count > 0)
                {
                    timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineActiveWars });
                    timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineBattles });
                    timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineBattleDeaths });
                    other.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.OtherWarLosses });
                }
            }
            else if (_focusObject.GetType() == typeof(Site))
            {
                Site site = _focusObject as Site;
                if (site.BeastAttacks.Count > 0)
                {
                    timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineBeastAttacks });
                }

                if (site.Warfare.OfType<Battle>().Any())
                {
                    timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineBattles });
                    timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineBattleDeaths });
                }
                if (site.Populations.Count > 0)
                {
                    other.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.OtherSitePopulations });
                }

                if (site.Events.OfType<HfDied>().Any() || site.Warfare.Count > 0)
                {
                    other.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.OtherDeaths });
                }
            }
            else if (_focusObject.GetType() == typeof(WorldRegion))
            {
                WorldRegion region = _focusObject as WorldRegion;
                if (region.Events.OfType<HfDied>().Any())
                {
                    other.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.OtherDeaths });
                }

                if (region.Battles.Count > 0)
                {
                    timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineBattles });
                    timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineBattleDeaths });
                }
            }
            else if (_focusObject.GetType() == typeof(War))
            {
                timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineBattles });
                timeline.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.TimelineBattleDeaths });
                other.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.OtherWarLosses });
            }
            else if (_focusObject.GetType() == typeof(Battle))
            {
                other.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.OtherBattleRemaining });
            }

            world.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.WorldHfRaces });
            world.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.WorldHfAlive });
            world.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.WorldHfRemaining });
            //World.DropDownItems.Insert(new ChartMenuItem(this) {Option = ChartOption.WorldHFDead});
            world.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.WorldRegionTypes });
            world.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.WorldSitePopulations });
            world.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.WorldSiteTypes });
            world.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.WorldDeaths });
            world.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.WorldOutdoorPopulations });
            world.DropDownItems.Add(new ChartMenuItem(this) { Option = ChartOption.WorldUndergroundPopulations });

            menu.Items.Add(timeline);
            menu.Items.Add(other);
            menu.Items.Add(world);
            Controls.Add(menu);
        }

        public void MenuClick(ChartMenuItem sender, ChartOption option)
        {
            switch (option)
            {
                case ChartOption.TimelineActiveSites:
                case ChartOption.TimelineActiveSitesByRace:
                case ChartOption.TimelineActiveWars:
                case ChartOption.TimelineAliveHFs:
                case ChartOption.TimeLineAliveHfSpecific:
                case ChartOption.TimelineBattles:
                case ChartOption.TimelineBeastAttacks:
                case ChartOption.TimelineEvents:
                case ChartOption.TimelineEventsFiltered:
                case ChartOption.TimelineBattleDeaths:
                    if (option == ChartOption.TimeLineAliveHfSpecific && !SeriesOptions.Contains(ChartOption.TimeLineAliveHfSpecific))
                    {
                        DlgHf selectHfRace = new DlgHf(_world);
                        selectHfRace.ShowDialog();
                        if (selectHfRace.SelectedRace == "")
                        {
                            return;
                        }

                        _aliveHfRace = selectHfRace.SelectedRace;
                    }
                    if (OtherChart)
                    {
                        while (SeriesOptions.Count > 0)
                        {
                            RemoveSeries(SeriesOptions[0]);
                        }
                    }

                    if (SeriesOptions.Contains(option))
                    {
                        RemoveSeries(option);
                    }
                    else
                    {
                        SeriesOptions.Add(option);
                    }
                    //sender.Checked = !sender.Checked;
                    OtherChart = false;
                    break;
                default:
                    while (SeriesOptions.Count > 0)
                    {
                        RemoveSeries(SeriesOptions[0]);
                    }

                    SeriesOptions.Add(option);
                    OtherChart = true;
                    break;
            }
            GenerateSeries();
        }
    }
}