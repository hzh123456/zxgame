using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zxgame_server
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

        public Room(user fangzhu, Socket fangzhu_socket, int playernum, int RoomId, Form1 form1, Dictionary<int, string> shenfen,int type)
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
            this.type = type;
            UseSkillNum = 0;
            CanUseSkillShenFen = 0;
            if(type==0)
            {//一夜狼
                for (int i = 0; i < playernum; i++)
                {
                    if (shenfen[i + 1] == "预言家" || shenfen[i + 1] == "盗贼" || shenfen[i + 1] == "小女孩" || shenfen[i + 1] == "狼王")
                    {
                        CanUseSkillShenFen++;
                    }
                }
            }
            if (type == 1)
            {//扇子狼人
                CanUseSkillShenFen = playernum;
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

        public void GameHandler(string shenfen, int[] index)
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
                        OneNightWolfSkilling();
                    }
                }
                else if (this.type == 1)
                {
                    //扇子狼游戏处理
                    UserSkill.Add(shenfen, index);
                    UseSkillNum++;
                    if (UseSkillNum == CanUseSkillShenFen)
                    {
                        shanziWolfSkilling();
                    }
                }
                else if (this.type == 2)
                {
                    //新游戏1 游戏处理
                }
                else if (this.type == 3)
                {
                    //新游戏2 游戏处理
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void shanziWolfSkilling()
        {
            double[] PiaoShu = new double[playernum + 1];
            int yuyanjia = -1;
            int nvwu = -1;
            int nvwutoupiao = -1; 
            int lieren = -1;
            int lierentoupiao = -1;
            int baichi = -1;
            int baichitoupiao = -1;
            foreach(string sf in UserSkill.Keys)
            {
                int[] index = UserSkill[sf];
                int zuowei = GetZuoWeiByShenfen(sf);
                user user = players[zuowei];
                int toupiao = index[0];
                if (toupiao == -1)
                {
                    if(sf=="预言家")
                    {
                        yuyanjia = zuowei;
                    }
                    FuPan += zuowei + "号 " + user.lastname + "： 选择翻牌\r\n";
                }
                else
                {
                    if (sf == "预言家")
                    {
                        PiaoShu[toupiao]++;
                    }
                    if (sf == "女巫")
                    {
                        nvwu = zuowei;
                        nvwutoupiao = toupiao;
                        PiaoShu[toupiao]++;
                    }
                    else if (sf == "猎人")
                    {
                        lieren = zuowei;
                        lierentoupiao = toupiao;
                        PiaoShu[toupiao]++;
                    }
                    else if (sf == "白痴")
                    {
                        baichi = zuowei;
                        baichitoupiao = toupiao;
                        PiaoShu[toupiao]++;
                    }
                    else if (sf.Contains("狼人") || sf.Contains("平民"))
                    {
                        PiaoShu[toupiao]++;
                    }
                    else if(sf == "警长" || sf=="狼王")
                    {
                        PiaoShu[toupiao]+=1.5;
                    }
                    FuPan += zuowei + "号 " + user.lastname + " " + sf + "： 选择投票" + toupiao + "号 " + players[toupiao].lastname + "\r\n";
                }
            }
            bool flag = true;
            if(yuyanjia>0)
            {
                flag = false;
                user user = players[yuyanjia];
                FuPan += yuyanjia + "号 " + user.lastname + "： 预言家翻牌成功！预言家单独胜利！\r\n";
            }
            if (nvwu > 0 && PiaoShu[nvwutoupiao] == 1 && yuyanjia<0)
            {
                flag = false;
                user user = players[nvwu];
                FuPan += nvwu + "号 " + user.lastname + "： 女巫选择投票的玩家只有一票！女巫胜利！\r\n";
            }
            if (lieren > 0 && yuyanjia < 0)
            {
                double max = -1;
                for (int i = 1; i < PiaoShu.Length;i++ )
                {
                    if (max < PiaoShu[i])
                    {
                        max = PiaoShu[i];
                    }
                }
                for (int i = 1; i < PiaoShu.Length;i++ )
                {
                    if (max == PiaoShu[i])
                    {
                        max = PiaoShu[i];
                        if(lierentoupiao==i)
                        {
                            flag = false;
                            user user = players[lieren];
                            FuPan += lieren + "号 " + user.lastname + "： 猎人选择投票最高的玩家！猎人胜利！\r\n";
                            break;
                        }
                    }
                }
            }
            if (baichi > 0 && baichitoupiao == baichi && yuyanjia < 0)
            {
                flag = false;
                user user = players[baichi];
                FuPan += yuyanjia + "号 " + user.lastname + "： 白痴选择投票给自己！白痴胜利！\r\n";
            }
            if (flag)
            {
                bool pingmin = true;
                bool langren = true;
                foreach(int index in shenfen.Keys)
                {
                    if(shenfen[index].Contains("平民") || shenfen[index]=="警长")
                    {
                        pingmin = false;
                    }
                    if (shenfen[index].Contains("狼人") || shenfen[index] == "狼王")
                    {
                        langren = false;
                    }
                    if(!pingmin && !langren)
                    {
                        break;
                    }
                }
                if(pingmin)
                {
                    FuPan += "因为没有狼人存在，且独立阵营未获胜，平民胜利！\r\n";
                }
                else if(langren)
                {
                    FuPan += "因为没有平民存在，且独立阵营未获胜，狼人胜利！\r\n";
                }
                else
                {
                    double max = -1;
                    for (int i = 1; i < PiaoShu.Length; i++)
                    {
                        if (max < PiaoShu[i])
                        {
                            max = PiaoShu[i];
                        }
                    }
                    List<int> maxpiaoshuPlayer = new List<int>();
                    for (int i = 1; i < PiaoShu.Length; i++)
                    {
                        if (max == PiaoShu[i])
                        {
                            maxpiaoshuPlayer.Add(i);
                        }
                    }
                    bool maxpiaoshuPingmin = false;
                    bool maxpiaoshuLangren = false;
                    foreach(int index in maxpiaoshuPlayer)
                    {
                        if (shenfen[index].Contains("狼人") || shenfen[index] == "狼王")
                        {
                            maxpiaoshuLangren = true;
                        }
                        else if (shenfen[index].Contains("平民") || shenfen[index] == "警长")
                        {
                            maxpiaoshuPingmin = true;
                        }
                    }
                    if (maxpiaoshuLangren && maxpiaoshuPingmin)
                    {
                        FuPan += "因为独立阵营未获胜，且被投票的狼人与被投票的平民票数相等，双方阵营都失败！\r\n";
                    }
                    else if (maxpiaoshuLangren)
                    {
                        FuPan += "因为独立阵营未获胜，且被投票的狼人票数最多，平民阵营胜利！\r\n";
                    }
                    else if (maxpiaoshuPingmin)
                    {
                        FuPan += "因为独立阵营未获胜，且被投票的平民票数最多，狼人阵营胜利！\r\n";
                    }
                }
            }
            SendMsg(FuPan, null);
        }

        //复盘的字符串
        public string FuPan = "FuPan|\r\n昨夜神牌操作如下：\r\n";

        public string RetrunFuPan()
        {
            int i = 0;
            FuPan += "\r\n最终身份如下：\r\n\r\n";
            foreach(int t in FinalShenFen.Keys)
            {
                FuPan += t+"号 "+ players[t].lastname +"："+FinalShenFen[t]+"\r\n";
                i++;
                if(i==playernum)
                {
                    break;
                }
            }
            FuPan += "\r\n";
            return FuPan;
        }

        public void OneNightWolfSkilling()
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
                    FuPan += GetZuoWeiByShenfen("预言家")+"号 "+ players[GetZuoWeiByShenfen("预言家")].lastname +"： 预言家查看了两张底牌为：" + shenfen[shenfen.Count - i] + "," + shenfen[shenfen.Count - j] + "\r\n";
                    lookshenfen = "两张底牌为:"+shenfen[shenfen.Count - i] + "," + shenfen[shenfen.Count - j]+"\r\n";
                }
                else
                {//预言家选择查看一名玩家身份
                    FuPan += GetZuoWeiByShenfen("预言家") + "号 " + players[GetZuoWeiByShenfen("预言家")].lastname + "： 预言家查看" + index[0] + "号玩家身份：" + shenfen[index[0]] + "\r\n";
                    lookshenfen = "他的身份是" + shenfen[index[0]] + "\r\n";
                }
                SendMsg("skill|"+lookshenfen,players[GetZuoWeiByShenfen("预言家")].username);
            }
            if (UserSkill.TryGetValue("狼王",out index))
            {//狼王选择一名玩家变为狼人
                string msg = "";
                FinalShenFen[index[0]] = "狼人";
                msg = index[0] + "号玩家被变成狼人\r\n";
                FuPan += GetZuoWeiByShenfen("狼王") + "号 " + players[GetZuoWeiByShenfen("狼王")].lastname + "： 狼王选择" + index[0] + "号玩家变为狼人\r\n";
                SendMsg("skill|" + msg, players[GetZuoWeiByShenfen("狼王")].username);
                ChangeWolfIndex = index[0];
            }
            if (UserSkill.TryGetValue("酒鬼", out index))
            {//酒鬼选择一张底牌，变为这张底牌
                string msg = "";
                int i = new Random().Next(0, 3);
                string newshenfen = shenfen[shenfen.Count - i];
                FinalShenFen[index[0]] = newshenfen;
                msg = "你查看的底牌为：" + newshenfen + "\r\n";
                FuPan += GetZuoWeiByShenfen("酒鬼") + "号 " + players[GetZuoWeiByShenfen("酒鬼")].lastname + "： 酒鬼变形的底牌为：" + newshenfen + "\r\n";
                SendMsg("skill|" + msg, players[GetZuoWeiByShenfen("酒鬼")].username);
            }
            if (UserSkill.TryGetValue("盗贼", out index))
            {//盗贼选择一名玩家，与他交换身份
                string oldshenfen = FinalShenFen[index[0]];
                string newshenfen = FinalShenFen[index[1]];
                FinalShenFen[index[0]] = newshenfen;
                FinalShenFen[index[1]] = oldshenfen;
                FuPan += GetZuoWeiByShenfen("盗贼") + "号 " + players[GetZuoWeiByShenfen("盗贼")].lastname + "： 盗贼交换的身份为：" + newshenfen + "\r\n";
                SendMsg("skill|你交换的身份为：" + newshenfen + "\r\n", players[GetZuoWeiByShenfen("盗贼")].username);
            }
            if (UserSkill.TryGetValue("小女孩", out index))
            {//小女孩选择两名玩家交换身份
                string oldshenfen = FinalShenFen[index[0]];
                string newshenfen = FinalShenFen[index[1]];
                FinalShenFen[index[0]] = newshenfen;
                FinalShenFen[index[1]] = oldshenfen;
                FuPan += GetZuoWeiByShenfen("小女孩") + "号 " + players[GetZuoWeiByShenfen("小女孩")].lastname + "： 小女交换的两名玩家为： " + index[0] + "号：" + oldshenfen + " ， " + index[1] + "号：" + newshenfen + " \r\n";
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
                    FuPan += GetZuoWeiByShenfen("守夜人") + "号 " + players[GetZuoWeiByShenfen("守夜人")].lastname + "： 守夜人的状态为：" + msg + "\r\n";
                    SendMsg("skill|你的状态是：" + msg + "\r\n", players[GetZuoWeiByShenfen("守夜人")].username);
                }
            }

            int wolfnum = 0;
            int wolf1 = -1;
            int wolf2 = -1;
            int num = 0;
            bool isZhaoYa = false;
            foreach (int shenfens in shenfen.Keys)
            {
                if (shenfen[shenfens] == "爪牙")
                {
                    isZhaoYa = true;
                    break;
                }
            }
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
                if (shenfen[wolf1] == "爪牙")
                {
                    int i = new Random().Next(0, 3);
                    string lookshenfen = shenfen[shenfen.Count - i];
                    FuPan += wolf1 + "号 " + players[wolf1].lastname + "： 单狼查看一张底牌：" + lookshenfen + "\r\n";
                    SendMsg("skill|单狼查看一张底牌：" + lookshenfen + "\r\n", players[wolf1].username);
                }
                if (shenfen[wolf1] != "爪牙" && !isZhaoYa)
                {
                    int i = new Random().Next(0, 3);
                    string lookshenfen = shenfen[shenfen.Count - i];
                    FuPan += wolf1 + "号 " + players[wolf1].lastname + "： 单狼查看一张底牌：" + lookshenfen + "\r\n";
                    SendMsg("skill|单狼查看一张底牌：" + lookshenfen + "\r\n", players[wolf1].username);
                }
                
            }
            else if(wolfnum > 1)
            {
                if (shenfen[wolf1] == "爪牙")
                {
                    FuPan += wolf1 + "号 " + players[wolf1].lastname + "： 爪牙查看的队友是：" + wolf2 + "\r\n";
                    SendMsg("skill|你的狼队友是：" + wolf2 + "号\r\n", players[wolf1].username);
                }
                else if (shenfen[wolf2] == "爪牙")
                {
                    FuPan += wolf2 + "号 " + players[wolf2].lastname + "： 爪牙查看的队友是：" + wolf1 + "\r\n";
                    SendMsg("skill|你的狼队友是：" + wolf1 + "号\r\n", players[wolf2].username);
                }
                else
                {
                    FuPan += wolf1 + "号 " + players[wolf1].lastname + "： 查看的队友是：" + wolf2 + "\r\n";
                    FuPan += wolf2 + "号 " + players[wolf2].lastname + "： 查看的队友是：" + wolf1 + "\r\n";
                    SendMsg("skill|你的狼队友是：" + wolf1 + "号\r\n", players[wolf2].username);
                    SendMsg("skill|你的狼队友是：" + wolf2 + "号\r\n", players[wolf1].username);
                }
            }
            if (ChangeWolfIndex>0)
            {
                SendMsg("changewolf|狼王将你变为狼人，请帮助狼人获胜！\r\n", players[ChangeWolfIndex].username);
            }
            int TalkNum = new Random().Next(1, playernum+1);
            SendMsg("请开始游戏，投票后房主点击查看结果按钮，查看最终结果！\r\n请" + TalkNum + "号玩家先发言！|请开始游戏，投票后房主点击查看结果按钮，查看最终结果！\r\n请" + TalkNum + "号玩家先发言！\r\n\r\n|Gaming", null);
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
            if(type==0)
            {
                SendMsg("startGame", null);
                if (CanUseSkillShenFen == 0)
                {
                    SendMsg("Gaming", null);
                }
            }
            if (type == 1)
            {
                SendMsg("startGame", null);
                int TalkNum = new Random().Next(1, playernum + 1);
                SendMsg("请" + TalkNum + "号玩家先发言！\r\n当所有人发言完毕后，玩家选择翻牌或点击其他玩家头像投票！\r\n\r\n|请" + TalkNum + "号玩家先发言！\r\n当所有人发言完毕后，玩家选择翻牌或点击其他玩家头像投票！\r\n\r\n|Gaming", null);
            }
            if (type == 2)
            {
            }
            if (type == 3)
            {
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
                SendMsg("QuitRoom", null);
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
