using System.Collections.Generic;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.WorldObjects
{
    public class MusicalForm : ArtForm
    {
        public static string Icon = "<i class=\"fa fa-fw fa-music\"></i>";

        public MusicalForm(List<Property> properties, World world)
            : base(properties, world)
        {
            FormType = FormType.Musical;
        }

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            if (link)
            {
                string title = "Musical Form";
                title += "&#13";
                title += "Events: " + Events.Count;

                return pov != this
                    ? Icon + "<a href=\"musicalform#" + Id + "\" title=\"" + title + "\">" + Name + "</a>"
                    : Icon + "<a title=\"" + title + "\">" + HtmlStyleUtil.CurrentDwarfObject(Name) + "</a>";
            }
            return Name;
        }

        public override string GetIcon()
        {
            return Icon;
        }
    }
}
