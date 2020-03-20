using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zxgame_server
{
    public partial class RoomForm : Form
    {

        public Dictionary<int, Room> rooms = new Dictionary<int, Room>();

        public RoomForm(Dictionary<int,Room> rooms)
        {
            this.rooms = rooms;
            InitializeComponent();
        }

        private void RoomForm_Load(object sender, EventArgs e)
        {
            foreach (int roomid in rooms.Keys) 
            {
                DataGridViewRow row = new DataGridViewRow();
                int index = data.Rows.Add(row);
                data.Rows[index].Cells[0].Value = rooms[roomid].RoomId;
                int type = rooms[roomid].type;
                string str = "";
                switch (type)
                {
                    case 0:
                        str = "一夜狼";
                        break;
                    case 1:
                        str = "扇子狼";
                        break;
                    case 2:
                        str = "新游戏1";
                        break;
                    case 3:
                        str = "新游戏2";
                        break;
                }
                data.Rows[index].Cells[1].Value = str;
                data.Rows[index].Cells[2].Value = rooms[roomid].fangzhu.username;
                data.Rows[index].Cells[3].Value = rooms[roomid].playernum;
                if(!rooms[roomid].style)
                {
                    str = "等待中";
                }
                else
                {
                    str = "正在游戏中";
                }
                data.Rows[index].Cells[4].Value = str;
                row.Tag = roomid;
            }
        }

        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)//双击表头或列头时不起作用
            {
                data1.Rows.Clear();
                try
                {
                    DataGridViewRow row = data.Rows[e.RowIndex];

                    if (row.Cells[0].Value != null)
                    {
                        int roomid1 = int.Parse(row.Cells[0].Value.ToString());
                     
                        Room room1;
                        if(rooms.TryGetValue(roomid1,out room1))
                        {
                            foreach (int user in room1.players.Keys)
                            {
                                DataGridViewRow row1 = new DataGridViewRow();
                                int index1 = data1.Rows.Add(row1);
                                data1.Rows[index1].Cells[0].Value = room1.players[user].username;
                                data1.Rows[index1].Cells[1].Value = room1.players[user].lastname;
                            }
                        }
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString());
                }
            }
        }
    }
}
