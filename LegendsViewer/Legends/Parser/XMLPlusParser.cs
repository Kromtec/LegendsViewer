using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using LegendsViewer.Controls;

namespace LegendsViewer.Legends.Parser
{
    public class XmlPlusParser : XmlParser
    {
        private readonly BackgroundWorker _worker;
        private bool _inMiddleOfSection;
        private List<Property> _currentItem;

        public XmlPlusParser(BackgroundWorker worker, World world, string xmlFile) : base(xmlFile)
        {
            _worker = worker;
            World = world;
            Init();
        }

        private void Init()
        {
            Parse();
        }

        public override void Parse()
        {
            if (Xml.ReadState == ReadState.Closed)
            {
                return;
            }

            while (!Xml.EOF && _currentItem == null)
            {
                if (!_inMiddleOfSection)
                {
                    CurrentSection = GetSectionType(Xml.Name);
                    if (CurrentSection != Section.Junk)
                    {
                        _worker.ReportProgress(0, "... " + CurrentSection.GetDescription() + " XMLPLUS");
                    }
                }

                if (CurrentSection == Section.Junk)
                {
                    Xml.Read();
                }
                else if (CurrentSection == Section.Unknown)
                {
                    SkipSection();
                }
                else
                {
                    ParseSection();
                }
            }

            if (Xml.EOF)
            {
                Xml.Close();
            }
        }

        protected override void ParseSection()
        {
            while (Xml.NodeType == XmlNodeType.EndElement || Xml.NodeType == XmlNodeType.None)
            {
                if (Xml.NodeType == XmlNodeType.None)
                {
                    return;
                }

                Xml.ReadEndElement();
            }

            if (!_inMiddleOfSection)
            {
                Xml.ReadStartElement();
                _inMiddleOfSection = true;
            }

            _currentItem = ParseItem();

            if (Xml.NodeType == XmlNodeType.EndElement)
            {
                Xml.ReadEndElement();
                _inMiddleOfSection = false;
            }
        }

        public void AddNewProperties(List<Property> existingProperties, Section xmlParserSection)
        {
            if (_currentItem == null)
            {
                return;
            }

            if (xmlParserSection > CurrentSection)
            {
                while (xmlParserSection > CurrentSection &&
                       (null != _currentItem || ReadState.Closed != Xml.ReadState))
                {
                    if (null != _currentItem)
                    {
                        AddItemToWorld(_currentItem);
                    }

                    _currentItem = null;
                    Parse();
                }
            }

            if (xmlParserSection < CurrentSection)
            {
                return;
            }

            if (_currentItem != null)
            {
                Property id = GetPropertyByName(existingProperties, "id");
                Property currentId = GetPropertyByName(_currentItem, "id");
                while (currentId?.ValueAsInt() < 0)
                {
                    _currentItem = ParseItem();
                    if (_currentItem != null)
                    {
                        currentId = GetPropertyByName(_currentItem, "id");
                    }
                }
                if (id != null && currentId != null && id.ValueAsInt().Equals(currentId.ValueAsInt()))
                {
                    if (_currentItem != null)
                    {
                        foreach (var property in _currentItem)
                        {
                            if (CurrentSection == Section.Entities &&
                                (property.Name == "entity_link" || property.Name == "child" ||
                                 property.Name == "entity_position" || property.Name == "entity_position_assignment" ||
                                 property.Name == "occasion" || property.Name == "weapon" || property.Name == "histfig_id"))
                            {
                                existingProperties.Add(property);
                                continue;
                            }
                            if (CurrentSection == Section.Artifacts && property.Name == "writing")
                            {
                                existingProperties.Add(property);
                                continue;
                            }
                            if (CurrentSection == Section.WrittenContent && property.Name == "style")
                            {
                                existingProperties.Add(property);
                                continue;
                            }
                            if (CurrentSection == Section.Events && property.Name == "bodies")
                            {
                                existingProperties.Add(property);
                                continue;
                            }
                            Property matchingProperty = GetPropertyByName(existingProperties, property.Name);
                            if (CurrentSection == Section.Events && matchingProperty != null &&
                                (matchingProperty.Name == "type" || matchingProperty.Name == "state" ||
                                 matchingProperty.Name == "slayer_race" || matchingProperty.Name == "circumstance" ||
                                 matchingProperty.Name == "reason"))
                            {
                                continue;
                            }

                            if (matchingProperty != null)
                            {
                                if (CurrentSection == Section.Sites && property.Name == "structures")
                                {
                                    matchingProperty.SubProperties = property.SubProperties;
                                    continue;
                                }
                                matchingProperty.Value = property.Value;
                                matchingProperty.Known = false;
                                if (matchingProperty.SubProperties == null)
                                {
                                    matchingProperty.SubProperties = property.SubProperties;
                                }
                                else
                                {
                                    matchingProperty.SubProperties.AddRange(property.SubProperties);
                                }
                            }
                            else
                            {
                                existingProperties.Add(property);
                            }
                        }
                    }

                    _currentItem = null;
                    Parse();
                }
            }
        }

        private static Property GetPropertyByName(List<Property> existingProperties, string name)
        {
            for (int i = 0; i < existingProperties.Count; i++)
            {
                if (existingProperties[i].Name == name)
                {
                    return existingProperties[i];
                }
            }
            return null;
        }
    }
}