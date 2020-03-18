using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string uname = username.Text;
                string pwd = password.Text;
                string lname = lastname.Text;
                if (!String.IsNullOrEmpty(uname) && !String.IsNullOrEmpty(pwd) && !String.IsNullOrEmpty(lname))
                {
                    bool flag = HZHUtils.AddUser(uname, pwd, lname);
                    MessageBox.Show("添加成功！");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("用户名、密码、昵称不能为空");
                }
            }
            catch 
            {
                MessageBox.Show("数据库操作异常！");
            }
        }
    }
}
