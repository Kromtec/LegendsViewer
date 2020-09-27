using System;

namespace LegendsViewer.Controls.Query.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowAdvancedSearchAttribute : Attribute
    {
        public bool IsSelectable { get; }
        public string Description { get; }

        public AllowAdvancedSearchAttribute()
        {
            
        }

        public AllowAdvancedSearchAttribute(string description)
        {
            Description = description;
        }

        public AllowAdvancedSearchAttribute(string description, bool isSelectable) : this(description)
        {
            IsSelectable = isSelectable;
        }

        public AllowAdvancedSearchAttribute(bool isSelectable)
        {
            IsSelectable = isSelectable;
        }
    }
}
