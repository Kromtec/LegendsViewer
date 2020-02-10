using System.ComponentModel;
using System.Windows.Forms;
using LegendsViewer.Controls.Tabs;

namespace LegendsViewer
{
    partial class FrmLegendsViewer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLegendsViewer));
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.hint = new System.Windows.Forms.ToolTip(this.components);
            this.lblVersion = new System.Windows.Forms.Label();
            this.scWorld = new System.Windows.Forms.SplitContainer();
            this.tcWorld = new System.Windows.Forms.TabControl();
            this.tpSummary = new System.Windows.Forms.TabPage();
            this.summaryTab1 = new LegendsViewer.Controls.Tabs.SummaryTab();
            this.tpHF = new System.Windows.Forms.TabPage();
            this.historicalFiguresTab1 = new LegendsViewer.Controls.Tabs.HistoricalFiguresTab();
            this.tpCivs = new System.Windows.Forms.TabPage();
            this.civsTab1 = new LegendsViewer.Controls.Tabs.CivsTab();
            this.tpArt = new System.Windows.Forms.TabPage();
            this.artAndCraftsTab1 = new LegendsViewer.Controls.Tabs.ArtAndCraftsTab();
            this.tpInfrastructure = new System.Windows.Forms.TabPage();
            this.infrastructureTab1 = new LegendsViewer.Controls.Tabs.InfrastructureTab();
            this.tpRegions = new System.Windows.Forms.TabPage();
            this.geographyTab1 = new LegendsViewer.Controls.Tabs.GeographyTab();
            this.tpWarfare = new System.Windows.Forms.TabPage();
            this.warfareTab1 = new LegendsViewer.Controls.Tabs.WarfareTab();
            this.tpCollections = new System.Windows.Forms.TabPage();
            this.collectionsTab1 = new LegendsViewer.Controls.Tabs.CollectionsTab();
            this.btnChart = new System.Windows.Forms.Button();
            this.btnMap = new System.Windows.Forms.Button();
            this.btnStats = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnForward = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.scWorld)).BeginInit();
            this.scWorld.Panel1.SuspendLayout();
            this.scWorld.Panel2.SuspendLayout();
            this.scWorld.SuspendLayout();
            this.tcWorld.SuspendLayout();
            this.tpSummary.SuspendLayout();
            this.tpHF.SuspendLayout();
            this.tpCivs.SuspendLayout();
            this.tpArt.SuspendLayout();
            this.tpInfrastructure.SuspendLayout();
            this.tpRegions.SuspendLayout();
            this.tpWarfare.SuspendLayout();
            this.tpCollections.SuspendLayout();
            this.SuspendLayout();
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "Dwarf Files | *.xml; *.txt; *.zip";
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.AutoSize = true;
            this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblVersion.Location = new System.Drawing.Point(930, 9);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(48, 13);
            this.lblVersion.TabIndex = 34;
            this.lblVersion.Text = "v1.00.00";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblVersion.Click += new System.EventHandler(this.Open_ReadMe);
            // 
            // scWorld
            // 
            this.scWorld.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scWorld.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scWorld.Location = new System.Drawing.Point(0, 0);
            this.scWorld.Name = "scWorld";
            // 
            // scWorld.Panel1
            // 
            this.scWorld.Panel1.Controls.Add(this.tcWorld);
            this.scWorld.Panel1MinSize = 279;
            // 
            // scWorld.Panel2
            // 
            this.scWorld.Panel2.Controls.Add(this.btnChart);
            this.scWorld.Panel2.Controls.Add(this.btnMap);
            this.scWorld.Panel2.Controls.Add(this.btnStats);
            this.scWorld.Panel2.Controls.Add(this.lblVersion);
            this.scWorld.Panel2.Controls.Add(this.btnBack);
            this.scWorld.Panel2.Controls.Add(this.btnForward);
            this.scWorld.Size = new System.Drawing.Size(1264, 681);
            this.scWorld.SplitterDistance = 279;
            this.scWorld.TabIndex = 35;
            // 
            // tcWorld
            // 
            this.tcWorld.Controls.Add(this.tpSummary);
            this.tcWorld.Controls.Add(this.tpHF);
            this.tcWorld.Controls.Add(this.tpCivs);
            this.tcWorld.Controls.Add(this.tpArt);
            this.tcWorld.Controls.Add(this.tpInfrastructure);
            this.tcWorld.Controls.Add(this.tpRegions);
            this.tcWorld.Controls.Add(this.tpWarfare);
            this.tcWorld.Controls.Add(this.tpCollections);
            this.tcWorld.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcWorld.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tcWorld.Location = new System.Drawing.Point(0, 0);
            this.tcWorld.Multiline = true;
            this.tcWorld.Name = "tcWorld";
            this.tcWorld.SelectedIndex = 0;
            this.tcWorld.Size = new System.Drawing.Size(279, 681);
            this.tcWorld.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tcWorld.TabIndex = 2;
            this.tcWorld.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
            // 
            // tpSummary
            // 
            this.tpSummary.Controls.Add(this.summaryTab1);
            this.tpSummary.Location = new System.Drawing.Point(4, 58);
            this.tpSummary.Name = "tpSummary";
            this.tpSummary.Size = new System.Drawing.Size(271, 619);
            this.tpSummary.TabIndex = 0;
            this.tpSummary.Text = "Summary";
            this.tpSummary.UseVisualStyleBackColor = true;
            // 
            // summaryTab1
            // 
            this.summaryTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.summaryTab1.Location = new System.Drawing.Point(0, 0);
            this.summaryTab1.Name = "summaryTab1";
            this.summaryTab1.Size = new System.Drawing.Size(271, 619);
            this.summaryTab1.TabIndex = 0;
            // 
            // tpHF
            // 
            this.tpHF.Controls.Add(this.historicalFiguresTab1);
            this.tpHF.Location = new System.Drawing.Point(4, 58);
            this.tpHF.Margin = new System.Windows.Forms.Padding(0);
            this.tpHF.Name = "tpHF";
            this.tpHF.Size = new System.Drawing.Size(271, 619);
            this.tpHF.TabIndex = 1;
            this.tpHF.Text = "Historical Figures";
            this.tpHF.UseVisualStyleBackColor = true;
            // 
            // historicalFiguresTab1
            // 
            this.historicalFiguresTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historicalFiguresTab1.Location = new System.Drawing.Point(0, 0);
            this.historicalFiguresTab1.Name = "historicalFiguresTab1";
            this.historicalFiguresTab1.Size = new System.Drawing.Size(271, 619);
            this.historicalFiguresTab1.TabIndex = 0;
            // 
            // tpCivs
            // 
            this.tpCivs.Controls.Add(this.civsTab1);
            this.tpCivs.Location = new System.Drawing.Point(4, 58);
            this.tpCivs.Name = "tpCivs";
            this.tpCivs.Size = new System.Drawing.Size(271, 619);
            this.tpCivs.TabIndex = 5;
            this.tpCivs.Text = "Civs and entities";
            this.tpCivs.UseVisualStyleBackColor = true;
            // 
            // civsTab1
            // 
            this.civsTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.civsTab1.Location = new System.Drawing.Point(0, 0);
            this.civsTab1.Name = "civsTab1";
            this.civsTab1.Size = new System.Drawing.Size(271, 619);
            this.civsTab1.TabIndex = 0;
            // 
            // tpArt
            // 
            this.tpArt.Controls.Add(this.artAndCraftsTab1);
            this.tpArt.Location = new System.Drawing.Point(4, 58);
            this.tpArt.Name = "tpArt";
            this.tpArt.Size = new System.Drawing.Size(271, 619);
            this.tpArt.TabIndex = 8;
            this.tpArt.Text = "Art & Crafts";
            this.tpArt.UseVisualStyleBackColor = true;
            // 
            // artAndCraftsTab1
            // 
            this.artAndCraftsTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.artAndCraftsTab1.Location = new System.Drawing.Point(0, 0);
            this.artAndCraftsTab1.Name = "artAndCraftsTab1";
            this.artAndCraftsTab1.Size = new System.Drawing.Size(271, 619);
            this.artAndCraftsTab1.TabIndex = 0;
            // 
            // tpInfrastructure
            // 
            this.tpInfrastructure.Controls.Add(this.infrastructureTab1);
            this.tpInfrastructure.Location = new System.Drawing.Point(4, 58);
            this.tpInfrastructure.Name = "tpInfrastructure";
            this.tpInfrastructure.Size = new System.Drawing.Size(271, 619);
            this.tpInfrastructure.TabIndex = 2;
            this.tpInfrastructure.Text = "Infrastructure";
            this.tpInfrastructure.UseVisualStyleBackColor = true;
            // 
            // infrastructureTab1
            // 
            this.infrastructureTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infrastructureTab1.Location = new System.Drawing.Point(0, 0);
            this.infrastructureTab1.Name = "infrastructureTab1";
            this.infrastructureTab1.Size = new System.Drawing.Size(271, 619);
            this.infrastructureTab1.TabIndex = 0;
            // 
            // tpRegions
            // 
            this.tpRegions.Controls.Add(this.geographyTab1);
            this.tpRegions.Location = new System.Drawing.Point(4, 58);
            this.tpRegions.Name = "tpRegions";
            this.tpRegions.Size = new System.Drawing.Size(271, 619);
            this.tpRegions.TabIndex = 3;
            this.tpRegions.Text = "Geography";
            this.tpRegions.UseVisualStyleBackColor = true;
            // 
            // geographyTab1
            // 
            this.geographyTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.geographyTab1.Location = new System.Drawing.Point(0, 0);
            this.geographyTab1.Name = "geographyTab1";
            this.geographyTab1.Size = new System.Drawing.Size(271, 619);
            this.geographyTab1.TabIndex = 0;
            // 
            // tpWarfare
            // 
            this.tpWarfare.Controls.Add(this.warfareTab1);
            this.tpWarfare.Location = new System.Drawing.Point(4, 58);
            this.tpWarfare.Name = "tpWarfare";
            this.tpWarfare.Size = new System.Drawing.Size(271, 619);
            this.tpWarfare.TabIndex = 6;
            this.tpWarfare.Text = "Warfare";
            this.tpWarfare.UseVisualStyleBackColor = true;
            // 
            // warfareTab1
            // 
            this.warfareTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.warfareTab1.Location = new System.Drawing.Point(0, 0);
            this.warfareTab1.Name = "warfareTab1";
            this.warfareTab1.Size = new System.Drawing.Size(271, 619);
            this.warfareTab1.TabIndex = 0;
            // 
            // tpCollections
            // 
            this.tpCollections.Controls.Add(this.collectionsTab1);
            this.tpCollections.Location = new System.Drawing.Point(4, 58);
            this.tpCollections.Name = "tpCollections";
            this.tpCollections.Size = new System.Drawing.Size(271, 619);
            this.tpCollections.TabIndex = 7;
            this.tpCollections.Text = "Other";
            this.tpCollections.UseVisualStyleBackColor = true;
            // 
            // collectionsTab1
            // 
            this.collectionsTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collectionsTab1.Location = new System.Drawing.Point(0, 0);
            this.collectionsTab1.Name = "collectionsTab1";
            this.collectionsTab1.Size = new System.Drawing.Size(271, 619);
            this.collectionsTab1.TabIndex = 0;
            // 
            // btnChart
            // 
            this.btnChart.Image = global::LegendsViewer.Properties.Resources.chart16x16;
            this.btnChart.Location = new System.Drawing.Point(265, 3);
            this.btnChart.Name = "btnChart";
            this.btnChart.Size = new System.Drawing.Size(90, 24);
            this.btnChart.TabIndex = 37;
            this.btnChart.Text = "Charts";
            this.btnChart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnChart.UseVisualStyleBackColor = true;
            this.btnChart.Click += new System.EventHandler(this.BtnChart_Click);
            // 
            // btnMap
            // 
            this.btnMap.Image = global::LegendsViewer.Properties.Resources.map16x16;
            this.btnMap.Location = new System.Drawing.Point(159, 3);
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(100, 24);
            this.btnMap.TabIndex = 36;
            this.btnMap.Text = "World Map";
            this.btnMap.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMap.UseVisualStyleBackColor = true;
            this.btnMap.Click += new System.EventHandler(this.BtnMap_Click);
            // 
            // btnStats
            // 
            this.btnStats.Image = global::LegendsViewer.Properties.Resources.globe16x16;
            this.btnStats.Location = new System.Drawing.Point(63, 3);
            this.btnStats.Name = "btnStats";
            this.btnStats.Size = new System.Drawing.Size(90, 24);
            this.btnStats.TabIndex = 35;
            this.btnStats.Text = "Summary";
            this.btnStats.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStats.UseVisualStyleBackColor = true;
            this.btnStats.Click += new System.EventHandler(this.BtnStats_Click);
            // 
            // btnBack
            // 
            this.btnBack.Image = global::LegendsViewer.Properties.Resources.arrow_back16x16;
            this.btnBack.Location = new System.Drawing.Point(3, 3);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(24, 24);
            this.btnBack.TabIndex = 30;
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // btnForward
            // 
            this.btnForward.Image = global::LegendsViewer.Properties.Resources.arrow_forward16x16;
            this.btnForward.Location = new System.Drawing.Point(33, 3);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(24, 24);
            this.btnForward.TabIndex = 31;
            this.btnForward.UseVisualStyleBackColor = true;
            this.btnForward.Click += new System.EventHandler(this.BtnForward_Click);
            // 
            // FrmLegendsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.scWorld);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmLegendsViewer";
            this.Text = "Legends Viewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmLegendsViewer_FormClosed);
            this.Shown += new System.EventHandler(this.FrmLegendsViewer_Shown);
            this.ResizeEnd += new System.EventHandler(this.FrmLegendsViewer_ResizeEnd);
            this.scWorld.Panel1.ResumeLayout(false);
            this.scWorld.Panel2.ResumeLayout(false);
            this.scWorld.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scWorld)).EndInit();
            this.scWorld.ResumeLayout(false);
            this.tcWorld.ResumeLayout(false);
            this.tpSummary.ResumeLayout(false);
            this.tpHF.ResumeLayout(false);
            this.tpCivs.ResumeLayout(false);
            this.tpArt.ResumeLayout(false);
            this.tpInfrastructure.ResumeLayout(false);
            this.tpRegions.ResumeLayout(false);
            this.tpWarfare.ResumeLayout(false);
            this.tpCollections.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tcWorld;
        private TabPage tpInfrastructure;
        private TabPage tpRegions;
        private TabPage tpCivs;
        private OpenFileDialog dlgOpen;
        private Button btnBack;
        private Button btnForward;
        private TabPage tpHF;
        private ToolTip hint;
        private TabPage tpWarfare;
        private TabPage tpCollections;
        private Label lblVersion;
        private SplitContainer scWorld;
        private TabPage tpSummary;
        private SummaryTab summaryTab1;
        private Button btnStats;
        private Button btnChart;
        private Button btnMap;
        private TabPage tpArt;
        private HistoricalFiguresTab historicalFiguresTab1;
        private CivsTab civsTab1;
        private ArtAndCraftsTab artAndCraftsTab1;
        private GeographyTab geographyTab1;
        private WarfareTab warfareTab1;
        private CollectionsTab collectionsTab1;
        private InfrastructureTab infrastructureTab1;
    }
}

