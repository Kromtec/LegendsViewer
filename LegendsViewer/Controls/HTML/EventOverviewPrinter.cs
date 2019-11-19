using System.Collections.Generic;
using System.Text;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;

namespace LegendsViewer.Controls.HTML
{
    public class EventOverviewPrinter : HtmlPrinter
    {
        private readonly List<WorldEvent> Events;
        private readonly List<EventCollection> EventCollections;
        private readonly DwarfObject _dwarfObject;
        private readonly World _world;

        public EventOverviewPrinter(DwarfObject dwarfObject, World world)
        {
            _dwarfObject = dwarfObject;
            if (_dwarfObject is WorldObject worldObject)
            {
                Events = worldObject.Events;
                EventCollections = worldObject.EventCollectons;
            }
            if (_dwarfObject is EventCollection eventCollection)
            {
                Events = eventCollection.AllEvents;
                EventCollections = new List<EventCollection> { eventCollection };
            }
            _world = world;
            if (_dwarfObject == null)
            {
                Events = _world.Events;
                EventCollections = _world.EventCollections;
            }
        }

        public override string Print()
        {
            Html = new StringBuilder();
            Html.AppendLine("<h1>" + GetTitle() + "</h1></br>");
            PrintEventStats(Events, EventCollections);
            return Html.ToString();
        }

        private void PrintEventStats(List<WorldEvent> eventList, List<EventCollection> eventCollectionList)
        {
            Html.AppendLine("<div class=\"container-fluid\">");
            Html.AppendLine("<div class=\"row\">");

            Html.AppendLine("<div class=\"col-sm-6\">");
            PrintEventDetails(eventList);
            Html.AppendLine("</div>");
            Html.AppendLine("<div class=\"col-sm-6\">");
            PrintEventCollectionDetails(eventCollectionList);
            Html.AppendLine("</div>");

            Html.AppendLine("</div>");
            Html.AppendLine("</div>");

            Html.AppendLine("</br>");
        }

        public override string GetTitle()
        {
            return _dwarfObject?.ToString() ?? _world.Name;
        }
    }
}
