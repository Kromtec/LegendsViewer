using System.Text;
using LegendsViewer.Legends;

namespace LegendsViewer.Controls.HTML
{
    public class ArtFormPrinter : HtmlPrinter
    {
        private readonly ArtForm _artform;
        private readonly World _world;

        public ArtFormPrinter(ArtForm artform, World world)
        {
            _artform = artform;
            _world = world;
        }

        public override string GetTitle()
        {
            return _artform.Name;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            Html.AppendLine("<h1>" + _artform.GetIcon() + " " + _artform.Name + ", " + _artform.FormType + " Form</h1><br />");
            if (!string.IsNullOrEmpty(_artform.Description))
            {
                Html.AppendLine(_artform.Description.Replace("[B]", "<br />") + "<br /><br />");
            }
            PrintEventLog(_world, _artform.Events, ArtForm.Filters, _artform);

            return Html.ToString();
        }
    }
}
