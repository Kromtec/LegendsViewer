using System.Collections.Generic;
using System.Text;

namespace LegendsViewer.Legends.Parser
{
    public class ParsingErrors
    {
        private readonly List<string> _reportedErrorTypes = new List<string>();
        private readonly StringBuilder _log = new StringBuilder();

        public void Report(string description, string details = null)
        {
            if (_reportedErrorTypes.FindIndex(error => error == description) == -1)
            {
                _log.Append(description);
                if (!string.IsNullOrWhiteSpace(details))
                {
                    _log.Append(" (").Append(details).AppendLine(")");
                }
                else
                {
                    _log.AppendLine();
                }

                _reportedErrorTypes.Add(description);
            }
        }

        public string Print()
        {
            return _log.ToString();
        }
    }
}
