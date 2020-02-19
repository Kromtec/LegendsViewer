using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LegendsViewer.Legends;

namespace LegendsViewer.Controls.Map
{
    public partial class DlgPopulation : Form
    {
        List<Population> _populations = new List<Population>();
        public List<string> SelectedPopulations = new List<string>();
        public DlgPopulation(World world)
        {
            InitializeComponent();
            
            var populationGrouped = from population in world.SitePopulations
                                    group population by population.Race into popType
                                    select new { Type = popType.Key, Count = popType.Sum(population => population.Count) };
            populationGrouped = populationGrouped.OrderBy(population => population.Type.NamePlural);
            foreach (var population in populationGrouped)
            {
                listPopulations.Items.Add(population.Type.NamePlural + ": " + population.Count);
                _populations.Add(new Population(population.Type, population.Count));
            }
        }


        private void btnAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listPopulations.Items.Count; i++)
            {
                listPopulations.SetSelected(i, true);
            }
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listPopulations.Items.Count; i++)
            {
                listPopulations.SetSelected(i, false);
            }
        }

        private void btnName_Click(object sender, EventArgs e)
        {
            listPopulations.Items.Clear();
            _populations = _populations.OrderBy(population => population.Race).ToList();
            _populations.ForEach(population => listPopulations.Items.Add(population.Race.NamePlural + ": " + population.Count));
        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            listPopulations.Items.Clear();
            _populations = _populations.OrderByDescending(population => population.Count).ToList();
            _populations.ForEach(population => listPopulations.Items.Add(population.Race.NamePlural + ": " + population.Count));
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listPopulations.SelectedIndices.Count; i++)
            {
                SelectedPopulations.Add(_populations[listPopulations.SelectedIndices[i]].Race.NamePlural);
            }

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SelectedPopulations.Clear();
            Close();
        }

    }



}
