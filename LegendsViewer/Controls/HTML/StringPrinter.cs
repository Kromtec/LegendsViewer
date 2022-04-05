using System.Text;

namespace LegendsViewer.Controls.HTML
{
    internal class StringPrinter : HtmlPrinter
    {
        private readonly string _title;
        public StringPrinter(string htmlString)
        {
            Html = new StringBuilder();
            _title = htmlString.Substring(0, htmlString.IndexOf("\n"));
            Html.Append(htmlString, htmlString.IndexOf("\n") + 1, htmlString.Length - htmlString.IndexOf("\n") + 1).AppendLine();
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
