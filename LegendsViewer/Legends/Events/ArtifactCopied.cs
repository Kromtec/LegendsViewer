using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class ArtifactCopied : WorldEvent
    {
        public Artifact Artifact { get; set; }
        public Site DestSite { get; set; }
        public int DestStructureId { get; set; }
        public Structure DestStructure { get; set; }
        public Entity DestEntity { get; set; }
        public Site SourceSite { get; set; }
        public int SourceStructureId { get; set; }
        public Structure SourceStructure { get; set; }
        public Entity SourceEntity { get; set; }
        public bool FromOriginal { get; set; }


        public ArtifactCopied(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "artifact_id": Artifact = world.GetArtifact(Convert.ToInt32(property.Value)); break;
                    case "dest_site_id": DestSite = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "dest_structure_id": DestStructureId = Convert.ToInt32(property.Value); break;
                    case "dest_entity_id": DestEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "source_site_id": SourceSite = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "source_structure_id": SourceStructureId = Convert.ToInt32(property.Value); break;
                    case "source_entity_id": SourceEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "from_original":
                        FromOriginal = true;
                        property.Known = true;
                        break;
                }
            }

            if (DestSite != null)
            {
                DestStructure = DestSite.Structures.FirstOrDefault(structure => structure.Id == DestStructureId);
            }

            if (SourceSite != null)
            {
                SourceStructure = SourceSite.Structures.FirstOrDefault(structure => structure.Id == SourceStructureId);
            }

            Artifact.AddEvent(this);
            DestSite.AddEvent(this);
            DestStructure.AddEvent(this);
            DestEntity.AddEvent(this);
            SourceSite.AddEvent(this);
            SourceStructure.AddEvent(this);
            SourceEntity.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += DestEntity.ToLink(link, pov, this);
            eventString += " made a copy of ";
            if (FromOriginal)
            {
                eventString += "the original ";
            }
            eventString += Artifact.ToLink(link, pov, this);
            eventString += " from ";
            eventString += SourceStructure.ToLink(link, pov, this);
            eventString += " in ";
            eventString += SourceSite.ToLink(link, pov, this);
            eventString += " of ";
            eventString += SourceEntity.ToLink(link, pov, this);
            eventString += " keeping it within ";
            eventString += DestStructure.ToLink(link, pov, this);
            eventString += " in ";
            eventString += DestSite.ToLink(link, pov, this);
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
