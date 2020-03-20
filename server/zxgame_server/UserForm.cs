using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class UserForm : Form
    {

        public Dictionary<user, Socket> users = new Dictionary<user, Socket>();

        public UserForm(Dictionary<user, Socket> users)
        {
            this.users = users;
            InitializeComponent();
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            foreach(user user in users.Keys)
            {
                if(!user.username.Contains("offline"))
                {
                    DataGridViewRow row = new DataGridViewRow();
                    int index = data.Rows.Add(row);
                    data.Rows[index].Cells[0].Value = user.username;
                    data.Rows[index].Cells[1].Value = user.lastname;
                }
            }
        }
    }
}
