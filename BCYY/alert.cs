using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace BCYY
{
    public partial class alert : Form
    {
        public delegate void UpdateUI();
        DataTable Reservation = new DataTable();
        public string FY_FE = "";
        public alert()
        {
            InitializeComponent();
        }

        private void alert_Load(object sender, EventArgs e)
        {
            if (FY_FE == "1")//妇婴
            {
                bandedGridView1.Appearance.Row.Font = new Font("宋体", 9);
                bandedGridView1.Appearance.BandPanel.Font = new Font("宋体", 9);
                bandedGridView1.Appearance.HeaderPanel.Font = new Font("宋体", 9);
                bandedGridView1.Appearance.FooterPanel.Font = new Font("宋体", 9);
            }
            else if (FY_FE == "2") //妇儿
            {
                bandedGridView1.Appearance.Row.Font = new Font("微软雅黑", 9);
                bandedGridView1.Appearance.BandPanel.Font = new Font("微软雅黑", 9);
                bandedGridView1.Appearance.HeaderPanel.Font = new Font("微软雅黑", 9);
                bandedGridView1.Appearance.FooterPanel.Font = new Font("微软雅黑", 9);
            }
        }

        public void Search()
        {
            string timeStart = Convert.ToDateTime(GlobalHelper.IDBHelper.GetServerDateTime()).AddDays(-8).ToString("yyyy-MM-dd");
            string timeStop  = Convert.ToDateTime(timeStart).AddDays(9).ToString("yyyy-MM-dd");

            string sql = string.Format(
                "select " //状态信息
                + "a.HZXM,a.CSRQ,a.DH,"//患者信息
                + "a.YRYRQ,a.YCQ,a.FM,a.FX,a.YYBZ"//入院信息 
                + " from T_CWYY a"
                + " inner join T_ZYDJ b on a.RYZ_ID=b.RYZ_ID" 
                + " where b.ZYZT='0'"  //未入院
                + " and a.YRYRQ between '{0} 00:00:00' and '{1} 23:59:59'", timeStart,timeStop    //8天内
                );

                try
                {
                    Reservation = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql.ToString());
                    gridControl1.DataSource = Reservation;
                    bandedGridView1.BestFitColumns();
            }
                catch (Exception ex)
                {
                    MessageBox.Show("错误:" + ex.Message, "提示");
                    return;
                }

                
        }
        
        private void bandedGridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "FX")
            {
                if (FY_FE == "1")//妇婴
                {
                    switch (e.Value.ToString())
                    {
                        case "0": e.DisplayText = "单人房"; break;
                        case "1": e.DisplayText = "标准房"; break;
                        case "2": e.DisplayText = "VIP套房"; break;
                        case "3": e.DisplayText = "LDR套房"; break;
                    }
                }else if (FY_FE == "2")//妇儿
                {
                    switch (e.Value.ToString())
                    {
                        case "0": e.DisplayText = "标准房"; break;
                        case "1": e.DisplayText = "VIP套房"; break;
                        case "2": e.DisplayText = "VIP豪华套房"; break;
                        case "3": e.DisplayText = "VIP豪华套房(LD2)"; break;
                    }
                }                    
                
            }

            if (e.Column.FieldName == "FM")
            {
                switch (e.Value.ToString())
                {
                    case "0": e.DisplayText = "顺产"; break;
                    case "1": e.DisplayText = "剖宫产"; break;
                }
            }
        }
    }
}
