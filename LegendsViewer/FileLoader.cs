using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using LegendsViewer.Controls;
using LegendsViewer.Legends;
using SevenZip;

namespace LegendsViewer
{
    public class FileLoader
    {
        public bool Working;
        private readonly Button _xmlButton, _historyButton, _sitesButton, _mapButton, _xmlPlusButton;
        private readonly TextBox _xmlText, _historyText, _sitesText, _mapText, _xmlPlusText;
        private readonly RichTextBox _logText;
        private readonly Label _statusLabel;
        private readonly Color _readyColor = Color.FromArgb(220, 255, 220);
        private readonly Color _notReadyColor = Color.FromArgb(255, 220, 220);
        private readonly Color _notAvailableColor = Color.DarkGray;
        private readonly Color _defaultColor = SystemColors.Control;
        private FileState _xmlState, _historyState, _sitesState, _mapState, _xmlPlusState;
        private readonly string _xmlTextDefault = "Legends XML / Archive";
        private const string HistoryTextDefault = "World History Text";
        private const string SitesTextDefault = "Sites and Populations Text";
        private const string MapTextDefault = "Map Image";
        private const string XmlPlusTextDefault = "Legends XML Plus";

        private const string FileIdentifierLegendsXml = "-legends.xml";
        private const string FileIdentifierWorldHistoryTxt = "-world_history.txt";
        private const string FileIdentifierWorldMapBmp = "-world_map.bmp";
        private const string FileIdentifierWorldSitesAndPops = "-world_sites_and_pops.txt";
        private const string FileIdentifierLegendsPlusXml = "-legends_plus.xml";

        public static string SaveDirectory { get; set; }
        public static string RegionId { get; set; }

        private FileState XmlState
        {
            get => _xmlState;
            set
            {
                _xmlState = value;
                switch (value)
                {
                    case FileState.Default:
                        _xmlText.Text = _xmlTextDefault;
                        _xmlText.BackColor = _defaultColor;
                        break;

                    case FileState.NotReady:
                        _xmlText.Text = _xmlTextDefault;
                        _xmlText.BackColor = _notReadyColor;
                        break;

                    case FileState.Ready:
                        _xmlText.BackColor = _readyColor;
                        break;

                    default:
                        throw new Exception("Unhandled File Loader File State.");
                }
            }
        }

        private FileState HistoryState
        {
            get => _historyState;
            set
            {
                _historyState = value;
                switch (value)
                {
                    case FileState.Default:
                        _historyText.Text = HistoryTextDefault;
                        _historyText.BackColor = _defaultColor;
                        break;

                    case FileState.NotReady:
                        _historyText.Text = HistoryTextDefault;
                        _historyText.BackColor = _notReadyColor;
                        break;

                    case FileState.Ready:
                        _historyText.BackColor = _readyColor;
                        break;

                    default:
                        throw new Exception("Unhandled File Loader File State.");
                }
            }
        }

        private FileState SitesState
        {
            get => _sitesState;
            set
            {
                _sitesState = value;
                switch (value)
                {
                    case FileState.Default:
                        _sitesText.Text = SitesTextDefault;
                        _sitesText.BackColor = _defaultColor;
                        break;

                    case FileState.NotReady:
                        _sitesText.Text = SitesTextDefault;
                        _sitesText.BackColor = _notReadyColor;
                        break;

                    case FileState.Ready:
                        _sitesText.BackColor = _readyColor;
                        break;

                    default:
                        throw new Exception("Unhandled File Loader File State.");
                }
            }
        }

        private FileState MapState
        {
            get => _mapState;
            set
            {
                _mapState = value;
                switch (value)
                {
                    case FileState.Default:
                        _mapText.Text = MapTextDefault;
                        _mapText.BackColor = _defaultColor;
                        break;

                    case FileState.NotReady:
                        _mapText.Text = MapTextDefault;
                        _mapText.BackColor = _notReadyColor;
                        break;

                    case FileState.Ready:
                        _mapText.BackColor = _readyColor;
                        break;

                    default:
                        throw new Exception("Unhandled File Loader File State.");
                }
            }
        }

        private FileState XmlPlusState
        {
            get => _xmlPlusState;
            set
            {
                _xmlPlusState = value;
                switch (value)
                {
                    case FileState.Default:
                        _xmlPlusText.Text = XmlPlusTextDefault;
                        _xmlPlusText.BackColor = _defaultColor;
                        break;

                    case FileState.NotReady:
                        _xmlPlusText.Text = XmlPlusTextDefault;
                        _xmlPlusText.BackColor = _notAvailableColor;
                        break;

                    case FileState.Ready:
                        _xmlPlusText.BackColor = _readyColor;
                        break;

                    default:
                        throw new Exception("Unhandled File Loader File State.");
                }
            }
        }

        private readonly List<string> _extractedFiles;

        public FileLoader(
              Button xmlButton, TextBox xmlText
            , Button historyButton, TextBox historyText
            , Button sitesButton, TextBox sitesText
            , Button mapButton, TextBox mapText
            , Button xmlPlusButton, TextBox xmlPlusText
            , Label statusLabel, RichTextBox logText)
        {
            _xmlButton = xmlButton;
            _xmlText = xmlText;
            _historyButton = historyButton;
            _historyText = historyText;
            _sitesButton = sitesButton;
            _sitesText = sitesText;
            _mapButton = mapButton;
            _mapText = mapText;
            _xmlPlusButton = xmlPlusButton;
            _xmlPlusText = xmlPlusText;
            _statusLabel = statusLabel;
            _logText = logText;

            XmlState = HistoryState = SitesState = MapState = FileState.Default;

            _xmlButton.Click += XmlClick;
            _historyButton.Click += HistoryClick;
            _sitesButton.Click += SitesClick;
            _mapButton.Click += MapClick;
            _xmlPlusButton.Click += XmlPlusClick;

            _extractedFiles = new List<string>();

            if (Environment.Is64BitProcess)
            {
                SevenZipBase.SetLibraryPath("7z64.dll");
            }
        }

        private string GetFile(string filter)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = filter
            };
            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : "";
        }

        private void XmlClick(object sender, EventArgs e)
        {
            AttemptLoadFrom(GetFile("Legends XML or Archive | *.xml; *.zip; *.7z;  "));
        }

        public void AttemptLoadFrom(string file)
        {
            if (!file.EndsWith(".xml") && file != "")
            {
                Extract(file);
            }
            else if (file.EndsWith(".xml"))
            {
                LocateOtherFiles(file);
            }
            Load();
        }

        private void HistoryClick(object sender, EventArgs e)
        {
            string historyFile = GetFile("History Text | *.txt");
            if (historyFile != "")
            {
                _historyText.Text = historyFile;
                HistoryState = FileState.Ready;
            }
            Load();
        }

        private void SitesClick(object sender, EventArgs e)
        {
            string sitesFile = GetFile("Sites and Pops Text | *.txt");
            if (sitesFile != "")
            {
                _sitesText.Text = sitesFile;
                SitesState = FileState.Ready;
            }
            Load();
        }

        private void MapClick(object sender, EventArgs e)
        {
            string mapFile = GetFile("Map Image | *.jpg;*.jpeg;*.bmp;*.png;|All Files|*.*");
            if (mapFile != "")
            {
                _mapText.Text = mapFile;
                MapState = FileState.Ready;
            }
            Load();
        }

        private void XmlPlusClick(object sender, EventArgs e)
        {
            string xmlFile = GetFile("Legends XML Plus|*legends_plus.xml;|Legends XML Plus|*.xml;|All Files|*.*");
            if (!string.IsNullOrEmpty(xmlFile))
            {
                _xmlPlusText.Text = xmlFile;
                XmlPlusState = FileState.Ready;
            }
            Load();
        }

        private void LocateOtherFiles(string xmlFile)
        {
            string directory = new FileInfo(xmlFile).DirectoryName;

            _xmlText.Text = Directory.EnumerateFiles(directory, "*" + FileIdentifierLegendsXml).FirstOrDefault();
            _historyText.Text = Directory.EnumerateFiles(directory, "*" + FileIdentifierWorldHistoryTxt).FirstOrDefault();
            _sitesText.Text = Directory.EnumerateFiles(directory, "*" + FileIdentifierWorldSitesAndPops).FirstOrDefault();
            _mapText.Text = Directory.EnumerateFiles(directory, "*" + FileIdentifierWorldMapBmp).FirstOrDefault();
            _xmlPlusText.Text = Directory.EnumerateFiles(directory, "*" + FileIdentifierLegendsPlusXml).FirstOrDefault();

            XmlState = string.IsNullOrEmpty(_xmlText.Text) ? FileState.NotReady : FileState.Ready;
            HistoryState = string.IsNullOrEmpty(_historyText.Text) ? FileState.NotReady : FileState.Ready;
            SitesState = string.IsNullOrEmpty(_sitesText.Text) ? FileState.NotReady : FileState.Ready;
            MapState = string.IsNullOrEmpty(_mapText.Text) ? FileState.NotReady : FileState.Ready;
            XmlPlusState = string.IsNullOrEmpty(_xmlPlusText.Text) ? FileState.NotReady : FileState.Ready;

            if (string.IsNullOrEmpty(_mapText.Text))
            {
                string imageFile = Array.Find(Directory.GetFiles(directory), f =>
                        (f.EndsWith(".bmp") ||
                         f.EndsWith(".png") ||
                         f.EndsWith(".jpg") ||
                         f.EndsWith(".jpeg")));
                if (!string.IsNullOrEmpty(imageFile))
                {
                    _mapText.Text = imageFile;
                    MapState = FileState.Ready;
                }
            }
        }

        private void Load()
        {
            if (XmlState != FileState.Ready || HistoryState != FileState.Ready || SitesState != FileState.Ready || MapState != FileState.Ready)
            {
                return;
            }

            if (!(File.Exists(_xmlText.Text) && File.Exists(_historyText.Text) && File.Exists(_sitesText.Text) && File.Exists(_mapText.Text)))
            {
                return;
            }

            string xmlPlusText = File.Exists(_xmlPlusText.Text) ? _xmlPlusText.Text : "";
            string[] files = { _xmlText.Text, _historyText.Text, _sitesText.Text, _mapText.Text, xmlPlusText };

            Working = true;
            _xmlButton.Enabled = _historyButton.Enabled = _sitesButton.Enabled = _mapButton.Enabled = _xmlPlusButton.Enabled = false;
            _logText.Clear();
            _statusLabel.Text = "Loading...";
            _statusLabel.ForeColor = Color.Blue;

            BackgroundWorker backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            backgroundWorker.ProgressChanged += BackgroundWorkerOnProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorkerOnRunWorkerCompleted;

            backgroundWorker.RunWorkerAsync(files);
            while (backgroundWorker.IsBusy)
            {
                Application.DoEvents();
            }

            Working = false;
            _xmlButton.Enabled = _historyButton.Enabled = _sitesButton.Enabled = _mapButton.Enabled = _xmlPlusButton.Enabled = true;
            XmlState = HistoryState = SitesState = MapState = XmlPlusState = FileState.Default;
        }

        private void BackgroundWorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            _logText.AppendText(progressChangedEventArgs.UserState + "\n");
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            string[] files = e.Argument as string[];
            try
            {
                e.Result = new World(worker, files[0], files[1], files[2], files[3], files[4]);
            }
            catch (XmlException ex)
            {
                string repairedXmlFile = RepairXmlFile(files[0], ex);
                if (repairedXmlFile != null)
                {
                    if (repairedXmlFile != files[0])
                    {
                        _extractedFiles.Add(repairedXmlFile);
                    }
                    e.Result = new World(worker, repairedXmlFile, files[1], files[2], files[3], files[4]);
                }
                else
                {
                    e.Result = null;
                }
            }
        }

        private static string RepairXmlFile(string xmlFile, XmlException xmlException)
        {
            var message = "There was an error loading this XML file! Do you wish to attempt a repair?";
#if DEBUG
            message += xmlException.ToString();
#endif
            DialogResult response =
            MessageBox.Show(message, "Error loading XML", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (response == DialogResult.Yes)
            {
                string safeFile = Path.GetTempFileName();
                using (StreamReader inputReader = new StreamReader(xmlFile))
                {
                    using (StreamWriter outputWriter = File.AppendText(safeFile))
                    {
                        string currentLine;
                        while (null != (currentLine = inputReader.ReadLine()))
                        {
                            var replacedSpecialChars = Regex.Replace(currentLine, "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]", string.Empty);
                            var fixedConspirator = replacedSpecialChars.Replace("</conspirator>", "</conspirator_hfid>");
                            if (fixedConspirator.Contains("<interrogator_hfid>"))
                            {
                                var fixedInterrogator = fixedConspirator.Replace("</convicted_hfid>", "</interrogator_hfid>");
                                outputWriter.WriteLine(fixedInterrogator);
                            }
                            else
                            {
                                outputWriter.WriteLine(fixedConspirator);
                            }
                        }
                        outputWriter.Close();
                    }
                    inputReader.Close();
                }
                //DialogResult overwrite =
                //    MessageBox.Show(
                //        "Repair completed. Would you like to overwrite the original file with the repaired version? (Note: No effect if opened from an archive)",
                //        "Repair Completed", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //if (overwrite == DialogResult.Yes)
                //{
                //    File.Delete(xmlFile);
                //    File.Copy(safeFile, xmlFile);
                //    return xmlFile;
                //}
                return safeFile;
            }
            return null;
        }

        public event EventHandler<EventArgs<World>> AfterLoad;

        private void BackgroundWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                if (e.Error != null)
                {
                    _logText.AppendText(e.Error.Message + "\n" + e.Error.StackTrace);
                    _statusLabel.Text = "ERROR!";
                    _statusLabel.ForeColor = Color.Red;
                }
                else
                {
                    AfterLoad?.Invoke(this, new EventArgs<World>(e.Result as World));
                }
            }
            else
            {
                _logText.AppendText("Repair of corrupt XML cancelled!");
                _statusLabel.Text = "ERROR!";
                _statusLabel.ForeColor = Color.Red;
            }
            foreach (string delete in _extractedFiles)
            {
                File.Delete(delete);
            }
            _extractedFiles.Clear();
        }

        private void Extract(string file)
        {
            _xmlButton.Enabled = _historyButton.Enabled = _sitesButton.Enabled = _mapButton.Enabled = false;
            _statusLabel.Text = "Extracting...";
            _statusLabel.ForeColor = Color.Orange;

            BackgroundWorker extract = new BackgroundWorker();
            extract.DoWork += extract_DoWork;
            extract.RunWorkerCompleted += extract_RunWorkerCompleted;

            extract.RunWorkerAsync(file);
            while (extract.IsBusy)
            {
                Application.DoEvents();
            }
        }

        private void extract_DoWork(object sender, DoWorkEventArgs e)
        {
            using (SevenZipExtractor extractor = new SevenZipExtractor(e.Argument as string))
            {
                string fileNameLegendsXml = extractor.ArchiveFileNames.FirstOrDefault(file => file.EndsWith(FileIdentifierLegendsXml, StringComparison.OrdinalIgnoreCase));
                if (string.IsNullOrEmpty(fileNameLegendsXml))
                {
                    throw new FileNotFoundException($"Could not find a 'region{FileIdentifierLegendsXml}' file.");
                }
                string region = fileNameLegendsXml.Replace(FileIdentifierLegendsXml, "");

                string fileNameWorldHistoryTxt = extractor.ArchiveFileNames.FirstOrDefault(file => file.Equals(region + FileIdentifierWorldHistoryTxt));
                if (string.IsNullOrEmpty(fileNameWorldHistoryTxt))
                {
                    throw new FileNotFoundException($"Could not find a 'region{FileIdentifierWorldHistoryTxt}' file.");
                }
                string fileNameWorldSitesAndPopsTxt = extractor.ArchiveFileNames.FirstOrDefault(file => file.Equals(region + FileIdentifierWorldSitesAndPops));
                if (string.IsNullOrEmpty(fileNameWorldSitesAndPopsTxt))
                {
                    throw new FileNotFoundException($"Could not find a 'region{FileIdentifierWorldSitesAndPops}' file.");
                }
                string fileNameWorldMapBmp = extractor.ArchiveFileNames.FirstOrDefault(file => file.Contains(region + FileIdentifierWorldMapBmp));
                if (string.IsNullOrEmpty(fileNameWorldMapBmp))
                {
                    var extns = new[] { ".bmp", ".png", ".jpg", ".jpeg" };
                    fileNameWorldMapBmp = extractor.ArchiveFileNames.FirstOrDefault(file => file.Contains(region) && extns.Contains(Path.GetExtension(file).ToLower()));
                }
                if (string.IsNullOrEmpty(fileNameWorldMapBmp))
                {
                    throw new FileNotFoundException($"Could not find a 'region{FileIdentifierWorldMapBmp}' file.");
                }
                string fileNameLegendsPlusXml = extractor.ArchiveFileNames.FirstOrDefault(file => file.Equals(region + FileIdentifierLegendsPlusXml));

                string outputDirectory = new FileInfo(extractor.FileName).DirectoryName;

                ExtractFile(extractor, outputDirectory, fileNameLegendsXml);
                ExtractFile(extractor, outputDirectory, fileNameWorldHistoryTxt);
                ExtractFile(extractor, outputDirectory, fileNameWorldSitesAndPopsTxt);
                ExtractFile(extractor, outputDirectory, fileNameWorldMapBmp);
                if (!string.IsNullOrEmpty(fileNameLegendsPlusXml))
                {
                    ExtractFile(extractor, outputDirectory, fileNameLegendsPlusXml);
                }
            }
        }

        private void ExtractFile(SevenZipExtractor extractor, string outputDirectory, string fileName)
        {
            if (!File.Exists(Path.Combine(outputDirectory, fileName)))
            {
                extractor.ExtractFiles(outputDirectory, fileName);
            }
            _extractedFiles.Add(Path.Combine(outputDirectory, fileName));
        }

        private void extract_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                _statusLabel.Text = "ERROR!";
                _statusLabel.ForeColor = Color.Red;
                _logText.Text = e.Error.Message;
                foreach (string delete in _extractedFiles)
                {
                    File.Delete(delete);
                }
                _extractedFiles.Clear();
                Working = false;
            }
            else
            {
                _xmlText.Text = _extractedFiles[0];
                XmlState = FileState.Ready;
                _historyText.Text = _extractedFiles[1];
                HistoryState = FileState.Ready;
                _sitesText.Text = _extractedFiles[2];
                SitesState = FileState.Ready;
                _mapText.Text = _extractedFiles[3];
                MapState = FileState.Ready;
                if (_extractedFiles.Count == 5)
                {
                    _xmlPlusText.Text = _extractedFiles[4];
                    XmlPlusState = FileState.Ready;
                }
                else
                {
                    XmlPlusState = FileState.NotReady;
                }
            }
        }

        private enum FileState
        {
            Default,
            Ready,
            NotReady
        }
    }
}