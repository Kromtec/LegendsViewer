using System.ComponentModel;
using System.Windows.Forms;
using BrightIdeasSoftware;
using LegendsViewer.Legends;

namespace LegendsViewer.Controls.Tabs
{
    partial class InfrastructureTab
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
            this.tcCollections = new System.Windows.Forms.TabControl();
            this.tpSites = new System.Windows.Forms.TabPage();
            this.tcSites = new System.Windows.Forms.TabControl();
            this.tpSiteSearch = new System.Windows.Forms.TabPage();
            this.listPanel = new System.Windows.Forms.Panel();
            this.lnkMaxResults = new System.Windows.Forms.LinkLabel();
            this.lblShownResults = new System.Windows.Forms.Label();
            this.btnSiteSearch = new System.Windows.Forms.Button();
            this.listSiteSearch = new BrightIdeasSoftware.ObjectListView();
            this.olvName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.txtSiteSearch = new System.Windows.Forms.TextBox();
            this.btnSiteListReset = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.radSiteBeastAttacks = new System.Windows.Forms.RadioButton();
            this.radSiteSortDeaths = new System.Windows.Forms.RadioButton();
            this.radSortConnections = new System.Windows.Forms.RadioButton();
            this.cmbSitePopulation = new System.Windows.Forms.ComboBox();
            this.radSiteSortPopulation = new System.Windows.Forms.RadioButton();
            this.radSiteSortWarfare = new System.Windows.Forms.RadioButton();
            this.radSiteSortFiltered = new System.Windows.Forms.RadioButton();
            this.radSiteOwners = new System.Windows.Forms.RadioButton();
            this.radSiteNone = new System.Windows.Forms.RadioButton();
            this.radSiteSortEvents = new System.Windows.Forms.RadioButton();
            this.label26 = new System.Windows.Forms.Label();
            this.cmbSiteType = new System.Windows.Forms.ComboBox();
            this.tpSiteEvents = new System.Windows.Forms.TabPage();
            this.tpStructures = new System.Windows.Forms.TabPage();
            this.tcStructures = new System.Windows.Forms.TabControl();
            this.tpStructureSearch = new System.Windows.Forms.TabPage();
            this.lblStructureResults = new System.Windows.Forms.Label();
            this.listStructureSearch = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btnStructuresSearch = new System.Windows.Forms.Button();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbStructureType = new System.Windows.Forms.ComboBox();
            this.groupBox26 = new System.Windows.Forms.GroupBox();
            this.radStructuresSortFiltered = new System.Windows.Forms.RadioButton();
            this.radStructuresSortNone = new System.Windows.Forms.RadioButton();
            this.radStructuresSortEvents = new System.Windows.Forms.RadioButton();
            this.txtStructuresSearch = new System.Windows.Forms.TextBox();
            this.tpStructureEvents = new System.Windows.Forms.TabPage();
            this.tpWorldConstructions = new System.Windows.Forms.TabPage();
            this.tcWorldConstructions = new System.Windows.Forms.TabControl();
            this.tpWorldConstructionSearch = new System.Windows.Forms.TabPage();
            this.lblWorldConstructionResult = new System.Windows.Forms.Label();
            this.listWorldConstructionsSearch = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn5 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn6 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btnWorldConstructionsSearch = new System.Windows.Forms.Button();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbConstructionType = new System.Windows.Forms.ComboBox();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.radWorldConstructionsSortFiltered = new System.Windows.Forms.RadioButton();
            this.radWorldConstructionsSortNone = new System.Windows.Forms.RadioButton();
            this.radWorldConstructionsSortEvents = new System.Windows.Forms.RadioButton();
            this.txtWorldConstructionsSearch = new System.Windows.Forms.TextBox();
            this.tpWorldConstructionEvents = new System.Windows.Forms.TabPage();
            this.tcCollections.SuspendLayout();
            this.tpSites.SuspendLayout();
            this.tcSites.SuspendLayout();
            this.tpSiteSearch.SuspendLayout();
            this.listPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listSiteSearch)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tpStructures.SuspendLayout();
            this.tcStructures.SuspendLayout();
            this.tpStructureSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listStructureSearch)).BeginInit();
            this.groupBox25.SuspendLayout();
            this.groupBox26.SuspendLayout();
            this.tpWorldConstructions.SuspendLayout();
            this.tcWorldConstructions.SuspendLayout();
            this.tpWorldConstructionSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listWorldConstructionsSearch)).BeginInit();
            this.groupBox23.SuspendLayout();
            this.groupBox24.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcCollections
            // 
            this.tcCollections.Controls.Add(this.tpSites);
            this.tcCollections.Controls.Add(this.tpStructures);
            this.tcCollections.Controls.Add(this.tpWorldConstructions);
            this.tcCollections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcCollections.Location = new System.Drawing.Point(0, 0);
            this.tcCollections.Multiline = true;
            this.tcCollections.Name = "tcCollections";
            this.tcCollections.SelectedIndex = 0;
            this.tcCollections.Size = new System.Drawing.Size(269, 518);
            this.tcCollections.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tcCollections.TabIndex = 2;
            // 
            // tpSites
            // 
            this.tpSites.Controls.Add(this.tcSites);
            this.tpSites.Location = new System.Drawing.Point(4, 22);
            this.tpSites.Name = "tpSites";
            this.tpSites.Size = new System.Drawing.Size(261, 492);
            this.tpSites.TabIndex = 0;
            this.tpSites.Text = "Sites";
            this.tpSites.UseVisualStyleBackColor = true;
            // 
            // tcSites
            // 
            this.tcSites.Controls.Add(this.tpSiteSearch);
            this.tcSites.Controls.Add(this.tpSiteEvents);
            this.tcSites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcSites.Location = new System.Drawing.Point(0, 0);
            this.tcSites.Name = "tcSites";
            this.tcSites.SelectedIndex = 0;
            this.tcSites.Size = new System.Drawing.Size(261, 492);
            this.tcSites.TabIndex = 2;
            // 
            // tpSiteSearch
            // 
            this.tpSiteSearch.Controls.Add(this.listPanel);
            this.tpSiteSearch.Location = new System.Drawing.Point(4, 22);
            this.tpSiteSearch.Name = "tpSiteSearch";
            this.tpSiteSearch.Size = new System.Drawing.Size(253, 466);
            this.tpSiteSearch.TabIndex = 0;
            this.tpSiteSearch.Text = "Search";
            this.tpSiteSearch.UseVisualStyleBackColor = true;
            // 
            // listPanel
            // 
            this.listPanel.Controls.Add(this.groupBox1);
            this.listPanel.Controls.Add(this.lnkMaxResults);
            this.listPanel.Controls.Add(this.lblShownResults);
            this.listPanel.Controls.Add(this.btnSiteSearch);
            this.listPanel.Controls.Add(this.listSiteSearch);
            this.listPanel.Controls.Add(this.txtSiteSearch);
            this.listPanel.Controls.Add(this.btnSiteListReset);
            this.listPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPanel.Location = new System.Drawing.Point(0, 0);
            this.listPanel.Name = "listPanel";
            this.listPanel.Size = new System.Drawing.Size(253, 466);
            this.listPanel.TabIndex = 45;
            // 
            // lnkMaxResults
            // 
            this.lnkMaxResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkMaxResults.AutoSize = true;
            this.lnkMaxResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkMaxResults.Location = new System.Drawing.Point(234, 241);
            this.lnkMaxResults.Name = "lnkMaxResults";
            this.lnkMaxResults.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lnkMaxResults.Size = new System.Drawing.Size(17, 9);
            this.lnkMaxResults.TabIndex = 53;
            this.lnkMaxResults.TabStop = true;
            this.lnkMaxResults.Text = "500";
            this.lnkMaxResults.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnkMaxResults.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMaxResults_LinkClicked);
            // 
            // lblShownResults
            // 
            this.lblShownResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblShownResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShownResults.Location = new System.Drawing.Point(131, 241);
            this.lblShownResults.Name = "lblShownResults";
            this.lblShownResults.Size = new System.Drawing.Size(95, 10);
            this.lblShownResults.TabIndex = 52;
            this.lblShownResults.Text = "0 / 0";
            this.lblShownResults.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnSiteSearch
            // 
            this.btnSiteSearch.Location = new System.Drawing.Point(3, 3);
            this.btnSiteSearch.Name = "btnSiteSearch";
            this.btnSiteSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSiteSearch.TabIndex = 31;
            this.btnSiteSearch.Text = "Search";
            this.btnSiteSearch.UseVisualStyleBackColor = true;
            this.btnSiteSearch.Click += new System.EventHandler(this.SearchSiteList);
            // 
            // listSiteSearch
            // 
            this.listSiteSearch.AllColumns.Add(this.olvName);
            this.listSiteSearch.AllColumns.Add(this.olvType);
            this.listSiteSearch.AlternateRowBackColor = System.Drawing.SystemColors.InactiveBorder;
            this.listSiteSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listSiteSearch.CellEditUseWholeCell = false;
            this.listSiteSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvName,
            this.olvType});
            this.listSiteSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.listSiteSearch.FullRowSelect = true;
            this.listSiteSearch.GridLines = true;
            this.listSiteSearch.HeaderWordWrap = true;
            this.listSiteSearch.HideSelection = false;
            this.listSiteSearch.Location = new System.Drawing.Point(3, 30);
            this.listSiteSearch.Name = "listSiteSearch";
            this.listSiteSearch.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.listSiteSearch.ShowCommandMenuOnRightClick = true;
            this.listSiteSearch.ShowImagesOnSubItems = true;
            this.listSiteSearch.ShowItemCountOnGroups = true;
            this.listSiteSearch.Size = new System.Drawing.Size(247, 197);
            this.listSiteSearch.TabIndex = 43;
            this.listSiteSearch.UseAlternatingBackColors = true;
            this.listSiteSearch.UseCompatibleStateImageBehavior = false;
            this.listSiteSearch.UseFiltering = true;
            this.listSiteSearch.UseHotItem = true;
            this.listSiteSearch.UseHyperlinks = true;
            this.listSiteSearch.View = System.Windows.Forms.View.Details;
            this.listSiteSearch.SelectedIndexChanged += new System.EventHandler(this.listSiteSearch_SelectedIndexChanged);
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
            this.olvType.Width = 85;
            // 
            // txtSiteSearch
            // 
            this.txtSiteSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSiteSearch.Location = new System.Drawing.Point(81, 5);
            this.txtSiteSearch.Name = "txtSiteSearch";
            this.txtSiteSearch.Size = new System.Drawing.Size(169, 20);
            this.txtSiteSearch.TabIndex = 33;
            this.txtSiteSearch.TextChanged += new System.EventHandler(this.SearchSiteList);
            // 
            // btnSiteListReset
            // 
            this.btnSiteListReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSiteListReset.Location = new System.Drawing.Point(2, 233);
            this.btnSiteListReset.Name = "btnSiteListReset";
            this.btnSiteListReset.Size = new System.Drawing.Size(50, 20);
            this.btnSiteListReset.TabIndex = 43;
            this.btnSiteListReset.Text = "Reset";
            this.btnSiteListReset.UseVisualStyleBackColor = true;
            this.btnSiteListReset.Click += new System.EventHandler(this.ResetSiteBaseList);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.cmbSiteType);
            this.groupBox1.Location = new System.Drawing.Point(0, 264);
            this.groupBox1.MinimumSize = new System.Drawing.Size(254, 202);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(254, 202);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter / Sort";
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.radSiteBeastAttacks);
            this.groupBox6.Controls.Add(this.radSiteSortDeaths);
            this.groupBox6.Controls.Add(this.radSortConnections);
            this.groupBox6.Controls.Add(this.cmbSitePopulation);
            this.groupBox6.Controls.Add(this.radSiteSortPopulation);
            this.groupBox6.Controls.Add(this.radSiteSortWarfare);
            this.groupBox6.Controls.Add(this.radSiteSortFiltered);
            this.groupBox6.Controls.Add(this.radSiteOwners);
            this.groupBox6.Controls.Add(this.radSiteNone);
            this.groupBox6.Controls.Add(this.radSiteSortEvents);
            this.groupBox6.Location = new System.Drawing.Point(6, 59);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(240, 137);
            this.groupBox6.TabIndex = 14;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Sort By";
            // 
            // radSiteBeastAttacks
            // 
            this.radSiteBeastAttacks.AutoSize = true;
            this.radSiteBeastAttacks.Location = new System.Drawing.Point(113, 87);
            this.radSiteBeastAttacks.Name = "radSiteBeastAttacks";
            this.radSiteBeastAttacks.Size = new System.Drawing.Size(91, 17);
            this.radSiteBeastAttacks.TabIndex = 23;
            this.radSiteBeastAttacks.TabStop = true;
            this.radSiteBeastAttacks.Text = "Beast Attacks";
            this.radSiteBeastAttacks.UseVisualStyleBackColor = true;
            this.radSiteBeastAttacks.CheckedChanged += new System.EventHandler(this.SearchSiteList);
            // 
            // radSiteSortDeaths
            // 
            this.radSiteSortDeaths.AutoSize = true;
            this.radSiteSortDeaths.Location = new System.Drawing.Point(6, 110);
            this.radSiteSortDeaths.Name = "radSiteSortDeaths";
            this.radSiteSortDeaths.Size = new System.Drawing.Size(59, 17);
            this.radSiteSortDeaths.TabIndex = 22;
            this.radSiteSortDeaths.TabStop = true;
            this.radSiteSortDeaths.Text = "Deaths";
            this.radSiteSortDeaths.UseVisualStyleBackColor = true;
            this.radSiteSortDeaths.CheckedChanged += new System.EventHandler(this.SearchSiteList);
            // 
            // radSortConnections
            // 
            this.radSortConnections.AutoSize = true;
            this.radSortConnections.Location = new System.Drawing.Point(113, 64);
            this.radSortConnections.Name = "radSortConnections";
            this.radSortConnections.Size = new System.Drawing.Size(84, 17);
            this.radSortConnections.TabIndex = 21;
            this.radSortConnections.TabStop = true;
            this.radSortConnections.Text = "Connections";
            this.radSortConnections.UseVisualStyleBackColor = true;
            this.radSortConnections.CheckedChanged += new System.EventHandler(this.SearchSiteList);
            // 
            // cmbSitePopulation
            // 
            this.cmbSitePopulation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSitePopulation.FormattingEnabled = true;
            this.cmbSitePopulation.Location = new System.Drawing.Point(113, 41);
            this.cmbSitePopulation.Name = "cmbSitePopulation";
            this.cmbSitePopulation.Size = new System.Drawing.Size(121, 21);
            this.cmbSitePopulation.TabIndex = 20;
            this.cmbSitePopulation.SelectedIndexChanged += new System.EventHandler(this.SearchSiteList);
            // 
            // radSiteSortPopulation
            // 
            this.radSiteSortPopulation.AutoSize = true;
            this.radSiteSortPopulation.Location = new System.Drawing.Point(113, 19);
            this.radSiteSortPopulation.Name = "radSiteSortPopulation";
            this.radSiteSortPopulation.Size = new System.Drawing.Size(75, 17);
            this.radSiteSortPopulation.TabIndex = 19;
            this.radSiteSortPopulation.TabStop = true;
            this.radSiteSortPopulation.Text = "Population";
            this.radSiteSortPopulation.UseVisualStyleBackColor = true;
            this.radSiteSortPopulation.CheckedChanged += new System.EventHandler(this.SearchSiteList);
            // 
            // radSiteSortWarfare
            // 
            this.radSiteSortWarfare.AutoSize = true;
            this.radSiteSortWarfare.Location = new System.Drawing.Point(6, 87);
            this.radSiteSortWarfare.Name = "radSiteSortWarfare";
            this.radSiteSortWarfare.Size = new System.Drawing.Size(57, 17);
            this.radSiteSortWarfare.TabIndex = 18;
            this.radSiteSortWarfare.TabStop = true;
            this.radSiteSortWarfare.Text = "Battles";
            this.radSiteSortWarfare.UseVisualStyleBackColor = true;
            this.radSiteSortWarfare.CheckedChanged += new System.EventHandler(this.SearchSiteList);
            // 
            // radSiteSortFiltered
            // 
            this.radSiteSortFiltered.AutoSize = true;
            this.radSiteSortFiltered.Location = new System.Drawing.Point(6, 42);
            this.radSiteSortFiltered.Name = "radSiteSortFiltered";
            this.radSiteSortFiltered.Size = new System.Drawing.Size(95, 17);
            this.radSiteSortFiltered.TabIndex = 17;
            this.radSiteSortFiltered.TabStop = true;
            this.radSiteSortFiltered.Text = "Filtered Events";
            this.radSiteSortFiltered.UseVisualStyleBackColor = true;
            this.radSiteSortFiltered.CheckedChanged += new System.EventHandler(this.SearchSiteList);
            // 
            // radSiteOwners
            // 
            this.radSiteOwners.AutoSize = true;
            this.radSiteOwners.Location = new System.Drawing.Point(6, 65);
            this.radSiteOwners.Name = "radSiteOwners";
            this.radSiteOwners.Size = new System.Drawing.Size(91, 17);
            this.radSiteOwners.TabIndex = 16;
            this.radSiteOwners.TabStop = true;
            this.radSiteOwners.Text = "Owner History";
            this.radSiteOwners.UseVisualStyleBackColor = true;
            this.radSiteOwners.CheckedChanged += new System.EventHandler(this.SearchSiteList);
            // 
            // radSiteNone
            // 
            this.radSiteNone.AutoSize = true;
            this.radSiteNone.Checked = true;
            this.radSiteNone.Location = new System.Drawing.Point(113, 110);
            this.radSiteNone.Name = "radSiteNone";
            this.radSiteNone.Size = new System.Drawing.Size(51, 17);
            this.radSiteNone.TabIndex = 15;
            this.radSiteNone.TabStop = true;
            this.radSiteNone.Text = "None";
            this.radSiteNone.UseVisualStyleBackColor = true;
            this.radSiteNone.CheckedChanged += new System.EventHandler(this.SearchSiteList);
            // 
            // radSiteSortEvents
            // 
            this.radSiteSortEvents.AutoSize = true;
            this.radSiteSortEvents.Location = new System.Drawing.Point(6, 19);
            this.radSiteSortEvents.Name = "radSiteSortEvents";
            this.radSiteSortEvents.Size = new System.Drawing.Size(58, 17);
            this.radSiteSortEvents.TabIndex = 14;
            this.radSiteSortEvents.Text = "Events";
            this.radSiteSortEvents.UseVisualStyleBackColor = true;
            this.radSiteSortEvents.CheckedChanged += new System.EventHandler(this.SearchSiteList);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(6, 16);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(31, 13);
            this.label26.TabIndex = 12;
            this.label26.Text = "Type";
            // 
            // cmbSiteType
            // 
            this.cmbSiteType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSiteType.FormattingEnabled = true;
            this.cmbSiteType.Location = new System.Drawing.Point(6, 32);
            this.cmbSiteType.Name = "cmbSiteType";
            this.cmbSiteType.Size = new System.Drawing.Size(121, 21);
            this.cmbSiteType.TabIndex = 0;
            this.cmbSiteType.SelectedIndexChanged += new System.EventHandler(this.SearchSiteList);
            // 
            // tpSiteEvents
            // 
            this.tpSiteEvents.AutoScroll = true;
            this.tpSiteEvents.Location = new System.Drawing.Point(4, 22);
            this.tpSiteEvents.Name = "tpSiteEvents";
            this.tpSiteEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tpSiteEvents.Size = new System.Drawing.Size(253, 466);
            this.tpSiteEvents.TabIndex = 1;
            this.tpSiteEvents.Text = "Events";
            this.tpSiteEvents.UseVisualStyleBackColor = true;
            // 
            // tpStructures
            // 
            this.tpStructures.Controls.Add(this.tcStructures);
            this.tpStructures.Location = new System.Drawing.Point(4, 22);
            this.tpStructures.Name = "tpStructures";
            this.tpStructures.Size = new System.Drawing.Size(261, 492);
            this.tpStructures.TabIndex = 4;
            this.tpStructures.Text = "Structures";
            this.tpStructures.UseVisualStyleBackColor = true;
            // 
            // tcStructures
            // 
            this.tcStructures.Controls.Add(this.tpStructureSearch);
            this.tcStructures.Controls.Add(this.tpStructureEvents);
            this.tcStructures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcStructures.Location = new System.Drawing.Point(0, 0);
            this.tcStructures.Name = "tcStructures";
            this.tcStructures.SelectedIndex = 0;
            this.tcStructures.Size = new System.Drawing.Size(261, 492);
            this.tcStructures.TabIndex = 4;
            // 
            // tpStructureSearch
            // 
            this.tpStructureSearch.Controls.Add(this.lblStructureResults);
            this.tpStructureSearch.Controls.Add(this.listStructureSearch);
            this.tpStructureSearch.Controls.Add(this.btnStructuresSearch);
            this.tpStructureSearch.Controls.Add(this.groupBox25);
            this.tpStructureSearch.Controls.Add(this.txtStructuresSearch);
            this.tpStructureSearch.Location = new System.Drawing.Point(4, 22);
            this.tpStructureSearch.Name = "tpStructureSearch";
            this.tpStructureSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tpStructureSearch.Size = new System.Drawing.Size(253, 466);
            this.tpStructureSearch.TabIndex = 0;
            this.tpStructureSearch.Text = "Search";
            this.tpStructureSearch.UseVisualStyleBackColor = true;
            // 
            // lblStructureResults
            // 
            this.lblStructureResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStructureResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStructureResults.Location = new System.Drawing.Point(155, 287);
            this.lblStructureResults.Name = "lblStructureResults";
            this.lblStructureResults.Size = new System.Drawing.Size(95, 10);
            this.lblStructureResults.TabIndex = 57;
            this.lblStructureResults.Text = "0 / 0";
            this.lblStructureResults.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // listStructureSearch
            // 
            this.listStructureSearch.AllColumns.Add(this.olvColumn3);
            this.listStructureSearch.AllColumns.Add(this.olvColumn4);
            this.listStructureSearch.AlternateRowBackColor = System.Drawing.SystemColors.InactiveBorder;
            this.listStructureSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listStructureSearch.CellEditUseWholeCell = false;
            this.listStructureSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn3,
            this.olvColumn4});
            this.listStructureSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.listStructureSearch.FullRowSelect = true;
            this.listStructureSearch.GridLines = true;
            this.listStructureSearch.HeaderWordWrap = true;
            this.listStructureSearch.HideSelection = false;
            this.listStructureSearch.Location = new System.Drawing.Point(3, 31);
            this.listStructureSearch.Name = "listStructureSearch";
            this.listStructureSearch.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.listStructureSearch.ShowCommandMenuOnRightClick = true;
            this.listStructureSearch.ShowImagesOnSubItems = true;
            this.listStructureSearch.ShowItemCountOnGroups = true;
            this.listStructureSearch.Size = new System.Drawing.Size(247, 251);
            this.listStructureSearch.TabIndex = 56;
            this.listStructureSearch.UseAlternatingBackColors = true;
            this.listStructureSearch.UseCompatibleStateImageBehavior = false;
            this.listStructureSearch.UseFiltering = true;
            this.listStructureSearch.UseHotItem = true;
            this.listStructureSearch.UseHyperlinks = true;
            this.listStructureSearch.View = System.Windows.Forms.View.Details;
            this.listStructureSearch.SelectedIndexChanged += new System.EventHandler(this.listStructuresSearch_SelectedIndexChanged);
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "Name";
            this.olvColumn3.IsEditable = false;
            this.olvColumn3.MinimumWidth = 50;
            this.olvColumn3.Text = "Name";
            this.olvColumn3.UseInitialLetterForGroup = true;
            this.olvColumn3.Width = 155;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "TypeAsString";
            this.olvColumn4.IsEditable = false;
            this.olvColumn4.Text = "Type";
            this.olvColumn4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumn4.Width = 70;
            // 
            // btnStructuresSearch
            // 
            this.btnStructuresSearch.Location = new System.Drawing.Point(3, 3);
            this.btnStructuresSearch.Name = "btnStructuresSearch";
            this.btnStructuresSearch.Size = new System.Drawing.Size(75, 23);
            this.btnStructuresSearch.TabIndex = 46;
            this.btnStructuresSearch.Text = "Search";
            this.btnStructuresSearch.UseVisualStyleBackColor = true;
            this.btnStructuresSearch.Click += new System.EventHandler(this.SearchStructureList);
            // 
            // groupBox25
            // 
            this.groupBox25.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox25.Controls.Add(this.label6);
            this.groupBox25.Controls.Add(this.cmbStructureType);
            this.groupBox25.Controls.Add(this.groupBox26);
            this.groupBox25.Location = new System.Drawing.Point(3, 296);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Size = new System.Drawing.Size(247, 164);
            this.groupBox25.TabIndex = 45;
            this.groupBox25.TabStop = false;
            this.groupBox25.Text = "Filter / Sort";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Type";
            // 
            // cmbStructureType
            // 
            this.cmbStructureType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStructureType.FormattingEnabled = true;
            this.cmbStructureType.Location = new System.Drawing.Point(6, 35);
            this.cmbStructureType.Name = "cmbStructureType";
            this.cmbStructureType.Size = new System.Drawing.Size(121, 21);
            this.cmbStructureType.TabIndex = 16;
            this.cmbStructureType.SelectedIndexChanged += new System.EventHandler(this.SearchStructureList);
            // 
            // groupBox26
            // 
            this.groupBox26.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox26.Controls.Add(this.radStructuresSortFiltered);
            this.groupBox26.Controls.Add(this.radStructuresSortNone);
            this.groupBox26.Controls.Add(this.radStructuresSortEvents);
            this.groupBox26.Location = new System.Drawing.Point(133, 19);
            this.groupBox26.Name = "groupBox26";
            this.groupBox26.Size = new System.Drawing.Size(108, 126);
            this.groupBox26.TabIndex = 15;
            this.groupBox26.TabStop = false;
            this.groupBox26.Text = "Sort By";
            // 
            // radStructuresSortFiltered
            // 
            this.radStructuresSortFiltered.AutoSize = true;
            this.radStructuresSortFiltered.Location = new System.Drawing.Point(6, 42);
            this.radStructuresSortFiltered.Name = "radStructuresSortFiltered";
            this.radStructuresSortFiltered.Size = new System.Drawing.Size(95, 17);
            this.radStructuresSortFiltered.TabIndex = 16;
            this.radStructuresSortFiltered.TabStop = true;
            this.radStructuresSortFiltered.Text = "Filtered Events";
            this.radStructuresSortFiltered.UseVisualStyleBackColor = true;
            this.radStructuresSortFiltered.CheckedChanged += new System.EventHandler(this.SearchStructureList);
            // 
            // radStructuresSortNone
            // 
            this.radStructuresSortNone.AutoSize = true;
            this.radStructuresSortNone.Checked = true;
            this.radStructuresSortNone.Location = new System.Drawing.Point(6, 65);
            this.radStructuresSortNone.Name = "radStructuresSortNone";
            this.radStructuresSortNone.Size = new System.Drawing.Size(51, 17);
            this.radStructuresSortNone.TabIndex = 14;
            this.radStructuresSortNone.TabStop = true;
            this.radStructuresSortNone.Text = "None";
            this.radStructuresSortNone.UseVisualStyleBackColor = true;
            this.radStructuresSortNone.CheckedChanged += new System.EventHandler(this.SearchStructureList);
            // 
            // radStructuresSortEvents
            // 
            this.radStructuresSortEvents.AutoSize = true;
            this.radStructuresSortEvents.Location = new System.Drawing.Point(6, 19);
            this.radStructuresSortEvents.Name = "radStructuresSortEvents";
            this.radStructuresSortEvents.Size = new System.Drawing.Size(58, 17);
            this.radStructuresSortEvents.TabIndex = 13;
            this.radStructuresSortEvents.Text = "Events";
            this.radStructuresSortEvents.UseVisualStyleBackColor = true;
            this.radStructuresSortEvents.CheckedChanged += new System.EventHandler(this.SearchStructureList);
            // 
            // txtStructuresSearch
            // 
            this.txtStructuresSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStructuresSearch.Location = new System.Drawing.Point(81, 5);
            this.txtStructuresSearch.Name = "txtStructuresSearch";
            this.txtStructuresSearch.Size = new System.Drawing.Size(169, 20);
            this.txtStructuresSearch.TabIndex = 44;
            this.txtStructuresSearch.TextChanged += new System.EventHandler(this.SearchStructureList);
            // 
            // tpStructureEvents
            // 
            this.tpStructureEvents.Location = new System.Drawing.Point(4, 22);
            this.tpStructureEvents.Name = "tpStructureEvents";
            this.tpStructureEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tpStructureEvents.Size = new System.Drawing.Size(253, 466);
            this.tpStructureEvents.TabIndex = 1;
            this.tpStructureEvents.Text = "Events";
            this.tpStructureEvents.UseVisualStyleBackColor = true;
            // 
            // tpWorldConstructions
            // 
            this.tpWorldConstructions.Controls.Add(this.tcWorldConstructions);
            this.tpWorldConstructions.Location = new System.Drawing.Point(4, 22);
            this.tpWorldConstructions.Name = "tpWorldConstructions";
            this.tpWorldConstructions.Size = new System.Drawing.Size(261, 492);
            this.tpWorldConstructions.TabIndex = 3;
            this.tpWorldConstructions.Text = "Constructions";
            this.tpWorldConstructions.UseVisualStyleBackColor = true;
            // 
            // tcWorldConstructions
            // 
            this.tcWorldConstructions.Controls.Add(this.tpWorldConstructionSearch);
            this.tcWorldConstructions.Controls.Add(this.tpWorldConstructionEvents);
            this.tcWorldConstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcWorldConstructions.Location = new System.Drawing.Point(0, 0);
            this.tcWorldConstructions.Name = "tcWorldConstructions";
            this.tcWorldConstructions.SelectedIndex = 0;
            this.tcWorldConstructions.Size = new System.Drawing.Size(261, 492);
            this.tcWorldConstructions.TabIndex = 4;
            // 
            // tpWorldConstructionSearch
            // 
            this.tpWorldConstructionSearch.Controls.Add(this.lblWorldConstructionResult);
            this.tpWorldConstructionSearch.Controls.Add(this.listWorldConstructionsSearch);
            this.tpWorldConstructionSearch.Controls.Add(this.btnWorldConstructionsSearch);
            this.tpWorldConstructionSearch.Controls.Add(this.groupBox23);
            this.tpWorldConstructionSearch.Controls.Add(this.txtWorldConstructionsSearch);
            this.tpWorldConstructionSearch.Location = new System.Drawing.Point(4, 22);
            this.tpWorldConstructionSearch.Name = "tpWorldConstructionSearch";
            this.tpWorldConstructionSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tpWorldConstructionSearch.Size = new System.Drawing.Size(253, 466);
            this.tpWorldConstructionSearch.TabIndex = 0;
            this.tpWorldConstructionSearch.Text = "Search";
            this.tpWorldConstructionSearch.UseVisualStyleBackColor = true;
            // 
            // lblWorldConstructionResult
            // 
            this.lblWorldConstructionResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWorldConstructionResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorldConstructionResult.Location = new System.Drawing.Point(155, 287);
            this.lblWorldConstructionResult.Name = "lblWorldConstructionResult";
            this.lblWorldConstructionResult.Size = new System.Drawing.Size(95, 10);
            this.lblWorldConstructionResult.TabIndex = 59;
            this.lblWorldConstructionResult.Text = "0 / 0";
            this.lblWorldConstructionResult.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // listWorldConstructionsSearch
            // 
            this.listWorldConstructionsSearch.AllColumns.Add(this.olvColumn5);
            this.listWorldConstructionsSearch.AllColumns.Add(this.olvColumn6);
            this.listWorldConstructionsSearch.AlternateRowBackColor = System.Drawing.SystemColors.InactiveBorder;
            this.listWorldConstructionsSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listWorldConstructionsSearch.CellEditUseWholeCell = false;
            this.listWorldConstructionsSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn5,
            this.olvColumn6});
            this.listWorldConstructionsSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.listWorldConstructionsSearch.FullRowSelect = true;
            this.listWorldConstructionsSearch.GridLines = true;
            this.listWorldConstructionsSearch.HeaderWordWrap = true;
            this.listWorldConstructionsSearch.HideSelection = false;
            this.listWorldConstructionsSearch.Location = new System.Drawing.Point(3, 31);
            this.listWorldConstructionsSearch.Name = "listWorldConstructionsSearch";
            this.listWorldConstructionsSearch.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.listWorldConstructionsSearch.ShowCommandMenuOnRightClick = true;
            this.listWorldConstructionsSearch.ShowImagesOnSubItems = true;
            this.listWorldConstructionsSearch.ShowItemCountOnGroups = true;
            this.listWorldConstructionsSearch.Size = new System.Drawing.Size(247, 251);
            this.listWorldConstructionsSearch.TabIndex = 58;
            this.listWorldConstructionsSearch.UseAlternatingBackColors = true;
            this.listWorldConstructionsSearch.UseCompatibleStateImageBehavior = false;
            this.listWorldConstructionsSearch.UseFiltering = true;
            this.listWorldConstructionsSearch.UseHotItem = true;
            this.listWorldConstructionsSearch.UseHyperlinks = true;
            this.listWorldConstructionsSearch.View = System.Windows.Forms.View.Details;
            this.listWorldConstructionsSearch.SelectionChanged += new System.EventHandler(this.listWorldConstructionsSearch_SelectedIndexChanged);
            // 
            // olvColumn5
            // 
            this.olvColumn5.AspectName = "Name";
            this.olvColumn5.IsEditable = false;
            this.olvColumn5.MinimumWidth = 50;
            this.olvColumn5.Text = "Name";
            this.olvColumn5.UseInitialLetterForGroup = true;
            this.olvColumn5.Width = 155;
            // 
            // olvColumn6
            // 
            this.olvColumn6.AspectName = "TypeAsString";
            this.olvColumn6.IsEditable = false;
            this.olvColumn6.Text = "Type";
            this.olvColumn6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumn6.Width = 70;
            // 
            // btnWorldConstructionsSearch
            // 
            this.btnWorldConstructionsSearch.Location = new System.Drawing.Point(3, 3);
            this.btnWorldConstructionsSearch.Name = "btnWorldConstructionsSearch";
            this.btnWorldConstructionsSearch.Size = new System.Drawing.Size(75, 23);
            this.btnWorldConstructionsSearch.TabIndex = 46;
            this.btnWorldConstructionsSearch.Text = "Search";
            this.btnWorldConstructionsSearch.UseVisualStyleBackColor = true;
            this.btnWorldConstructionsSearch.Click += new System.EventHandler(this.SearchWorldConstructionList);
            // 
            // groupBox23
            // 
            this.groupBox23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox23.Controls.Add(this.label7);
            this.groupBox23.Controls.Add(this.cmbConstructionType);
            this.groupBox23.Controls.Add(this.groupBox24);
            this.groupBox23.Location = new System.Drawing.Point(3, 296);
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.Size = new System.Drawing.Size(247, 164);
            this.groupBox23.TabIndex = 45;
            this.groupBox23.TabStop = false;
            this.groupBox23.Text = "Filter / Sort";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Type";
            // 
            // cmbConstructionType
            // 
            this.cmbConstructionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConstructionType.FormattingEnabled = true;
            this.cmbConstructionType.Location = new System.Drawing.Point(6, 35);
            this.cmbConstructionType.Name = "cmbConstructionType";
            this.cmbConstructionType.Size = new System.Drawing.Size(121, 21);
            this.cmbConstructionType.TabIndex = 18;
            this.cmbConstructionType.SelectedIndexChanged += new System.EventHandler(this.SearchWorldConstructionList);
            // 
            // groupBox24
            // 
            this.groupBox24.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox24.Controls.Add(this.radWorldConstructionsSortFiltered);
            this.groupBox24.Controls.Add(this.radWorldConstructionsSortNone);
            this.groupBox24.Controls.Add(this.radWorldConstructionsSortEvents);
            this.groupBox24.Location = new System.Drawing.Point(133, 19);
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.Size = new System.Drawing.Size(108, 126);
            this.groupBox24.TabIndex = 15;
            this.groupBox24.TabStop = false;
            this.groupBox24.Text = "Sort By";
            // 
            // radWorldConstructionsSortFiltered
            // 
            this.radWorldConstructionsSortFiltered.AutoSize = true;
            this.radWorldConstructionsSortFiltered.Location = new System.Drawing.Point(6, 42);
            this.radWorldConstructionsSortFiltered.Name = "radWorldConstructionsSortFiltered";
            this.radWorldConstructionsSortFiltered.Size = new System.Drawing.Size(95, 17);
            this.radWorldConstructionsSortFiltered.TabIndex = 16;
            this.radWorldConstructionsSortFiltered.TabStop = true;
            this.radWorldConstructionsSortFiltered.Text = "Filtered Events";
            this.radWorldConstructionsSortFiltered.UseVisualStyleBackColor = true;
            this.radWorldConstructionsSortFiltered.CheckedChanged += new System.EventHandler(this.SearchWorldConstructionList);
            // 
            // radWorldConstructionsSortNone
            // 
            this.radWorldConstructionsSortNone.AutoSize = true;
            this.radWorldConstructionsSortNone.Checked = true;
            this.radWorldConstructionsSortNone.Location = new System.Drawing.Point(6, 65);
            this.radWorldConstructionsSortNone.Name = "radWorldConstructionsSortNone";
            this.radWorldConstructionsSortNone.Size = new System.Drawing.Size(51, 17);
            this.radWorldConstructionsSortNone.TabIndex = 14;
            this.radWorldConstructionsSortNone.TabStop = true;
            this.radWorldConstructionsSortNone.Text = "None";
            this.radWorldConstructionsSortNone.UseVisualStyleBackColor = true;
            this.radWorldConstructionsSortNone.CheckedChanged += new System.EventHandler(this.SearchWorldConstructionList);
            // 
            // radWorldConstructionsSortEvents
            // 
            this.radWorldConstructionsSortEvents.AutoSize = true;
            this.radWorldConstructionsSortEvents.Location = new System.Drawing.Point(6, 19);
            this.radWorldConstructionsSortEvents.Name = "radWorldConstructionsSortEvents";
            this.radWorldConstructionsSortEvents.Size = new System.Drawing.Size(58, 17);
            this.radWorldConstructionsSortEvents.TabIndex = 13;
            this.radWorldConstructionsSortEvents.Text = "Events";
            this.radWorldConstructionsSortEvents.UseVisualStyleBackColor = true;
            this.radWorldConstructionsSortEvents.CheckedChanged += new System.EventHandler(this.SearchWorldConstructionList);
            // 
            // txtWorldConstructionsSearch
            // 
            this.txtWorldConstructionsSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWorldConstructionsSearch.Location = new System.Drawing.Point(81, 5);
            this.txtWorldConstructionsSearch.Name = "txtWorldConstructionsSearch";
            this.txtWorldConstructionsSearch.Size = new System.Drawing.Size(169, 20);
            this.txtWorldConstructionsSearch.TabIndex = 44;
            this.txtWorldConstructionsSearch.TextChanged += new System.EventHandler(this.SearchWorldConstructionList);
            // 
            // tpWorldConstructionEvents
            // 
            this.tpWorldConstructionEvents.Location = new System.Drawing.Point(4, 22);
            this.tpWorldConstructionEvents.Name = "tpWorldConstructionEvents";
            this.tpWorldConstructionEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tpWorldConstructionEvents.Size = new System.Drawing.Size(253, 466);
            this.tpWorldConstructionEvents.TabIndex = 1;
            this.tpWorldConstructionEvents.Text = "Events";
            this.tpWorldConstructionEvents.UseVisualStyleBackColor = true;
            // 
            // InfrastructureTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcCollections);
            this.Name = "InfrastructureTab";
            this.tcCollections.ResumeLayout(false);
            this.tpSites.ResumeLayout(false);
            this.tcSites.ResumeLayout(false);
            this.tpSiteSearch.ResumeLayout(false);
            this.listPanel.ResumeLayout(false);
            this.listPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listSiteSearch)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tpStructures.ResumeLayout(false);
            this.tcStructures.ResumeLayout(false);
            this.tpStructureSearch.ResumeLayout(false);
            this.tpStructureSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listStructureSearch)).EndInit();
            this.groupBox25.ResumeLayout(false);
            this.groupBox25.PerformLayout();
            this.groupBox26.ResumeLayout(false);
            this.groupBox26.PerformLayout();
            this.tpWorldConstructions.ResumeLayout(false);
            this.tcWorldConstructions.ResumeLayout(false);
            this.tpWorldConstructionSearch.ResumeLayout(false);
            this.tpWorldConstructionSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listWorldConstructionsSearch)).EndInit();
            this.groupBox23.ResumeLayout(false);
            this.groupBox23.PerformLayout();
            this.groupBox24.ResumeLayout(false);
            this.groupBox24.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tcCollections;
        private TabPage tpSites;
        private TabPage tpStructures;
        private TabControl tcStructures;
        private TabPage tpStructureSearch;
        private Label lblStructureResults;
        private ObjectListView listStructureSearch;
        private OLVColumn olvColumn3;
        private OLVColumn olvColumn4;
        private Button btnStructuresSearch;
        private GroupBox groupBox25;
        private Label label6;
        private ComboBox cmbStructureType;
        private GroupBox groupBox26;
        private RadioButton radStructuresSortFiltered;
        private RadioButton radStructuresSortNone;
        private RadioButton radStructuresSortEvents;
        private TextBox txtStructuresSearch;
        private TabPage tpStructureEvents;
        private TabPage tpWorldConstructions;
        private TabControl tcWorldConstructions;
        private TabPage tpWorldConstructionSearch;
        private Label lblWorldConstructionResult;
        private ObjectListView listWorldConstructionsSearch;
        private OLVColumn olvColumn5;
        private OLVColumn olvColumn6;
        private Button btnWorldConstructionsSearch;
        private GroupBox groupBox23;
        private Label label7;
        private ComboBox cmbConstructionType;
        private GroupBox groupBox24;
        private RadioButton radWorldConstructionsSortFiltered;
        private RadioButton radWorldConstructionsSortNone;
        private RadioButton radWorldConstructionsSortEvents;
        private TextBox txtWorldConstructionsSearch;
        private TabPage tpWorldConstructionEvents;
        private TabControl tcSites;
        private TabPage tpSiteSearch;
        private Panel listPanel;
        private LinkLabel lnkMaxResults;
        private Label lblShownResults;
        private Button btnSiteSearch;
        private ObjectListView listSiteSearch;
        private OLVColumn olvName;
        private OLVColumn olvType;
        private TextBox txtSiteSearch;
        private Button btnSiteListReset;
        private GroupBox groupBox1;
        private GroupBox groupBox6;
        private RadioButton radSiteBeastAttacks;
        private RadioButton radSiteSortDeaths;
        private RadioButton radSortConnections;
        private ComboBox cmbSitePopulation;
        private RadioButton radSiteSortPopulation;
        private RadioButton radSiteSortWarfare;
        private RadioButton radSiteSortFiltered;
        private RadioButton radSiteOwners;
        private RadioButton radSiteNone;
        private RadioButton radSiteSortEvents;
        private Label label26;
        private ComboBox cmbSiteType;
        private TabPage tpSiteEvents;
    }
}
