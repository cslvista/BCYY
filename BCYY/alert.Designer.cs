namespace BCYY
{
    partial class alert
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(alert));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.bandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.姓名 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.电话 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.生日 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.预入院日期 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.预产期 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.分娩方式 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.房型 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.备注 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand4 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gridControl1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(976, 579);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControl1.Location = new System.Drawing.Point(3, 2);
            this.gridControl1.MainView = this.bandedGridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(970, 575);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bandedGridView1});
            // 
            // bandedGridView1
            // 
            this.bandedGridView1.Appearance.BandPanel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridView1.Appearance.BandPanel.Options.UseFont = true;
            this.bandedGridView1.Appearance.FooterPanel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridView1.Appearance.FooterPanel.Options.UseFont = true;
            this.bandedGridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.bandedGridView1.Appearance.Row.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridView1.Appearance.Row.Options.UseFont = true;
            this.bandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand2,
            this.gridBand3,
            this.gridBand4});
            this.bandedGridView1.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.姓名,
            this.生日,
            this.电话,
            this.预入院日期,
            this.预产期,
            this.房型,
            this.分娩方式,
            this.备注});
            this.bandedGridView1.GridControl = this.gridControl1;
            this.bandedGridView1.IndicatorWidth = 10;
            this.bandedGridView1.Name = "bandedGridView1";
            this.bandedGridView1.OptionsBehavior.Editable = false;
            this.bandedGridView1.OptionsMenu.EnableFooterMenu = false;
            this.bandedGridView1.OptionsPrint.AutoWidth = false;
            this.bandedGridView1.OptionsView.ColumnAutoWidth = false;
            this.bandedGridView1.OptionsView.ShowFooter = true;
            this.bandedGridView1.OptionsView.ShowGroupPanel = false;
            this.bandedGridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.预入院日期, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.bandedGridView1.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.bandedGridView1_CustomColumnDisplayText);
            // 
            // gridBand2
            // 
            this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand2.Caption = "预约客户";
            this.gridBand2.Columns.Add(this.姓名);
            this.gridBand2.Columns.Add(this.电话);
            this.gridBand2.Columns.Add(this.生日);
            this.gridBand2.Image = ((System.Drawing.Image)(resources.GetObject("gridBand2.Image")));
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.VisibleIndex = 0;
            this.gridBand2.Width = 305;
            // 
            // 姓名
            // 
            this.姓名.Caption = "姓名";
            this.姓名.FieldName = "HZXM";
            this.姓名.Image = ((System.Drawing.Image)(resources.GetObject("姓名.Image")));
            this.姓名.Name = "姓名";
            this.姓名.Visible = true;
            this.姓名.Width = 80;
            // 
            // 电话
            // 
            this.电话.Caption = "电话";
            this.电话.FieldName = "DH";
            this.电话.Image = ((System.Drawing.Image)(resources.GetObject("电话.Image")));
            this.电话.Name = "电话";
            this.电话.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "DH", "总计：{0} ")});
            this.电话.Visible = true;
            this.电话.Width = 115;
            // 
            // 生日
            // 
            this.生日.Caption = "生日";
            this.生日.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.生日.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.生日.FieldName = "CSRQ";
            this.生日.Name = "生日";
            this.生日.Visible = true;
            this.生日.Width = 110;
            // 
            // gridBand3
            // 
            this.gridBand3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand3.Caption = "预约信息";
            this.gridBand3.Columns.Add(this.预入院日期);
            this.gridBand3.Columns.Add(this.预产期);
            this.gridBand3.Columns.Add(this.分娩方式);
            this.gridBand3.Columns.Add(this.房型);
            this.gridBand3.Columns.Add(this.备注);
            this.gridBand3.Image = ((System.Drawing.Image)(resources.GetObject("gridBand3.Image")));
            this.gridBand3.Name = "gridBand3";
            this.gridBand3.VisibleIndex = 1;
            this.gridBand3.Width = 630;
            // 
            // 预入院日期
            // 
            this.预入院日期.Caption = "预入院日期";
            this.预入院日期.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.预入院日期.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.预入院日期.FieldName = "YRYRQ";
            this.预入院日期.Image = ((System.Drawing.Image)(resources.GetObject("预入院日期.Image")));
            this.预入院日期.Name = "预入院日期";
            this.预入院日期.Visible = true;
            this.预入院日期.Width = 120;
            // 
            // 预产期
            // 
            this.预产期.Caption = "预产期";
            this.预产期.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.预产期.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.预产期.FieldName = "YCQ";
            this.预产期.Name = "预产期";
            this.预产期.Visible = true;
            this.预产期.Width = 110;
            // 
            // 分娩方式
            // 
            this.分娩方式.Caption = "分娩方式";
            this.分娩方式.FieldName = "FM";
            this.分娩方式.Name = "分娩方式";
            this.分娩方式.Visible = true;
            this.分娩方式.Width = 80;
            // 
            // 房型
            // 
            this.房型.Caption = "房型";
            this.房型.FieldName = "FX";
            this.房型.Name = "房型";
            this.房型.Visible = true;
            this.房型.Width = 130;
            // 
            // 备注
            // 
            this.备注.Caption = "备注";
            this.备注.FieldName = "YYBZ";
            this.备注.Name = "备注";
            this.备注.Visible = true;
            this.备注.Width = 190;
            // 
            // gridBand4
            // 
            this.gridBand4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand4.Caption = "登记";
            this.gridBand4.Image = ((System.Drawing.Image)(resources.GetObject("gridBand4.Image")));
            this.gridBand4.Name = "gridBand4";
            this.gridBand4.Visible = false;
            this.gridBand4.VisibleIndex = -1;
            this.gridBand4.Width = 375;
            // 
            // alert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 579);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "alert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "今明两日预入院客户";
            this.Load += new System.EventHandler(this.alert_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn 姓名;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn 电话;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn 生日;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn 预入院日期;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn 预产期;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn 分娩方式;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn 房型;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn 备注;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand4;
        public DevExpress.XtraGrid.GridControl gridControl1;
        public DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedGridView1;
    }
}