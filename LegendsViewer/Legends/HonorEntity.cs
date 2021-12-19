using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public class HonorEntity
    {
        public int EntityId { get; }
        public List<int> HonorIds { get; set; }
        public Entity Entity { get; set; }
        public List<Honor> Honors { get; set; }
        public int Battles { get; set; }
        public int Kills { get; set; }

        public HonorEntity(List<Property> properties, World world)
        {
            HonorIds = new List<int>();
            Honors = new List<Honor>();
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "entity": EntityId = Convert.ToInt32(property.Value); break;
                    case "battles": Battles = Convert.ToInt32(property.Value); break;
                    case "kills": Kills = Convert.ToInt32(property.Value); break;
                    case "honor_id": HonorIds.Add(Convert.ToInt32(property.Value)); break;
                }
            }
        }

        public void Resolve(World world, HistoricalFigure historicalFigure)
        {
            Entity = world.GetEntity(EntityId);
            if (Entity != null)
            {
                foreach (var honorId in HonorIds)
                {
                    var honor = Entity.Honors.Find(h => h.Id == honorId);
                    if (honor != null)
                    {
                        honor.HonoredHfs.Add(historicalFigure);
                        Honors.Add(honor);
                    }
                }
            }
        }
    }
}
