using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCYY
{
    public partial class AdmissionInfo : Form
    {
        public string PatientID="";
        public string AdmissionID = "";//入院证
        public string Deposit = "";//押金
        public string FY_FE = "";
        public AdmissionInfo()
        {
            InitializeComponent();
        }

        private void AdmissionInfo_Load(object sender, EventArgs e)
        {
            UILocation();

            string sql = "";

            if (FY_FE == "1")//妇婴
            {
                sql = string.Format("select "
                + " HZXM,XB,CSRQ,DH,XZZ,ZJHM,HZNL," //基础信息
                + "HZBLH,ZYZD,MZKSMC,MZYSMC,ZYKSMC,ZCRQ,ZYYJ as YJFY,YJZYTS,RYSJ"
                + " from Y_RYZ where RYZ_ID='{0}'", AdmissionID);
            }
            else if (FY_FE == "2") //妇儿
            {
                sql = string.Format("select "
                + " HZXM,XB,CSRQ,DH,XZZ,ZJHM,HZNL," //基础信息
                + "HZBLH,ZYZD,MZKSMC,MZYSMC,ZYKSMC,ZCRQ,YJFY,YJZYTS,RYSJ"
                + " from Y_RYZ where RYZ_ID='{0}'", AdmissionID);
            }

            
            DataTable AdmissionInfo = new DataTable();

            try
            {
                AdmissionInfo = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message, "提示");
                return;
            }

            //1.基础信息
            textBoxName.Text = AdmissionInfo.Rows[0]["HZXM"].ToString();
            textBoxSex.Text = AdmissionInfo.Rows[0]["XB"].ToString();
            textBoxBirthday.Text = Convert.ToDateTime(AdmissionInfo.Rows[0]["CSRQ"].ToString()).ToString("yyyy-MM-dd");
            textBoxTel.Text = AdmissionInfo.Rows[0]["DH"].ToString();
            textBoxAddress.Text = AdmissionInfo.Rows[0]["XZZ"].ToString();
            textBoxAge.Text = AdmissionInfo.Rows[0]["HZNL"].ToString()+" 岁";
            textBoxID.Text= AdmissionInfo.Rows[0]["ZJHM"].ToString();

            //2.入院信息
            textBoxBLH.Text = AdmissionInfo.Rows[0]["HZBLH"].ToString();
            textBoxRYZ.Text = AdmissionID;
            textBoxZYZD.Text = AdmissionInfo.Rows[0]["ZYZD"].ToString();
            textBoxDeposit.Text= Deposit;
            textBoxCost.Text= AdmissionInfo.Rows[0]["YJFY"].ToString() + " 元" ;
            textBoxMZ.Text = AdmissionInfo.Rows[0]["MZKSMC"].ToString();
            textBoxDoctor.Text = AdmissionInfo.Rows[0]["MZYSMC"].ToString();
            textBoxTime.Text = Convert.ToDateTime(AdmissionInfo.Rows[0]["ZCRQ"].ToString()).ToString("yyyy-MM-dd HH:mm");
            textBoxDueDate.Text= AdmissionInfo.Rows[0]["RYSJ"].ToString();
            textBoxDays.Text= AdmissionInfo.Rows[0]["YJZYTS"].ToString();
            textBoxRYKS.Text = AdmissionInfo.Rows[0]["ZYKSMC"].ToString();
        }

        private void UILocation()
        {
            int x = 3;
            int y = 2;
            label12.Location = new Point(textBoxBLH.Location.X - label12.Width - x, textBoxBLH.Location.Y + y);
            label3.Location = new Point(textBoxRYZ.Location.X - label3.Width - x, textBoxRYZ.Location.Y + y);
            label4.Location = new Point(textBoxZYZD.Location.X - label4.Width - x, textBoxZYZD.Location.Y + y);
            label1.Location = new Point(textBoxDeposit.Location.X - label1.Width - x, textBoxDeposit.Location.Y + y);
            label2.Location = new Point(textBoxCost.Location.X - label2.Width - x, textBoxCost.Location.Y + y);
            label6.Location = new Point(textBoxDueDate.Location.X - label6.Width - x, textBoxDueDate.Location.Y + y);
            label7.Location = new Point(textBoxDays.Location.X - label7.Width - x, textBoxDays.Location.Y + y);
            label8.Location = new Point(textBoxMZ.Location.X - label8.Width - x, textBoxMZ.Location.Y + y);
            label9.Location = new Point(textBoxDoctor.Location.X - label9.Width - x, textBoxDoctor.Location.Y + y);
            label10.Location = new Point(textBoxRYKS.Location.X - label10.Width - x, textBoxRYKS.Location.Y + y);
            label11.Location = new Point(textBoxTime.Location.X - label11.Width - x, textBoxTime.Location.Y + y);

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

