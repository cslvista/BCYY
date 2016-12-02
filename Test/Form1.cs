using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoLogin;
using System.Threading;
using System.Runtime.InteropServices;


namespace Test
{
    public partial class Form1 : Form
    {
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Func<bool> method = GlobalHelper.UserHelper.UserLog;
            method.BeginInvoke(null, null);
          
            IntPtr ParenthWnd = new IntPtr(0);
            AutoLogin.AutoLogin AL = new AutoLogin.AutoLogin();
            if (AL.autoLogin(textBox1.Text, textBox3.Text,Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox4.Text), "") == true)
            {
                Thread.Sleep(50);
                ParenthWnd = FindWindow(null, "用户登录");
                if (ParenthWnd.Equals(IntPtr.Zero))
                {
                    BCYY.Main frm = new BCYY.Main();
                    frm.Show();

                }               
            }
             

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "MZSYS";
            textBox3.Text = "111111";
            textBox2.Text = "1000";
            textBox4.Text = "540";

            button1_Click(null, null);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GlobalHelper.UserHelper.UserLog();

            BCYY.Main frm = new BCYY.Main();
            frm.Show();
        }
    }
}
