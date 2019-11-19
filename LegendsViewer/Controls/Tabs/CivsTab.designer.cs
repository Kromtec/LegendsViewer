using System.ComponentModel;
using System.Windows.Forms;
using BrightIdeasSoftware;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.Tabs
{
    partial class CivsTab
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
            this.tcCivs = new System.Windows.Forms.TabControl();
            this.tpCivSearch = new System.Windows.Forms.TabPage();
            this.lblShownResults = new System.Windows.Forms.Label();
            this.listCivSearch = new BrightIdeasSoftware.ObjectListView();
            this.olvName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbEntityType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.cmbEntityPopulation = new System.Windows.Forms.ComboBox();
            this.radEntitySortPopulation = new System.Windows.Forms.RadioButton();
            this.radCivSortWars = new System.Windows.Forms.RadioButton();
            this.radCivSortFiltered = new System.Windows.Forms.RadioButton();
            this.radCivSites = new System.Windows.Forms.RadioButton();
            this.radEntityNone = new System.Windows.Forms.RadioButton();
            this.radEntitySortEvents = new System.Windows.Forms.RadioButton();
            this.chkCiv = new System.Windows.Forms.CheckBox();
            this.cmbCivRace = new System.Windows.Forms.ComboBox();
            this.txtCivSearch = new System.Windows.Forms.TextBox();
            this.btnCivSearch = new System.Windows.Forms.Button();
            this.tpCivEvents = new System.Windows.Forms.TabPage();
            this.tcCivs.SuspendLayout();
            this.tpCivSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listCivSearch)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcCivs
            // 
            this.tcCivs.Controls.Add(this.tpCivSearch);
            this.tcCivs.Controls.Add(this.tpCivEvents);
            this.tcCivs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcCivs.Location = new System.Drawing.Point(0, 0);
            this.tcCivs.Name = "tcCivs";
            this.tcCivs.SelectedIndex = 0;
            this.tcCivs.Size = new System.Drawing.Size(269, 518);
            this.tcCivs.TabIndex = 1;
            // 
            // tpCivSearch
            // 
            this.tpCivSearch.Controls.Add(this.lblShownResults);
            this.tpCivSearch.Controls.Add(this.listCivSearch);
            this.tpCivSearch.Controls.Add(this.groupBox4);
            this.tpCivSearch.Controls.Add(this.txtCivSearch);
            this.tpCivSearch.Controls.Add(this.btnCivSearch);
            this.tpCivSearch.Location = new System.Drawing.Point(4, 22);
            this.tpCivSearch.Name = "tpCivSearch";
            this.tpCivSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tpCivSearch.Size = new System.Drawing.Size(261, 492);
            this.tpCivSearch.TabIndex = 0;
            this.tpCivSearch.Text = "Search";
            this.tpCivSearch.UseVisualStyleBackColor = true;
            // 
            // lblShownResults
            // 
            this.lblShownResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblShownResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShownResults.Location = new System.Drawing.Point(160, 262);
            this.lblShownResults.Name = "lblShownResults";
            this.lblShownResults.Size = new System.Drawing.Size(95, 10);
            this.lblShownResults.TabIndex = 51;
            this.lblShownResults.Text = "0 / 0";
            this.lblShownResults.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.hint.SetToolTip(this.lblShownResults, "Results Shown");
            // 
            // listCivSearch
            // 
            this.listCivSearch.AllColumns.Add(this.olvName);
            this.listCivSearch.AllColumns.Add(this.olvType);
            this.listCivSearch.AlternateRowBackColor = System.Drawing.SystemColors.InactiveBorder;
            this.listCivSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listCivSearch.CellEditUseWholeCell = false;
            this.listCivSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvName,
            this.olvType});
            this.listCivSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.listCivSearch.FullRowSelect = true;
            this.listCivSearch.GridLines = true;
            this.listCivSearch.HeaderWordWrap = true;
            this.listCivSearch.SelectedBackColor = System.Drawing.Color.Empty;
            this.listCivSearch.SelectedForeColor = System.Drawing.Color.Empty;
            this.listCivSearch.Location = new System.Drawing.Point(3, 31);
            this.listCivSearch.Name = "listCivSearch";
            this.listCivSearch.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.listCivSearch.ShowCommandMenuOnRightClick = true;
            this.listCivSearch.ShowImagesOnSubItems = true;
            this.listCivSearch.ShowItemCountOnGroups = true;
            this.listCivSearch.Size = new System.Drawing.Size(253, 228);
            this.listCivSearch.TabIndex = 50;
            this.listCivSearch.UseAlternatingBackColors = true;
            this.listCivSearch.UseCompatibleStateImageBehavior = false;
            this.listCivSearch.UseFiltering = true;
            this.listCivSearch.UseHotItem = true;
            this.listCivSearch.UseHyperlinks = true;
            this.listCivSearch.View = System.Windows.Forms.View.Details;
            this.listCivSearch.SelectedIndexChanged += new System.EventHandler(this.listCivSearch_SelectedIndexChanged);
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
            this.olvType.AspectName = nameof(Entity.TypeAsString);
            this.olvType.Text = "Type";
            this.olvType.Width = 85;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.cmbEntityType);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.groupBox9);
            this.groupBox4.Controls.Add(this.chkCiv);
            this.groupBox4.Controls.Add(this.cmbCivRace);
            this.groupBox4.Location = new System.Drawing.Point(3, 275);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(254, 211);
            this.groupBox4.TabIndex = 37;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Filter / Sort";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Type";
            // 
            // cmbEntityType
            // 
            this.cmbEntityType.FormattingEnabled = true;
            this.cmbEntityType.Location = new System.Drawing.Point(6, 32);
            this.cmbEntityType.Name = "cmbEntityType";
            this.cmbEntityType.Size = new System.Drawing.Size(121, 21);
            this.cmbEntityType.TabIndex = 17;
            this.cmbEntityType.SelectedIndexChanged += new System.EventHandler(this.SearchEntityList);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Race";
            // 
            // groupBox9
            // 
            this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox9.Controls.Add(this.cmbEntityPopulation);
            this.groupBox9.Controls.Add(this.radEntitySortPopulation);
            this.groupBox9.Controls.Add(this.radCivSortWars);
            this.groupBox9.Controls.Add(this.radCivSortFiltered);
            this.groupBox9.Controls.Add(this.radCivSites);
            this.groupBox9.Controls.Add(this.radEntityNone);
            this.groupBox9.Controls.Add(this.radEntitySortEvents);
            this.groupBox9.Location = new System.Drawing.Point(6, 98);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(240, 107);
            this.groupBox9.TabIndex = 15;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Sort By";
            // 
            // cmbEntityPopulation
            // 
            this.cmbEntityPopulation.FormattingEnabled = true;
            this.cmbEntityPopulation.Location = new System.Drawing.Point(113, 38);
            this.cmbEntityPopulation.Name = "cmbEntityPopulation";
            this.cmbEntityPopulation.Size = new System.Drawing.Size(121, 21);
            this.cmbEntityPopulation.TabIndex = 19;
            this.cmbEntityPopulation.SelectedIndexChanged += new System.EventHandler(this.SearchEntityList);
            // 
            // radEntitySortPopulation
            // 
            this.radEntitySortPopulation.AutoSize = true;
            this.radEntitySortPopulation.Location = new System.Drawing.Point(114, 19);
            this.radEntitySortPopulation.Name = "radEntitySortPopulation";
            this.radEntitySortPopulation.Size = new System.Drawing.Size(75, 17);
            this.radEntitySortPopulation.TabIndex = 18;
            this.radEntitySortPopulation.TabStop = true;
            this.radEntitySortPopulation.Text = "Population";
            this.radEntitySortPopulation.UseVisualStyleBackColor = true;
            this.radEntitySortPopulation.CheckedChanged += new System.EventHandler(this.SearchEntityList);
            // 
            // radCivSortWars
            // 
            this.radCivSortWars.AutoSize = true;
            this.radCivSortWars.Location = new System.Drawing.Point(6, 88);
            this.radCivSortWars.Name = "radCivSortWars";
            this.radCivSortWars.Size = new System.Drawing.Size(50, 17);
            this.radCivSortWars.TabIndex = 17;
            this.radCivSortWars.TabStop = true;
            this.radCivSortWars.Text = "Wars";
            this.radCivSortWars.UseVisualStyleBackColor = true;
            this.radCivSortWars.CheckedChanged += new System.EventHandler(this.SearchEntityList);
            // 
            // radCivSortFiltered
            // 
            this.radCivSortFiltered.AutoSize = true;
            this.radCivSortFiltered.Location = new System.Drawing.Point(6, 42);
            this.radCivSortFiltered.Name = "radCivSortFiltered";
            this.radCivSortFiltered.Size = new System.Drawing.Size(95, 17);
            this.radCivSortFiltered.TabIndex = 16;
            this.radCivSortFiltered.TabStop = true;
            this.radCivSortFiltered.Text = "Filtered Events";
            this.radCivSortFiltered.UseVisualStyleBackColor = true;
            this.radCivSortFiltered.CheckedChanged += new System.EventHandler(this.SearchEntityList);
            // 
            // radCivSites
            // 
            this.radCivSites.AutoSize = true;
            this.radCivSites.Location = new System.Drawing.Point(6, 65);
            this.radCivSites.Name = "radCivSites";
            this.radCivSites.Size = new System.Drawing.Size(48, 17);
            this.radCivSites.TabIndex = 15;
            this.radCivSites.TabStop = true;
            this.radCivSites.Text = "Sites";
            this.radCivSites.UseVisualStyleBackColor = true;
            this.radCivSites.CheckedChanged += new System.EventHandler(this.SearchEntityList);
            // 
            // radEntityNone
            // 
            this.radEntityNone.AutoSize = true;
            this.radEntityNone.Checked = true;
            this.radEntityNone.Location = new System.Drawing.Point(114, 65);
            this.radEntityNone.Name = "radEntityNone";
            this.radEntityNone.Size = new System.Drawing.Size(51, 17);
            this.radEntityNone.TabIndex = 14;
            this.radEntityNone.TabStop = true;
            this.radEntityNone.Text = "None";
            this.radEntityNone.UseVisualStyleBackColor = true;
            this.radEntityNone.CheckedChanged += new System.EventHandler(this.SearchEntityList);
            // 
            // radEntitySortEvents
            // 
            this.radEntitySortEvents.AutoSize = true;
            this.radEntitySortEvents.Location = new System.Drawing.Point(6, 19);
            this.radEntitySortEvents.Name = "radEntitySortEvents";
            this.radEntitySortEvents.Size = new System.Drawing.Size(58, 17);
            this.radEntitySortEvents.TabIndex = 13;
            this.radEntitySortEvents.Text = "Events";
            this.radEntitySortEvents.UseVisualStyleBackColor = true;
            this.radEntitySortEvents.CheckedChanged += new System.EventHandler(this.SearchEntityList);
            // 
            // chkCiv
            // 
            this.chkCiv.AutoSize = true;
            this.chkCiv.Location = new System.Drawing.Point(160, 75);
            this.chkCiv.Name = "chkCiv";
            this.chkCiv.Size = new System.Drawing.Size(80, 17);
            this.chkCiv.TabIndex = 14;
            this.chkCiv.Text = "Civilizations";
            this.chkCiv.UseVisualStyleBackColor = true;
            this.chkCiv.CheckedChanged += new System.EventHandler(this.SearchEntityList);
            // 
            // cmbCivRace
            // 
            this.cmbCivRace.FormattingEnabled = true;
            this.cmbCivRace.Location = new System.Drawing.Point(6, 72);
            this.cmbCivRace.Name = "cmbCivRace";
            this.cmbCivRace.Size = new System.Drawing.Size(121, 21);
            this.cmbCivRace.TabIndex = 13;
            this.cmbCivRace.SelectedIndexChanged += new System.EventHandler(this.SearchEntityList);
            // 
            // txtCivSearch
            // 
            this.txtCivSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCivSearch.Location = new System.Drawing.Point(81, 5);
            this.txtCivSearch.Name = "txtCivSearch";
            this.txtCivSearch.Size = new System.Drawing.Size(177, 20);
            this.txtCivSearch.TabIndex = 36;
            this.txtCivSearch.TextChanged += new System.EventHandler(this.SearchEntityList);
            // 
            // btnCivSearch
            // 
            this.btnCivSearch.Location = new System.Drawing.Point(3, 3);
            this.btnCivSearch.Name = "btnCivSearch";
            this.btnCivSearch.Size = new System.Drawing.Size(75, 23);
            this.btnCivSearch.TabIndex = 34;
            this.btnCivSearch.Text = "Search";
            this.btnCivSearch.UseVisualStyleBackColor = true;
            this.btnCivSearch.Click += new System.EventHandler(this.SearchEntityList);
            // 
            // tpCivEvents
            // 
            this.tpCivEvents.AutoScroll = true;
            this.tpCivEvents.Location = new System.Drawing.Point(4, 22);
            this.tpCivEvents.Name = "tpCivEvents";
            this.tpCivEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tpCivEvents.Size = new System.Drawing.Size(261, 492);
            this.tpCivEvents.TabIndex = 1;
            this.tpCivEvents.Text = "Events";
            this.tpCivEvents.UseVisualStyleBackColor = true;
            // 
            // CivsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcCivs);
            this.Name = "CivsTab";
            this.tcCivs.ResumeLayout(false);
            this.tpCivSearch.ResumeLayout(false);
            this.tpCivSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listCivSearch)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tcCivs;
        private TabPage tpCivSearch;
        private GroupBox groupBox4;
        private Label label1;
        private GroupBox groupBox9;
        private ComboBox cmbEntityPopulation;
        private RadioButton radEntitySortPopulation;
        private RadioButton radCivSortWars;
        private RadioButton radCivSortFiltered;
        private RadioButton radCivSites;
        private RadioButton radEntityNone;
        private RadioButton radEntitySortEvents;
        private CheckBox chkCiv;
        private ComboBox cmbCivRace;
        private TextBox txtCivSearch;
        private Button btnCivSearch;
        private TabPage tpCivEvents;
        private Label label2;
        private ComboBox cmbEntityType;
        private OLVColumn olvName;
        private ObjectListView listCivSearch;
        private OLVColumn olvType;
        private Label lblShownResults;
    }
}
