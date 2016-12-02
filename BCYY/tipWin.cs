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
    public partial class tipWin : Form
    {
        DataTable PatientInfo = new DataTable();
        public tipWin()
        {
            InitializeComponent();
        }

        private void tipWin_Load(object sender, EventArgs e)
        {
            gridView1.BestFitColumns();
        }

        public void CloseTipWin()
        {
            this.Close();
        }

        public void BindSource(DataTable dt)
        {
            DataTable dt1 = dt.Copy();
            gridControl1.DataSource = dt1;

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            add_alter form = (add_alter)this.Owner;
           form.ReturnTipWin = true;

            //读取入院押金
            DataTable Deposit = new DataTable();
            try
            {
                string sql = string.Format("select SUM(JinE) from T_ZYYJ  where SFFS = '押金' and YJLX = '押金' and ZYID = '{0}' group by ZYID", gridView1.GetFocusedRowCellValue("ZYID").ToString());
                Deposit = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message);
                return;
            }


            //读取入院证信息
            try
            {
                string sql1 = String.Format("select RYZ_ID,MZKSMC,MZYSMC,ZCRQ,RYSJ from Y_RYZ "
                    + " where HZID = '{0}' and ZT='0' and ZYKSID='A0010001' order by ZCRQ desc", gridView1.GetFocusedRowCellValue("HZID").ToString());

                form.AdmissionInfo = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql1);

                if (form.AdmissionInfo.Rows.Count != 0)
                {                    
                    form.AdmissionID.Append(form.AdmissionInfo.Rows[0][0].ToString());//入院证ID
                    form.Deposit.Append(Deposit.Rows[0][0].ToString());//押金
                    form.AdmissionDep.Append(form.AdmissionInfo.Rows[0][1].ToString());//科室
                    form.AdmissionDoctor.Append(form.AdmissionInfo.Rows[0][2].ToString());//医生
                    form.AdmissionTime.Append(form.AdmissionInfo.Rows[0][3].ToString());//开立时间
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message);
                return;
            }

            //如果不存在入院证，则判定为外院患者
            if (form.AdmissionInfo.Rows.Count == 0)
            {
                try
                {
                    string sql2 = String.Format("select MZKSMC,MZYSMC,DJSJ,JHRYSJ as RYSJ from T_ZYDJ   where HZID = '{0}' and ZYZT='0'", gridView1.GetFocusedRowCellValue("HZID").ToString());
                    form.AdmissionInfo = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql2);
                    form.textBoxRYZ.Text = "东湖门诊";
                    form.AdmissionID.Append("东湖门诊");//入院证ID
                    form.Deposit.Append(Deposit.Rows[0][0].ToString());//押金
                    form.AdmissionDep.Append(form.AdmissionInfo.Rows[0][0].ToString());//科室
                    form.AdmissionDoctor.Append(form.AdmissionInfo.Rows[0][1].ToString());//医生
                    form.AdmissionTime.Append(form.AdmissionInfo.Rows[0][2].ToString());//开立时间
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误3:" + ex.Message);
                    return;
                }
            }
               
            //将信息写在主界面
            try
            {
                form.textBoxName.Text = gridView1.GetFocusedRowCellValue("HZXM").ToString();//放这里就可以避免被清空
                form.PatientName.Append(gridView1.GetFocusedRowCellValue("HZXM").ToString());//患者姓名
                form.PatientID.Append(gridView1.GetFocusedRowCellValue("HZID").ToString());//患者ID
                form.HospitalID.Append(gridView1.GetFocusedRowCellValue("ZYID").ToString());//住院ID                
                form.textBoxTel.Text = gridView1.GetFocusedRowCellValue("LXDH").ToString();
                form.textBoxBirthday.Text = Convert.ToDateTime(gridView1.GetFocusedRowCellValue("CSRQ").ToString()).ToString("yyyy-MM-dd");

                //右上角信息
                form.textBoxRYZ.Text = form.AdmissionID.ToString();
                form.textBoxDeposit.Text = form.Deposit.ToString() + " 元";
                form.textBoxDoctor.Text = form.AdmissionDoctor.ToString();
                form.textBoxTime.Text = Convert.ToDateTime(form.AdmissionTime.ToString()).ToString("yyyy-MM-dd  HH:mm");
               
                //下边信息
                if (form.AdmissionInfo.Rows[0]["RYSJ"].ToString()!="")
                {
                    form.dateEditCheckInDate.Text = Convert.ToDateTime(form.AdmissionInfo.Rows[0][4].ToString()).ToString("yyyy-MM-dd");
                }
                
                form.comboBoxDep.Text = form.AdmissionDep.ToString();
                form.comboBoxDoctor.Text = form.AdmissionDoctor.ToString();                            
                form.tipWinOpen = false;//主界面不要再显示tipWin
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误4:" + ex.Message);                
                return;
            }
            finally
            {
                this.Close();
            }
                      
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
