using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends
{
    public class IntriguePlot
    {
        public int LocalId { get; set; }
        public string Type { get; set; }
        public int EntityId { get; }
        public bool OnHold { get; set; }
        public int ArtifactId { get; set; }
        public int ActorId { get; set; }
        public int ParentPlotId { get; set; }
        public int ParentPlotHfId { get; set; }
        public int DelegatedPlotId { get; set; }
        public int DelegatedPlotHfId { get; set; }
        public List<PlotActor> PlotActors { get; set; }

        public IntriguePlot(List<Property> properties)
        {
            PlotActors = new List<PlotActor>();

            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "local_id": LocalId = Convert.ToInt32(property.Value); break;
                    case "type": Type = property.Value; break;
                    case "entity_id": EntityId = Convert.ToInt32(property.Value); break;
                    case "artifact_id": ArtifactId = Convert.ToInt32(property.Value); break;
                    case "actor_id": ActorId = Convert.ToInt32(property.Value); break;
                    case "parent_plot_id": ParentPlotId = Convert.ToInt32(property.Value); break;
                    case "parent_plot_hfid": ParentPlotHfId = Convert.ToInt32(property.Value); break;
                    case "delegated_plot_id": DelegatedPlotId = Convert.ToInt32(property.Value); break;
                    case "delegated_plot_hfid": DelegatedPlotHfId = Convert.ToInt32(property.Value); break;
                    case "on_hold":
                        property.Known = true;
                        OnHold = true;
                        break;
                    case "plot_actor":
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            PlotActors.Add(new PlotActor(property.SubProperties));
                        }
                        break;
                }
            }
        }
    }
}
