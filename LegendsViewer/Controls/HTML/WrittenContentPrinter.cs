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
            Html.AppendLine("<h1>" + _writtenContent.GetIcon() + " " + _writtenContent.Name + "</h1>");
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
                Html.AppendLine(string.Join(", ", _writtenContent.Styles));
                Html.AppendLine(type.ToLower() + " written by " + _writtenContent.Author.ToLink() + ".</b>");
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
                Html.AppendLine("<b>" + artForm.FormType.GetDescription() + "</b><br />");
                Html.AppendLine("<ul>");
                Html.AppendLine("<li>" + artForm.ToLink() + "</li>");
                Html.AppendLine("</ul>");
                Html.AppendLine("</br>");
            }
        }

        private void PrintReferences()
        {
            if (_writtenContent.References.Any())
            {
                Html.AppendLine("<b>References</b><br />");
                Html.AppendLine("<ul>");
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
                                    Html.AppendLine("<li>" + worldEvent.Print() + "</li>");
                                }
                                break;
                            case ReferenceType.Entity:
                                referencedObject = _world.GetEntity(reference.ID);
                                break;
                            case ReferenceType.HistoricalFigure:
                                referencedObject = _world.GetHistoricalFigure(reference.ID);
                                break;
                            case ReferenceType.ValueLevel:
                                Html.AppendLine("<li>" + reference.Type + ": " + reference.ID + "</li>");
                                break;
                            case ReferenceType.KnowledgeScholarFlag:
                                Html.AppendLine("<li>" + reference.Type + ": " + reference.ID + "</li>");
                                break;
                            case ReferenceType.Interaction:
                                Html.AppendLine("<li>" + reference.Type + ": " + reference.ID + "</li>");
                                break;
                            case ReferenceType.Language:
                                Html.AppendLine("<li>" + reference.Type + ": " + reference.ID + "</li>");
                                break;
                            case ReferenceType.Subregion:
                                referencedObject = _world.GetUndergroundRegion(reference.ID);
                                break;
                            case ReferenceType.AbstractBuilding:
                                Html.AppendLine("<li>" + reference.Type + ": " + reference.ID + "</li>");
                                break;
                            case ReferenceType.Artifact:
                                referencedObject = _world.GetArtifact(reference.ID);
                                break;
                            case ReferenceType.Sphere:
                                Html.AppendLine("<li>" + reference.Type + ": " + reference.ID + "</li>");
                                break;
                        }
                        if (referencedObject != null)
                        {
                            Html.AppendLine("<li>" + referencedObject.ToLink() + "</li>");
                        }
                    }
                }
                Html.AppendLine("</ul>");
                Html.AppendLine("</br>");
            }
        }

        public override string GetTitle()
        {
            return _writtenContent.Name;
        }
    }
}
