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
    public partial class del : Form
    {
        public string PatientName;
        public string Tel;
        public string Birthday;
        public string DoctorName;
        public string Room;
        public string ChildBirth;
        public string CheckInDate;
        public string AdmissionID;
        public string PatientID;

        public del()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string sql = string.Format("delete from T_CWYY  where RYZ_ID='{0}' and HZID='{1}'", AdmissionID,PatientID);

            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Main form = (Main)this.Owner;
            form.simpleButton7_Click(null, null);
            this.Close();
        }

        private void del_Load(object sender, EventArgs e)
        {
            label6.Text = "姓名：  " + PatientName;
            label7.Text = "电话：  " + Tel;
            label4.Text = "生日：  " + Convert.ToDateTime(Birthday).ToString("yyyy-MM-dd");
            label3.Text = "医生：  " + DoctorName;
            label5.Text = "分娩：  " + ChildBirth;
            label1.Text = "房型：  " + Room;
            label2.Text = "预入院日期：  " + Convert.ToDateTime(CheckInDate).ToString("yyyy-MM-dd");
        }
    }
}
