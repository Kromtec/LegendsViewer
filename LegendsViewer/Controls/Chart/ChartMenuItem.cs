using System;
using System.Windows.Forms;

namespace LegendsViewer.Controls.Chart
{
    public class ChartMenuItem : ToolStripMenuItem
    {
        private ChartOption _chartOption;

        public ChartOption Option
        {
            get => _chartOption;
            set
            {
                _chartOption = value;
                switch (_chartOption)
                {
                    case ChartOption.TimelineEvents: Text = "Events"; break;
                    case ChartOption.TimelineEventsFiltered: Text = "Events (Filtered)"; break;
                    case ChartOption.TimelineBattleDeaths: Text = "Battle Deaths"; break;
                    case ChartOption.TimelineActiveSites: Text = "Active Sites"; break;
                    case ChartOption.TimelineActiveSitesByRace: Text = "Active Sites by Race"; break;
                    case ChartOption.TimelineAliveHFs: Text = "Alive Historical Figures"; break;
                    case ChartOption.TimeLineAliveHfSpecific: Text = "Alive Historical Figures..."; break;
                    case ChartOption.TimelineActiveWars: Text = "Active Wars"; break;
                    case ChartOption.TimelineBattles: Text = "Battles"; break;
                    case ChartOption.TimelineBeastAttacks: Text = "Beast Attacks"; break;
                    case ChartOption.OtherEventTypes: Text = "Event Types"; break;
                    case ChartOption.OtherKillsByRace: Text = "Kills"; break;
                    case ChartOption.OtherEntityPopulations: Text = "Site Populations"; break;
                    case ChartOption.OtherDeaths: Text = "Deaths"; break; ;
                    case ChartOption.OtherSitePopulations: Text = "Populations"; break;
                    case ChartOption.OtherWarLosses: Text = "War Losses"; break;
                    case ChartOption.OtherBattleRemaining: Text = "Remaining Forces"; break;
                    case ChartOption.WorldHfRaces: Text = "Historical Figures"; break;
                    case ChartOption.WorldHfAlive: Text = "Historical Figures - Alive"; break;
                    case ChartOption.WorldHfRemaining: Text = "Historical Figures - Remaining"; break;
                    case ChartOption.WorldSitePopulations: Text = "Site Populations"; break;
                    case ChartOption.WorldDeaths: Text = "Deaths"; break;
                    case ChartOption.WorldSiteTypes: Text = "Sites"; break;
                    case ChartOption.WorldRegionTypes: Text = "Regions"; break;
                    case ChartOption.WorldOutdoorPopulations: Text = "Outdoor Populations"; break;
                    case ChartOption.WorldUndergroundPopulations: Text = "Underground Populations"; break;
                }
            }
        }

        private readonly ChartPanel _chart;
        public ChartMenuItem(ChartPanel chart)
        {
            _chart = chart;
        }

        protected override void OnClick(EventArgs e)
        {
            _chart.MenuClick(this, Option);
        }
    }
}