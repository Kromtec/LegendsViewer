using System;

namespace LegendsViewer.Controls.Query.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ShowInAdvancedSearchResultsAttribute : Attribute
    {
        public string Header { get; }

        public ShowInAdvancedSearchResultsAttribute()
        {
        }

        public ShowInAdvancedSearchResultsAttribute(string header)
        {
            Header = header;
        }
    }
}
