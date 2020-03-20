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
        //房间号
        public int RoomId
        {
            get;
            set;
        }

        //房间游戏类型
        public int type
        {
            get;
            set;
        }

        //房间状态，等待中或游戏中
        public bool style
        {
            get;
            set;
        }

        //主窗体
        public Form1 Form
        {
            get;
            set;
        }

        //房主的sokcet
        public Socket fangzhu_socket
        {
            get;
            set;
        }
        
        //房主的用户信息
        public user fangzhu
        {
            get;
            set;
        }

        //每个人的身份
        public Dictionary<int,string> shenfen
        {
            get;
            set;
        }

        //最终的身份
        public Dictionary<int, string> FinalShenFen = new Dictionary<int,string>();

        //根据身份寻找座位
        public Dictionary<string, int> shenfenBack = new Dictionary<string, int>();

        //玩家的信息t
        public Dictionary<int, user> players
        {
            get;
            set;
        }

        //玩家的sokcet
        public Dictionary<int, Socket> playersSocket 
        {
            get;
            set;
        }

        //房间玩家数量
        public int playernum
        {
            get;
            set;
        }

        //可以使用技能的玩家数量
        public int CanUseSkillShenFen = 0;

        //使用技能的玩家
        public int UseSkillNum = 0;

        public bool[] ZuoWei;

        //谁使用的技能
        public Dictionary<string, int[]> UserSkill = new Dictionary<string, int[]>();

        public int GetNullZuoWei()
        {
            int index = -1;
            for (int i = 0; i < ZuoWei.Length;i++ )
            {
                if(ZuoWei[i]==false)
                {
                    index = i;
                    break;
                }
            }
            ZuoWei[index] = true;
            return index + 1;
        }

        public Room(user fangzhu, Socket fangzhu_socket, int playernum, int RoomId, Form1 form1, Dictionary<int, string> shenfen)
        {
            ZuoWei = new bool[playernum];
            int index = GetNullZuoWei();
            this.shenfen = shenfen;
            this.fangzhu = fangzhu;
            this.fangzhu_socket = fangzhu_socket;
            players = new Dictionary<int, user>();
            players.Add(index, fangzhu);
            playersSocket = new Dictionary<int, Socket>();
            playersSocket.Add(index, fangzhu_socket);
            this.playernum = playernum;
            this.RoomId = RoomId;
            this.Form = form1;
            this.style = false;
            UseSkillNum = 0;
            CanUseSkillShenFen = 0;
            for (int i = 0; i < playernum;i++ )
            {
                if (shenfen[i + 1] == "预言家" || shenfen[i + 1] == "盗贼" || shenfen[i + 1] == "小女孩" || shenfen[i + 1] == "狼王")
                {
                    CanUseSkillShenFen++;
                }
            }
            foreach(int indexx in shenfen.Keys)
            {
                shenfenBack.Add(shenfen[indexx], indexx);
                FinalShenFen.Add(indexx, shenfen[indexx]);
            }
            for(int t=0; t<ZuoWei.Length; t++)
            {
                ZuoWei[t] = false;
            }
        }

        public void GameHandler(string shenfen, int[] index, RichTextBox ShowText)
        {
            try
            {
                if (this.type == 0)
                {
                    //一夜郎游戏处理
                    UserSkill.Add(shenfen, index);
                    UseSkillNum++;
                    if (UseSkillNum == CanUseSkillShenFen)
                    {
                        OneNightWolfSkilling(ShowText);
                    }
                }
                else if (type == 1)
                {
                    //扇子狼游戏处理
                }
                else if (type == 2)
                {
                    //新游戏1 游戏处理
                }
                else if (type == 3)
                {
                    //新游戏2 游戏处理
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        //复盘的字符串
        public string FuPan = "FuPan|昨夜神牌操作如下：\r\n";

        public string RetrunFuPan()
        {
            int i = 0;
            FuPan += "\r\n最终身份如下：\r\n\r\n";
            foreach(int t in FinalShenFen.Keys)
            {
                FuPan += t+"号："+FinalShenFen[t]+"\r\n";
                i++;
                if(i==playernum)
                {
                    break;
                }
            }
            FuPan += "\r\n";
            return FuPan;
        }

        public void OneNightWolfSkilling(RichTextBox ShowText)
        {
            int[] index;
            int ChangeWolfIndex = -1;
            if (UserSkill.TryGetValue("预言家",out index))
            {
                string lookshenfen = "";
                if (index[0] == -1)
                {//预言家选择查看2张底牌
                    int i = new Random().Next(0, 3);
                    int j = new Random().Next(0, 3);
                    while(j==i)
                    {
                        j = new Random().Next(0, 3);
                    }
                    FuPan += GetZuoWeiByShenfen("预言家")+"号： 预言家查看了两张底牌为：" + shenfen[shenfen.Count - 1 - i] + "," + shenfen[shenfen.Count - 1 - j] + "\r\n";
                    lookshenfen = "两张底牌为:"+shenfen[shenfen.Count-1-i] + "," + shenfen[shenfen.Count-1-j]+"\r\n";
                }
                else
                {//预言家选择查看一名玩家身份
                    FuPan += GetZuoWeiByShenfen("预言家")+"号： 预言家查看" + index[0] + "号玩家身份：" + shenfen[index[0]] + "\r\n";
                    lookshenfen = "他的身份是" + shenfen[index[0]] + "\r\n";
                }
                SendMsg("skill|"+lookshenfen,players[GetZuoWeiByShenfen("预言家")].username);
            }
            if (UserSkill.TryGetValue("狼王",out index))
            {//狼王选择一名玩家变为狼人
                string msg = "";
                FinalShenFen[index[0]] = "狼人";
                msg = index[0] + "号玩家被变成狼人\r\n";
                FuPan += GetZuoWeiByShenfen("狼王")+"号： 狼王选择" + index[0] + "号玩家变为狼人\r\n";
                SendMsg("skill|" + msg, players[GetZuoWeiByShenfen("狼王")].username);
                ChangeWolfIndex = index[0];
            }
            if (UserSkill.TryGetValue("酒鬼", out index))
            {//酒鬼选择一张底牌，变为这张底牌
                string msg = "";
                int i = new Random().Next(0, 3);
                string newshenfen = shenfen[shenfen.Count - 1 - i];
                FinalShenFen[index[0]] = newshenfen;
                msg = "你查看的底牌为：" + newshenfen + "\r\n";
                FuPan += GetZuoWeiByShenfen("酒鬼")+"号： 酒鬼变形的底牌为：" + newshenfen + "\r\n";
                SendMsg("skill|" + msg, players[GetZuoWeiByShenfen("酒鬼")].username);
            }
            if (UserSkill.TryGetValue("盗贼", out index))
            {//盗贼选择一名玩家，与他交换身份
                string oldshenfen = FinalShenFen[index[0]];
                string newshenfen = FinalShenFen[index[1]];
                FinalShenFen[index[0]] = newshenfen;
                FinalShenFen[index[1]] = oldshenfen;
                FuPan += GetZuoWeiByShenfen("盗贼")+"号： 盗贼交换的身份为：" + newshenfen + "\r\n";
                SendMsg("skill|你交换的身份为：" + newshenfen + "\r\n", players[GetZuoWeiByShenfen("盗贼")].username);
            }
            if (UserSkill.TryGetValue("小女孩", out index))
            {//小女孩选择两名玩家交换身份
                string oldshenfen = FinalShenFen[index[0]];
                string newshenfen = FinalShenFen[index[1]];
                FinalShenFen[index[0]] = newshenfen;
                FinalShenFen[index[1]] = oldshenfen;
                FuPan += GetZuoWeiByShenfen("小女孩")+"号： 小女交换的两名玩家为： " + index[0] + "号：" + oldshenfen + " ， " + index[1] + "号：" + newshenfen + " \r\n";
                SendMsg("skill|" + index[0] + "号与" + index[1] + "号" + "交换成功\r\n", players[GetZuoWeiByShenfen("小女孩")].username);
            }
            int indexx;
            if (shenfenBack.TryGetValue("守夜人", out indexx))
            {//守夜人查看自己是否被交换
                string msg = "";
                string myself_shenfen = FinalShenFen[indexx];
                if (myself_shenfen == "守夜人")
                {
                    msg = "没被换过";
                }
                else
                {
                    msg = "被换过";
                }
                if(indexx<=playernum)
                {
                    FuPan += GetZuoWeiByShenfen("守夜人")+"号： 守夜人的状态为：" + msg + "\r\n";
                    ShowText.Text = msg + " 守夜人\r\n";
                    SendMsg("skill|你的状态是：" + msg + "\r\n", players[GetZuoWeiByShenfen("守夜人")].username);
                }
            }

            int wolfnum = 0;
            int wolf1 = -1;
            int wolf2 = -1;
            int num = 0;
            foreach(int shenfens in shenfen.Keys)
            {
                if (shenfen[shenfens].Contains("狼人") || shenfen[shenfens] == "爪牙" )
                {
                    if(wolf1<0)
                    {
                        wolf1 = shenfens;
                    }
                    else if( wolf1>=0 && wolf2<0)
                    {
                        wolf2 = shenfens;
                    }
                    wolfnum++;
                }
                num++;
                if (num == playernum)
                {
                    break;
                }
            }
            if (wolfnum == 1)
            {
                int i = new Random().Next(0, 3);
                string lookshenfen = shenfen[shenfen.Count - 1 - i];
                FuPan += wolf1+"号： 单狼查看一张底牌：" + lookshenfen + "\r\n";
                SendMsg("skill|单狼查看一张底牌：" + lookshenfen + "\r\n", players[wolf1].username);
            }
            else if(wolfnum > 1)
            {
                if (shenfen[wolf1] == "爪牙")
                {
                    FuPan += wolf1 + "号： 爪牙查看的队友是：" + wolf2 + "\r\n";
                    SendMsg("skill|你的狼队友是：" + wolf2 + "号\r\n", players[wolf1].username);
                }
                else if (shenfen[wolf2] == "爪牙")
                {
                    FuPan += wolf2 + "号： 爪牙查看的队友是：" + wolf1 + "\r\n";
                    SendMsg("skill|你的狼队友是：" + wolf1 + "号\r\n", players[wolf2].username);
                }
                else
                {
                    FuPan += wolf1 + "号： 查看的队友是：" + wolf2 + "\r\n";
                    FuPan += wolf2 + "号： 查看的队友是：" + wolf1 + "\r\n";
                    SendMsg("skill|你的狼队友是：" + wolf1 + "号\r\n", players[wolf2].username);
                    SendMsg("skill|你的狼队友是：" + wolf2 + "号\r\n", players[wolf1].username);
                }
            }
            if (ChangeWolfIndex>0)
            {  
                SendMsg("changewolf", players[ChangeWolfIndex].username);
                ShowText.Text = ChangeWolfIndex + "号被交换 发送消息成功\r\n";
            }

            SendMsg("Gaming",null);
        }

        //加入房间 
        public void join(user player,Socket socket)
        {
            int index = GetNullZuoWei();
            players.Add(index + 1,player);
            playersSocket.Add(index + 1, socket);
            //在房间内对其他所有玩家发送进入房间消息
            SendMsg("Join|" + player.username + "," + player.lastname, null);
        }

        public void StartGame()
        {
            SendMsg("startGame", null);
            if (CanUseSkillShenFen == 0)
            {
                SendMsg("Gaming", null);
            }
        }

        //房间信息
        public string RoomInfo()
        {
            string str = "";
            foreach (int index in players.Keys)
            {
                str += players[index].username + "," + players[index].lastname + "," + shenfen[index] + "|";
            }
            str += "roomInfo"+"|"+playernum;
            return str;
        }
      
        //退出房间
        public void QuitRoom(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                SendMsg("QuitRoom",null);
            }
            else
            {
                int intdex = GetZuoWeiByUsername(username);
                user users = players[intdex];
                SendMsg("Quit|" + users.username + "," + users.lastname, null);
                players.Remove(intdex);
                playersSocket.Remove(intdex);
                //对房间其他所有玩家发送退出房间消息
                
            }
        }

        //通过身份获取用户
        public int GetZuoWeiByShenfen(string shenfens)
        {
            int users = -1;
            foreach (int user in shenfen.Keys)
            {
                if (shenfen[user] == shenfens)
                {
                    users = user;
                    break;
                }
            }
            return users;
        }

        //通过昵称获取用户
        public int GetZuoWeiByLastname(string lastname)
        {
            int users = -1;
            foreach (int user in players.Keys)
            {
                if (players[user].lastname == lastname)
                {
                    users = user;
                    break;
                }
            }
            return users;
        }

        //通过用户名获取用户
        public int GetZuoWeiByUsername(string username)
        {
            int users = -1;
            foreach (int user in players.Keys)
            {
                if (players[user].username == username)
                {
                    users = user;
                    break;
                }
            }
            return users;
        }

        //发送消息
        public void SendMsg(string msg,string username)
        {
            if (username == null)
            {
                //群发消息
                foreach (int user in playersSocket.Keys)
                {
                    if (playersSocket[user] != null && playersSocket[user].Connected)
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(msg);
                        playersSocket[user].Send(buffer);
                    }
                }
            }
            else
            {
                foreach (int user in playersSocket.Keys)
                {
                    //私聊发消息
                    if (players[user].username == username)
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(msg);
                        playersSocket[user].Send(buffer);
                        break;
                    }
                    
                }
            }
        }

    }
}
