using System.ComponentModel;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace LegendsViewer.Controls.Tabs
{
    partial class GeographyTab
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tcRegionsSub = new System.Windows.Forms.TabControl();
            this.tpOverworld = new System.Windows.Forms.TabPage();
            this.tcRegions = new System.Windows.Forms.TabControl();
            this.tpRegionSearch = new System.Windows.Forms.TabPage();
            this.lblShownResults = new System.Windows.Forms.Label();
            this.listRegionSearch = new BrightIdeasSoftware.ObjectListView();
            this.olvName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.radRegionSortArea = new System.Windows.Forms.RadioButton();
            this.radRegionSortDeaths = new System.Windows.Forms.RadioButton();
            this.radRegionSortBattles = new System.Windows.Forms.RadioButton();
            this.radRegionSortFiltered = new System.Windows.Forms.RadioButton();
            this.radRegionNone = new System.Windows.Forms.RadioButton();
            this.radRegionSortEvents = new System.Windows.Forms.RadioButton();
            this.label27 = new System.Windows.Forms.Label();
            this.cmbRegionType = new System.Windows.Forms.ComboBox();
            this.txtRegionSearch = new System.Windows.Forms.TextBox();
            this.btnRegionSearch = new System.Windows.Forms.Button();
            this.tpRegionEvents = new System.Windows.Forms.TabPage();
            this.tpUnderground = new System.Windows.Forms.TabPage();
            this.tcURegions = new System.Windows.Forms.TabControl();
            this.tpURegionSearch = new System.Windows.Forms.TabPage();
            this.lblURegionResults = new System.Windows.Forms.Label();
            this.listURegionSearch = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btnURegionSearch = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.radURegionSortArea = new System.Windows.Forms.RadioButton();
            this.radURegionSortFiltered = new System.Windows.Forms.RadioButton();
            this.radURegionNone = new System.Windows.Forms.RadioButton();
            this.radURegionSortEvents = new System.Windows.Forms.RadioButton();
            this.label28 = new System.Windows.Forms.Label();
            this.cmbURegionType = new System.Windows.Forms.ComboBox();
            this.tpURegionEvents = new System.Windows.Forms.TabPage();
            this.tpLandmasses = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblLandmassResults = new System.Windows.Forms.Label();
            this.listLandmassesSearch = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn5 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btnLandmassSearch = new System.Windows.Forms.Button();
            this.groupBox27 = new System.Windows.Forms.GroupBox();
            this.groupBox28 = new System.Windows.Forms.GroupBox();
            this.radLandmassFiltered = new System.Windows.Forms.RadioButton();
            this.radLandmassNone = new System.Windows.Forms.RadioButton();
            this.radLandmassEvents = new System.Windows.Forms.RadioButton();
            this.txtLandmassSearch = new System.Windows.Forms.TextBox();
            this.tpLandmassEvents = new System.Windows.Forms.TabPage();
            this.tpMountainPeaks = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lblMountainPeakResults = new System.Windows.Forms.Label();
            this.listMountainPeakSearch = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn6 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn9 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn7 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btnMountainPeakSearch = new System.Windows.Forms.Button();
            this.groupBox29 = new System.Windows.Forms.GroupBox();
            this.groupBox30 = new System.Windows.Forms.GroupBox();
            this.radMountainPeakFiltered = new System.Windows.Forms.RadioButton();
            this.radMountainPeakNone = new System.Windows.Forms.RadioButton();
            this.radMountainPeakEvents = new System.Windows.Forms.RadioButton();
            this.txtMountainPeakSearch = new System.Windows.Forms.TextBox();
            this.tpMountainPeakEvents = new System.Windows.Forms.TabPage();
            this.tpRivers = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblRiverResults = new System.Windows.Forms.Label();
            this.listRiverSearch = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn8 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btnRiverSearch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radRiverFiltered = new System.Windows.Forms.RadioButton();
            this.radRiverNone = new System.Windows.Forms.RadioButton();
            this.radRiverEvents = new System.Windows.Forms.RadioButton();
            this.txtRiverSearch = new System.Windows.Forms.TextBox();
            this.tpRiverEvents = new System.Windows.Forms.TabPage();
            this.tcRegionsSub.SuspendLayout();
            this.tpOverworld.SuspendLayout();
            this.tcRegions.SuspendLayout();
            this.tpRegionSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listRegionSearch)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tpUnderground.SuspendLayout();
            this.tcURegions.SuspendLayout();
            this.tpURegionSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listURegionSearch)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tpLandmasses.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listLandmassesSearch)).BeginInit();
            this.groupBox27.SuspendLayout();
            this.groupBox28.SuspendLayout();
            this.tpMountainPeaks.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listMountainPeakSearch)).BeginInit();
            this.groupBox29.SuspendLayout();
            this.groupBox30.SuspendLayout();
            this.tpRivers.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listRiverSearch)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcRegionsSub
            // 
            this.tcRegionsSub.Controls.Add(this.tpOverworld);
            this.tcRegionsSub.Controls.Add(this.tpUnderground);
            this.tcRegionsSub.Controls.Add(this.tpLandmasses);
            this.tcRegionsSub.Controls.Add(this.tpMountainPeaks);
            this.tcRegionsSub.Controls.Add(this.tpRivers);
            this.tcRegionsSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcRegionsSub.Location = new System.Drawing.Point(0, 0);
            this.tcRegionsSub.Multiline = true;
            this.tcRegionsSub.Name = "tcRegionsSub";
            this.tcRegionsSub.SelectedIndex = 0;
            this.tcRegionsSub.Size = new System.Drawing.Size(269, 518);
            this.tcRegionsSub.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tcRegionsSub.TabIndex = 2;
            // 
            // tpOverworld
            // 
            this.tpOverworld.Controls.Add(this.tcRegions);
            this.tpOverworld.Location = new System.Drawing.Point(4, 40);
            this.tpOverworld.Name = "tpOverworld";
            this.tpOverworld.Padding = new System.Windows.Forms.Padding(3);
            this.tpOverworld.Size = new System.Drawing.Size(261, 474);
            this.tpOverworld.TabIndex = 0;
            this.tpOverworld.Text = "Regions";
            this.tpOverworld.UseVisualStyleBackColor = true;
            // 
            // tcRegions
            // 
            this.tcRegions.Controls.Add(this.tpRegionSearch);
            this.tcRegions.Controls.Add(this.tpRegionEvents);
            this.tcRegions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcRegions.Location = new System.Drawing.Point(3, 3);
            this.tcRegions.Name = "tcRegions";
            this.tcRegions.SelectedIndex = 0;
            this.tcRegions.Size = new System.Drawing.Size(255, 468);
            this.tcRegions.TabIndex = 1;
            // 
            // tpRegionSearch
            // 
            this.tpRegionSearch.Controls.Add(this.lblShownResults);
            this.tpRegionSearch.Controls.Add(this.listRegionSearch);
            this.tpRegionSearch.Controls.Add(this.groupBox2);
            this.tpRegionSearch.Controls.Add(this.txtRegionSearch);
            this.tpRegionSearch.Controls.Add(this.btnRegionSearch);
            this.tpRegionSearch.Location = new System.Drawing.Point(4, 22);
            this.tpRegionSearch.Name = "tpRegionSearch";
            this.tpRegionSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tpRegionSearch.Size = new System.Drawing.Size(247, 442);
            this.tpRegionSearch.TabIndex = 0;
            this.tpRegionSearch.Text = "Search";
            this.tpRegionSearch.UseVisualStyleBackColor = true;
            // 
            // lblShownResults
            // 
            this.lblShownResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblShownResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShownResults.Location = new System.Drawing.Point(161, 262);
            this.lblShownResults.Name = "lblShownResults";
            this.lblShownResults.Size = new System.Drawing.Size(80, 11);
            this.lblShownResults.TabIndex = 54;
            this.lblShownResults.Text = "0 / 0";
            this.lblShownResults.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.hint.SetToolTip(this.lblShownResults, "Results Shown");
            // 
            // listRegionSearch
            // 
            this.listRegionSearch.AllColumns.Add(this.olvName);
            this.listRegionSearch.AllColumns.Add(this.olvType);
            this.listRegionSearch.AlternateRowBackColor = System.Drawing.SystemColors.InactiveBorder;
            this.listRegionSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listRegionSearch.CellEditUseWholeCell = false;
            this.listRegionSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvName,
            this.olvType});
            this.listRegionSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.listRegionSearch.FullRowSelect = true;
            this.listRegionSearch.GridLines = true;
            this.listRegionSearch.HeaderWordWrap = true;
            this.listRegionSearch.HideSelection = false;
            this.listRegionSearch.Location = new System.Drawing.Point(3, 32);
            this.listRegionSearch.Name = "listRegionSearch";
            this.listRegionSearch.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.listRegionSearch.ShowCommandMenuOnRightClick = true;
            this.listRegionSearch.ShowImagesOnSubItems = true;
            this.listRegionSearch.ShowItemCountOnGroups = true;
            this.listRegionSearch.Size = new System.Drawing.Size(240, 227);
            this.listRegionSearch.TabIndex = 53;
            this.listRegionSearch.UseAlternatingBackColors = true;
            this.listRegionSearch.UseCompatibleStateImageBehavior = false;
            this.listRegionSearch.UseFiltering = true;
            this.listRegionSearch.UseHotItem = true;
            this.listRegionSearch.UseHyperlinks = true;
            this.listRegionSearch.View = System.Windows.Forms.View.Details;
            this.listRegionSearch.SelectedIndexChanged += new System.EventHandler(this.listRegionSearch_SelectedIndexChanged);
            // 
            // olvName
            // 
            this.olvName.AspectName = "Name";
            this.olvName.IsEditable = false;
            this.olvName.MinimumWidth = 50;
            this.olvName.Text = "Name";
            this.olvName.UseInitialLetterForGroup = true;
            this.olvName.Width = 145;
            // 
            // olvType
            // 
            this.olvType.AspectName = "Type";
            this.olvType.IsEditable = false;
            this.olvType.Text = "Type";
            this.olvType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvType.Width = 75;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.groupBox7);
            this.groupBox2.Controls.Add(this.label27);
            this.groupBox2.Controls.Add(this.cmbRegionType);
            this.groupBox2.Location = new System.Drawing.Point(3, 276);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(240, 163);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filter / Sort";
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.radRegionSortArea);
            this.groupBox7.Controls.Add(this.radRegionSortDeaths);
            this.groupBox7.Controls.Add(this.radRegionSortBattles);
            this.groupBox7.Controls.Add(this.radRegionSortFiltered);
            this.groupBox7.Controls.Add(this.radRegionNone);
            this.groupBox7.Controls.Add(this.radRegionSortEvents);
            this.groupBox7.Location = new System.Drawing.Point(6, 59);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(226, 98);
            this.groupBox7.TabIndex = 13;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Sort By";
            // 
            // radRegionSortArea
            // 
            this.radRegionSortArea.AutoSize = true;
            this.radRegionSortArea.Location = new System.Drawing.Point(116, 19);
            this.radRegionSortArea.Name = "radRegionSortArea";
            this.radRegionSortArea.Size = new System.Drawing.Size(47, 17);
            this.radRegionSortArea.TabIndex = 17;
            this.radRegionSortArea.Text = "Area";
            this.radRegionSortArea.UseVisualStyleBackColor = true;
            this.radRegionSortArea.CheckedChanged += new System.EventHandler(this.SearchRegionList);
            // 
            // radRegionSortDeaths
            // 
            this.radRegionSortDeaths.AutoSize = true;
            this.radRegionSortDeaths.Location = new System.Drawing.Point(116, 42);
            this.radRegionSortDeaths.Name = "radRegionSortDeaths";
            this.radRegionSortDeaths.Size = new System.Drawing.Size(59, 17);
            this.radRegionSortDeaths.TabIndex = 16;
            this.radRegionSortDeaths.TabStop = true;
            this.radRegionSortDeaths.Text = "Deaths";
            this.radRegionSortDeaths.UseVisualStyleBackColor = true;
            this.radRegionSortDeaths.CheckedChanged += new System.EventHandler(this.SearchRegionList);
            // 
            // radRegionSortBattles
            // 
            this.radRegionSortBattles.AutoSize = true;
            this.radRegionSortBattles.Location = new System.Drawing.Point(116, 65);
            this.radRegionSortBattles.Name = "radRegionSortBattles";
            this.radRegionSortBattles.Size = new System.Drawing.Size(57, 17);
            this.radRegionSortBattles.TabIndex = 15;
            this.radRegionSortBattles.TabStop = true;
            this.radRegionSortBattles.Text = "Battles";
            this.radRegionSortBattles.UseVisualStyleBackColor = true;
            this.radRegionSortBattles.CheckedChanged += new System.EventHandler(this.SearchRegionList);
            // 
            // radRegionSortFiltered
            // 
            this.radRegionSortFiltered.AutoSize = true;
            this.radRegionSortFiltered.Location = new System.Drawing.Point(6, 42);
            this.radRegionSortFiltered.Name = "radRegionSortFiltered";
            this.radRegionSortFiltered.Size = new System.Drawing.Size(95, 17);
            this.radRegionSortFiltered.TabIndex = 14;
            this.radRegionSortFiltered.TabStop = true;
            this.radRegionSortFiltered.Text = "Filtered Events";
            this.radRegionSortFiltered.UseVisualStyleBackColor = true;
            this.radRegionSortFiltered.CheckedChanged += new System.EventHandler(this.SearchRegionList);
            // 
            // radRegionNone
            // 
            this.radRegionNone.AutoSize = true;
            this.radRegionNone.Checked = true;
            this.radRegionNone.Location = new System.Drawing.Point(6, 65);
            this.radRegionNone.Name = "radRegionNone";
            this.radRegionNone.Size = new System.Drawing.Size(51, 17);
            this.radRegionNone.TabIndex = 13;
            this.radRegionNone.TabStop = true;
            this.radRegionNone.Text = "None";
            this.radRegionNone.UseVisualStyleBackColor = true;
            this.radRegionNone.CheckedChanged += new System.EventHandler(this.SearchRegionList);
            // 
            // radRegionSortEvents
            // 
            this.radRegionSortEvents.AutoSize = true;
            this.radRegionSortEvents.Location = new System.Drawing.Point(6, 19);
            this.radRegionSortEvents.Name = "radRegionSortEvents";
            this.radRegionSortEvents.Size = new System.Drawing.Size(58, 17);
            this.radRegionSortEvents.TabIndex = 12;
            this.radRegionSortEvents.Text = "Events";
            this.radRegionSortEvents.UseVisualStyleBackColor = true;
            this.radRegionSortEvents.CheckedChanged += new System.EventHandler(this.SearchRegionList);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(6, 16);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(31, 13);
            this.label27.TabIndex = 12;
            this.label27.Text = "Type";
            // 
            // cmbRegionType
            // 
            this.cmbRegionType.FormattingEnabled = true;
            this.cmbRegionType.Location = new System.Drawing.Point(6, 32);
            this.cmbRegionType.Name = "cmbRegionType";
            this.cmbRegionType.Size = new System.Drawing.Size(121, 21);
            this.cmbRegionType.TabIndex = 0;
            this.cmbRegionType.SelectedIndexChanged += new System.EventHandler(this.SearchRegionList);
            // 
            // txtRegionSearch
            // 
            this.txtRegionSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRegionSearch.Location = new System.Drawing.Point(81, 5);
            this.txtRegionSearch.Name = "txtRegionSearch";
            this.txtRegionSearch.Size = new System.Drawing.Size(163, 20);
            this.txtRegionSearch.TabIndex = 33;
            this.txtRegionSearch.TextChanged += new System.EventHandler(this.SearchRegionList);
            // 
            // btnRegionSearch
            // 
            this.btnRegionSearch.Location = new System.Drawing.Point(3, 3);
            this.btnRegionSearch.Name = "btnRegionSearch";
            this.btnRegionSearch.Size = new System.Drawing.Size(75, 23);
            this.btnRegionSearch.TabIndex = 31;
            this.btnRegionSearch.Text = "Search";
            this.btnRegionSearch.UseVisualStyleBackColor = true;
            this.btnRegionSearch.Click += new System.EventHandler(this.SearchRegionList);
            // 
            // tpRegionEvents
            // 
            this.tpRegionEvents.AutoScroll = true;
            this.tpRegionEvents.Location = new System.Drawing.Point(4, 22);
            this.tpRegionEvents.Name = "tpRegionEvents";
            this.tpRegionEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tpRegionEvents.Size = new System.Drawing.Size(247, 442);
            this.tpRegionEvents.TabIndex = 1;
            this.tpRegionEvents.Text = "Events";
            this.tpRegionEvents.UseVisualStyleBackColor = true;
            // 
            // tpUnderground
            // 
            this.tpUnderground.Controls.Add(this.tcURegions);
            this.tpUnderground.Location = new System.Drawing.Point(4, 40);
            this.tpUnderground.Name = "tpUnderground";
            this.tpUnderground.Padding = new System.Windows.Forms.Padding(3);
            this.tpUnderground.Size = new System.Drawing.Size(261, 474);
            this.tpUnderground.TabIndex = 1;
            this.tpUnderground.Text = "Underground";
            this.tpUnderground.UseVisualStyleBackColor = true;
            // 
            // tcURegions
            // 
            this.tcURegions.Controls.Add(this.tpURegionSearch);
            this.tcURegions.Controls.Add(this.tpURegionEvents);
            this.tcURegions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcURegions.Location = new System.Drawing.Point(3, 3);
            this.tcURegions.Name = "tcURegions";
            this.tcURegions.SelectedIndex = 0;
            this.tcURegions.Size = new System.Drawing.Size(255, 468);
            this.tcURegions.TabIndex = 1;
            // 
            // tpURegionSearch
            // 
            this.tpURegionSearch.Controls.Add(this.lblURegionResults);
            this.tpURegionSearch.Controls.Add(this.listURegionSearch);
            this.tpURegionSearch.Controls.Add(this.btnURegionSearch);
            this.tpURegionSearch.Controls.Add(this.groupBox3);
            this.tpURegionSearch.Location = new System.Drawing.Point(4, 22);
            this.tpURegionSearch.Name = "tpURegionSearch";
            this.tpURegionSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tpURegionSearch.Size = new System.Drawing.Size(247, 442);
            this.tpURegionSearch.TabIndex = 0;
            this.tpURegionSearch.Text = "Search";
            this.tpURegionSearch.UseVisualStyleBackColor = true;
            // 
            // lblURegionResults
            // 
            this.lblURegionResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblURegionResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblURegionResults.Location = new System.Drawing.Point(161, 262);
            this.lblURegionResults.Name = "lblURegionResults";
            this.lblURegionResults.Size = new System.Drawing.Size(80, 11);
            this.lblURegionResults.TabIndex = 56;
            this.lblURegionResults.Text = "0 / 0";
            this.lblURegionResults.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.hint.SetToolTip(this.lblURegionResults, "Results Shown");
            // 
            // listURegionSearch
            // 
            this.listURegionSearch.AllColumns.Add(this.olvColumn1);
            this.listURegionSearch.AllColumns.Add(this.olvColumn2);
            this.listURegionSearch.AllColumns.Add(this.olvColumn3);
            this.listURegionSearch.AllColumns.Add(this.olvColumn4);
            this.listURegionSearch.AlternateRowBackColor = System.Drawing.SystemColors.InactiveBorder;
            this.listURegionSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listURegionSearch.CellEditUseWholeCell = false;
            this.listURegionSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.olvColumn3,
            this.olvColumn4});
            this.listURegionSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.listURegionSearch.FullRowSelect = true;
            this.listURegionSearch.GridLines = true;
            this.listURegionSearch.HeaderWordWrap = true;
            this.listURegionSearch.HideSelection = false;
            this.listURegionSearch.Location = new System.Drawing.Point(3, 32);
            this.listURegionSearch.Name = "listURegionSearch";
            this.listURegionSearch.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.listURegionSearch.ShowCommandMenuOnRightClick = true;
            this.listURegionSearch.ShowImagesOnSubItems = true;
            this.listURegionSearch.ShowItemCountOnGroups = true;
            this.listURegionSearch.Size = new System.Drawing.Size(240, 227);
            this.listURegionSearch.TabIndex = 55;
            this.listURegionSearch.UseAlternatingBackColors = true;
            this.listURegionSearch.UseCompatibleStateImageBehavior = false;
            this.listURegionSearch.UseFiltering = true;
            this.listURegionSearch.UseHotItem = true;
            this.listURegionSearch.UseHyperlinks = true;
            this.listURegionSearch.View = System.Windows.Forms.View.Details;
            this.listURegionSearch.SelectedIndexChanged += new System.EventHandler(this.listURegionSearch_SelectedIndexChanged);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "ID";
            this.olvColumn1.IsEditable = false;
            this.olvColumn1.Text = "ID";
            this.olvColumn1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumn1.Width = 30;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Type";
            this.olvColumn2.IsEditable = false;
            this.olvColumn2.Text = "Type";
            this.olvColumn2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumn2.Width = 90;
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "Depth";
            this.olvColumn3.Text = "Depth";
            this.olvColumn3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumn3.Width = 45;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "EventCount";
            this.olvColumn4.Text = "Events";
            this.olvColumn4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumn4.Width = 55;
            // 
            // btnURegionSearch
            // 
            this.btnURegionSearch.Location = new System.Drawing.Point(3, 3);
            this.btnURegionSearch.Name = "btnURegionSearch";
            this.btnURegionSearch.Size = new System.Drawing.Size(75, 23);
            this.btnURegionSearch.TabIndex = 34;
            this.btnURegionSearch.Text = "Search";
            this.btnURegionSearch.UseVisualStyleBackColor = true;
            this.btnURegionSearch.Click += new System.EventHandler(this.SearchURegionList);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.groupBox8);
            this.groupBox3.Controls.Add(this.label28);
            this.groupBox3.Controls.Add(this.cmbURegionType);
            this.groupBox3.Location = new System.Drawing.Point(3, 274);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(240, 162);
            this.groupBox3.TabIndex = 33;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Filter / Sort";
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.radURegionSortArea);
            this.groupBox8.Controls.Add(this.radURegionSortFiltered);
            this.groupBox8.Controls.Add(this.radURegionNone);
            this.groupBox8.Controls.Add(this.radURegionSortEvents);
            this.groupBox8.Location = new System.Drawing.Point(6, 64);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(228, 92);
            this.groupBox8.TabIndex = 13;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Sort By";
            // 
            // radURegionSortArea
            // 
            this.radURegionSortArea.AutoSize = true;
            this.radURegionSortArea.Location = new System.Drawing.Point(114, 19);
            this.radURegionSortArea.Name = "radURegionSortArea";
            this.radURegionSortArea.Size = new System.Drawing.Size(47, 17);
            this.radURegionSortArea.TabIndex = 18;
            this.radURegionSortArea.Text = "Area";
            this.radURegionSortArea.UseVisualStyleBackColor = true;
            this.radURegionSortArea.CheckedChanged += new System.EventHandler(this.SearchURegionList);
            // 
            // radURegionSortFiltered
            // 
            this.radURegionSortFiltered.AutoSize = true;
            this.radURegionSortFiltered.Location = new System.Drawing.Point(6, 42);
            this.radURegionSortFiltered.Name = "radURegionSortFiltered";
            this.radURegionSortFiltered.Size = new System.Drawing.Size(95, 17);
            this.radURegionSortFiltered.TabIndex = 14;
            this.radURegionSortFiltered.TabStop = true;
            this.radURegionSortFiltered.Text = "Filtered Events";
            this.radURegionSortFiltered.UseVisualStyleBackColor = true;
            this.radURegionSortFiltered.CheckedChanged += new System.EventHandler(this.SearchURegionList);
            // 
            // radURegionNone
            // 
            this.radURegionNone.AutoSize = true;
            this.radURegionNone.Checked = true;
            this.radURegionNone.Location = new System.Drawing.Point(6, 65);
            this.radURegionNone.Name = "radURegionNone";
            this.radURegionNone.Size = new System.Drawing.Size(51, 17);
            this.radURegionNone.TabIndex = 13;
            this.radURegionNone.TabStop = true;
            this.radURegionNone.Text = "None";
            this.radURegionNone.UseVisualStyleBackColor = true;
            this.radURegionNone.CheckedChanged += new System.EventHandler(this.SearchURegionList);
            // 
            // radURegionSortEvents
            // 
            this.radURegionSortEvents.AutoSize = true;
            this.radURegionSortEvents.Location = new System.Drawing.Point(6, 19);
            this.radURegionSortEvents.Name = "radURegionSortEvents";
            this.radURegionSortEvents.Size = new System.Drawing.Size(58, 17);
            this.radURegionSortEvents.TabIndex = 12;
            this.radURegionSortEvents.Text = "Events";
            this.radURegionSortEvents.UseVisualStyleBackColor = true;
            this.radURegionSortEvents.CheckedChanged += new System.EventHandler(this.SearchURegionList);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(6, 16);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(31, 13);
            this.label28.TabIndex = 12;
            this.label28.Text = "Type";
            // 
            // cmbURegionType
            // 
            this.cmbURegionType.FormattingEnabled = true;
            this.cmbURegionType.Location = new System.Drawing.Point(6, 32);
            this.cmbURegionType.Name = "cmbURegionType";
            this.cmbURegionType.Size = new System.Drawing.Size(121, 21);
            this.cmbURegionType.TabIndex = 0;
            this.cmbURegionType.SelectedIndexChanged += new System.EventHandler(this.SearchURegionList);
            // 
            // tpURegionEvents
            // 
            this.tpURegionEvents.AutoScroll = true;
            this.tpURegionEvents.Location = new System.Drawing.Point(4, 22);
            this.tpURegionEvents.Name = "tpURegionEvents";
            this.tpURegionEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tpURegionEvents.Size = new System.Drawing.Size(247, 442);
            this.tpURegionEvents.TabIndex = 1;
            this.tpURegionEvents.Text = "Events";
            this.tpURegionEvents.UseVisualStyleBackColor = true;
            // 
            // tpLandmasses
            // 
            this.tpLandmasses.Controls.Add(this.tabControl2);
            this.tpLandmasses.Location = new System.Drawing.Point(4, 40);
            this.tpLandmasses.Name = "tpLandmasses";
            this.tpLandmasses.Size = new System.Drawing.Size(261, 474);
            this.tpLandmasses.TabIndex = 2;
            this.tpLandmasses.Text = "Landmasses";
            this.tpLandmasses.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tpLandmassEvents);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(261, 474);
            this.tabControl2.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblLandmassResults);
            this.tabPage1.Controls.Add(this.listLandmassesSearch);
            this.tabPage1.Controls.Add(this.btnLandmassSearch);
            this.tabPage1.Controls.Add(this.groupBox27);
            this.tabPage1.Controls.Add(this.txtLandmassSearch);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(253, 448);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Search";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblLandmassResults
            // 
            this.lblLandmassResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLandmassResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLandmassResults.Location = new System.Drawing.Point(167, 261);
            this.lblLandmassResults.Name = "lblLandmassResults";
            this.lblLandmassResults.Size = new System.Drawing.Size(80, 11);
            this.lblLandmassResults.TabIndex = 56;
            this.lblLandmassResults.Text = "0 / 0";
            this.lblLandmassResults.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.hint.SetToolTip(this.lblLandmassResults, "Results Shown");
            // 
            // listLandmassesSearch
            // 
            this.listLandmassesSearch.AllColumns.Add(this.olvColumn5);
            this.listLandmassesSearch.AlternateRowBackColor = System.Drawing.SystemColors.InactiveBorder;
            this.listLandmassesSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listLandmassesSearch.CellEditUseWholeCell = false;
            this.listLandmassesSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn5});
            this.listLandmassesSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.listLandmassesSearch.FullRowSelect = true;
            this.listLandmassesSearch.GridLines = true;
            this.listLandmassesSearch.HeaderWordWrap = true;
            this.listLandmassesSearch.HideSelection = false;
            this.listLandmassesSearch.Location = new System.Drawing.Point(3, 31);
            this.listLandmassesSearch.Name = "listLandmassesSearch";
            this.listLandmassesSearch.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.listLandmassesSearch.ShowCommandMenuOnRightClick = true;
            this.listLandmassesSearch.ShowImagesOnSubItems = true;
            this.listLandmassesSearch.ShowItemCountOnGroups = true;
            this.listLandmassesSearch.Size = new System.Drawing.Size(247, 227);
            this.listLandmassesSearch.TabIndex = 55;
            this.listLandmassesSearch.UseAlternatingBackColors = true;
            this.listLandmassesSearch.UseCompatibleStateImageBehavior = false;
            this.listLandmassesSearch.UseFiltering = true;
            this.listLandmassesSearch.UseHotItem = true;
            this.listLandmassesSearch.UseHyperlinks = true;
            this.listLandmassesSearch.View = System.Windows.Forms.View.Details;
            this.listLandmassesSearch.SelectedIndexChanged += new System.EventHandler(this.listLandmassSearch_SelectedIndexChanged);
            // 
            // olvColumn5
            // 
            this.olvColumn5.AspectName = "Name";
            this.olvColumn5.IsEditable = false;
            this.olvColumn5.MinimumWidth = 50;
            this.olvColumn5.Text = "Name";
            this.olvColumn5.UseInitialLetterForGroup = true;
            this.olvColumn5.Width = 220;
            // 
            // btnLandmassSearch
            // 
            this.btnLandmassSearch.Location = new System.Drawing.Point(3, 3);
            this.btnLandmassSearch.Name = "btnLandmassSearch";
            this.btnLandmassSearch.Size = new System.Drawing.Size(75, 23);
            this.btnLandmassSearch.TabIndex = 46;
            this.btnLandmassSearch.Text = "Search";
            this.btnLandmassSearch.UseVisualStyleBackColor = true;
            this.btnLandmassSearch.Click += new System.EventHandler(this.SearchLandmassList);
            // 
            // groupBox27
            // 
            this.groupBox27.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox27.Controls.Add(this.groupBox28);
            this.groupBox27.Location = new System.Drawing.Point(3, 278);
            this.groupBox27.Name = "groupBox27";
            this.groupBox27.Size = new System.Drawing.Size(247, 164);
            this.groupBox27.TabIndex = 45;
            this.groupBox27.TabStop = false;
            this.groupBox27.Text = "Filter / Sort";
            // 
            // groupBox28
            // 
            this.groupBox28.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox28.Controls.Add(this.radLandmassFiltered);
            this.groupBox28.Controls.Add(this.radLandmassNone);
            this.groupBox28.Controls.Add(this.radLandmassEvents);
            this.groupBox28.Location = new System.Drawing.Point(133, 19);
            this.groupBox28.Name = "groupBox28";
            this.groupBox28.Size = new System.Drawing.Size(108, 126);
            this.groupBox28.TabIndex = 15;
            this.groupBox28.TabStop = false;
            this.groupBox28.Text = "Sort By";
            // 
            // radLandmassFiltered
            // 
            this.radLandmassFiltered.AutoSize = true;
            this.radLandmassFiltered.Location = new System.Drawing.Point(6, 42);
            this.radLandmassFiltered.Name = "radLandmassFiltered";
            this.radLandmassFiltered.Size = new System.Drawing.Size(95, 17);
            this.radLandmassFiltered.TabIndex = 16;
            this.radLandmassFiltered.TabStop = true;
            this.radLandmassFiltered.Text = "Filtered Events";
            this.radLandmassFiltered.UseVisualStyleBackColor = true;
            this.radLandmassFiltered.CheckedChanged += new System.EventHandler(this.SearchLandmassList);
            // 
            // radLandmassNone
            // 
            this.radLandmassNone.AutoSize = true;
            this.radLandmassNone.Checked = true;
            this.radLandmassNone.Location = new System.Drawing.Point(6, 65);
            this.radLandmassNone.Name = "radLandmassNone";
            this.radLandmassNone.Size = new System.Drawing.Size(51, 17);
            this.radLandmassNone.TabIndex = 14;
            this.radLandmassNone.TabStop = true;
            this.radLandmassNone.Text = "None";
            this.radLandmassNone.UseVisualStyleBackColor = true;
            this.radLandmassNone.CheckedChanged += new System.EventHandler(this.SearchLandmassList);
            // 
            // radLandmassEvents
            // 
            this.radLandmassEvents.AutoSize = true;
            this.radLandmassEvents.Location = new System.Drawing.Point(6, 19);
            this.radLandmassEvents.Name = "radLandmassEvents";
            this.radLandmassEvents.Size = new System.Drawing.Size(58, 17);
            this.radLandmassEvents.TabIndex = 13;
            this.radLandmassEvents.Text = "Events";
            this.radLandmassEvents.UseVisualStyleBackColor = true;
            this.radLandmassEvents.CheckedChanged += new System.EventHandler(this.SearchLandmassList);
            // 
            // txtLandmassSearch
            // 
            this.txtLandmassSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLandmassSearch.Location = new System.Drawing.Point(81, 5);
            this.txtLandmassSearch.Name = "txtLandmassSearch";
            this.txtLandmassSearch.Size = new System.Drawing.Size(169, 20);
            this.txtLandmassSearch.TabIndex = 44;
            this.txtLandmassSearch.TextChanged += new System.EventHandler(this.SearchLandmassList);
            // 
            // tpLandmassEvents
            // 
            this.tpLandmassEvents.Location = new System.Drawing.Point(4, 22);
            this.tpLandmassEvents.Name = "tpLandmassEvents";
            this.tpLandmassEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tpLandmassEvents.Size = new System.Drawing.Size(253, 448);
            this.tpLandmassEvents.TabIndex = 1;
            this.tpLandmassEvents.Text = "Events";
            this.tpLandmassEvents.UseVisualStyleBackColor = true;
            // 
            // tpMountainPeaks
            // 
            this.tpMountainPeaks.Controls.Add(this.tabControl3);
            this.tpMountainPeaks.Location = new System.Drawing.Point(4, 40);
            this.tpMountainPeaks.Name = "tpMountainPeaks";
            this.tpMountainPeaks.Size = new System.Drawing.Size(261, 474);
            this.tpMountainPeaks.TabIndex = 3;
            this.tpMountainPeaks.Text = "Mountains";
            this.tpMountainPeaks.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage3);
            this.tabControl3.Controls.Add(this.tpMountainPeakEvents);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(261, 474);
            this.tabControl3.TabIndex = 6;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lblMountainPeakResults);
            this.tabPage3.Controls.Add(this.listMountainPeakSearch);
            this.tabPage3.Controls.Add(this.btnMountainPeakSearch);
            this.tabPage3.Controls.Add(this.groupBox29);
            this.tabPage3.Controls.Add(this.txtMountainPeakSearch);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(253, 448);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Search";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lblMountainPeakResults
            // 
            this.lblMountainPeakResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMountainPeakResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMountainPeakResults.Location = new System.Drawing.Point(160, 264);
            this.lblMountainPeakResults.Name = "lblMountainPeakResults";
            this.lblMountainPeakResults.Size = new System.Drawing.Size(87, 11);
            this.lblMountainPeakResults.TabIndex = 56;
            this.lblMountainPeakResults.Text = "0 / 0";
            this.lblMountainPeakResults.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.hint.SetToolTip(this.lblMountainPeakResults, "Results Shown");
            // 
            // listMountainPeakSearch
            // 
            this.listMountainPeakSearch.AllColumns.Add(this.olvColumn6);
            this.listMountainPeakSearch.AllColumns.Add(this.olvColumn9);
            this.listMountainPeakSearch.AllColumns.Add(this.olvColumn7);
            this.listMountainPeakSearch.AlternateRowBackColor = System.Drawing.SystemColors.InactiveBorder;
            this.listMountainPeakSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listMountainPeakSearch.CellEditUseWholeCell = false;
            this.listMountainPeakSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn6,
            this.olvColumn9,
            this.olvColumn7});
            this.listMountainPeakSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.listMountainPeakSearch.FullRowSelect = true;
            this.listMountainPeakSearch.GridLines = true;
            this.listMountainPeakSearch.HeaderWordWrap = true;
            this.listMountainPeakSearch.HideSelection = false;
            this.listMountainPeakSearch.Location = new System.Drawing.Point(3, 31);
            this.listMountainPeakSearch.Name = "listMountainPeakSearch";
            this.listMountainPeakSearch.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.listMountainPeakSearch.ShowCommandMenuOnRightClick = true;
            this.listMountainPeakSearch.ShowImagesOnSubItems = true;
            this.listMountainPeakSearch.ShowItemCountOnGroups = true;
            this.listMountainPeakSearch.Size = new System.Drawing.Size(247, 230);
            this.listMountainPeakSearch.TabIndex = 55;
            this.listMountainPeakSearch.UseAlternatingBackColors = true;
            this.listMountainPeakSearch.UseCompatibleStateImageBehavior = false;
            this.listMountainPeakSearch.UseFiltering = true;
            this.listMountainPeakSearch.UseHotItem = true;
            this.listMountainPeakSearch.UseHyperlinks = true;
            this.listMountainPeakSearch.View = System.Windows.Forms.View.Details;
            this.listMountainPeakSearch.SelectedIndexChanged += new System.EventHandler(this.listMountainPeakSearch_SelectedIndexChanged);
            // 
            // olvColumn6
            // 
            this.olvColumn6.AspectName = "Name";
            this.olvColumn6.IsEditable = false;
            this.olvColumn6.MinimumWidth = 50;
            this.olvColumn6.Text = "Name";
            this.olvColumn6.UseInitialLetterForGroup = true;
            this.olvColumn6.Width = 120;
            // 
            // olvColumn9
            // 
            this.olvColumn9.AspectName = "TypeAsString";
            this.olvColumn9.Text = "Type";
            this.olvColumn9.Width = 65;
            // 
            // olvColumn7
            // 
            this.olvColumn7.AspectName = "HeightMeter";
            this.olvColumn7.IsEditable = false;
            this.olvColumn7.Text = "Height";
            this.olvColumn7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumn7.Width = 55;
            // 
            // btnMountainPeakSearch
            // 
            this.btnMountainPeakSearch.Location = new System.Drawing.Point(3, 3);
            this.btnMountainPeakSearch.Name = "btnMountainPeakSearch";
            this.btnMountainPeakSearch.Size = new System.Drawing.Size(75, 23);
            this.btnMountainPeakSearch.TabIndex = 46;
            this.btnMountainPeakSearch.Text = "Search";
            this.btnMountainPeakSearch.UseVisualStyleBackColor = true;
            this.btnMountainPeakSearch.Click += new System.EventHandler(this.SearchMountainPeakList);
            // 
            // groupBox29
            // 
            this.groupBox29.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox29.Controls.Add(this.groupBox30);
            this.groupBox29.Location = new System.Drawing.Point(3, 278);
            this.groupBox29.Name = "groupBox29";
            this.groupBox29.Size = new System.Drawing.Size(247, 164);
            this.groupBox29.TabIndex = 45;
            this.groupBox29.TabStop = false;
            this.groupBox29.Text = "Filter / Sort";
            // 
            // groupBox30
            // 
            this.groupBox30.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox30.Controls.Add(this.radMountainPeakFiltered);
            this.groupBox30.Controls.Add(this.radMountainPeakNone);
            this.groupBox30.Controls.Add(this.radMountainPeakEvents);
            this.groupBox30.Location = new System.Drawing.Point(133, 19);
            this.groupBox30.Name = "groupBox30";
            this.groupBox30.Size = new System.Drawing.Size(108, 126);
            this.groupBox30.TabIndex = 15;
            this.groupBox30.TabStop = false;
            this.groupBox30.Text = "Sort By";
            // 
            // radMountainPeakFiltered
            // 
            this.radMountainPeakFiltered.AutoSize = true;
            this.radMountainPeakFiltered.Location = new System.Drawing.Point(6, 42);
            this.radMountainPeakFiltered.Name = "radMountainPeakFiltered";
            this.radMountainPeakFiltered.Size = new System.Drawing.Size(95, 17);
            this.radMountainPeakFiltered.TabIndex = 16;
            this.radMountainPeakFiltered.TabStop = true;
            this.radMountainPeakFiltered.Text = "Filtered Events";
            this.radMountainPeakFiltered.UseVisualStyleBackColor = true;
            this.radMountainPeakFiltered.CheckedChanged += new System.EventHandler(this.SearchMountainPeakList);
            // 
            // radMountainPeakNone
            // 
            this.radMountainPeakNone.AutoSize = true;
            this.radMountainPeakNone.Checked = true;
            this.radMountainPeakNone.Location = new System.Drawing.Point(6, 65);
            this.radMountainPeakNone.Name = "radMountainPeakNone";
            this.radMountainPeakNone.Size = new System.Drawing.Size(51, 17);
            this.radMountainPeakNone.TabIndex = 14;
            this.radMountainPeakNone.TabStop = true;
            this.radMountainPeakNone.Text = "None";
            this.radMountainPeakNone.UseVisualStyleBackColor = true;
            this.radMountainPeakNone.CheckedChanged += new System.EventHandler(this.SearchMountainPeakList);
            // 
            // radMountainPeakEvents
            // 
            this.radMountainPeakEvents.AutoSize = true;
            this.radMountainPeakEvents.Location = new System.Drawing.Point(6, 19);
            this.radMountainPeakEvents.Name = "radMountainPeakEvents";
            this.radMountainPeakEvents.Size = new System.Drawing.Size(58, 17);
            this.radMountainPeakEvents.TabIndex = 13;
            this.radMountainPeakEvents.Text = "Events";
            this.radMountainPeakEvents.UseVisualStyleBackColor = true;
            this.radMountainPeakEvents.CheckedChanged += new System.EventHandler(this.SearchMountainPeakList);
            // 
            // txtMountainPeakSearch
            // 
            this.txtMountainPeakSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMountainPeakSearch.Location = new System.Drawing.Point(81, 5);
            this.txtMountainPeakSearch.Name = "txtMountainPeakSearch";
            this.txtMountainPeakSearch.Size = new System.Drawing.Size(169, 20);
            this.txtMountainPeakSearch.TabIndex = 44;
            this.txtMountainPeakSearch.TextChanged += new System.EventHandler(this.SearchMountainPeakList);
            // 
            // tpMountainPeakEvents
            // 
            this.tpMountainPeakEvents.Location = new System.Drawing.Point(4, 22);
            this.tpMountainPeakEvents.Name = "tpMountainPeakEvents";
            this.tpMountainPeakEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tpMountainPeakEvents.Size = new System.Drawing.Size(253, 448);
            this.tpMountainPeakEvents.TabIndex = 1;
            this.tpMountainPeakEvents.Text = "Events";
            this.tpMountainPeakEvents.UseVisualStyleBackColor = true;
            // 
            // tpRivers
            // 
            this.tpRivers.Controls.Add(this.tabControl1);
            this.tpRivers.Location = new System.Drawing.Point(4, 40);
            this.tpRivers.Name = "tpRivers";
            this.tpRivers.Padding = new System.Windows.Forms.Padding(3);
            this.tpRivers.Size = new System.Drawing.Size(261, 474);
            this.tpRivers.TabIndex = 4;
            this.tpRivers.Text = "Rivers";
            this.tpRivers.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tpRiverEvents);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(255, 468);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblRiverResults);
            this.tabPage2.Controls.Add(this.listRiverSearch);
            this.tabPage2.Controls.Add(this.btnRiverSearch);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.txtRiverSearch);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(247, 442);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Search";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblRiverResults
            // 
            this.lblRiverResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRiverResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRiverResults.Location = new System.Drawing.Point(154, 258);
            this.lblRiverResults.Name = "lblRiverResults";
            this.lblRiverResults.Size = new System.Drawing.Size(87, 11);
            this.lblRiverResults.TabIndex = 56;
            this.lblRiverResults.Text = "0 / 0";
            this.lblRiverResults.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.hint.SetToolTip(this.lblRiverResults, "Results Shown");
            // 
            // listRiverSearch
            // 
            this.listRiverSearch.AllColumns.Add(this.olvColumn8);
            this.listRiverSearch.AlternateRowBackColor = System.Drawing.SystemColors.InactiveBorder;
            this.listRiverSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listRiverSearch.CellEditUseWholeCell = false;
            this.listRiverSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn8});
            this.listRiverSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.listRiverSearch.FullRowSelect = true;
            this.listRiverSearch.GridLines = true;
            this.listRiverSearch.HeaderWordWrap = true;
            this.listRiverSearch.HideSelection = false;
            this.listRiverSearch.Location = new System.Drawing.Point(3, 31);
            this.listRiverSearch.Name = "listRiverSearch";
            this.listRiverSearch.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.listRiverSearch.ShowCommandMenuOnRightClick = true;
            this.listRiverSearch.ShowImagesOnSubItems = true;
            this.listRiverSearch.ShowItemCountOnGroups = true;
            this.listRiverSearch.Size = new System.Drawing.Size(241, 224);
            this.listRiverSearch.TabIndex = 55;
            this.listRiverSearch.UseAlternatingBackColors = true;
            this.listRiverSearch.UseCompatibleStateImageBehavior = false;
            this.listRiverSearch.UseFiltering = true;
            this.listRiverSearch.UseHotItem = true;
            this.listRiverSearch.UseHyperlinks = true;
            this.listRiverSearch.View = System.Windows.Forms.View.Details;
            this.listRiverSearch.SelectedIndexChanged += new System.EventHandler(this.listRiverSearch_SelectedIndexChanged);
            // 
            // olvColumn8
            // 
            this.olvColumn8.AspectName = "Name";
            this.olvColumn8.IsEditable = false;
            this.olvColumn8.MinimumWidth = 50;
            this.olvColumn8.Text = "Name";
            this.olvColumn8.UseInitialLetterForGroup = true;
            this.olvColumn8.Width = 225;
            // 
            // btnRiverSearch
            // 
            this.btnRiverSearch.Location = new System.Drawing.Point(3, 3);
            this.btnRiverSearch.Name = "btnRiverSearch";
            this.btnRiverSearch.Size = new System.Drawing.Size(75, 23);
            this.btnRiverSearch.TabIndex = 46;
            this.btnRiverSearch.Text = "Search";
            this.btnRiverSearch.UseVisualStyleBackColor = true;
            this.btnRiverSearch.Click += new System.EventHandler(this.SearchRiverList);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Location = new System.Drawing.Point(3, 272);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(241, 164);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter / Sort";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.radRiverFiltered);
            this.groupBox4.Controls.Add(this.radRiverNone);
            this.groupBox4.Controls.Add(this.radRiverEvents);
            this.groupBox4.Location = new System.Drawing.Point(133, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(102, 126);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Sort By";
            // 
            // radRiverFiltered
            // 
            this.radRiverFiltered.AutoSize = true;
            this.radRiverFiltered.Location = new System.Drawing.Point(6, 42);
            this.radRiverFiltered.Name = "radRiverFiltered";
            this.radRiverFiltered.Size = new System.Drawing.Size(95, 17);
            this.radRiverFiltered.TabIndex = 16;
            this.radRiverFiltered.TabStop = true;
            this.radRiverFiltered.Text = "Filtered Events";
            this.radRiverFiltered.UseVisualStyleBackColor = true;
            this.radRiverFiltered.CheckedChanged += new System.EventHandler(this.SearchRiverList);
            // 
            // radRiverNone
            // 
            this.radRiverNone.AutoSize = true;
            this.radRiverNone.Checked = true;
            this.radRiverNone.Location = new System.Drawing.Point(6, 65);
            this.radRiverNone.Name = "radRiverNone";
            this.radRiverNone.Size = new System.Drawing.Size(51, 17);
            this.radRiverNone.TabIndex = 14;
            this.radRiverNone.TabStop = true;
            this.radRiverNone.Text = "None";
            this.radRiverNone.UseVisualStyleBackColor = true;
            this.radRiverNone.CheckedChanged += new System.EventHandler(this.SearchRiverList);
            // 
            // radRiverEvents
            // 
            this.radRiverEvents.AutoSize = true;
            this.radRiverEvents.Location = new System.Drawing.Point(6, 19);
            this.radRiverEvents.Name = "radRiverEvents";
            this.radRiverEvents.Size = new System.Drawing.Size(58, 17);
            this.radRiverEvents.TabIndex = 13;
            this.radRiverEvents.Text = "Events";
            this.radRiverEvents.UseVisualStyleBackColor = true;
            this.radRiverEvents.CheckedChanged += new System.EventHandler(this.SearchRiverList);
            // 
            // txtRiverSearch
            // 
            this.txtRiverSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRiverSearch.Location = new System.Drawing.Point(81, 5);
            this.txtRiverSearch.Name = "txtRiverSearch";
            this.txtRiverSearch.Size = new System.Drawing.Size(163, 20);
            this.txtRiverSearch.TabIndex = 44;
            this.txtRiverSearch.TextChanged += new System.EventHandler(this.SearchRiverList);
            // 
            // tpRiverEvents
            // 
            this.tpRiverEvents.Location = new System.Drawing.Point(4, 22);
            this.tpRiverEvents.Name = "tpRiverEvents";
            this.tpRiverEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tpRiverEvents.Size = new System.Drawing.Size(247, 442);
            this.tpRiverEvents.TabIndex = 1;
            this.tpRiverEvents.Text = "Events";
            this.tpRiverEvents.UseVisualStyleBackColor = true;
            // 
            // GeographyTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcRegionsSub);
            this.Name = "GeographyTab";
            this.tcRegionsSub.ResumeLayout(false);
            this.tpOverworld.ResumeLayout(false);
            this.tcRegions.ResumeLayout(false);
            this.tpRegionSearch.ResumeLayout(false);
            this.tpRegionSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listRegionSearch)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tpUnderground.ResumeLayout(false);
            this.tcURegions.ResumeLayout(false);
            this.tpURegionSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listURegionSearch)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tpLandmasses.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listLandmassesSearch)).EndInit();
            this.groupBox27.ResumeLayout(false);
            this.groupBox28.ResumeLayout(false);
            this.groupBox28.PerformLayout();
            this.tpMountainPeaks.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listMountainPeakSearch)).EndInit();
            this.groupBox29.ResumeLayout(false);
            this.groupBox30.ResumeLayout(false);
            this.groupBox30.PerformLayout();
            this.tpRivers.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listRiverSearch)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tcRegionsSub;
        private TabPage tpOverworld;
        private TabControl tcRegions;
        private TabPage tpRegionSearch;
        private GroupBox groupBox2;
        private GroupBox groupBox7;
        private RadioButton radRegionSortDeaths;
        private RadioButton radRegionSortBattles;
        private RadioButton radRegionSortFiltered;
        private RadioButton radRegionNone;
        private RadioButton radRegionSortEvents;
        private Label label27;
        private ComboBox cmbRegionType;
        private TextBox txtRegionSearch;
        private Button btnRegionSearch;
        private TabPage tpRegionEvents;
        private TabPage tpUnderground;
        private TabControl tcURegions;
        private TabPage tpURegionSearch;
        private Button btnURegionSearch;
        private GroupBox groupBox3;
        private GroupBox groupBox8;
        private RadioButton radURegionSortFiltered;
        private RadioButton radURegionNone;
        private RadioButton radURegionSortEvents;
        private Label label28;
        private ComboBox cmbURegionType;
        private TabPage tpURegionEvents;
        private TabPage tpLandmasses;
        private TabControl tabControl2;
        private TabPage tabPage1;
        private Button btnLandmassSearch;
        private GroupBox groupBox27;
        private GroupBox groupBox28;
        private RadioButton radLandmassFiltered;
        private RadioButton radLandmassNone;
        private RadioButton radLandmassEvents;
        private TextBox txtLandmassSearch;
        private TabPage tpLandmassEvents;
        private TabPage tpMountainPeaks;
        private TabControl tabControl3;
        private TabPage tabPage3;
        private Button btnMountainPeakSearch;
        private GroupBox groupBox29;
        private GroupBox groupBox30;
        private RadioButton radMountainPeakFiltered;
        private RadioButton radMountainPeakNone;
        private RadioButton radMountainPeakEvents;
        private TextBox txtMountainPeakSearch;
        private TabPage tpMountainPeakEvents;
        private Label lblShownResults;
        private OLVColumn olvName;
        private OLVColumn olvType;
        private ObjectListView listRegionSearch;
        private Label lblURegionResults;
        private OLVColumn olvColumn1;
        private OLVColumn olvColumn2;
        private ObjectListView listURegionSearch;
        private OLVColumn olvColumn3;
        private OLVColumn olvColumn4;
        private Label lblLandmassResults;
        private ObjectListView listLandmassesSearch;
        private OLVColumn olvColumn5;
        private Label lblMountainPeakResults;
        private OLVColumn olvColumn6;
        private OLVColumn olvColumn7;
        private ObjectListView listMountainPeakSearch;
        private RadioButton radRegionSortArea;
        private RadioButton radURegionSortArea;
        private TabPage tpRivers;
        private TabControl tabControl1;
        private TabPage tabPage2;
        private Label lblRiverResults;
        private ObjectListView listRiverSearch;
        private OLVColumn olvColumn8;
        private Button btnRiverSearch;
        private GroupBox groupBox1;
        private GroupBox groupBox4;
        private RadioButton radRiverFiltered;
        private RadioButton radRiverNone;
        private RadioButton radRiverEvents;
        private TextBox txtRiverSearch;
        private TabPage tpRiverEvents;
        private OLVColumn olvColumn9;
    }
}
