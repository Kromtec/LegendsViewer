using System.ComponentModel;
using System.Windows.Forms;

namespace LegendsViewer.Controls.Tabs
{
    partial class SummaryTab
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
            this.btnXMLPlus = new System.Windows.Forms.Button();
            this.txtXMLPlus = new System.Windows.Forms.TextBox();
            this.btnAdvancedSearch = new System.Windows.Forms.Button();
            this.btnMap = new System.Windows.Forms.Button();
            this.txtMap = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtSitePops = new System.Windows.Forms.TextBox();
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.btnSitePops = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnXML = new System.Windows.Forms.Button();
            this.txtXML = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.btnChart = new System.Windows.Forms.Button();
            this.btnStats = new System.Windows.Forms.Button();
            this.btnShowMap = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnXMLPlus
            // 
            this.btnXMLPlus.Location = new System.Drawing.Point(8, 110);
            this.btnXMLPlus.Name = "btnXMLPlus";
            this.btnXMLPlus.Size = new System.Drawing.Size(30, 23);
            this.btnXMLPlus.TabIndex = 37;
            this.btnXMLPlus.Text = "...";
            this.btnXMLPlus.UseVisualStyleBackColor = true;
            // 
            // txtXMLPlus
            // 
            this.txtXMLPlus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtXMLPlus.BackColor = System.Drawing.SystemColors.MenuBar;
            this.txtXMLPlus.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtXMLPlus.Location = new System.Drawing.Point(42, 111);
            this.txtXMLPlus.Name = "txtXMLPlus";
            this.txtXMLPlus.ReadOnly = true;
            this.txtXMLPlus.Size = new System.Drawing.Size(218, 20);
            this.txtXMLPlus.TabIndex = 36;
            this.txtXMLPlus.Text = "Legends Plus XML";
            // 
            // btnAdvancedSearch
            // 
            this.btnAdvancedSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdvancedSearch.Image = global::LegendsViewer.Properties.Resources.search16x16;
            this.btnAdvancedSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdvancedSearch.Location = new System.Drawing.Point(10, 488);
            this.btnAdvancedSearch.Name = "btnAdvancedSearch";
            this.btnAdvancedSearch.Size = new System.Drawing.Size(250, 23);
            this.btnAdvancedSearch.TabIndex = 35;
            this.btnAdvancedSearch.Text = "Advanced Search";
            this.btnAdvancedSearch.UseVisualStyleBackColor = true;
            this.btnAdvancedSearch.Click += new System.EventHandler(this.btnAdvancedSearch_Click);
            // 
            // btnMap
            // 
            this.btnMap.Location = new System.Drawing.Point(8, 86);
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(30, 23);
            this.btnMap.TabIndex = 31;
            this.btnMap.Text = "...";
            this.btnMap.UseVisualStyleBackColor = true;
            // 
            // txtMap
            // 
            this.txtMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMap.BackColor = System.Drawing.SystemColors.MenuBar;
            this.txtMap.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtMap.Location = new System.Drawing.Point(42, 87);
            this.txtMap.Name = "txtMap";
            this.txtMap.ReadOnly = true;
            this.txtMap.Size = new System.Drawing.Size(218, 20);
            this.txtMap.TabIndex = 30;
            this.txtMap.Text = "Map Image";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Green;
            this.lblStatus.Location = new System.Drawing.Point(7, 144);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(253, 20);
            this.lblStatus.TabIndex = 29;
            this.lblStatus.Text = "Select Files";
            // 
            // txtSitePops
            // 
            this.txtSitePops.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSitePops.BackColor = System.Drawing.SystemColors.MenuBar;
            this.txtSitePops.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtSitePops.Location = new System.Drawing.Point(42, 63);
            this.txtSitePops.Name = "txtSitePops";
            this.txtSitePops.ReadOnly = true;
            this.txtSitePops.Size = new System.Drawing.Size(218, 20);
            this.txtSitePops.TabIndex = 28;
            this.txtSitePops.Text = "Sites and Populations Text";
            // 
            // txtHistory
            // 
            this.txtHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHistory.BackColor = System.Drawing.SystemColors.MenuBar;
            this.txtHistory.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtHistory.Location = new System.Drawing.Point(42, 40);
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.ReadOnly = true;
            this.txtHistory.Size = new System.Drawing.Size(218, 20);
            this.txtHistory.TabIndex = 27;
            this.txtHistory.Text = "World History Text";
            // 
            // btnSitePops
            // 
            this.btnSitePops.Location = new System.Drawing.Point(8, 62);
            this.btnSitePops.Name = "btnSitePops";
            this.btnSitePops.Size = new System.Drawing.Size(30, 23);
            this.btnSitePops.TabIndex = 26;
            this.btnSitePops.Text = "...";
            this.btnSitePops.UseVisualStyleBackColor = true;
            // 
            // btnHistory
            // 
            this.btnHistory.Location = new System.Drawing.Point(8, 38);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(30, 23);
            this.btnHistory.TabIndex = 25;
            this.btnHistory.Text = "...";
            this.btnHistory.UseVisualStyleBackColor = true;
            // 
            // btnXML
            // 
            this.btnXML.Location = new System.Drawing.Point(8, 14);
            this.btnXML.Name = "btnXML";
            this.btnXML.Size = new System.Drawing.Size(30, 23);
            this.btnXML.TabIndex = 24;
            this.btnXML.Text = "...";
            this.btnXML.UseVisualStyleBackColor = true;
            // 
            // txtXML
            // 
            this.txtXML.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtXML.BackColor = System.Drawing.SystemColors.MenuBar;
            this.txtXML.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtXML.Location = new System.Drawing.Point(42, 16);
            this.txtXML.Name = "txtXML";
            this.txtXML.ReadOnly = true;
            this.txtXML.Size = new System.Drawing.Size(218, 20);
            this.txtXML.TabIndex = 23;
            this.txtXML.Text = "Legends XML / Archive";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(8, 167);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(252, 284);
            this.txtLog.TabIndex = 22;
            this.txtLog.Text = "";
            this.txtLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLog_KeyDown);
            // 
            // btnChart
            // 
            this.btnChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChart.AutoSize = true;
            this.btnChart.Image = global::LegendsViewer.Properties.Resources.chart16x16;
            this.btnChart.Location = new System.Drawing.Point(182, 457);
            this.btnChart.Name = "btnChart";
            this.btnChart.Size = new System.Drawing.Size(78, 25);
            this.btnChart.TabIndex = 34;
            this.btnChart.Text = "Charts";
            this.btnChart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnChart.UseVisualStyleBackColor = true;
            this.btnChart.Click += new System.EventHandler(this.btnChart_Click);
            // 
            // btnStats
            // 
            this.btnStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStats.AutoSize = true;
            this.btnStats.Image = global::LegendsViewer.Properties.Resources.globe16x16;
            this.btnStats.Location = new System.Drawing.Point(10, 457);
            this.btnStats.Name = "btnStats";
            this.btnStats.Size = new System.Drawing.Size(76, 25);
            this.btnStats.TabIndex = 33;
            this.btnStats.Text = "Summary";
            this.btnStats.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStats.UseVisualStyleBackColor = true;
            this.btnStats.Click += new System.EventHandler(this.btnStats_Click);
            // 
            // btnShowMap
            // 
            this.btnShowMap.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnShowMap.AutoSize = true;
            this.btnShowMap.Image = global::LegendsViewer.Properties.Resources.map16x16;
            this.btnShowMap.Location = new System.Drawing.Point(91, 457);
            this.btnShowMap.Name = "btnShowMap";
            this.btnShowMap.Size = new System.Drawing.Size(85, 25);
            this.btnShowMap.TabIndex = 32;
            this.btnShowMap.Text = "World Map";
            this.btnShowMap.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnShowMap.UseVisualStyleBackColor = true;
            this.btnShowMap.Click += new System.EventHandler(this.btnShowMap_Click);
            // 
            // SummaryTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnXMLPlus);
            this.Controls.Add(this.txtXMLPlus);
            this.Controls.Add(this.btnAdvancedSearch);
            this.Controls.Add(this.btnChart);
            this.Controls.Add(this.btnStats);
            this.Controls.Add(this.btnShowMap);
            this.Controls.Add(this.btnMap);
            this.Controls.Add(this.txtMap);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtSitePops);
            this.Controls.Add(this.txtHistory);
            this.Controls.Add(this.btnSitePops);
            this.Controls.Add(this.btnHistory);
            this.Controls.Add(this.btnXML);
            this.Controls.Add(this.txtXML);
            this.Controls.Add(this.txtLog);
            this.Name = "SummaryTab";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnXMLPlus;
        private TextBox txtXMLPlus;
        private Button btnAdvancedSearch;
        private Button btnChart;
        private Button btnStats;
        private Button btnShowMap;
        private Button btnMap;
        private TextBox txtMap;
        private Label lblStatus;
        private TextBox txtSitePops;
        private TextBox txtHistory;
        private Button btnSitePops;
        private Button btnHistory;
        private Button btnXML;
        private TextBox txtXML;
        private RichTextBox txtLog;
    }
}
