using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Controls.Query.Attributes;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.WorldObjects
{
    public class Artifact : WorldObject
    {
        public static string Icon = "<i class=\"fa fa-fw fa-diamond\"></i>";

        [AllowAdvancedSearch]
        [ShowInAdvancedSearchResults]
        public string Name { get; set; }
        [AllowAdvancedSearch]
        [ShowInAdvancedSearchResults]
        public string Item { get; set; }
        [AllowAdvancedSearch]
        public HistoricalFigure Creator { get; set; }
        [AllowAdvancedSearch]
        public HistoricalFigure Holder { get; set; }
        public Structure Structure { get; set; }
        public int HolderId { get; set; }
        [AllowAdvancedSearch]
        [ShowInAdvancedSearchResults]
        public string Type { get; set; } // legends_plus.xml
        [AllowAdvancedSearch]
        [ShowInAdvancedSearchResults("Sub Type")]
        public string SubType { get; set; } // legends_plus.xml
        public string Description { get; set; } // legends_plus.xml
        [AllowAdvancedSearch]
        [ShowInAdvancedSearchResults]
        public string Material { get; set; } // legends_plus.xml
        [AllowAdvancedSearch("Page Count")]
        public int PageCount { get; set; } // legends_plus.xml

        public List<int> WrittenContentIds { get; set; }
        public List<WrittenContent> WrittenContents { get; set; }

        public int AbsTileX { get; set; }
        public int AbsTileY { get; set; }
        public int AbsTileZ { get; set; }
        public Location Coordinates { get; set; }

        [AllowAdvancedSearch]
        public Site Site { get; set; }
        [AllowAdvancedSearch]
        public WorldRegion Region { get; set; }

        public static List<string> Filters;
        public override List<WorldEvent> FilteredEvents => Events.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList();

        public Artifact(List<Property> properties, World world)
            : base(properties, world)
        {
            Name = "Untitled";
            Type = "Unknown";
            SubType = "";
            WrittenContentIds = new List<int>();

            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "name": Name = Formatting.InitCaps(property.Value); break;
                    case "item":
                        if (property.SubProperties != null)
                        {
                            property.Known = true;
                            foreach (Property subProperty in property.SubProperties)
                            {
                                switch (subProperty.Name)
                                {
                                    case "name_string":
                                        Item = Formatting.InitCaps(subProperty.Value);
                                        break;
                                    case "page_number":
                                        PageCount = Convert.ToInt32(subProperty.Value);
                                        break;
                                    case "page_written_content_id":
                                    case "writing_written_content_id":
                                        if (!WrittenContentIds.Contains(Convert.ToInt32(subProperty.Value)))
                                        {
                                            WrittenContentIds.Add(Convert.ToInt32(subProperty.Value));
                                        }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            Item = Formatting.InitCaps(property.Value);
                        }
                        break;
                    case "item_type": Type = Formatting.InitCaps(property.Value); break;
                    case "item_subtype": SubType = string.Intern(property.Value); break;
                    case "item_description": Description = Formatting.InitCaps(property.Value); break;
                    case "mat": Material = string.Intern(property.Value); break;
                    case "page_count": PageCount = Convert.ToInt32(property.Value); break;
                    case "abs_tile_x": AbsTileX = Convert.ToInt32(property.Value); break;
                    case "abs_tile_y": AbsTileY = Convert.ToInt32(property.Value); break;
                    case "abs_tile_z": AbsTileZ = Convert.ToInt32(property.Value); break;
                    case "writing": if (!WrittenContentIds.Contains(Convert.ToInt32(property.Value))) { WrittenContentIds.Add(Convert.ToInt32(property.Value)); } break;
                    case "site_id":
                        Site = world.GetSite(Convert.ToInt32(property.Value));
                        break;
                    case "subregion_id":
                        Region = world.GetRegion(Convert.ToInt32(property.Value));
                        break;
                    case "holder_hfid":
                        HolderId = Convert.ToInt32(property.Value);
                        break;
                    case "structure_local_id":
                        Structure = Site?.Structures.Find(structure => structure.Id == Convert.ToInt32(property.Value));
                        break;
                }
            }
            if (AbsTileX > 0 && AbsTileY > 0)
            {
                Coordinates = new Location(AbsTileX / 816, AbsTileY / 816);
            }
            else if (Site != null)
            {
                Coordinates = Site.Coordinates;
            }
        }

        public void Resolve(World world)
        {
            if (HolderId > 0)
            {
                Holder = world.GetHistoricalFigure(HolderId);
                if (Holder?.HoldingArtifacts.Contains(this) == false)
                {
                    Holder.HoldingArtifacts.Add(this);
                }
            }
            if (WrittenContentIds.Count > 0)
            {
                WrittenContents = new List<WrittenContent>();
                foreach (var writtenContentId in WrittenContentIds)
                {
                    WrittenContents.Add(world.GetWrittenContent(writtenContentId));
                }
            }
        }

        public override string ToString() { return Name; }

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            if (link)
            {
                string title = "Artifact" + (!string.IsNullOrEmpty(Type) ? ", " + Type : "");
                title += "&#13";
                title += "Events: " + Events.Count;
                return pov != this
                    ? Icon + "<a href = \"artifact#" + Id + "\" title=\"" + title + "\">" + Name + "</a>"
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
