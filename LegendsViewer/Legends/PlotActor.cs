using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends
{
    public class PlotActor
    {
        public int ActorId { get; set; }
        public string PlotRole { get; set; }
        public int AgreementId { get; set; }
        public bool AgreementHasMessenger { get; set; }

        public PlotActor(List<Property> properties)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "actor_id": ActorId = Convert.ToInt32(property.Value); break;
                    case "plot_role": PlotRole = property.Value; break;
                    case "agreement_id": AgreementId = Convert.ToInt32(property.Value); break;
                    case "agreement_has_messenger":
                        property.Known = true;
                        AgreementHasMessenger = true;
                        break;
                }
            }
        }
    }
}
