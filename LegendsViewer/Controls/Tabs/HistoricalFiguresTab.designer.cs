using System.ComponentModel;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace LegendsViewer.Controls.Tabs
{
    partial class HistoricalFiguresTab
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
            this.baseRenderer1 = new BrightIdeasSoftware.BaseRenderer();
            this.tcHF = new System.Windows.Forms.TabControl();
            this.tpHFSearch = new System.Windows.Forms.TabPage();
            this.listPanel = new System.Windows.Forms.Panel();
            this.lblShownResults = new System.Windows.Forms.Label();
            this.lnkMaxResults = new System.Windows.Forms.LinkLabel();
            this.btnHFSearch = new System.Windows.Forms.Button();
            this.txtHFSearch = new System.Windows.Forms.TextBox();
            this.listHFSearch = new BrightIdeasSoftware.ObjectListView();
            this.olvName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btnHFListReset = new System.Windows.Forms.Button();
            this.grpHFFilter = new System.Windows.Forms.GroupBox();
            this.chkAnimated = new System.Windows.Forms.CheckBox();
            this.chkNecromancer = new System.Windows.Forms.CheckBox();
            this.chkWerebeast = new System.Windows.Forms.CheckBox();
            this.chkVampire = new System.Windows.Forms.CheckBox();
            this.chkHFLeader = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.radSortMiscKills = new System.Windows.Forms.RadioButton();
            this.radHFSortBattles = new System.Windows.Forms.RadioButton();
            this.radHFSortFiltered = new System.Windows.Forms.RadioButton();
            this.radHFNone = new System.Windows.Forms.RadioButton();
            this.radHFSortEvents = new System.Windows.Forms.RadioButton();
            this.radSortKills = new System.Windows.Forms.RadioButton();
            this.chkForce = new System.Windows.Forms.CheckBox();
            this.label30 = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label29 = new System.Windows.Forms.Label();
            this.cmbCaste = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.cmbRace = new System.Windows.Forms.ComboBox();
            this.chkAlive = new System.Windows.Forms.CheckBox();
            this.chkGhost = new System.Windows.Forms.CheckBox();
            this.chkDeity = new System.Windows.Forms.CheckBox();
            this.tpHFEvents = new System.Windows.Forms.TabPage();
            this.tcHF.SuspendLayout();
            this.tpHFSearch.SuspendLayout();
            this.listPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listHFSearch)).BeginInit();
            this.grpHFFilter.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcHF
            // 
            this.tcHF.Controls.Add(this.tpHFSearch);
            this.tcHF.Controls.Add(this.tpHFEvents);
            this.tcHF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcHF.Location = new System.Drawing.Point(0, 0);
            this.tcHF.Margin = new System.Windows.Forms.Padding(0);
            this.tcHF.Name = "tcHF";
            this.tcHF.Padding = new System.Drawing.Point(0, 0);
            this.tcHF.SelectedIndex = 0;
            this.tcHF.Size = new System.Drawing.Size(269, 518);
            this.tcHF.TabIndex = 1;
            this.tcHF.Selected += new System.Windows.Forms.TabControlEventHandler(this.OnSelected);
            // 
            // tpHFSearch
            // 
            this.tpHFSearch.Controls.Add(this.listPanel);
            this.tpHFSearch.Location = new System.Drawing.Point(4, 22);
            this.tpHFSearch.Margin = new System.Windows.Forms.Padding(0);
            this.tpHFSearch.Name = "tpHFSearch";
            this.tpHFSearch.Size = new System.Drawing.Size(261, 492);
            this.tpHFSearch.TabIndex = 0;
            this.tpHFSearch.Text = "Search";
            this.tpHFSearch.UseVisualStyleBackColor = true;
            // 
            // listPanel
            // 
            this.listPanel.Controls.Add(this.grpHFFilter);
            this.listPanel.Controls.Add(this.lblShownResults);
            this.listPanel.Controls.Add(this.lnkMaxResults);
            this.listPanel.Controls.Add(this.btnHFSearch);
            this.listPanel.Controls.Add(this.txtHFSearch);
            this.listPanel.Controls.Add(this.listHFSearch);
            this.listPanel.Controls.Add(this.btnHFListReset);
            this.listPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPanel.Location = new System.Drawing.Point(0, 0);
            this.listPanel.Name = "listPanel";
            this.listPanel.Size = new System.Drawing.Size(261, 492);
            this.listPanel.TabIndex = 44;
            // 
            // lblShownResults
            // 
            this.lblShownResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblShownResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShownResults.Location = new System.Drawing.Point(142, 220);
            this.lblShownResults.Name = "lblShownResults";
            this.lblShownResults.Size = new System.Drawing.Size(95, 10);
            this.lblShownResults.TabIndex = 44;
            this.lblShownResults.Text = "0 / 0";
            this.lblShownResults.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.hint.SetToolTip(this.lblShownResults, "Results Shown");
            // 
            // lnkMaxResults
            // 
            this.lnkMaxResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkMaxResults.AutoSize = true;
            this.lnkMaxResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkMaxResults.Location = new System.Drawing.Point(240, 220);
            this.lnkMaxResults.Name = "lnkMaxResults";
            this.lnkMaxResults.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lnkMaxResults.Size = new System.Drawing.Size(17, 9);
            this.lnkMaxResults.TabIndex = 43;
            this.lnkMaxResults.TabStop = true;
            this.lnkMaxResults.Text = "500";
            this.lnkMaxResults.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.hint.SetToolTip(this.lnkMaxResults, "Maximum number of results");
            this.lnkMaxResults.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkMaxResults_LinkClicked);
            // 
            // btnHFSearch
            // 
            this.btnHFSearch.Location = new System.Drawing.Point(3, 3);
            this.btnHFSearch.Name = "btnHFSearch";
            this.btnHFSearch.Size = new System.Drawing.Size(75, 23);
            this.btnHFSearch.TabIndex = 30;
            this.btnHFSearch.Text = "Search";
            this.btnHFSearch.UseVisualStyleBackColor = true;
            this.btnHFSearch.Click += new System.EventHandler(this.SearchHfList);
            // 
            // txtHFSearch
            // 
            this.txtHFSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHFSearch.Location = new System.Drawing.Point(81, 5);
            this.txtHFSearch.Name = "txtHFSearch";
            this.txtHFSearch.Size = new System.Drawing.Size(177, 20);
            this.txtHFSearch.TabIndex = 32;
            this.txtHFSearch.TextChanged += new System.EventHandler(this.SearchHfList);
            // 
            // listHFSearch
            // 
            this.listHFSearch.AllColumns.Add(this.olvName);
            this.listHFSearch.AlternateRowBackColor = System.Drawing.SystemColors.InactiveBorder;
            this.listHFSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listHFSearch.CellEditUseWholeCell = false;
            this.listHFSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvName});
            this.listHFSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.listHFSearch.FullRowSelect = true;
            this.listHFSearch.GridLines = true;
            this.listHFSearch.HeaderWordWrap = true;
            this.listHFSearch.HideSelection = false;
            this.listHFSearch.Location = new System.Drawing.Point(3, 30);
            this.listHFSearch.Name = "listHFSearch";
            this.listHFSearch.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.listHFSearch.ShowCommandMenuOnRightClick = true;
            this.listHFSearch.ShowImagesOnSubItems = true;
            this.listHFSearch.ShowItemCountOnGroups = true;
            this.listHFSearch.Size = new System.Drawing.Size(255, 174);
            this.listHFSearch.TabIndex = 42;
            this.listHFSearch.UseAlternatingBackColors = true;
            this.listHFSearch.UseCompatibleStateImageBehavior = false;
            this.listHFSearch.UseFiltering = true;
            this.listHFSearch.UseHotItem = true;
            this.listHFSearch.UseHyperlinks = true;
            this.listHFSearch.View = System.Windows.Forms.View.Details;
            this.listHFSearch.SelectedIndexChanged += new System.EventHandler(this.ListHFSearch_SelectedIndexChanged);
            // 
            // olvName
            // 
            this.olvName.AspectName = "Name";
            this.olvName.IsEditable = false;
            this.olvName.MinimumWidth = 50;
            this.olvName.Text = "Name";
            this.olvName.UseInitialLetterForGroup = true;
            this.olvName.Width = 235;
            // 
            // btnHFListReset
            // 
            this.btnHFListReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHFListReset.Location = new System.Drawing.Point(3, 210);
            this.btnHFListReset.Name = "btnHFListReset";
            this.btnHFListReset.Size = new System.Drawing.Size(50, 20);
            this.btnHFListReset.TabIndex = 41;
            this.btnHFListReset.Text = "Reset";
            this.btnHFListReset.UseVisualStyleBackColor = true;
            this.btnHFListReset.Click += new System.EventHandler(this.ResetHfBaseList);
            // 
            // grpHFFilter
            // 
            this.grpHFFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpHFFilter.Controls.Add(this.chkAnimated);
            this.grpHFFilter.Controls.Add(this.chkNecromancer);
            this.grpHFFilter.Controls.Add(this.chkWerebeast);
            this.grpHFFilter.Controls.Add(this.chkVampire);
            this.grpHFFilter.Controls.Add(this.chkHFLeader);
            this.grpHFFilter.Controls.Add(this.groupBox5);
            this.grpHFFilter.Controls.Add(this.chkForce);
            this.grpHFFilter.Controls.Add(this.label30);
            this.grpHFFilter.Controls.Add(this.cmbType);
            this.grpHFFilter.Controls.Add(this.label29);
            this.grpHFFilter.Controls.Add(this.cmbCaste);
            this.grpHFFilter.Controls.Add(this.label25);
            this.grpHFFilter.Controls.Add(this.cmbRace);
            this.grpHFFilter.Controls.Add(this.chkAlive);
            this.grpHFFilter.Controls.Add(this.chkGhost);
            this.grpHFFilter.Controls.Add(this.chkDeity);
            this.grpHFFilter.Location = new System.Drawing.Point(0, 236);
            this.grpHFFilter.MinimumSize = new System.Drawing.Size(249, 209);
            this.grpHFFilter.Name = "grpHFFilter";
            this.grpHFFilter.Size = new System.Drawing.Size(258, 253);
            this.grpHFFilter.TabIndex = 33;
            this.grpHFFilter.TabStop = false;
            this.grpHFFilter.Text = "Filter / Sort";
            // 
            // chkAnimated
            // 
            this.chkAnimated.AutoSize = true;
            this.chkAnimated.Location = new System.Drawing.Point(6, 229);
            this.chkAnimated.Name = "chkAnimated";
            this.chkAnimated.Size = new System.Drawing.Size(106, 17);
            this.chkAnimated.TabIndex = 21;
            this.chkAnimated.Text = "Animated Corpse";
            this.chkAnimated.UseVisualStyleBackColor = true;
            this.chkAnimated.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // chkNecromancer
            // 
            this.chkNecromancer.AutoSize = true;
            this.chkNecromancer.Location = new System.Drawing.Point(6, 206);
            this.chkNecromancer.Name = "chkNecromancer";
            this.chkNecromancer.Size = new System.Drawing.Size(90, 17);
            this.chkNecromancer.TabIndex = 20;
            this.chkNecromancer.Text = "Necromancer";
            this.chkNecromancer.UseVisualStyleBackColor = true;
            this.chkNecromancer.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // chkWerebeast
            // 
            this.chkWerebeast.AutoSize = true;
            this.chkWerebeast.Location = new System.Drawing.Point(6, 185);
            this.chkWerebeast.Name = "chkWerebeast";
            this.chkWerebeast.Size = new System.Drawing.Size(78, 17);
            this.chkWerebeast.TabIndex = 19;
            this.chkWerebeast.Text = "Werebeast";
            this.chkWerebeast.UseVisualStyleBackColor = true;
            this.chkWerebeast.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // chkVampire
            // 
            this.chkVampire.AutoSize = true;
            this.chkVampire.Location = new System.Drawing.Point(6, 162);
            this.chkVampire.Name = "chkVampire";
            this.chkVampire.Size = new System.Drawing.Size(64, 17);
            this.chkVampire.TabIndex = 18;
            this.chkVampire.Text = "Vampire";
            this.chkVampire.UseVisualStyleBackColor = true;
            this.chkVampire.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // chkHFLeader
            // 
            this.chkHFLeader.AutoSize = true;
            this.chkHFLeader.Location = new System.Drawing.Point(6, 139);
            this.chkHFLeader.Name = "chkHFLeader";
            this.chkHFLeader.Size = new System.Drawing.Size(59, 17);
            this.chkHFLeader.TabIndex = 17;
            this.chkHFLeader.Text = "Leader";
            this.chkHFLeader.UseVisualStyleBackColor = true;
            this.chkHFLeader.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.radSortMiscKills);
            this.groupBox5.Controls.Add(this.radHFSortBattles);
            this.groupBox5.Controls.Add(this.radHFSortFiltered);
            this.groupBox5.Controls.Add(this.radHFNone);
            this.groupBox5.Controls.Add(this.radHFSortEvents);
            this.groupBox5.Controls.Add(this.radSortKills);
            this.groupBox5.Location = new System.Drawing.Point(133, 9);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(119, 168);
            this.groupBox5.TabIndex = 16;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sort By";
            // 
            // radSortMiscKills
            // 
            this.radSortMiscKills.Location = new System.Drawing.Point(6, 139);
            this.radSortMiscKills.Name = "radSortMiscKills";
            this.radSortMiscKills.Size = new System.Drawing.Size(103, 15);
            this.radSortMiscKills.TabIndex = 23;
            this.radSortMiscKills.Text = "Misc Kills";
            this.radSortMiscKills.UseVisualStyleBackColor = true;
            this.radSortMiscKills.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // radHFSortBattles
            // 
            this.radHFSortBattles.AutoSize = true;
            this.radHFSortBattles.Location = new System.Drawing.Point(6, 91);
            this.radHFSortBattles.Name = "radHFSortBattles";
            this.radHFSortBattles.Size = new System.Drawing.Size(57, 17);
            this.radHFSortBattles.TabIndex = 21;
            this.radHFSortBattles.TabStop = true;
            this.radHFSortBattles.Text = "Battles";
            this.radHFSortBattles.UseVisualStyleBackColor = true;
            this.radHFSortBattles.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // radHFSortFiltered
            // 
            this.radHFSortFiltered.AutoSize = true;
            this.radHFSortFiltered.Location = new System.Drawing.Point(6, 43);
            this.radHFSortFiltered.Name = "radHFSortFiltered";
            this.radHFSortFiltered.Size = new System.Drawing.Size(95, 17);
            this.radHFSortFiltered.TabIndex = 20;
            this.radHFSortFiltered.TabStop = true;
            this.radHFSortFiltered.Text = "Filtered Events";
            this.radHFSortFiltered.UseVisualStyleBackColor = true;
            this.radHFSortFiltered.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // radHFNone
            // 
            this.radHFNone.AutoSize = true;
            this.radHFNone.Checked = true;
            this.radHFNone.Location = new System.Drawing.Point(6, 115);
            this.radHFNone.Name = "radHFNone";
            this.radHFNone.Size = new System.Drawing.Size(51, 17);
            this.radHFNone.TabIndex = 19;
            this.radHFNone.TabStop = true;
            this.radHFNone.Text = "None";
            this.radHFNone.UseVisualStyleBackColor = true;
            this.radHFNone.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // radHFSortEvents
            // 
            this.radHFSortEvents.AutoSize = true;
            this.radHFSortEvents.Location = new System.Drawing.Point(6, 19);
            this.radHFSortEvents.Name = "radHFSortEvents";
            this.radHFSortEvents.Size = new System.Drawing.Size(58, 17);
            this.radHFSortEvents.TabIndex = 18;
            this.radHFSortEvents.Text = "Events";
            this.radHFSortEvents.UseVisualStyleBackColor = true;
            this.radHFSortEvents.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // radSortKills
            // 
            this.radSortKills.AutoSize = true;
            this.radSortKills.Location = new System.Drawing.Point(6, 67);
            this.radSortKills.Name = "radSortKills";
            this.radSortKills.Size = new System.Drawing.Size(83, 17);
            this.radSortKills.TabIndex = 17;
            this.radSortKills.Text = "Notable Kills";
            this.radSortKills.UseVisualStyleBackColor = true;
            this.radSortKills.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // chkForce
            // 
            this.chkForce.AutoSize = true;
            this.chkForce.Location = new System.Drawing.Point(192, 229);
            this.chkForce.Name = "chkForce";
            this.chkForce.Size = new System.Drawing.Size(53, 17);
            this.chkForce.TabIndex = 15;
            this.chkForce.Text = "Force";
            this.chkForce.UseVisualStyleBackColor = true;
            this.chkForce.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(3, 96);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(31, 13);
            this.label30.TabIndex = 14;
            this.label30.Text = "Type";
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(6, 112);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(121, 21);
            this.cmbType.TabIndex = 13;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.SearchHfList);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(3, 57);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(34, 13);
            this.label29.TabIndex = 12;
            this.label29.Text = "Caste";
            // 
            // cmbCaste
            // 
            this.cmbCaste.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCaste.FormattingEnabled = true;
            this.cmbCaste.Location = new System.Drawing.Point(6, 73);
            this.cmbCaste.Name = "cmbCaste";
            this.cmbCaste.Size = new System.Drawing.Size(121, 21);
            this.cmbCaste.TabIndex = 11;
            this.cmbCaste.SelectedIndexChanged += new System.EventHandler(this.SearchHfList);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(3, 17);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(33, 13);
            this.label25.TabIndex = 10;
            this.label25.Text = "Race";
            // 
            // cmbRace
            // 
            this.cmbRace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRace.FormattingEnabled = true;
            this.cmbRace.Location = new System.Drawing.Point(6, 33);
            this.cmbRace.Name = "cmbRace";
            this.cmbRace.Size = new System.Drawing.Size(121, 21);
            this.cmbRace.TabIndex = 6;
            this.cmbRace.SelectedIndexChanged += new System.EventHandler(this.SearchHfList);
            // 
            // chkAlive
            // 
            this.chkAlive.AutoSize = true;
            this.chkAlive.Location = new System.Drawing.Point(133, 229);
            this.chkAlive.Name = "chkAlive";
            this.chkAlive.Size = new System.Drawing.Size(49, 17);
            this.chkAlive.TabIndex = 4;
            this.chkAlive.Text = "Alive";
            this.chkAlive.UseVisualStyleBackColor = true;
            this.chkAlive.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // chkGhost
            // 
            this.chkGhost.AutoSize = true;
            this.chkGhost.Location = new System.Drawing.Point(133, 206);
            this.chkGhost.Name = "chkGhost";
            this.chkGhost.Size = new System.Drawing.Size(54, 17);
            this.chkGhost.TabIndex = 3;
            this.chkGhost.Text = "Ghost";
            this.chkGhost.UseVisualStyleBackColor = true;
            this.chkGhost.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // chkDeity
            // 
            this.chkDeity.AutoSize = true;
            this.chkDeity.Location = new System.Drawing.Point(192, 206);
            this.chkDeity.Name = "chkDeity";
            this.chkDeity.Size = new System.Drawing.Size(50, 17);
            this.chkDeity.TabIndex = 0;
            this.chkDeity.Text = "Deity";
            this.chkDeity.UseVisualStyleBackColor = true;
            this.chkDeity.CheckedChanged += new System.EventHandler(this.SearchHfList);
            // 
            // tpHFEvents
            // 
            this.tpHFEvents.AutoScroll = true;
            this.tpHFEvents.Location = new System.Drawing.Point(4, 22);
            this.tpHFEvents.Name = "tpHFEvents";
            this.tpHFEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tpHFEvents.Size = new System.Drawing.Size(261, 492);
            this.tpHFEvents.TabIndex = 1;
            this.tpHFEvents.Text = "Events";
            this.tpHFEvents.UseVisualStyleBackColor = true;
            // 
            // HistoricalFiguresTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcHF);
            this.Name = "HistoricalFiguresTab";
            this.tcHF.ResumeLayout(false);
            this.tpHFSearch.ResumeLayout(false);
            this.listPanel.ResumeLayout(false);
            this.listPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listHFSearch)).EndInit();
            this.grpHFFilter.ResumeLayout(false);
            this.grpHFFilter.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tcHF;
        private TabPage tpHFSearch;
        private Button btnHFListReset;
        private GroupBox grpHFFilter;
        private CheckBox chkWerebeast;
        private CheckBox chkVampire;
        private CheckBox chkHFLeader;
        private GroupBox groupBox5;
        private RadioButton radSortMiscKills;
        private RadioButton radHFSortBattles;
        private RadioButton radHFSortFiltered;
        private RadioButton radHFNone;
        private RadioButton radHFSortEvents;
        private RadioButton radSortKills;
        private CheckBox chkForce;
        private Label label30;
        private ComboBox cmbType;
        private Label label29;
        private ComboBox cmbCaste;
        private Label label25;
        private ComboBox cmbRace;
        private CheckBox chkAlive;
        private CheckBox chkGhost;
        private CheckBox chkDeity;
        private TextBox txtHFSearch;
        private Button btnHFSearch;
        private TabPage tpHFEvents;
        private ObjectListView listHFSearch;
        private OLVColumn olvName;
        private BaseRenderer baseRenderer1;
        private Panel listPanel;
        private LinkLabel lnkMaxResults;
        private Label lblShownResults;
        private CheckBox chkNecromancer;
        private CheckBox chkAnimated;
    }
}
