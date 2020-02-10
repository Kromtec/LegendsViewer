using System.Text;
using LegendsViewer.Legends;

namespace LegendsViewer.Controls.HTML
{
    class StringPrinter : HtmlPrinter
    {
        string _title;
        public StringPrinter(string htmlString)
        {
            Html = new StringBuilder();
            _title = htmlString.Substring(0, htmlString.IndexOf("\n"));
            Html.AppendLine(htmlString.Substring(htmlString.IndexOf("\n") + 1));
        }

        public override string Print()
        {
            return Html.ToString();
        }

        public override string GetTitle()
        {
            return _title;
        }
    }
}
