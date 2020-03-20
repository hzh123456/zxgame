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
using System.Timers;
using System.Windows.Forms;

namespace zxgame_server
{
    public partial class Form2 : Form
    {

        public static bool flags = false;

        public Form2()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            
        }

       

        private static bool flag = false;

        Socket client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private void button1_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            IPAddress ip;

            int port;
            if (flags)
            {
                IPHostEntry hostinfo = Dns.GetHostEntry("hzhweb.picp.vip");
                IPAddress[] aryIP = hostinfo.AddressList;
                ip = aryIP[0];
                port = 11994;
            }
            else
            {
                ip = IPAddress.Parse("127.0.0.1");

                port = 8888;
            }
            IPEndPoint point = new IPEndPoint(ip, port);
            try
            {
                client_socket.Connect(point);
                //ConnectText.text = "连接成功";

                //client.Connect(point);
                Thread th = new Thread(ReviceMsg);
                th.IsBackground = true;
                th.Start(client_socket);
                button.Enabled = false;
                flag = true;
            }
            catch { }
        }
        private void ReviceMsg(Object o)
        {
            Socket client_Socket = o as Socket;
            while (true)
            {
                if (!client_Socket.Connected)
                {
                    client_Socket.Close();
                    break;
                }
                try
                {
                    //定义byte数组存放从客户端接收过来的数据
                    byte[] buffer = new byte[1024 * 1024];
                    int n = client_Socket.Receive(buffer);
                    string words = Encoding.UTF8.GetString(buffer, 0, n);
                    text.Text += words + "\r\n";
                    //ReviceText.text = "接受消息成功";
                }
                catch(Exception e) {
                    
                }
            }
        }

        private void SendMsg(string msg)
        {
            if (flag)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(msg);
                client_socket.Send(buffer);
            }
            else
            {
                MessageBox.Show("请先连接服务器！");
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(client_socket!=null && client_socket.Connected){
                client_socket.Shutdown(SocketShutdown.Both);
                client_socket.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                SendMsg("login|hzh,123456");
            }
            else
            {
                MessageBox.Show("请先连接服务器！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                SendMsg("login|hzh,12345");
            }
            else
            {
                MessageBox.Show("请先连接服务器！");
            }
        }

        private void text_TextChanged(object sender, EventArgs e)
        {
            text.SelectionStart = text.TextLength;
            //将控件内容滚动到当前插入符号位置
            text.ScrollToCaret();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            flags = false;
            button5.Enabled = false;
            button6.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            flags = true;
            button5.Enabled = true;
            button6.Enabled = false;
        }

    }
}
