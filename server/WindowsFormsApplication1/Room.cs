using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class Room
    {

        private Thread th;

        public int RoomId
        {
            get;
            set;
        }

        public bool style
        {
            get;
            set;
        }

        public Form1 Form
        {
            get;
            set;
        }

        public Socket fangzhu_socket
        {
            get;
            set;
        }
        public user fangzhu
        {
            get;
            set;
        }
        public Dictionary<user, Socket> players
        {
            get;
            set;
        }

        public int playernum
        {
            get;
            set;
        }

        public Room(user fangzhu, Socket fangzhu_socket, int playernum, int RoomId,Form1 form1)
        {
            this.fangzhu = fangzhu;
            this.fangzhu_socket = fangzhu_socket;
            players = new Dictionary<user, Socket>();
            players.Add(fangzhu, fangzhu_socket);
            this.playernum = playernum;
            this.RoomId = RoomId;
            this.Form = form1;
            this.style = false;
            th = new Thread(new ThreadStart(CheckFangZhu));
            th.IsBackground = true;
            th.Start();
        }

        public void CheckFangZhu()
        {
            while(true)
            {
                if (fangzhu_socket==null || !fangzhu_socket.Connected)
                {
                    //HZHUtils.QuitRoom(fangzhu.username);
                    //QuitRoom(null);
                    this.Form.MsgHandler("Quitroom|" + fangzhu.username, null);
                    break;
                }
                Thread.Sleep(1000);
            }
        }

        public void Remove()
        {
            th.Abort();
        }

        public void join(user player,Socket socket)
        {
            players.Add(player, socket);
            //在房间内对其他所有玩家发送进入房间消息
            SendMsg("Join|" + player.username + "," + player.lastname, null);
        }

        public string RoomInfo()
        {
            string str = "";
            foreach (user user in players.Keys)
            {
                str += user.username+","+user.lastname+"|";
            }
            str += "roomInfo"+"|"+playernum;
            return str;
        }

        
        public void QuitRoom(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                SendMsg("QuitRoom",null);
            }
            else
            {
                user users = GetUserByUsername(username);
                SendMsg("Quit|" + users.username + "," + users.lastname, null);
                players.Remove(GetUserByUsername(username));
                //对房间其他所有玩家发送退出房间消息
                
            }
        }

        public user GetUserByLastname(string lastname)
        {
            user users = null;
            foreach (user user in players.Keys)
            {
                if (user.lastname == lastname)
                {
                    users = user;
                    break;
                }
            }
            return users;
        }

        public user GetUserByUsername(string username)
        {
            user users = null;
            foreach (user user in players.Keys)
            {
                if (user.username == username)
                {
                    users = user;
                    break;
                }
            }
            return users;
        }

        public void SendMsg(string msg,string username)
        {
            if (username == null)
            {
                //群发消息
                foreach (user user in players.Keys)
                {
                    if (players[user] != null && players[user].Connected)
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(msg);
                        players[user].Send(buffer);
                    }
                }
            }
            else
            {
                foreach (user user in players.Keys)
                {
                    //私聊发消息
                    if(user.username==username)
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(msg);
                        players[user].Send(buffer);
                        break;
                    }
                    
                }
            }
        }

    }
}
