using System.Collections.Generic;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.WorldObjects
{
    public class DanceForm : ArtForm
    {
        public static string Icon = "<i class=\"fa fa-fw fa-street-view\"></i>";

        public DanceForm(List<Property> properties, World world)
            : base(properties, world)
        {
            FormType = FormType.Dance;
        }

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            if (link)
            {
                string linkedString = "";
                if (pov != this)
                {
                    string title = "Dance Form";
                    title += "&#13";
                    title += "Events: " + Events.Count;

                    linkedString = Icon + "<a href=\"danceform#" + Id + "\" title=\"" + title + "\">" + Name + "</a>";
                }
                else
                {
                    linkedString = Icon + HtmlStyleUtil.CurrentDwarfObject(Name);
                }
                return linkedString;
            }
            return Name;
        }

        public override string GetIcon()
        {
            return Icon;
        }
    }
}
