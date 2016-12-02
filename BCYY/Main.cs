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
using DevExpress.XtraPrinting;

namespace BCYY
{
    public partial class Main : Form
    {
        System.Data.DataTable Reservation = new System.Data.DataTable();
        public delegate void UpdateUI();
        StringBuilder date = new StringBuilder();
        StringBuilder sql = new StringBuilder();
        string FY_FE = "";//妇儿妇婴

        public bool ReturnTipWinDoctor = false;//用于提示医生姓名
        public bool tipWinDoctorOpen = false;
        bool isClosed = false;
        public bool BasicSearch_flag = false;
        bool authorization = false;

        public delegate void EventHandler(System.Data.DataTable dt);
        public event EventHandler ShowData;
        public event System.Action CloseTipWinDoctor;
        public Main()
        {
            InitializeComponent();
        }



        private void Main_Load(object sender, EventArgs e)
        {
            ButtonMoreSearch.Location = new System.Drawing.Point(toolStrip2.Location.X + toolStrip2.Width + 5, toolStrip2.Location.Y);
            simpleButton3.Visible =false;

            toolStripLabel1.Text = "用户：" + GlobalHelper.UserHelper.User["U_NAME"].ToString() + "       ";
            toolStripLabel2.Text = "科室：" + GlobalHelper.UserHelper.UT_TITLE + "       ";
            toolStripLabel3.Text = "时间：" + Convert.ToDateTime(GlobalHelper.IDBHelper.GetServerDateTime()).ToString("yyyy年MM月dd日    HH:mm");
            date.Append(Convert.ToDateTime(GlobalHelper.IDBHelper.GetServerDateTime()).ToString("yyyy-MM-dd"));
            tableLayoutPanel1.RowStyles[0].Height = spliter.Location.Y;
            toolStripComboBoxDJ.Items.Add("");
            toolStripComboBoxDJ.Items.Add("当日");
            toolStripComboBoxDJ.Items.Add("当月");
            BasicSearch_flag = true;
            toolStripComboBoxDJ.Text = "60";

            toolStripComboBoxZT.Items.Add("全部");
            toolStripComboBoxZT.Items.Add("未入院");
            toolStripComboBoxZT.Items.Add("已入院");
            toolStripComboBoxZT.Items.Add("取消入院");
            BasicSearch_flag = false;
            toolStripComboBoxZT.Text = "全部";

            comboBoxEdit1.Properties.Items.Add("");
            comboBoxEdit1.Properties.Items.Add("顺产");
            comboBoxEdit1.Properties.Items.Add("剖宫产");
            comboBoxEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

            comboBoxEdit3.Properties.Items.Add("全部");
            comboBoxEdit3.Properties.Items.Add("未入院");
            comboBoxEdit3.Properties.Items.Add("已入院");
            comboBoxEdit3.Properties.Items.Add("取消入院");
            comboBoxEdit3.Text = "全部";
            comboBoxEdit3.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

            dateEdit1.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit1.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit2.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit2.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit3.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit3.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit4.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit4.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit5.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit5.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit6.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit6.Properties.Mask.EditMask = "yyyy-MM-dd";

            //提醒ToolStripMenuItem_Click(null, null);

            if (GlobalHelper.GloValue.Logflg == "1")//妇婴
            {
                FY_FE = "1";
                bandedGridView1.Appearance.Row.Font = new Font("宋体", 9);
                bandedGridView1.Appearance.BandPanel.Font = new Font("宋体", 9);
                bandedGridView1.Appearance.HeaderPanel.Font = new Font("宋体", 9);
                bandedGridView1.Appearance.FooterPanel.Font = new Font("宋体", 9);
                comboBoxEdit2.Properties.Items.Add("");
                comboBoxEdit2.Properties.Items.Add("单人房");
                comboBoxEdit2.Properties.Items.Add("标准房");
                comboBoxEdit2.Properties.Items.Add("VIP套房");
                comboBoxEdit2.Properties.Items.Add("LDR套房");
                comboBoxEdit2.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            }
            else if (GlobalHelper.GloValue.Logflg == "2") //妇儿
            {
                FY_FE = "2";
                bandedGridView1.Appearance.Row.Font = new Font("微软雅黑", 9);
                bandedGridView1.Appearance.BandPanel.Font = new Font("微软雅黑", 9);
                bandedGridView1.Appearance.HeaderPanel.Font = new Font("微软雅黑", 9);
                bandedGridView1.Appearance.FooterPanel.Font = new Font("微软雅黑", 9);
                comboBoxEdit2.Properties.Items.Add("");
                comboBoxEdit2.Properties.Items.Add("标准房");
                comboBoxEdit2.Properties.Items.Add("VIP套房");
                comboBoxEdit2.Properties.Items.Add("VIP豪华套房");
                comboBoxEdit2.Properties.Items.Add("VIP豪华套房(LD2)");
                comboBoxEdit2.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            }


            //权限
            if (Authorization() == true)
            {
                authorization = true;
            } else
            {
                authorization = false;
            }

        }

        public void BasicSearch()
        {
            BasicSearch_flag = true;//用于防止线程多开
            StringBuilder sql = new StringBuilder();
            sql.Append(String.Format(
                "select b.ZYZT,b.RYSJ," //状态信息
                + "a.HZID,a.ZYID,a.HZXM,a.CSRQ,a.DH,"//患者信息
                + "a.RYZ_ID,a.YRYRQ,a.YCQ,a.YYKS,a.YYYS,c.ZYYJ,a.FM,a.FX,a.YYBZ,"//入院信息 
                + "a.DJSJ,a.DJRID,a.DJR,a.XGSJ,a.XGRID,a.XGR"  //登记信息
                + " from T_CWYY a"
                + " inner join T_ZYDJ b on a.ZYID=b.ZYID"
                + " left join (select ZYID,sum(JinE)as ZYYJ from T_ZYYJ group by ZYID) c on  c.ZYID=b.ZYID"
                + " where 1=1"
                ));

            this.BeginInvoke(new UpdateUI(delegate ()
            {
                if (toolStripTextBoxTel.Text.Length != 0)//电话
                {
                    sql.Append(string.Format(" and a.DH='{0}'", toolStripTextBoxTel.Text.Trim()));
                }

                if (toolStripTextBoxName.Text.Length != 0)//姓名
                {
                    sql.Append(string.Format(" and a.HZXM like '{0}%'", toolStripTextBoxName.Text.Trim()));
                }


                switch (toolStripComboBoxZT.Text)//状态
                {
                    case "全部": sql.Append(" and b.ZYZT>=0"); break;
                    case "未入院": sql.Append(" and b.ZYZT=0"); break;
                    case "已入院": sql.Append(" and b.ZYZT>=1 and b.ZYZT<6"); break;
                    case "取消入院": sql.Append(" and b.ZYZT=6"); break;
                }


                if (toolStripComboBoxDJ.Text.Length != 0)//登记时间
                {
                    if (toolStripComboBoxDJ.Text == "当日")
                    {
                        sql.Append(string.Format(" and a.DJSJ>'{0} 00:00:00'", date.ToString()));
                    }
                    else if (toolStripComboBoxDJ.Text == "当月")
                    {
                        sql.Append(string.Format(" and a.DJSJ>'{0} 00:00:00'", Convert.ToDateTime(date.ToString()).ToString("yyyy-MM-01")));
                    }
                    else
                    {
                        try
                        {
                            double date_num = Convert.ToDouble(toolStripComboBoxDJ.Text);
                            sql.Append(string.Format(" and a.DJSJ > '{0}'", Convert.ToDateTime(date.ToString()).AddDays(-date_num).ToString()));
                        }
                        catch
                        {
                            MessageBox.Show("登记日期输入错误！");
                            BasicSearch_flag = false;
                            return;
                        }
                    }
                }
                try
                {
                    Reservation = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql.ToString());
                    gridControl1.DataSource = Reservation;
                    if (FY_FE == "1")
                    {
                        bandedGridView1.BestFitColumns();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误1:" + ex.Message, "提示");
                    return;
                }
                finally
                {
                    BasicSearch_flag = false;
                }
            }));

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripLabel3.Text = "时间：" + Convert.ToDateTime(GlobalHelper.IDBHelper.GetServerDateTime()).ToString("yyyy年MM月dd日   HH:mm");

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (authorization == false)
            {
                MessageBox.Show("操作权限只授予产科与国际门诊的医护人员！");
                return;
            }

            try
            {
                if (bandedGridView1.GetFocusedRowCellValue("ZYZT").ToString() == "0")//未入院
                {
                    add_alter form = new add_alter();
                    form.alter = true;
                    form.ReadCard = true;//防止弹出窗口
                    form.FY_FE = FY_FE;
                    //左上
                    form.textBoxCard.Text = bandedGridView1.GetFocusedRowCellDisplayText("ZYZT").ToString();
                    form.textBoxName.Text = bandedGridView1.GetFocusedRowCellDisplayText("HZXM").ToString();
                    form.textBoxTel.Text = bandedGridView1.GetFocusedRowCellDisplayText("DH").ToString();
                    form.textBoxBirthday.Text = bandedGridView1.GetFocusedRowCellDisplayText("CSRQ").ToString();

                    form.AdmissionID.Append(bandedGridView1.GetFocusedRowCellDisplayText("RYZ_ID").ToString());
                    form.HospitalID.Append(bandedGridView1.GetFocusedRowCellDisplayText("ZYID").ToString());
                    form.PatientID.Append(bandedGridView1.GetFocusedRowCellDisplayText("HZID").ToString());
                    //右上
                    form.textBoxRYZ.Text = bandedGridView1.GetFocusedRowCellDisplayText("RYZ_ID").ToString();
                    form.Deposit.Append(bandedGridView1.GetFocusedRowCellValue("ZYYJ").ToString());
                    form.textBoxDeposit.Text = bandedGridView1.GetFocusedRowCellDisplayText("ZYYJ").ToString()+".00 元";
                    form.textBoxDoctor.Text = bandedGridView1.GetFocusedRowCellDisplayText("YYYS").ToString();
                    //下方
                    form.AdmissionDoctor.Append(bandedGridView1.GetFocusedRowCellDisplayText("YYYS").ToString());
                    form.ChildBirth= bandedGridView1.GetFocusedRowCellDisplayText("FM").ToString();
                    form.Room= bandedGridView1.GetFocusedRowCellDisplayText("FX").ToString();
                    form.dateEditCheckInDate.Text = bandedGridView1.GetFocusedRowCellDisplayText("YRYRQ").ToString();
                    form.dateEditDueDate.Text = bandedGridView1.GetFocusedRowCellDisplayText("YCQ").ToString();
                    form.textBoxNote.Text = bandedGridView1.GetFocusedRowCellDisplayText("YYBZ").ToString();
                    form.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show("该患者已经入院，信息不能变更！");
                }

            }
            catch
            {

            }

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.RowStyles[0].Height == spliter.Location.Y)
            {
                tableLayoutPanel1.RowStyles[0].Height = dateEdit5.Location.Y + dateEdit5.Height + 10;
            }
            else
            {
                tableLayoutPanel1.RowStyles[0].Height = spliter.Location.Y;
            }

        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (authorization == true)
            {
                add_alter form = new add_alter();
                form.FY_FE = FY_FE;
                form.Show(this);
                
            } else
            {
                MessageBox.Show("操作权限只授予产科与国际门诊的医护人员！");
            }

        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {
            toolStripTextBoxTel.SelectAll();
        }


        public bool Authorization()
        {
            if (FY_FE == "2")//妇儿
            {
                if (GlobalHelper.UserHelper.User["U_NAME"].ToString() == "MZSYS")
                {
                    删除ToolStripMenuItem.Visible = true;
                    return true;
                }
                else
                {
                    删除ToolStripMenuItem.Visible = false;
                }

                switch (GlobalHelper.UserHelper.UT_TITLE)
                {
                    case "产科": return true;
                    case "国际门诊": return true;
                    default: return false;
                }
            }
            else //妇婴
            {
                if (GlobalHelper.UserHelper.User["U_NAME"].ToString() == "门诊管理员")
                {
                    删除ToolStripMenuItem.Visible = true;
                }
                else
                {
                    删除ToolStripMenuItem.Visible = false;
                }
                return true;
            }
        }



        public void simpleButton7_Click(object sender, EventArgs e)
        {
            if (BasicSearch_flag == false)
            {
                Thread t1 = new Thread(BasicSearch);
                t1.IsBackground = true;
                t1.Start();
            }
        }


        private void simpleButton6_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            dateEdit1.Text = "";
            dateEdit2.Text = "";
            dateEdit3.Text = "";
            dateEdit4.Text = "";
            dateEdit5.Text = "";
            dateEdit6.Text = "";
            comboBoxEdit1.Text = "";
            comboBoxEdit2.Text = "";
            comboBoxEdit3.Text = "全部";
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            sql.Clear();
            sql.Append(String.Format(
                "select b.ZYZT,b.RYSJ," //状态信息
                + "a.HZID,a.ZYID,a.HZXM,a.CSRQ,a.DH,"//患者信息
                + "a.RYZ_ID,a.YRYRQ,a.YCQ,a.YYKS,a.YYYS,c.ZYYJ,a.FM,a.FX,a.YYBZ,"//入院信息 
                + "a.DJSJ,a.DJRID,a.DJR,a.XGSJ,a.XGRID,a.XGR"  //登记信息
                + " from T_CWYY a"
                + " inner join T_ZYDJ b on a.ZYID=b.ZYID"
                + " left join (select ZYID,sum(JinE)as ZYYJ from T_ZYYJ group by ZYID) c on  c.ZYID=b.ZYID"
                + " where 1=1"
                ));

            if (textBox1.Text.Length != 0)//姓名         
            {
                sql.Append(String.Format(" and a.HZXM like '{0}%'", textBox1.Text.Trim()));
            }

            if (textBox2.Text.Length != 0)//电话    
            {
                sql.Append(String.Format(" and a.DH='{0}'", textBox2.Text.Trim()));
            }

            if (textBox3.Text.Length != 0)//医生
            {
                sql.Append(String.Format(" and a.YYYS='{0}'", textBox3.Text.Trim()));
            }

            if (textBox4.Text.Length != 0)//备注         
            {
                sql.Append(String.Format(" and a.YYBZ like '%{0}%'", textBox4.Text.Trim()));
            }

            if (textBox5.Text.Length != 0)//登记人        
            {
                sql.Append(String.Format(" and a.DJR='{0}'", textBox5.Text.Trim()));
            }

            if (textBox6.Text.Length != 0)//修改人        
            {
                sql.Append(String.Format(" and a.XGR='{0}'", textBox6.Text.Trim()));
            }

            if (comboBoxEdit1.Text.Length != 0) //分娩
            {
                switch (comboBoxEdit1.Text)
                {
                    case "顺产": sql.Append(" and a.FM='0'"); ; break;
                    case "剖宫产": sql.Append(" and a.FM='1'"); ; break;
                }
            }

            if (comboBoxEdit2.Text.Length != 0) //房型
            {
                if (FY_FE == "1")
                {
                    switch (comboBoxEdit2.Text)
                    {
                        case "单人房": sql.Append(" and a.FX='0'"); break;
                        case "标准房": sql.Append(" and a.FX='1'"); break;
                        case "VIP套房": sql.Append(" and a.FX='2'"); break;
                        case "LDR套房": sql.Append(" and a.FX='3'"); break;
                    }
                }
                else
                {
                    switch (comboBoxEdit2.Text)
                    {
                        case "标准房": sql.Append(" and a.FX='0'"); break;
                        case "VIP套房": sql.Append(" and a.FX='1'"); break;
                        case "VIP豪华套房": sql.Append(" and a.FX='2'"); break;
                        case "VIP豪华套房(LD2)": sql.Append(" and a.FX='3'"); break;
                    }
                }               
            }

            if (comboBoxEdit3.Text.Length != 0) //状态
            {
                switch (comboBoxEdit3.Text)
                {
                    case "全部": sql.Append(" and b.ZYZT>=0"); break;
                    case "已入院": sql.Append(" and b.ZYZT>=1 and b.ZYZT<=5"); break;
                    case "未入院": sql.Append(" and b.ZYZT=0"); break;
                    case "取消入院": sql.Append(" and b.ZYZT=6"); break;
                }
            }

            if (dateEdit1.Text != "" || dateEdit2.Text != "") //预入住日期
            {
                if (dateEdit1.Text == "" && dateEdit2.Text != "")
                {
                    sql.Append(String.Format(" and a.YRYRQ<='{0} 23:59:59'", Convert.ToDateTime(dateEdit2.Text).ToString("yyyy-MM-dd")));
                }
                else if (dateEdit1.Text != "" && dateEdit2.Text == "")
                {
                    sql.Append(String.Format(" and a.YRYRQ>='{0} 00:00:00'", Convert.ToDateTime(dateEdit1.Text).ToString("yyyy-MM-dd")));
                }
                else if (dateEdit1.Text != "" && dateEdit2.Text != "")
                {
                    sql.Append(String.Format(" and a.YRYRQ between  '{0} 00:00:00' and '{1} 23:59:59'", Convert.ToDateTime(dateEdit1.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(dateEdit2.Text).ToString("yyyy-MM-dd")));
                }

            }

            if (dateEdit3.Text != "" || dateEdit4.Text != "")  //登记日期
            {
                if (dateEdit3.Text == "" && dateEdit4.Text != "")
                {
                    sql.Append(String.Format(" and a.DJSJ<='{0} 23:59:59'", Convert.ToDateTime(dateEdit4.Text).ToString("yyyy-MM-dd")));
                }
                else if (dateEdit3.Text != "" && dateEdit4.Text == "")
                {
                    sql.Append(String.Format(" and a.DJSJ>='{0} 00:00:00'", Convert.ToDateTime(dateEdit3.Text).ToString("yyyy-MM-dd")));
                }
                else if (dateEdit3.Text != "" && dateEdit4.Text != "")
                {
                    sql.Append(String.Format(" and a.DJSJ between '{0} 00:00:00' and '{1} 23:59:59'", Convert.ToDateTime(dateEdit3.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(dateEdit4.Text).ToString("yyyy-MM-dd")));
                }
            }

            if (dateEdit5.Text != "" || dateEdit6.Text != "")  //入院日期
            {
                if (dateEdit5.Text == "" && dateEdit6.Text != "")
                {
                    sql.Append(String.Format(" and b.RYSJ<='{0} 23:59:59'", Convert.ToDateTime(dateEdit6.Text).ToString("yyyy-MM-dd")));
                }
                else if (dateEdit5.Text != "" && dateEdit6.Text == "")
                {
                    sql.Append(String.Format(" and b.RYSJ>='{0} 00:00:00'", Convert.ToDateTime(dateEdit5.Text).ToString("yyyy-MM-dd")));
                }
                else if (dateEdit5.Text != "" && dateEdit6.Text != "")
                {
                    sql.Append(String.Format(" and b.RYSJ between '{0} 00:00:00' and '{1} 23:59:59'", Convert.ToDateTime(dateEdit5.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(dateEdit6.Text).ToString("yyyy-MM-dd")));
                }
            }

            BasicSearch_flag = true;//防止触发线程
            toolStripComboBoxZT.Text = comboBoxEdit3.Text;//主搜索条的状态和次搜索面板的状态一致
            BasicSearch_flag = false;

            Thread t1 = new Thread(MoreSearch);//将搜索结果显示到表格上
            t1.IsBackground = true;
            t1.Start();
        }

        public void MoreSearch()
        {
            this.BeginInvoke(new UpdateUI(delegate ()
            {
                try
                {
                    Reservation = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql.ToString());
                    gridControl1.DataSource = Reservation;
                    if (FY_FE == "1")
                    {
                        bandedGridView1.BestFitColumns();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误:" + ex.Message, "提示");
                    return;
                }
            }));
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            MenuItem查看入院证_Click(null, null);
        }


        private void toolStripComboBoxDJ_TextChanged(object sender, EventArgs e)
        {
            simpleButton7_Click(null, null);

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton5_Click(null, null);
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton5_Click(null, null);
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton5_Click(null, null);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetDataObject(bandedGridView1.GetFocusedValue());
            }
            catch
            {

            }

        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton5_Click(null, null);
            }
        }


        private void toolStripTextBoxName_Click(object sender, EventArgs e)
        {
            toolStripTextBoxName.SelectAll();
        }

        private void toolStripTextBoxTel_Click(object sender, EventArgs e)
        {
            toolStripTextBoxTel.SelectAll();
        }

        private void toolStripTextBoxTel_KeyDown(object sender, KeyEventArgs e)
        {
            LocalSearch();
        }

        private void toolStripTextBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            LocalSearch();
        }

        private void toolStripComboBoxZT_SelectedIndexChanged(object sender, EventArgs e)
        {
            simpleButton7_Click(null, null);
        }

        private void 提醒ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            alert form = new alert();
            form.FY_FE = FY_FE;
            form.Search();
            form.Width = form.bandedGridView1.Columns["HZXM"].Width + form.bandedGridView1.Columns["DH"].Width + form.bandedGridView1.Columns["CSRQ"].Width
                 + form.bandedGridView1.Columns["YRYRQ"].Width + form.bandedGridView1.Columns["YCQ"].Width
                 + form.bandedGridView1.Columns["FM"].Width + form.bandedGridView1.Columns["FX"].Width + form.bandedGridView1.Columns["YYBZ"].Width
                 + 43;
            form.Show();

        }


        private void MenuItem查看入院证_Click(object sender, EventArgs e)
        {
            try
            {
                if (bandedGridView1.GetFocusedRowCellDisplayText("RYZ_ID").ToString()=="东湖门诊")
                {
                    MessageBox.Show("东湖门诊患者暂无法查看入院证信息！");
                }else
                {
                    AdmissionInfo form = new AdmissionInfo();
                    form.Deposit = bandedGridView1.GetFocusedRowCellDisplayText("ZYYJ").ToString() + ".00 元";
                    form.AdmissionID = bandedGridView1.GetFocusedRowCellValue("RYZ_ID").ToString();
                    form.FY_FE = FY_FE;
                    form.Show();
                }
                
            }
            catch
            {

            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

            if (ReturnTipWinDoctor == true)//单击tipWin关闭后不显示界面
            {
                ReturnTipWinDoctor = false;
                return;
            }

            if (textBox3.Text.Length == 0 && tipWinDoctorOpen == true)
            {
                CloseTipWinDoctor();
                tipWinDoctorOpen = false;
            }
            else if (textBox3.Text.Length > 0)
            {
                Thread t1 = new Thread(StartTipWinDoctor);//将用户信息显示到表格上
                t1.IsBackground = true;
                t1.Start();
            }

        }

        public void StartTipWinDoctor()
        {
            //1.判断输入的第一个字是中文还是英文
            StringBuilder sql = new StringBuilder();
            if (IsLetter(textBox3.Text) == true)//如果是英文

            {
                sql.Append(String.Format("select U_NAME,U_PYM_OLD from USERS where U_PYM_OLD like '{0}%' ", textBox3.Text));
            }
            else//如果是中文
            {
                sql.Append(String.Format("select U_NAME,U_PYM_OLD from USERS where U_NAME like '{0}%'", textBox3.Text));
            }

            System.Data.DataTable DoctorName = new System.Data.DataTable();
            DoctorName.Columns.Add("U_NAME", typeof(string));
            DoctorName.Columns.Add("U_PYM_OLD", typeof(string));

            try
            {
                DoctorName = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.USER, sql.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message, "提示");
                return;
            }

            //2.打开窗体
            if (tipWinDoctorOpen == false)
            {
                this.Invoke(new UpdateUI(delegate ()
                {
                    tipWinDoctor form = new tipWinDoctor();
                    tipWinDoctorOpen = true;
                    this.ShowData += form.BindSource;
                    this.CloseTipWinDoctor += form.CloseTipWinDoctor;
                    form.Show(this);
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = new System.Drawing.Point(textBox3.PointToScreen(new System.Drawing.Point(0, 0)).X, textBox3.PointToScreen(new System.Drawing.Point(0, 0)).Y + textBox3.Height);
                    ShowData(DoctorName);
                    textBox3.Focus();
                }));
            }
            else
            {
                this.Invoke(new UpdateUI(delegate ()
                {
                    ShowData(DoctorName);
                    textBox3.Focus();
                }));
            }

            isClosed = true;
        }

        public bool IsLetter(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z]+$");
            return reg1.IsMatch(str.Substring(0, 1));
        }

        private void bandedGridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "ZYZT")
            {
                string ZYZT = bandedGridView1.GetRowCellDisplayText(e.RowHandle, bandedGridView1.Columns["ZYZT"]);

                if (ZYZT == "已入院")
                {
                    e.Appearance.ForeColor = Color.Blue;
                }
                else if (ZYZT == "取消入院")
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }
        }

        private void bandedGridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            int hand = e.RowHandle;
            if (hand < 0) return;
            DataRow dr = this.bandedGridView1.GetDataRow(hand);

            if (dr[0].ToString() == "6")
            {
                e.Appearance.FontStyleDelta = FontStyle.Strikeout;
                e.Appearance.ForeColor = Color.Gray;
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
                }
                else if (FY_FE == "2")//妇儿
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

                if (e.Column.FieldName == "ZYZT")
                {
                    switch (e.Value.ToString())
                    {
                        case "0": e.DisplayText = "未入院"; break;
                        case "6": e.DisplayText = "取消入院"; break;
                        default: e.DisplayText = "已入院"; break;
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

        private void MenuItem变更_Click(object sender, EventArgs e)
        {
            simpleButton2_Click(null, null);
        }

        private void simpleButton3_Click_2(object sender, EventArgs e)
        {
            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "Excel文件(*.xlsx)|*.xlsx|所有文件(*.*)|*.*";
            //sfd.FilterIndex = 1;
            //sfd.RestoreDirectory = true;
            //if (sfd.ShowDialog() == DialogResult.OK)
            //{
            //    string path = sfd.FileName.ToString();
            //    try
            //    {
            //        XlsxExportOptions options = new XlsxExportOptions();
            //        options.ExportMode = XlsxExportMode.SingleFile;
            //        options.TextExportMode = TextExportMode.Text;
            //        options.RawDataMode = false;
            //        options.SheetName = "abc";
            //        gridControl1.ExportToXlsx(path, options);
            //        MessageBox.Show("导出成功！");
            //    }
            //    catch
            //    {
            //        MessageBox.Show("导出失败！");
            //    }

            //}


            string sql1 = "select a.HZID,b.ZYID from T_CWYY a inner join T_ZYDJ b on a.HZID=b.HZID where b.RYKBID='A0010001'";

            DataTable ZYID = new DataTable();

            try
            {
                ZYID = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

            for (int i = 0; i < ZYID.Rows.Count; i++)
            {
                string sql2 = string.Format("update T_CWYY set ZYID='{0}' where HZID='{1}'", ZYID.Rows[i][1], ZYID.Rows[i][0]);

                try
                {
                    GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql2);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误2:" + ex.Message, "提示");
                    return;
                }
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            del form = new del();
            form.Show(this);
        }

        private void simpleButton4_Click_2(object sender, EventArgs e)
        {
            gridControl1.ShowRibbonPrintPreview();
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton5_Click(null, null);
            }
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton5_Click(null, null);
            }
        }

        private void toolStripTextBoxTel_TextChanged(object sender, EventArgs e)
        {
            LocalSearch();//本地数据搜索           
        }

        private void LocalSearch()
        {         
            Reservation.DefaultView.RowFilter = string.Format("HZXM like '{0}%' and DH like '{1}%'", toolStripTextBoxName.Text.Trim(), toolStripTextBoxTel.Text.Trim());
        }


        private void toolStripTextBoxName_TextChanged(object sender, EventArgs e)
        {            
            LocalSearch();//本地数据搜索
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "电子表格(*.xls)|*.xls";
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DevExpress.XtraExport.IExportProvider provider = new DevExpress.XtraExport.ExportXlsProvider(sf.FileName);
                DevExpress.XtraGrid.Export.BaseExportLink link = bandedGridView1.CreateExportLink(provider);
                link.ExportCellsAsDisplayText = true;
                try { link.ExportTo(true); }
                catch { }
                if (MessageBox.Show("是否打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                    return;
                try
                {
                    System.Diagnostics.Process.Start(sf.FileName);
                }
                catch { }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                del form = new del();
                form.PatientName = bandedGridView1.GetFocusedRowCellValue("HZXM").ToString();
                form.Tel = bandedGridView1.GetFocusedRowCellValue("DH").ToString();
                form.Birthday = bandedGridView1.GetFocusedRowCellValue("CSRQ").ToString();
                form.Room = bandedGridView1.GetFocusedRowCellDisplayText("FX").ToString();
                form.DoctorName= bandedGridView1.GetFocusedRowCellValue("YYYS").ToString();
                form.ChildBirth = bandedGridView1.GetFocusedRowCellDisplayText("FM").ToString();
                form.CheckInDate = bandedGridView1.GetFocusedRowCellValue("YRYRQ").ToString();
                form.AdmissionID = bandedGridView1.GetFocusedRowCellValue("RYZ_ID").ToString();
                form.PatientID = bandedGridView1.GetFocusedRowCellValue("HZID").ToString();
                form.ShowDialog(this);
            }
            catch
            {

            }            
        }
    }
}
