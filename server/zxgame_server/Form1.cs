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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            timeText.Text = DateTime.Now.ToLocalTime().ToString();
            ShowText.Text = "系统信息：服务器未开启\r\n= = = = = = = = = = = = = = = = = = =\r\n";
            userText.Text = "用户信息：\r\n= = = = = = = = = =\r\n\r\n";
            ErrorInfo.Text = "操作信息：\r\n= = = = = = = = = = = = = = = = = = = = \r\n\r\n";
            clearTime.Text = "自动清屏倒计时：" + Time / 60 + "分" + Time % 60 + "秒";
            
        }

        public static Socket server_socket;

        //用来储存连接的用户
        public static Dictionary<user, Socket> users = new Dictionary<user, Socket>();
        public static Dictionary<Socket, user> usersBack = new Dictionary<Socket, user>();
        //用来储存房间
        public static Dictionary<int, Room> rooms = new Dictionary<int, Room>();

        private static bool flag = false;

        private static int portss;

        private static int count = 0 ;

        private static int RealOnlineNum = 0;

        public void QuitRoom(int roomid)
        {
            rooms[roomid].Remove();
            rooms.Remove(roomid);
            ShowText.Text += DateTime.Now.ToLocalTime().ToString()+"：\r\n系统：房间号："+roomid+"，已关闭";
        }

        public void test()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");

            int port = 8888;
            IPEndPoint point = new IPEndPoint(ip, port);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(point);
                //ConnectText.text = "连接成功";
            Room room = new Room(new user("hzh","黄志濠"),s,4,100000,this);
            rooms.Add(10000,room);
        }

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

        private void ReviceMsg(Object o)
        {
            Socket client_Socket = o as Socket;
            while (flag)
            {
                try
                {
                    if (!client_Socket.Connected || client_Socket==null)
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
                            if (!words.Contains("RoomInfo"))
                            {
                                string username = usersBack[client_Socket].username;
                                ErrorInfo.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + username + "：" + words + "\r\n";
                            }
                            MsgHandler(words, client_Socket);
                        }
                    }
                    
                }
                catch(Exception e)
                {
                    ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n系统：" + e.ToString() + "\r\n";
                    string username = usersBack[client_Socket].username;
                    userText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n系统：" + username + "：下线" + "\r\n";
                }
            }
        }

        private void SendMsg(string msg,Socket client_socket)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
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

        private void button3_Click(object sender, EventArgs e)
        {
            new Form2().Show();
        }

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
                        if (users[index].Poll(1000, SelectMode.SelectRead))
                        {

                            users[index].Close();//关闭socket
                            users.Remove(index);//从列表中删除断开的socke
                            if (!index.username.Contains("offline"))
                            {
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

        private void timer2_Tick(object sender, EventArgs e)
        {
            timeText.Text = "时间："+DateTime.Now.ToLocalTime().ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string str = sendText.Text;
            if(!String.IsNullOrEmpty(str)){
                SendMsg(str,null);
                sendText.Text = "";
            }
        }

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

        public void MsgHandler(string msg,Socket client_socket)
        {
            string index = "";
            if (client_socket!=null && client_socket.Connected)
            {
                index = usersBack[client_socket].username;
            }
                string[] data;
                string command = HZHUtils.JieXiMsg(msg, out data);
                switch (command)
                {
                    case "login":
                        string lastname;
                        bool flag = HZHUtils.IsLogin(data[0], data[1], out lastname);
                        if (flag)
                        {
                            bool ff = false;
                            foreach(user usernames in users.Keys)
                            {
                                if (usernames.username == data[0])
                                {
                                    ff = true;
                                    break;
                                }
                            }
                            if (ff)
                            {
                                ShowText.Text += DateTime.Now.ToLocalTime().ToString() + " " + data[0]+"在线\r\n";
                                SendMsg("Online", client_socket); 
                            }
                            else
                            {
                                ShowText.Text += DateTime.Now.ToLocalTime().ToString() + " " + data[0] + "离线\r\n";
                                SendMsg(lastname, client_socket);
                                ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + data[0] + "：登录操作，用户名：" + data[0] + "，成功\r\n";
                                //更改连接用户的用户名
                                string lastusername = usersBack[client_socket].username;
                                foreach(user userss in users.Keys)
                                {
                                    if(userss.username==lastusername)
                                    {
                                        userss.username = data[0];
                                        break;
                                    }
                                }
                                usersBack[client_socket].username = data[0];
                                RealOnlineNum++;
                                userText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n系统：" + data[0] + "：上线\r\n";
 
                            }

                        }
                        else
                        {
                            SendMsg("NO", client_socket);
                            ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + data[0] + "：登录操作，用户名：" + data[0] + "，失败\r\n";
                        }
                        break;
                     
                    case "msg":
                        ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n房间：" + data[2] + "，" + index + "：" + data[1] + "\r\n";
                        int roomidss = int.Parse(data[2]);
                        //在房间内对其他玩家发送消息
                        rooms[roomidss].SendMsg("msg|" + data[0] + "," + data[1], null);
                        break;

                    case "create":
                        bool f = HZHUtils.CreateRoom(data[0], int.Parse(data[1]), int.Parse(data[2]),int.Parse(data[3]));  
                        int roomid;
                        f = HZHUtils.GetRoomId(data[0], out roomid);
                        SendMsg(roomid.ToString(), client_socket);
                        ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + roomid + "：创建房间操作，房间号：" + roomid + "，成功\r\n";
                        Room room = new Room(new user(data[0], data[4]), client_socket, int.Parse(data[1]), roomid, this);
                        rooms.Add(roomid, room);
                        break;
                    case "Isjoin":
                        string f1 = HZHUtils.IsRoom(int.Parse(data[0]),int.Parse(data[1]));
                        ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + index + "：查询房间操作，房间号：" + data[0] + "，查询状态："+f1+"\r\n";
                        SendMsg(f1, client_socket);
                        break;
                    case "join":
                        ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + index + "：加入房间操作，房间号：" + data[1] + "，成功\r\n";
                        rooms[int.Parse(data[1])].join(new user(data[0],data[2]),client_socket);
                        
                        break;
                    case "Quitroom":
                        int f2 = HZHUtils.QuitRoom(data[0]);
                        int roomidid = int.Parse(data[1]);
                        if (f2==-1)
                        {
                            ShowText.Text += ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "RoomId:"+f2;
                            rooms[roomidid].QuitRoom(data[0]);
                            ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + index + "：退出房间操作，状态：房间成员\r\n";
                            
                        }else
                        {
                            rooms[roomidid].QuitRoom(null);
                            ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n" + index + "：退出房间操作，状态：房主\r\n";
                            QuitRoom(f2);
                        }
                        break;
                    case "RoomInfo":
                        int room_id = int.Parse(data[0]);
                        //ShowText.Text += DateTime.Now.ToLocalTime().ToString() + "：\r\n系统：" + rooms[room_id].RoomInfo() + "\r\n";
                        SendMsg(rooms[room_id].RoomInfo(),client_socket);
                        break;
                }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new Form3().Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            userText.Text = "用户信息：\r\n= = = = = = = = = =\r\n\r\n";
        }

        private void button7_Click(object sender, EventArgs e)
        {

            ErrorInfo.Text = "操作信息：\r\n= = = = = = = = = = = = = = = = = = = = \r\n\r\n";
        }

        private int Time = 5;

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

        private void ShowText_TextChanged(object sender, EventArgs e)
        {
            ShowText.SelectionStart = ShowText.TextLength;
            //将控件内容滚动到当前插入符号位置
            ShowText.ScrollToCaret();
        }

        private void userText_TextChanged(object sender, EventArgs e)
        {
            userText.SelectionStart = userText.TextLength;
            //将控件内容滚动到当前插入符号位置
            userText.ScrollToCaret();
        }

        private void ErrorInfo_TextChanged(object sender, EventArgs e)
        {
            ErrorInfo.SelectionStart = ErrorInfo.TextLength;
            //将控件内容滚动到当前插入符号位置
            ErrorInfo.ScrollToCaret();
        }

        private bool AutoClear = false;

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

        private void button9_Click(object sender, EventArgs e)
        {
            test();
        }

        private void portText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 48 || e.KeyChar >= 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
            {
                e.Handled = true;
            }
        }

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

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

    }
}
