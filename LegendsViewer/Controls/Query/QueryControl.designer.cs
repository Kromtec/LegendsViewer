using System.ComponentModel;
using System.Windows.Forms;

namespace LegendsViewer.Controls.Query
{
    partial class QueryControl
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SelectList = new System.Windows.Forms.ComboBox();
            this.dgResults = new System.Windows.Forms.DataGridView();
            this.lblResults = new System.Windows.Forms.Label();
            this.btnMapResults = new System.Windows.Forms.Button();
            this.OrderByPanel = new LegendsViewer.Controls.Query.CriteriaPanel();
            this.lblOrderCriteria = new System.Windows.Forms.Label();
            this.SearchPanel = new LegendsViewer.Controls.Query.CriteriaPanel();
            this.lblSearchCriteria = new System.Windows.Forms.Label();
            this.SelectionPanel = new LegendsViewer.Controls.Query.CriteriaPanel();
            this.lblSelectCriteria = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
            this.OrderByPanel.SuspendLayout();
            this.SearchPanel.SuspendLayout();
            this.SelectionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(9, 258);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Select";
            // 
            // SelectList
            // 
            this.SelectList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectList.FormattingEnabled = true;
            this.SelectList.Items.AddRange(new object[] {
            "Historical Figures",
            "Entities",
            "Sites",
            "Regions",
            "Underground Regions",
            "Structures",
            "Artifacts",
            "Written Content",
            "Wars",
            "Battles",
            "Conquerings",
            "Rampages",
            "Raids"});
            this.SelectList.Location = new System.Drawing.Point(3, 18);
            this.SelectList.Name = "SelectList";
            this.SelectList.Size = new System.Drawing.Size(134, 21);
            this.SelectList.TabIndex = 7;
            this.SelectList.SelectedIndexChanged += new System.EventHandler(this.SelectList_SelectedIndexChanged);
            // 
            // dgResults
            // 
            this.dgResults.AllowUserToAddRows = false;
            this.dgResults.AllowUserToDeleteRows = false;
            this.dgResults.AllowUserToOrderColumns = true;
            this.dgResults.AllowUserToResizeRows = false;
            this.dgResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgResults.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgResults.Location = new System.Drawing.Point(3, 287);
            this.dgResults.MinimumSize = new System.Drawing.Size(500, 200);
            this.dgResults.MultiSelect = false;
            this.dgResults.Name = "dgResults";
            this.dgResults.ReadOnly = true;
            this.dgResults.RowHeadersWidth = 65;
            this.dgResults.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgResults.Size = new System.Drawing.Size(600, 200);
            this.dgResults.TabIndex = 9;
            this.dgResults.VirtualMode = true;
            this.dgResults.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgResults_CellClick);
            this.dgResults.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgResults_CellFormatting);
            this.dgResults.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgResults_RowEnter);
            this.dgResults.MouseEnter += new System.EventHandler(this.dgResults_MouseEnter);
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Location = new System.Drawing.Point(90, 263);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(0, 13);
            this.lblResults.TabIndex = 11;
            // 
            // btnMapResults
            // 
            this.btnMapResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMapResults.Location = new System.Drawing.Point(517, 258);
            this.btnMapResults.Name = "btnMapResults";
            this.btnMapResults.Size = new System.Drawing.Size(75, 23);
            this.btnMapResults.TabIndex = 12;
            this.btnMapResults.Text = "Map Results";
            this.btnMapResults.UseVisualStyleBackColor = true;
            this.btnMapResults.Visible = false;
            this.btnMapResults.Click += new System.EventHandler(this.btnMapResults_Click);
            // 
            // OrderByPanel
            // 
            this.OrderByPanel.AutoSize = true;
            this.OrderByPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.OrderByPanel.Controls.Add(this.lblOrderCriteria);
            this.OrderByPanel.Location = new System.Drawing.Point(3, 182);
            this.OrderByPanel.Name = "OrderByPanel";
            this.OrderByPanel.Size = new System.Drawing.Size(74, 16);
            this.OrderByPanel.TabIndex = 4;
            // 
            // lblOrderCriteria
            // 
            this.lblOrderCriteria.AutoSize = true;
            this.lblOrderCriteria.Location = new System.Drawing.Point(3, 3);
            this.lblOrderCriteria.Name = "lblOrderCriteria";
            this.lblOrderCriteria.Size = new System.Drawing.Size(68, 13);
            this.lblOrderCriteria.TabIndex = 0;
            this.lblOrderCriteria.Text = "Order Criteria";
            // 
            // SearchPanel
            // 
            this.SearchPanel.AutoSize = true;
            this.SearchPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SearchPanel.Controls.Add(this.lblSearchCriteria);
            this.SearchPanel.Location = new System.Drawing.Point(3, 112);
            this.SearchPanel.Name = "SearchPanel";
            this.SearchPanel.Size = new System.Drawing.Size(82, 16);
            this.SearchPanel.TabIndex = 3;
            // 
            // lblSearchCriteria
            // 
            this.lblSearchCriteria.AutoSize = true;
            this.lblSearchCriteria.Location = new System.Drawing.Point(3, 3);
            this.lblSearchCriteria.Name = "lblSearchCriteria";
            this.lblSearchCriteria.Size = new System.Drawing.Size(76, 13);
            this.lblSearchCriteria.TabIndex = 0;
            this.lblSearchCriteria.Text = "Search Criteria";
            // 
            // SelectionPanel
            // 
            this.SelectionPanel.AutoSize = true;
            this.SelectionPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SelectionPanel.Controls.Add(this.lblSelectCriteria);
            this.SelectionPanel.Location = new System.Drawing.Point(3, 42);
            this.SelectionPanel.Name = "SelectionPanel";
            this.SelectionPanel.Size = new System.Drawing.Size(78, 16);
            this.SelectionPanel.TabIndex = 2;
            this.SelectionPanel.Visible = false;
            // 
            // lblSelectCriteria
            // 
            this.lblSelectCriteria.AutoSize = true;
            this.lblSelectCriteria.Location = new System.Drawing.Point(3, 3);
            this.lblSelectCriteria.Name = "lblSelectCriteria";
            this.lblSelectCriteria.Size = new System.Drawing.Size(72, 13);
            this.lblSelectCriteria.TabIndex = 4;
            this.lblSelectCriteria.Text = "Select Criteria";
            // 
            // QueryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.btnMapResults);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.dgResults);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelectList);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.OrderByPanel);
            this.Controls.Add(this.SearchPanel);
            this.Controls.Add(this.SelectionPanel);
            this.Name = "QueryControl";
            this.Size = new System.Drawing.Size(606, 493);
            this.Resize += new System.EventHandler(this.QueryControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
            this.OrderByPanel.ResumeLayout(false);
            this.OrderByPanel.PerformLayout();
            this.SearchPanel.ResumeLayout(false);
            this.SearchPanel.PerformLayout();
            this.SelectionPanel.ResumeLayout(false);
            this.SelectionPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public CriteriaPanel SelectionPanel;
        public CriteriaPanel SearchPanel;
        public CriteriaPanel OrderByPanel;
        private Label lblSearchCriteria;
        private Label lblOrderCriteria;
        private Label lblSelectCriteria;
        private Button btnSearch;
        private Label label1;
        private ComboBox SelectList;
        private DataGridView dgResults;
        private Label lblResults;
        private Button btnMapResults;

    }
}
