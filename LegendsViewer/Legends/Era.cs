﻿using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public class Era : WorldObject
    {
        public static List<string> Filters;
        public List<War> Wars { get; set; }
        public override List<WorldEvent> FilteredEvents => Events.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList();
        public int StartYear, EndYear;
        public string Name;
        public Era(List<Property> properties, World world)
            : base(properties, world)
        {
            Wars = new List<War>();
            Id = world.Eras.Count;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "start_year": StartYear = Convert.ToInt32(property.Value); break;
                    case "name": Name = property.Value; break;
                }
            }
        }

        public Era(int startYear, int endYear, World world)
        {
            Events = new List<WorldEvent>();
            Wars = new List<War>();
            world.TempEras.Add(this);
            Id = world.Eras.Count - 1 + world.TempEras.Count;
            StartYear = startYear; EndYear = endYear; Name = "";
            Events.AddRange(world.Events.Where(ev => ev.Year >= StartYear && ev.Year <= EndYear).OrderBy(events => events.Year));
            Wars.AddRange(world.EventCollections.OfType<War>().Where(war => war.StartYear >= StartYear && war.EndYear <= EndYear && war.EndYear != -1 //entire war between
                                                                                                    || war.StartYear >= StartYear && war.StartYear <= EndYear //war started before & ended
                                                                                                    || war.EndYear >= StartYear && war.EndYear <= EndYear && war.EndYear != -1 //war started during
                                                                                                    || war.StartYear <= StartYear && war.EndYear >= EndYear //war started before & ended after
                                                                                                    || war.StartYear <= StartYear && war.EndYear == -1));
        }

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            return Name != "" ? Name : $"({StartYear} - {EndYear})";
        }
        public override string ToString() { return Name; }
    }
}
