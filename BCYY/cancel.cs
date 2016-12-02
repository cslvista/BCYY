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
    public partial class cancel : Form
    {
        public string PatientName;
        public string Tel;
        public string Birthday;
        public string DoctorName;
        public string Room;
        public string ChildBirth;
        public string CheckInDate;
        public string AdmissionID;

        public cancel()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                MessageBox.Show("请填写取消原因");
                return;
            }

            string sql = string.Format("update T_CWYY set HZZT='1',YYBZ='{}' where RYZ_ID='{0}'", AdmissionID);

            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void cancel_Load(object sender, EventArgs e)
        {
            label6.Text = "姓名:  " + PatientName;
            label7.Text = "电话:  " + Tel;
            label4.Text = "生日:  " + Convert.ToDateTime(Birthday).ToString("yyyy-MM-dd");
            label3.Text = "医生:  " + DoctorName;
            label5.Text = "分娩:  " + ChildBirth;
            label1.Text = "房型:  " + Room;
            label2.Text = "预入院日期:  " + Convert.ToDateTime(CheckInDate).ToString("yyyy-MM-dd");
        }
    }
}
