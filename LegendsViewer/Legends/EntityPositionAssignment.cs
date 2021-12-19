using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public class EntityPositionAssignment
    {
        public int Id { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public int PositionId { get; set; }
        public int SquadId { get; set; }

        public EntityPositionAssignment(List<Property> properties, World world)
        {
            Id = -1;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "id": Id = Convert.ToInt32(property.Value); break;
                    case "histfig": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "position_id": PositionId = Convert.ToInt32(property.Value); break;
                    case "squad_id": SquadId = Convert.ToInt32(property.Value); break;
                }
            }
        }
    }
}
