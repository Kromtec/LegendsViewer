using System;
using System.Linq;
using System.Text;
using LegendsViewer.Legends;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    public class WrittenContentPrinter : HtmlPrinter
    {
        private readonly WrittenContent _writtenContent;
        private readonly World _world;

        public WrittenContentPrinter(WrittenContent writtenContent, World world)
        {
            _writtenContent = writtenContent;
            _world = world;
        }

        public override string Print()
        {
            Html = new StringBuilder();
            Html.Append("<h1>").Append(_writtenContent.GetIcon()).Append(' ').Append(_writtenContent.Name).AppendLine("</h1>");
            if (_writtenContent.Type != WrittenContentType.Unknown)
            {
                var type = _writtenContent.Type.GetDescription();
                string firstWord = _writtenContent.Styles.Count > 0 ? _writtenContent.Styles.First() : type;
                if (firstWord.StartsWith("A") || firstWord.StartsWith("E") || firstWord.StartsWith("I") || firstWord.StartsWith("O") || firstWord.StartsWith("U"))
                {
                    Html.AppendLine("<b>An ");
                }
                else
                {
                    Html.AppendLine("<b>A ");
                }
                Html.AppendLine(string.Join(", ", _writtenContent.Styles))
                    .AppendLine(type.ToLower());
                if (_writtenContent.Author != null)
                {
                    Html.Append(" written by ").Append(_writtenContent.Author.ToLink()).AppendLine(".</b>");
                }
                Html.AppendLine("<br/>");
            }
            Html.AppendLine("<br/>");

            PrintReferences();
            PrintArtform();
            PrintEventLog(_world, _writtenContent.Events, WrittenContent.Filters, _writtenContent);
            return Html.ToString();
        }

        private void PrintArtform()
        {
            if (_writtenContent.FormId == -1)
            {
                return;
            }
            ArtForm artForm = null;
            if (_writtenContent.Type == WrittenContentType.Poem)
            {
                artForm = _world.GetPoeticForm(_writtenContent.FormId);
            }
            else if (_writtenContent.Type == WrittenContentType.MusicalComposition)
            {
                artForm = _world.GetMusicalForm(_writtenContent.FormId);
            }
            else if (_writtenContent.Type == WrittenContentType.Choreography)
            {
                artForm = _world.GetDanceForm(_writtenContent.FormId);
            }
            // TODO
            // Does not seam to be right for other types like 'novel, 'essay', 'shortstory', 'guide', ...
            // Not sure which art form is correct in these cases
            //else
            //{
            //    artForm = _world.GetPoeticForm(_writtenContent.FormId);
            //}
            if (artForm != null)
            {
                Html.Append("<b>").Append(artForm.FormType.GetDescription()).AppendLine("</b><br />")
                    .AppendLine("<ul>")
                    .Append("<li>").Append(artForm.ToLink()).AppendLine("</li>")
                    .AppendLine("</ul>")
                    .AppendLine("</br>");
            }
        }

        private void PrintReferences()
        {
            if (_writtenContent.References.Count > 0)
            {
                Html.AppendLine("<b>References</b><br />")
                    .AppendLine("<ul>");
                foreach (Reference reference in _writtenContent.References)
                {
                    if (reference.ID != -1)
                    {
                        WorldObject referencedObject = null;
                        switch (reference.Type)
                        {
                            case ReferenceType.WrittenContent:
                                referencedObject = _world.GetWrittenContent(reference.ID);
                                break;
                            case ReferenceType.PoeticForm:
                                referencedObject = _world.GetPoeticForm(reference.ID);
                                break;
                            case ReferenceType.MusicalForm:
                                referencedObject = _world.GetMusicalForm(reference.ID);
                                break;
                            case ReferenceType.DanceForm:
                                referencedObject = _world.GetDanceForm(reference.ID);
                                break;
                            case ReferenceType.Site:
                                referencedObject = _world.GetSite(reference.ID);
                                break;
                            case ReferenceType.HistoricalEvent:
                                WorldEvent worldEvent = _world.GetEvent(reference.ID);
                                if (worldEvent != null)
                                {
                                    Html.Append("<li>").Append(worldEvent.Print()).AppendLine("</li>");
                                }
                                break;
                            case ReferenceType.Entity:
                                referencedObject = _world.GetEntity(reference.ID);
                                break;
                            case ReferenceType.HistoricalFigure:
                                referencedObject = _world.GetHistoricalFigure(reference.ID);
                                break;
                            case ReferenceType.ValueLevel:
                            case ReferenceType.KnowledgeScholarFlag:
                            case ReferenceType.Interaction:
                            case ReferenceType.Language:
                                Html.Append("<li>").Append(reference.Type).Append(": ").Append(reference.ID).AppendLine("</li>");
                                break;
                            case ReferenceType.Subregion:
                                referencedObject = _world.GetUndergroundRegion(reference.ID);
                                break;
                            case ReferenceType.AbstractBuilding:
                                Html.Append("<li>").Append(reference.Type).Append(": ").Append(reference.ID).AppendLine("</li>");
                                break;
                            case ReferenceType.Artifact:
                                referencedObject = _world.GetArtifact(reference.ID);
                                break;
                            case ReferenceType.Sphere:
                                Html.Append("<li>").Append(reference.Type).Append(": ").Append(reference.ID).AppendLine("</li>");
                                break;
                        }
                        if (referencedObject != null)
                        {
                            Html.Append("<li>").Append(referencedObject.ToLink()).AppendLine("</li>");
                        }
                    }
                }
                Html.AppendLine("</ul>")
                    .AppendLine("</br>");
            }
        }

        public override string GetTitle()
        {
            return _writtenContent.Name;
        }
    }
}
