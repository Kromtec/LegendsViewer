using System;
using System.Collections.Generic;

namespace LegendsViewer.Legends.Parser
{
    public class Property
    {
        public string Name;
        public bool Known;

        public List<Property> SubProperties { get; set; }

        private string _value;
        public string Value
        {
            get
            {
                Known = true;
                return _value ?? string.Empty;
            }
            set => _value = value;
        }

        public int ValueAsInt()
        {
            return Convert.ToInt32(Value);
        }
    }
}