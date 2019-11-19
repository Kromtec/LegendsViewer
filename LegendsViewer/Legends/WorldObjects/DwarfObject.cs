namespace LegendsViewer.Legends.WorldObjects
{
    public abstract class DwarfObject
    {
        public virtual string GetIcon()
        {
            return "";
        }

        public virtual string ToLink(bool link = true, DwarfObject pov = null)
        {
            return "";
        }
    }
}
