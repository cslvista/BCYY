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
    public partial class tipWinDoctor : Form
    {
        public tipWinDoctor()
        {
            InitializeComponent();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            Main form = (Main)this.Owner;
            form.ReturnTipWinDoctor = true;
            try
            {
                form.textBox3.Text = gridView1.GetFocusedRowCellValue("U_NAME").ToString();
            }
            catch
            {

            }
            form.tipWinDoctorOpen = false;
            this.Close();

        }

        public void CloseTipWinDoctor()
        {
            this.Close();
        }

        public void BindSource(DataTable dt)
        {
            DataTable dt1 = dt.Copy();
            gridControl1.DataSource = dt1;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Main form = (Main)this.Owner;
            form.tipWinDoctorOpen = false;
            this.Close();
        }
    }
}
