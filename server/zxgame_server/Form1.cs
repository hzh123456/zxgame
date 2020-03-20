using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zxgame_server
{ 
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //使非主线程可以使用主线程的控件
            Control.CheckForIllegalCrossThreadCalls = false;
            //设置时间
            timeText.Text = DateTime.Now.ToLocalTime().ToString();
            ShowText.Text = "系统信息：服务器未开启\r\n= = = = = = = = = = = = = = = = = = =\r\n";
            userText.Text = "用户信息：\r\n= = = = = = = = = =\r\n\r\n";
            ErrorInfo.Text = "操作信息：\r\n= = = = = = = = = = = = = = = = = = = = \r\n\r\n";
            clearTime.Text = "自动清屏倒计时：" + Time / 60 + "分" + Time % 60 + "秒";
            
        }

        //服务器的Socket
        public static Socket server_socket;

        //用来储存连接的用户
        public static Dictionary<user, Socket> users = new Dictionary<user, Socket>();

        //通过连接查找用户
        public static Dictionary<Socket, user> usersBack = new Dictionary<Socket, user>();

        //用来储存房间 
        public static Dictionary<int, Room> rooms = new Dictionary<int, Room>();

        //判断服务器是否开启
        private static bool flag = false;

        //远程端口号
        private static int portss;

        //连接人数
        private static int count = 0 ;

        //登录人数
        private static int RealOnlineNum = 0;

        //自动清屏刷新时间，以秒为单位
        private int Time = 5;

        //自动清理开关
        private bool AutoClear = false;

        // ================================================================================================================
        // ===========================================上方为变量===========================================================
        // ================================================================================================================
        // ================================================================================================================
        // ===========================================下方为函数===========================================================
        // ================================================================================================================
        // ================================================================================================================

        //处理消息函数
        public void MsgHandler(string msg, Socket client_socket)
        {
            string index = "";
            //如果当前用户未断开连接，则取出用户名
            if (client_socket != null && client_socket.Connected)
            {
                index = usersBack[client_socket].username;
            }
            string[] data;
            //解析用户发送的消息函数
            string command = HZHUtils.JieXiMsg(msg, out data);
            switch (command)
            {
                //登录消息处理
                case "login":
                    string lastname;
                    //判断是否登录
                    bool flag = HZHUtils.IsLogin(data[0], data[1], out lastname);
                    if (flag)
                    {
                        bool ff = false;
                        foreach (user usernames in users.Keys)
                        {
                            if (usernames.username == data[0])
                            {
                                ff = true;
                                break;
                            }
                        }
                        if (ff)
                        {
                            //用户已在线，发送用户在线消息
                            SendMsg("Online", client_socket);
                        }
                        else
                        {
                            SendMsg(lastname, client_socket);
                            ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + data[0] + "：登录操作，用户名：" + data[0] + "，成功\r\n";
                            //更改连接用户的用户名
                            string lastusername = usersBack[client_socket].username;
                            foreach (user userss in users.Keys)
                            {
                                if (userss.username == lastusername)
                                {
                                    userss.username = data[0];
                                    userss.lastname = lastname;
                                    break;
                                }
                            }
                            usersBack[client_socket].username = data[0];
                            usersBack[client_socket].lastname = lastname;
                            RealOnlineNum++;
                            userText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n系统：" + data[0] + "：上线\r\n";
                        }

                    }
                    else
                    {
                        //用户登录失败
                        SendMsg("NO", client_socket);
                        ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + data[0] + "：登录操作，用户名：" + data[0] + "，失败\r\n";
                    }
                    break;
                //房间内玩家发送消息处理
                case "msg":
                    ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n房间：" + data[2] + "，" + index + "：" + data[1] + "\r\n";
                    int roomidss = int.Parse(data[2]);
                    //在房间内对其他玩家发送消息
                    rooms[roomidss].SendMsg("msg|" + data[0] + "," + data[1], null);
                    break;
                //创建房间消息处理
                case "create":
                    bool f = HZHUtils.CreateRoom(data[0], int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]));
                    int roomid;
                    f = HZHUtils.GetRoomId(data[0], out roomid);
                    ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + roomid + "：创建房间操作，房间号：" + roomid + "，成功\r\n";
                    List<string> shenfen = new List<string>();
                        string[] shenfens = data[5].Split('.');
                        foreach (string str in shenfens)
                        {
                            shenfen.Add(str);
                        }
                        bool fff = false;
                        while (!fff)
                        {
                            shenfen = HZHUtils.DaLuanList<string>(shenfen);
                            for (int i = 0; i < shenfen.Count - 3; i++)
                            {
                                if (shenfen[i] == "狼人1" || shenfen[i] == "狼人2" || shenfen[i] == "狼王" || shenfen[i] == "爪牙")
                                {
                                    fff = true;
                                }
                            }
                        }

                        int ii = 1;
                        Dictionary<int, string> shenfenss = new Dictionary<int, string>();
                        foreach (string strss in shenfen)
                        {
                            shenfenss.Add(ii++, strss);
                        }
                        Room room = new Room(new user(data[0], data[4]), client_socket, int.Parse(data[1]), roomid, this, shenfenss);
                        rooms.Add(roomid, room);
                        ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n系统：" + rooms[roomid].RoomInfo()+ "\r\n";
                        SendMsg(roomid.ToString(), client_socket);
                    break;
                //用户能否登录消息处理
                case "Isjoin":
                    string f1 = HZHUtils.IsRoom(int.Parse(data[0]), int.Parse(data[1]));
                    ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + index + "：查询房间操作，房间号：" + data[0] + "，查询状态：" + f1 + "\r\n";
                    SendMsg(f1, client_socket);
                    break;
                //用户加入房间消息处理
                case "join":
                    ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + index + "：加入房间操作，房间号：" + data[1] + "，成功\r\n";
                    rooms[int.Parse(data[1])].join(new user(data[0], data[2]), client_socket);

                    break;
                //用户退出房间消息处理
                case "Quitroom":
                    int f2 = HZHUtils.QuitRoom(data[0]);
                    int roomidid = int.Parse(data[1]);
                    if (f2 == -1)
                    {
                        ShowText.Text += ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "RoomId:" + f2;
                        rooms[roomidid].QuitRoom(data[0]);
                        ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + index + "：退出房间操作，状态：房间成员\r\n";

                    }
                    else
                    {
                        rooms[roomidid].QuitRoom(null);
                        ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + index + "：退出房间操作，状态：房主\r\n";
                        QuitRoom(f2);
                    }
                    break;
                //房间信息消息处理
                case "RoomInfo":
                    int room_id = int.Parse(data[0]);
                    //ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n系统：" + rooms[room_id].RoomInfo() + "\r\n";
                    SendMsg(rooms[room_id].RoomInfo(), client_socket);
                    break;

                //开始游戏消息处理 start|1
                case "startGame":
                    int room_id1 = int.Parse(data[0]);
                    Room room1 = rooms[room_id1];
                    room1.style = true;
                    bool t = HZHUtils.UpdateRoom(room_id1.ToString());
                    if (t)
                    {
                        room1.StartGame();
                        ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n系统：房间号：" + room_id1 + "，游戏开始\r\n";
                    }
                    else
                    {
                        ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n系统：房间号：" + room_id1 + "，游戏开始s失败\r\n";
                    }
                    ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n系统："+rooms[room_id1].RoomInfo()+"\r\n";
                    break;

                //游戏内容消息处理 game|预言家,1 or game|预言家,-1 or game|小女孩,2.3
                case "game":
                    int room_id2 = int.Parse(data[1]);
                    Room room2 = rooms[room_id2];
                    string shenfen2 = data[0];
                    string[] index2 = data[2].Split('.');
                    int[] index22 = new int[2];
                    int j = 0;
                    foreach(string str in index2)
                    {
                        index22[j++] = int.Parse(str);
                    }
                    ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n系统：" + shenfen2+":"+index22 + "\r\n";
                    room2.GameHandler(shenfen2, index22, ShowText);
                    break;
                //游戏结束消息处理 endGame|1
                case "endGame":
                    int room_id3 = int.Parse(data[0]);
                    Room room3 = rooms[room_id3];
                    room3.SendMsg(room3.RetrunFuPan(), null);
                    break;
            }
        }

        //退出房间函数
        public void QuitRoom(int roomid)
        {
            rooms.Remove(roomid);
            ShowText.Text += DateTime.Now.ToLocalTime().ToString()+"：\r\n系统：房间号："+roomid+"，已关闭\r\n";
        }
        //获取请求，并且返回远端端口 portss
        public static void GetContext(IAsyncResult ar)
        {
            HttpListener httpListener = ar.AsyncState as HttpListener;
            HttpListenerContext context = httpListener.EndGetContext(ar);  //接收到的请求context（一个环境封装体）

            httpListener.BeginGetContext(new AsyncCallback(GetContext), httpListener);  //开始 第二次 异步接收request请求

            HttpListenerRequest request = context.Request;  //接收的request数据
            HttpListenerResponse response = context.Response;  //用来向客户端发送回复

            response.ContentType = "html";
            response.ContentEncoding = Encoding.UTF8;
             
            using (Stream output = response.OutputStream)  //发送回复
            {
                byte[] buffer = Encoding.UTF8.GetBytes(portss.ToString());
                output.Write(buffer, 0, buffer.Length);
            }
        }
        //开启服务器按钮
        private void button1_Click(object sender, EventArgs e)
        {
            if(portBtn.Enabled)
            {
                MessageBox.Show("请先设置远程端口！");
            }
            else
            {
                Button button = sender as Button;

            string ports = "8080";
            IPAddress ips = IPAddress.Parse("127.0.0.1");
            //开启接收网页端请求的服务器
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add(string.Format("http://{0}:{1}/", ips, ports));
            httpListener.Start();
            httpListener.BeginGetContext(new AsyncCallback(GetContext), httpListener);  //开始异步接收request请求

            if (!flag)
            {
                button.Text = "关闭服务器";
                flag = true;
                //ip地址
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                //端口号
                int port = 8888;
                IPEndPoint point = new IPEndPoint(ip, port);
                try
                {
                    //服务器socket 使用IPv4地址，流式socket方式，tcp协议传递数据
                    server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //让socket监听point
                    server_socket.Bind(point);
                    //同一个时间点过来12个客户端，排队
                    server_socket.Listen(12);
                    //实例化一个线程 执行阻塞方法AcceptInfo
                    Thread thread = new Thread(AcceptInfo);
                    ShowText.Text = "系统信息：服务器已开启\r\n= = = = = = = = = = = = = = = = = =\r\n\r\n";
                    //在后台运行
                    thread.IsBackground = true;
                    //开始执行线程 并且传参server_socket
                    thread.Start(server_socket);
                }
                catch { }
            }
            else
            {
                //关闭服务器
                button.Text = "启动服务器";

                users.Clear();
                usersBack.Clear();
                foreach(int roomid in rooms.Keys)
                {
                    rooms[roomid].QuitRoom(roomid.ToString());
                }
                rooms.Clear();
                ShowText.Text = "系统信息：服务器未开启\r\n= = = = = = = = = = = = = = = = = =\r\n\r\n";
                server_socket.Close();

                flag = false;
            }
            }
  
        }

        //等待用户连接函数
        private void AcceptInfo(object o) 
        {
            Socket server_socket = o as Socket; 
            while(flag){
                try
                {   
                    //获取连接上的socket
                    Socket client_Socket = server_socket.Accept();
                    //存入list中
                    users.Add(new user("offline" + count, "offline" + count), client_Socket);
                    usersBack.Add(client_Socket, new user("offline" + count, "offline" + count));
                    count++;
                    Thread th = new Thread(ReviceMsg);
                    th.IsBackground = true;
                    th.Start(client_Socket);
                }
                catch(Exception e) {
                    ErrorInfo.Text += DateTime.Now.ToLocalTime().ToString()+"：\r\n服务器关闭"+e.ToString()+"\r\n";
                }
            }
        }

        //接收用户消息函数
        private void ReviceMsg(Object o)
        {
            Socket client_Socket = o as Socket;
            while (flag)
            {
                try
                {
                    if (client_Socket == null || !client_Socket.Connected)
                    {
                        break;
                    }
                    else
                    {
                        //定义byte数组存放从客户端接收过来的数据
                        byte[] buffer = new byte[1024 * 1024];
                        int n = client_Socket.Receive(buffer);
                        string words = Encoding.UTF8.GetString(buffer, 0, n);
                        if (!String.IsNullOrEmpty(words))
                        {
                            // 因为客户端1s请求一次RommInfo，所以不让RoomInfo打印在控制台
                            if (!words.Contains("RoomInfo"))
                            {
                                string username = usersBack[client_Socket].username;
                                ErrorInfo.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + username + "：" + words + "\r\n";
                            }
                            // 处理用户发送的消息的函数
                            MsgHandler(words, client_Socket);
                        } 
                    }
                    
                }
                catch(Exception e)
                {
                    //userText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n错误信息：" + e.ToString() + "\r\n";
                }
            }
        }

        //发送消息函数
        private void SendMsg(string msg,Socket client_socket)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(msg);

            //如果传值client_socket为空，表示群发 注意：一般服务器不群发，只在房间内对所有玩家群发

            if (client_socket == null)
            {
                foreach (Socket socket in users.Values)
                {
                    socket.Send(buffer);
                }
                ShowText.Text += DateTime.Now.ToLocalTime().ToString()+"：\r\n系统：" + msg + "\r\n";
            }
            else
            {
                client_socket.Send(buffer);
            }
        }

        // 启动连接测试窗口函数
        private void button3_Click(object sender, EventArgs e)
        {
            if (portBtn.Enabled)
            {
                MessageBox.Show("请先设置远程端口！");
            }
            else if (!flag)
            {
                MessageBox.Show("请先启动服务器！");
            }
            else
            {
                new Form2().Show();
            }
        }

        //检查用户socket是否断开函数
        private void timer1_Tick(object sender, EventArgs e)
        {
            OnlineText.Text = "连接数：" + users.Count;
            RoomNumText.Text = "房间数：" + rooms.Count;
            RealOnline.Text = "在线数：" + RealOnlineNum;
            try
            {
                if (users.Count > 0)
                {
                    foreach (user index in users.Keys)
                    {
                        //当用户断开连接时
                        if (users[index].Poll(100, SelectMode.SelectRead))
                        {
                            usersBack.Remove(users[index]);//usersBack删除用户
                            users[index].Close();//关闭socket
                            users.Remove(index);//从列表中删除断开的socke
                            bool falg = CheckFangZhu(index);//判断断开的用户是否是房间内的房主
                            if(!falg)
                            {
                                //如果断开的用户不是房主，在判断此用户是否在房间内
                                CheckIsRoomPlayer(index);
                            }
                            //连接用户用户名处理
                            if (!index.username.Contains("offline"))
                            {
                                userText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n系统：" + index.username + "：下线" + "\r\n";
                                RealOnlineNum--;
                            }
                            continue;
                        }
                    }
                }
            }
            catch 
            { }
        }

        //检查断开的用户是否是玩家
        private void CheckIsRoomPlayer(user user)
        {
            foreach(int roomid in rooms.Keys)
            {
                foreach(int index in rooms[roomid].players.Keys)
                {
                    if (rooms[roomid].players[index].username == user.username)
                    {
                        rooms[roomid].QuitRoom(user.username);
                    }
                }
            }
        }

        //检查断开的用户是否是游戏房间内成员
        private bool CheckFangZhu(user index)
        {
            bool falg = false;
            foreach(int roomids in rooms.Keys)
            {
                if(rooms[roomids].fangzhu.username == index.username)
                {
                    rooms[roomids].QuitRoom(null);
                    rooms.Remove(roomids);
                    ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n系统：房间号：" + roomids + "，已关闭\r\n";
                    HZHUtils.RemoveRoom(roomids);
                    falg = true;
                    break;
                }
            }
            return falg;
        }

        //实时显示本地时间
        private void timer2_Tick(object sender, EventArgs e)
        {
            timeText.Text = "时间："+DateTime.Now.ToLocalTime().ToString();
        }

        //发送消息按钮 注意：一般服务器不群发消息，只在房间内群发消息
        private void button4_Click(object sender, EventArgs e)
        {
            string str = sendText.Text;
            if(!String.IsNullOrEmpty(str)){
                SendMsg(str,null);
                sendText.Text = "";
            }
        }

        // 清屏按钮
        private void button2_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                ShowText.Text = "系统信息：服务器已开启\r\n= = = = = = = = = = = = = = = = = =\r\n\r\n";
            }
            else
            {
                ShowText.Text = "系统信息：服务器未开启\r\n= = = = = = = = = = = = = = = = = =\r\n\r\n";
            }
            
        }

        //添加用户函数
        private void button5_Click(object sender, EventArgs e)
        {
            new Form3().Show();
        }

        //清屏按钮
        private void button6_Click(object sender, EventArgs e)
        {
            userText.Text = "用户信息：\r\n= = = = = = = = = =\r\n\r\n";
        }

        //清屏按钮
        private void button7_Click(object sender, EventArgs e)
        {

            ErrorInfo.Text = "操作信息：\r\n= = = = = = = = = = = = = = = = = = = = \r\n\r\n";
        }       

        //自动清屏函数
        private void timer3_Tick(object sender, EventArgs e)
        {
            clearTime.Text = "自动清屏倒计时：" + Time / 60 + "分" + Time % 60 + "秒";
            if (Time == 0)
            {
                if (flag)
                {
                    ShowText.Text = "系统信息：服务器已开启\r\n= = = = = = = = = = = = = = = = = =\r\n\r\n";
                }
                else
                {
                    ShowText.Text = "系统信息：服务器未开启\r\n= = = = = = = = = = = = = = = = = =\r\n\r\n";
                }
                userText.Text = "用户信息：\r\n= = = = = = = = =\r\n\r\n";
                ErrorInfo.Text = "操作信息：\r\n= = = = = = = = = = = = = = = = = = = \r\n\r\n";
                Time = 5;
            }
            else
            {
                Time--;
            }
            
        }

        //使滚动条在最下方
        private void ShowText_TextChanged(object sender, EventArgs e)
        {
            ShowText.SelectionStart = ShowText.TextLength;
            //将控件内容滚动到当前插入符号位置
            ShowText.ScrollToCaret();
        }

        //使滚动条在最下方
        private void userText_TextChanged(object sender, EventArgs e)
        {
            userText.SelectionStart = userText.TextLength;
            //将控件内容滚动到当前插入符号位置
            userText.ScrollToCaret();
        }

        //使滚动条在最下方
        private void ErrorInfo_TextChanged(object sender, EventArgs e)
        {
            ErrorInfo.SelectionStart = ErrorInfo.TextLength;
            //将控件内容滚动到当前插入符号位置
            ErrorInfo.ScrollToCaret();
        }      

        //自动清理按钮
        private void button8_Click(object sender, EventArgs e)
        {
            AutoClear = !AutoClear;
            timer3.Enabled = AutoClear;
            if (AutoClear)
            {
                button8.Text = "关闭自动清屏";
            }
            else
            {
                button8.Text = "开启自动清屏";
            }
            
        }

        //端口号输入设置函数，只能输入数字
        private void portText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 48 || e.KeyChar >= 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
            {
                e.Handled = true;
            }
        }

        //设置端口号按钮
        private void port_Click(object sender, EventArgs e)
        {
            string portstr = portText.Text;
            if(String.IsNullOrEmpty(portstr))
            {
                MessageBox.Show("远程端口不能为空！");
            }
            else
            {
                portText.Enabled = false;
                portss = int.Parse(portText.Text);
                Button button = sender as Button;
                button.Enabled = false;
            }
        }

        //在线用户窗口显示函数
        private void button11_Click(object sender, EventArgs e)
        {
            if (portBtn.Enabled)
            {
                MessageBox.Show("请先设置远程端口！");
            }
            else if (!flag)
            {
                MessageBox.Show("请先启动服务器！");
            }
            else
            {
                new UserForm(users).Show();
            }
        }

        //房间窗口显示函数
        private void button10_Click(object sender, EventArgs e)
        {
            if (portBtn.Enabled)
            {
                MessageBox.Show("请先设置远程端口！");
            }
            else if (!flag)
            {
                MessageBox.Show("请先启动服务器！");
            }
            else
            {
                new RoomForm(rooms).Show();
            }
        }

    }
}
