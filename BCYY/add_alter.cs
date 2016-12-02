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
    public partial class add_alter : Form
    {

        public bool ReturnTipWin = false;
        public bool tipWinOpen = false;
        public bool alter = false;
        public bool ReadCard = false;
        public bool comboboxdep_flag = false;//用于防止初始化时连接到数据库
        bool isClosed = true;

        public delegate void UpdateUI();
        public delegate void EventHandler(DataTable dt);
        public event EventHandler ShowData;
        public event Action CloseTipWin;

        //患者基本信息
        public StringBuilder PatientName = new StringBuilder();
        public StringBuilder PatientID = new StringBuilder();
        public StringBuilder HospitalID = new StringBuilder();
        public StringBuilder Tel = new StringBuilder();
        public StringBuilder Birthday = new StringBuilder();

        //入院证信息
        public StringBuilder Deposit = new StringBuilder();//押金
        public StringBuilder AdmissionID = new StringBuilder();//入院证 
        public StringBuilder AdmissionDep = new StringBuilder();//开立科室
        public StringBuilder AdmissionDoctor = new StringBuilder();//开立医生
        public StringBuilder AdmissionTime = new StringBuilder();//开立时间 

        public string ChildBirth = "";//分娩方式
        public string Room = "";//房型
        public string FY_FE = "";

        StringBuilder UT_ID = new StringBuilder();//科室ID
        StringBuilder DoctorID = new StringBuilder();

        DataTable PatientInfo = new DataTable();
        DataTable DoctorName = new DataTable();
        DataTable deposit = new DataTable();
        public DataTable AdmissionInfo = new DataTable();
        public add_alter()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UILocation();

            dateEditCheckInDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEditCheckInDate.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEditDueDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEditDueDate.Properties.Mask.EditMask = "yyyy-MM-dd";

            comboboxdep_flag = true;//防止初始化时读取数据库
            if (FY_FE == "1")//妇婴
            {
                comboBoxRoom.DataSource = BCYY.common.CommonData.Room_FY();
                comboBoxDep.DataSource = BCYY.common.CommonData.Dep_FY();
            }
            else if (FY_FE == "2") //妇儿
            {
                comboBoxRoom.DataSource = BCYY.common.CommonData.Room_FE();
                comboBoxDep.DataSource = BCYY.common.CommonData.Dep_FE();
            }

            comboBoxRoom.DisplayMember = "Room";
            comboBoxRoom.ValueMember = "Room_ID";
            comboBoxDep.DisplayMember = "Dep";
            comboBoxDep.ValueMember = "Dep_ID";
            comboboxdep_flag = false;

            comboBoxChildBirth.Items.Add("顺产");
            comboBoxChildBirth.Items.Add("剖宫产");

            label14.Text = "";
            label15.Text = "";
            labelBirth.Location = new Point(textBoxTel.Location.X, label2.Location.Y);

            if (alter == true)
            {
                this.Text = "变更";
                textBoxCard.ReadOnly = true;
                simpleButton3.Visible = false;
                textBoxName.ReadOnly = true;
                simpleButton1.Text = "变更";
                comboBoxChildBirth.Text = ChildBirth;
                comboBoxRoom.Text = Room;
                comboBoxDoctor.Text = AdmissionDoctor.ToString();
                label4.Text = "状态：";

                if (FY_FE == "1")
                {
                    ReadRoom_FY();
                }
                else if (FY_FE == "2")
                {
                    ReadRoom_FE();
                }
                //读取入院证的附加信息
                ReadAddtionalInfo();
            }
            else
            {
                this.Text = "新增";
                comboBoxDep.Text = null;
                comboBoxRoom.Text = null;
                ButtonDetail.Enabled = false;
                textBoxName.Focus();
            }


        }

        private void UILocation()
        {
            int x = 5;
            int y = 2;
            label7.Location = new Point(comboBoxDep.Location.X - label7.Width - x, comboBoxDep.Location.Y + y);
            label5.Location = new Point(comboBoxDoctor.Location.X - label5.Width - x, comboBoxDoctor.Location.Y + y);
            label11.Location = new Point(comboBoxChildBirth.Location.X - label11.Width - x, comboBoxChildBirth.Location.Y + y);
            label8.Location = new Point(comboBoxRoom.Location.X - label8.Width - x, comboBoxRoom.Location.Y + y);
            label12.Location = new Point(dateEditCheckInDate.Location.X - label12.Width - x, dateEditCheckInDate.Location.Y + y);
            label6.Location = new Point(dateEditDueDate.Location.X - label6.Width - x, dateEditDueDate.Location.Y + y);
            label10.Location = new Point(textBoxNote.Location.X - label10.Width - x, textBoxNote.Location.Y + y);
        }

        private void ReadRoom_FE()
        {
            if (dateEditCheckInDate.Text == "")
            {
                return;
            }

            string CheckInDate = Convert.ToDateTime(dateEditCheckInDate.Text).ToString("yyyy-MM-dd");

            string sql =
             "select COUNT(*) from M_BINGC where KSID='A0010001' and BCZTID<2"
            + " union all "
            + "select COUNT(*) from M_BINGC where KSID='A0010001' and CW_MC='标准房' and BCZTID<2"
            + " union all "
            + "select COUNT(*) from M_BINGC where KSID='A0010001' and CW_MC='VIP套房' and BCZTID<2"
            + " union all "
            + "select COUNT(*) from M_BINGC where KSID = 'A0010001' and CW_MC = 'VIP豪华套房' and BCZTID<2"
            + " union all "
            + string.Format("select COUNT(*) from T_CWYY a inner  join T_ZYDJ b on a.RYZ_ID=b.RYZ_ID where a.YRYRQ='{0}' and b.ZYZT='0'", CheckInDate)
            + " union all "
            + string.Format("select COUNT(*) from T_CWYY a inner  join T_ZYDJ b on a.RYZ_ID=b.RYZ_ID where a.YRYRQ='{0}' and b.ZYZT='0' and a.FX = '0'", CheckInDate)
            + " union all "
            + string.Format("select COUNT(*) from T_CWYY a inner  join T_ZYDJ b on a.RYZ_ID=b.RYZ_ID where a.YRYRQ='{0}' and b.ZYZT='0' and a.FX = '1'", CheckInDate)
            + " union all "
            + string.Format("select COUNT(*) from T_CWYY a inner  join T_ZYDJ b on a.RYZ_ID=b.RYZ_ID where a.YRYRQ='{0}' and b.ZYZT='0' and (a.FX = '2' or a.FX = '3')", CheckInDate);

            DataTable RoomCount = new DataTable();

            try
            {
                RoomCount = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message, "提示");
                return;
            }

            label14.Text = "预入院日期: " + dateEditCheckInDate.Text + "           " + "妇产科总预约: " + RoomCount.Rows[4][0];
            label15.Text = "标准房: " + RoomCount.Rows[1][0] + string.Format("（预约{0}）", RoomCount.Rows[5][0]) + "           "
                         + "VIP套房: " + RoomCount.Rows[2][0] + string.Format("（预约{0}）", RoomCount.Rows[6][0]) + "          "
                         + "VIP豪华套房: " + RoomCount.Rows[3][0] + string.Format("（预约{0}）", RoomCount.Rows[7][0]);

        }

        private void ReadRoom_FY()
        {
            if (dateEditCheckInDate.Text == "")
            {
                return;
            }

            string CheckInDate = Convert.ToDateTime(dateEditCheckInDate.Text).ToString("yyyy-MM-dd");

            string sql =
              "select COUNT(*) from M_BINGC where KSID='A0010001' and CW_MC='雅致单人房' and BCZTID<2"
            + " union all "
            + "select COUNT(*) from M_BINGC where KSID='A0010001' and CW_MC='温馨标准房' and BCZTID<2"
            + " union all "
            + "select COUNT(*) from M_BINGC where KSID='A0010001' and CW_MC='VIP套房' and BCZTID<2"
            + " union all "
            + "select COUNT(*) from M_BINGC where KSID = 'A0010001' and CW_MC = 'LDR家庭式套房' and BCZTID<2"
            + " union all "
            + string.Format("select COUNT(*) from T_CWYY a inner  join T_ZYDJ b on a.RYZ_ID=b.RYZ_ID where a.YRYRQ='{0}' and b.ZYZT='0'", CheckInDate)
            + " union all "
            + string.Format("select COUNT(*) from T_CWYY a inner  join T_ZYDJ b on a.RYZ_ID=b.RYZ_ID where a.YRYRQ='{0}' and b.ZYZT='0' and a.FX = '0'", CheckInDate)
            + " union all "
            + string.Format("select COUNT(*) from T_CWYY a inner  join T_ZYDJ b on a.RYZ_ID=b.RYZ_ID where a.YRYRQ='{0}' and b.ZYZT='0' and a.FX = '1'", CheckInDate)
            + " union all "
            + string.Format("select COUNT(*) from T_CWYY a inner  join T_ZYDJ b on a.RYZ_ID=b.RYZ_ID where a.YRYRQ='{0}' and b.ZYZT='0' and (a.FX = '2')", CheckInDate)
            + " union all "
            + string.Format("select COUNT(*) from T_CWYY a inner  join T_ZYDJ b on a.RYZ_ID=b.RYZ_ID where a.YRYRQ='{0}' and b.ZYZT='0' and (a.FX = '3')", CheckInDate);

            DataTable RoomCount = new DataTable();

            try
            {
                RoomCount = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message, "提示");
                return;
            }

            label14.Text = "预入院日期: " + dateEditCheckInDate.Text + "           " + string.Format("妇产科床位总预约：{0}", RoomCount.Rows[4][0]);
            label15.Text = "单人房: " + RoomCount.Rows[0][0] + string.Format("（预{0}）", RoomCount.Rows[5][0]) + "     "
                         + "标准房: " + RoomCount.Rows[1][0] + string.Format("（预{0}）", RoomCount.Rows[6][0]) + "    "
                         + "VIP套房: " + RoomCount.Rows[2][0] + string.Format("（预{0}）", RoomCount.Rows[7][0]) + "    "
                         + "LDR套房: " + RoomCount.Rows[3][0] + string.Format("（预{0}）", RoomCount.Rows[8][0]);

        }
        private void textBoxName_TextChanged(object sender, EventArgs e)
        {

            if (ReadCard == true)//读卡后不显示界面
            {
                ReadCard = false;
                return;
            }

            if (PatientID.Length > 0)//清除
            {
                ClearAll(1);
            }

            if (ReturnTipWin == true)//单击tipWin关闭后不显示界面
            {
                ReturnTipWin = false;
                return;
            }

            if (isClosed == true && textBoxName.Text.Length > 1)
            {
                Thread t1 = new Thread(StartTipWin);//将用户信息显示到表格上
                t1.IsBackground = true;
                t1.Start();
                isClosed = false;
            }

            if (textBoxName.Text.Length <= 1 && tipWinOpen == true)
            {
                CloseTipWin();
                tipWinOpen = false;
            }
        }

        public void StartTipWin()
        {
            //1.接收数据
            isClosed = false;
            StringBuilder sql = new StringBuilder();
            sql.Append(String.Format("select HZID,ZYID,HZXM,LXDH,CSRQ from T_ZYDJ where HZXM like '{0}%' and RYKBID='A0010001' and ZYZT='0'", textBoxName.Text));
            DataTable PaitentInfo = new DataTable();

            try
            {
                PaitentInfo = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message, "提示");
                return;
            }

            //2.打开窗体
            if (tipWinOpen == false)
            {
                this.Invoke(new UpdateUI(delegate ()
                {
                    tipWin frm = new tipWin();
                    tipWinOpen = true;
                    this.ShowData += frm.BindSource;
                    this.CloseTipWin += frm.CloseTipWin;
                    frm.Show(this);
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = new Point(textBoxName.PointToScreen(new Point(0, 0)).X, textBoxName.PointToScreen(new Point(0, 0)).Y + textBoxName.Height);
                    ShowData(PaitentInfo);
                    textBoxName.Focus();
                }));
            }
            else
            {
                this.Invoke(new UpdateUI(delegate ()
                {
                    ShowData(PaitentInfo);
                    textBoxName.Focus();
                }));
            }

            isClosed = true;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //1.权限判断

            //2.输入校验
            if (PatientID.Length == 0)
            {
                MessageBox.Show("请输入客户信息！");
                return;
            }

            if (AdmissionID.Length == 0)
            {
                MessageBox.Show("没有入院证！");
                return;
            }

            if (comboBoxDep.Text.Length == 0 || comboBoxDoctor.Text.Length == 0)
            {
                MessageBox.Show("请输入预约科室和医生");
                return;
            }

            if (comboBoxChildBirth.Text.Length == 0 || comboBoxRoom.Text.Length == 0)
            {
                MessageBox.Show("请输入分娩方式和房型！");
                return;
            }

            if (dateEditCheckInDate.Text.Length == 0 || dateEditDueDate.Text.Length == 0)
            {
                MessageBox.Show("请输入预入住日期和预产期");
                return;
            }

            //2.1判断日期是否正确
            DateTime dt1 = Convert.ToDateTime(dateEditCheckInDate.Text);
            DateTime dt2 = Convert.ToDateTime(dateEditDueDate.Text);
            DateTime dt3 = Convert.ToDateTime(Convert.ToDateTime(GlobalHelper.IDBHelper.GetServerDateTime()).ToString("yyyy-MM-dd"));

            if (dt1.CompareTo(dt3) < 0)
            {
                MessageBox.Show("预入院日期必须大于今天！");
                return;
            }

            if (dt1.CompareTo(dt2) > 0)
            {
                MessageBox.Show("预产期必须大于预入院日期！");
                return;
            }

            //3.变更或者新增
            if (alter == true)
            {
                Alter();
            } else
            {
                Add();
            }

        }

        private void textBoxCard_TextChanged(object sender, EventArgs e)
        {
            if (PatientID.Length != 0)
            {
                ClearAll(0);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            ClearAll(0);

            //1.读取患者基本信息            
            try
            {
                string sql = "select a.HZID,c.ZYID,b.HZXM,b.DH,b.CSRQ from H_KXX a"
                + " inner join H_HZXX b on a.HZID=b.HZID"
                + " inner join T_ZYDJ c on c.HZID=b.HZID"
                + string.Format(" where a.KH='{0}' and c.RYKBID='A0010001'", textBoxCard.Text.Trim());

                PatientInfo = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);

                if (PatientInfo.Rows.Count == 0)
                {
                    MessageBox.Show("没有这张卡！");
                    return;
                }
                else
                {
                    ReadCard = true;//让姓名文本框不显示提示框
                    textBoxName.Text = PatientInfo.Rows[0][2].ToString();
                    PatientID.Append(PatientInfo.Rows[0][0].ToString());
                    HospitalID.Append(PatientInfo.Rows[0][1].ToString());
                    PatientName.Append(PatientInfo.Rows[0][2].ToString());
                    textBoxTel.Text = PatientInfo.Rows[0][3].ToString();
                    textBoxBirthday.Text = Convert.ToDateTime(PatientInfo.Rows[0][4].ToString()).ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

            //2.读取入院证
            string sql1 = String.Format("select RYZ_ID,MZKSMC,MZYSMC,ZCRQ,RYSJ from Y_RYZ "
                     + " where HZID = '{0}' and ZT='0' and ZYKSID='A0010001' order by ZCRQ desc", PatientID.ToString());

            try
            {
                AdmissionInfo = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql1);

                if (AdmissionInfo.Rows.Count == 0)
                {
                    textBoxRYZ.Text = "无";
                }
                else
                {
                    AdmissionID.Append(AdmissionInfo.Rows[0][0].ToString());
                    textBoxRYZ.Text = AdmissionID.ToString();
                    comboBoxDep.Text = AdmissionInfo.Rows[0][1].ToString();
                    textBoxDoctor.Text = AdmissionInfo.Rows[0][2].ToString();
                    comboBoxDoctor.Text = AdmissionInfo.Rows[0][2].ToString();
                    textBoxTime.Text = Convert.ToDateTime(AdmissionInfo.Rows[0][3].ToString()).ToString("yyyy-MM-dd  HH:mm");
                    dateEditCheckInDate.Text = Convert.ToDateTime(AdmissionInfo.Rows[0][4].ToString()).ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message, "提示");
                return;
            }

            //3.读取押金
            DataTable Deposit_dt = new DataTable();
            try
            {
                string sql2 = string.Format("select sum(JinE) from T_ZYYJ  where SFFS = '押金' and YJLX = '押金' and ZYID = '{0}' group by ZYID", HospitalID.ToString());
                Deposit_dt = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql2);
                textBoxDeposit.Text = Deposit_dt.Rows[0][0].ToString() + "元";
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误3:" + ex.Message);
                return;
            }
        }


        private void ClearAll(int ClearControl)
        {
            //清除标签和文本框   
            textBoxTel.Text = "";
            textBoxBirthday.Text = "";

            textBoxDeposit.Text = "";
            textBoxRYZ.Text = "";
            textBoxDoctor.Text = "";
            textBoxTime.Text = "";

            comboBoxDep.Text = null;
            comboBoxDoctor.Text = null;
            dateEditCheckInDate.Text = null;
            label14.Text = "";
            label15.Text = "";

            //清除字段
            PatientName.Clear();
            PatientID.Clear();
            HospitalID.Clear();
            Tel.Clear();
            Birthday.Clear();
            Deposit.Clear();
            AdmissionID.Clear();
            AdmissionDep.Clear();
            AdmissionDoctor.Clear();
            AdmissionTime.Clear();

            //清除表格
            AdmissionInfo.Clear();
            PatientInfo.Clear();


            if (ClearControl == 0)//清除姓名
            {
                textBoxName.Text = "";
            }
            else if (ClearControl == 1)//清除卡号
            {
                textBoxCard.Text = "";
            }

        }

        private void comboBoxDep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboboxdep_flag == true || comboBoxDep.Text=="")
            {
                return;
            }

            UT_ID.Length = 0;
            UT_ID.Append(comboBoxDep.SelectedValue);
            
           //2.获取科室医生
            DoctorName.Clear();

            string sql = String.Format("select a.U_ID,b.U_NAME from UserWork a inner join users b on a.U_ID=b.U_ID where a.UT_ID='{0}' and (U_ACCOUNT like 'Y%'  or U_ACCOUNT like 'G%' or U_ACCOUNT like 'LDH%')", UT_ID.ToString());
            try
            {
                DoctorName = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.USER, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message, "提示");
                return;
            }

            //3.写入combobox
            comboBoxDoctor.DataSource = DoctorName;            
            comboBoxDoctor.DisplayMember = "U_NAME";
            comboBoxDoctor.ValueMember = "U_ID";         
            comboBoxDoctor.Text = null;

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton3_Click(null, null);
            }
        }

        private void dateEditCheckInDate_EditValueChanged(object sender, EventArgs e)
        {
            if (FY_FE=="1")
            {
                ReadRoom_FY();
            }
            else if (FY_FE == "2")
            {
                ReadRoom_FE();
            }

            
        }

        private void comboBoxDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoctorID.Length = 0;
            if (comboBoxDoctor.Text != "")
            {
                DoctorID.Append(comboBoxDoctor.SelectedValue.ToString());
            }           
        }

        private void comboBoxRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxRoom.Text== "VIP豪华套房(LD2)")
            {
                comboBoxChildBirth.Text = "顺产";
                comboBoxChildBirth.Enabled = false;
            }else
            {
                comboBoxChildBirth.Enabled = true;
            }
        }

        private void Add()
        {
            //1.判断是否是东湖门诊患者
            if (AdmissionID.ToString() == "东湖门诊")
            {
                string sql = String.Format("select HZID from T_CWYY where HZID='{0}'", PatientID.ToString());
                DataTable isExist = new DataTable();
                try
                {
                    isExist = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                    if (isExist.Rows.Count != 0)
                    {
                        MessageBox.Show("该患者已经预约床位，无需重复预约！");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误1:" + ex.Message);
                    return;
                }
            }
            else //如果是本院患者,通过入院证ID判断是否已经添加了患者
            {
                string sql = String.Format("select RYZ_ID from T_CWYY where RYZ_ID='{0}'", AdmissionID.ToString());
                DataTable isExist = new DataTable();
                try
                {
                    isExist = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                    if (isExist.Rows.Count != 0)
                    {
                        MessageBox.Show("该患者已经预约床位，无需重复预约！");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误1:" + ex.Message);
                    return;
                }

                //4.判断是否是妇产科住院部的
                string sql1 = String.Format("select ZYKSMC from Y_RYZ where RYZ_ID='{0}'", AdmissionID.ToString());
                DataTable ZYKSMC = new DataTable();
                try
                {
                    ZYKSMC = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql1);
                    if (ZYKSMC.Rows[0][0].ToString() != "妇产科住院部")
                    {
                        MessageBox.Show("该客户没有预约妇产科住院部床位！");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误1:" + ex.Message);
                    return;
                }
            }


            string ChildBirth = "";
            switch (comboBoxChildBirth.Text)
            {
                case "顺产": ChildBirth = "0"; break;
                case "剖宫产": ChildBirth = "1"; break;
            }

            //5.写入数据库
            string sql2 = String.Format("insert into T_CWYY"
               + " (HZID,ZYID,HZXM,CSRQ,DH,YRYRQ,YCQ,RYZ_ID,YYKSID,YYKS,YYYSID,YYYS,FM,FX,YYBZ,DJSJ,DJRID,DJR)"
               + " values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}')",
                PatientID.ToString(), HospitalID.ToString(), textBoxName.Text, textBoxBirthday.Text, textBoxTel.Text.Trim(),
                Convert.ToDateTime(dateEditCheckInDate.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(dateEditDueDate.Text).ToString("yyyy-MM-dd"), AdmissionID.ToString(), UT_ID.ToString(), comboBoxDep.Text, comboBoxDoctor.SelectedValue, comboBoxDoctor.Text,
                ChildBirth, comboBoxRoom.SelectedValue, textBoxNote.Text.Trim(),
                GlobalHelper.IDBHelper.GetServerDateTime(), GlobalHelper.UserHelper.User["U_ID"].ToString(), GlobalHelper.UserHelper.User["U_NAME"].ToString());
            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message);
                return;
            }

            if (AdmissionID.ToString() != "东湖门诊")
            {
                //6.查找入院登记中的入院证ID
                string sql3 = String.Format("select RYZ_ID from T_ZYDJ where HZID='{0}' and ZYZT='0' and RYKBID='A0010001'", PatientID.ToString());
                DataTable DT_RYZ_ID = new DataTable();
                try
                {
                    DT_RYZ_ID = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql3);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误3:" + ex.Message);
                    return;
                }

                //7.如果没有ID，则写入入院证ID
                if (DT_RYZ_ID.Rows.Count == 0 || DT_RYZ_ID.Rows[0][0].ToString() == "")
                {
                    string sql4 = String.Format("update T_ZYDJ set RYZ_ID='{0}' where HZID='{1}' and ZYZT='0' and RYKBID='A0010001'", AdmissionID.ToString(), PatientID.ToString());
                    try
                    {
                        GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql4);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("错误4:" + ex.Message);
                        return;
                    }
                }
            }


            //8.写入主界面
            Main form = (Main)this.Owner;
            form.BasicSearch_flag = true;
            form.toolStripComboBoxDJ.Text = "1";
            form.toolStripTextBoxName.Text = "";
            form.toolStripTextBoxTel.Text = "";
            form.toolStripComboBoxZT.Text = "全部";
            form.BasicSearch_flag = false;
            form.simpleButton7_Click(null, null);
            this.Close();
        }


        private void Alter()
        {
            //1.更新表记录
            string ChildBirth1 = "";
            switch (comboBoxChildBirth.Text)
            {
                case "顺产": ChildBirth1 = "0"; break;
                case "剖宫产": ChildBirth1 = "1"; break;
            }

            string sql1 = String.Format("update T_CWYY set"
               + " DH='{0}',YRYRQ='{1}',YCQ='{2}',YYKSID='{3}',YYKS='{4}',YYYSID='{5}',YYYS='{6}',FM='{7}',FX='{8}',YYBZ='{9}',XGSJ='{10}',XGRID='{11}',XGR='{12}'"
               + " where RYZ_ID='{13}' and ZYID='{14}'",
               textBoxTel.Text.Trim(),
               dateEditCheckInDate.Text, dateEditDueDate.Text, UT_ID.ToString(), comboBoxDep.Text, comboBoxDoctor.SelectedValue, comboBoxDoctor.Text,
               ChildBirth1, comboBoxRoom.SelectedValue, textBoxNote.Text,
               GlobalHelper.IDBHelper.GetServerDateTime(), GlobalHelper.UserHelper.User["U_ID"].ToString(), GlobalHelper.UserHelper.User["U_NAME"].ToString(),
               AdmissionID,HospitalID);

            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message);
                return;
            }

            //2.主界面
            Main form = (Main)this.Owner;
            form.simpleButton7_Click(null, null);

            this.Close();
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            AdmissionInfo form = new AdmissionInfo();
            form.AdmissionID = textBoxRYZ.Text;
            form.Deposit = Deposit.ToString()+" 元";
            form.FY_FE = FY_FE;
            form.Show();
        }

        private void ReadAddtionalInfo()
        {
            //1.读取入院证的附加信息
            if (AdmissionID.ToString() != "东湖门诊")
            {
                string sql = String.Format("select ZCRQ,MZYSMC from Y_RYZ "
                               + " where RYZ_ID='{0}'", AdmissionID.ToString());

                DataTable AdmissionInfo = new DataTable();

                try
                {
                    AdmissionInfo = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                    textBoxTime.Text = Convert.ToDateTime(AdmissionInfo.Rows[0][0].ToString()).ToString("yyyy-MM-dd HH:mm");
                    textBoxDoctor.Text = AdmissionInfo.Rows[0][1].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误1:" + ex.Message, "提示");
                    return;
                }
            }
            
            //2.读取登记的信息
            string sql1 = String.Format("select YYKS,YYYS from T_CWYY "
                               + " where ZYID='{0}'", HospitalID);

            DataTable ReservationInfo = new DataTable();
            try
            {
                ReservationInfo = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql1);
                comboBoxDep.Text = ReservationInfo.Rows[0][0].ToString();
                comboBoxDep_SelectedIndexChanged(null, null);
                comboBoxDoctor.Text = ReservationInfo.Rows[0][1].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message, "提示");
                return;
            }
        }

        private void comboBoxDoctor_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void textBoxRYZ_TextChanged(object sender, EventArgs e)
        {
            if (textBoxRYZ.Text == "" || textBoxRYZ.Text == "无" || textBoxRYZ.Text == "东湖门诊")
            {
                ButtonDetail.Enabled = false;
            }
            else
            {
                ButtonDetail.Enabled = true;
            }
        }

    }
}
