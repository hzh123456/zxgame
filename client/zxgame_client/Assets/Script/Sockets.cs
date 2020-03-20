using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Assets.Script;
using UnityEngine;
using UnityEngine.UI;

public class Sockets {

    public static Socket clientSocket;

    private static Sockets instance;

    private Thread recvProcess = null, sendProcess = null;

    private volatile bool stopSendProcess, stopRecvProcess;

    private static int ports;

    public static bool connected = false;
     
    //发送队列
    private static Queue<string> sendQueue;

    private static Queue<string> recvQueue;

    public bool Connected()
    {
        return connected;
    }

    public string GetMsg()
    {
        if (recvQueue.Count > 0 && connected)
        {
            string msg = recvQueue.Dequeue();
            return msg; 
        }
        else
        {
            return "wait";
        }
        
    }

    public static Sockets GetInstance(int port)
    {
        try
        {
            if (instance == null)
            {
                instance = new Sockets(port);

            }
            sendQueue = new Queue<string>();
            recvQueue = new Queue<string>();
            return instance; 
        }
        catch
        {
            throw new Exception();
        }
    }

    Sockets(int port)
    {
        try
        {
            CreateConnection(port);
        }
        catch
        {
            throw new Exception();
        }
        
    }

    public void TryConnect()
    {
        CreateConnection(ports);
    }

    public void CreateConnection(int port)
    {
        try
        {
            ports = port;

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPHostEntry hostinfo = Dns.GetHostEntry("hzhweb.picp.vip");
            IPAddress[] aryIP = hostinfo.AddressList;
            IPAddress ipAddress = aryIP[0];

            //IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
           // int port = 8888;

            IPEndPoint point = new IPEndPoint(ipAddress, ports);
            IAsyncResult result = clientSocket.BeginConnect(point, null, clientSocket);
            connected = result.AsyncWaitHandle.WaitOne(1000, true);

            if (connected)
            {
                clientSocket.EndConnect(result);
            }
            else
            {
                clientSocket.Close();
            }
        }
        catch(Exception e)
        {
            connected = false;
            clientSocket.Close();
            throw new Exception();
        }

        if (connected)
        {
            if (recvProcess == null)
            {
                recvProcess = new Thread(new ThreadStart(ReviceMsg));
                recvProcess.IsBackground = true;
                recvProcess.Start();
                sendProcess = new Thread(new ThreadStart(SendProcess));
                sendProcess.IsBackground = true;
                sendProcess.Start();
                stopSendProcess = false; 
                stopRecvProcess = false;
            }

        }
    }

    private void SendProcess()
    {
        while (!stopSendProcess)
        {
            if (!connected)
            {
                TryConnect();
            }
            while (sendQueue.Count > 0 && connected)
            {
                string item = sendQueue.Dequeue();
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(item);
                clientSocket.BeginSend(buffer, 0, buffer.Length,SocketFlags.None, null, clientSocket);
            }
            Thread.Sleep(5);
        }
    }

    private void ReviceMsg()
    {
                while (!stopRecvProcess)
                {
                    if (!connected)
                    {
                        connected = true;
                        TryConnect();
                        break;
                    }
                    try
                    {
                        byte[] buffer = new byte[1024 * 1024];
                        int n = clientSocket.Receive(buffer);
                        string words = Encoding.UTF8.GetString(buffer, 0, n);
                        //消息加到 recvQueue
                        lock (recvQueue)
                        {
                            recvQueue.Enqueue(words);
                        }
                    }
                    catch
                    {
                        clientSocket.Disconnect(true);
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                        break;
                    }

                }
    }
    

    void OnApplicationQuit()
    {
        stopSendProcess = true;
        stopRecvProcess = true;

        if (connected)
        {
            connected = false;
            clientSocket.Close();
        }

        if (instance!=null)
        {
            instance = null;
        }

        if (recvProcess != null)
        {
            recvProcess.Abort();
            // 如果没有正确关闭线程，这里的Join就会阻塞，就会卡死编辑器
            //recvProcess.Join();
        }

        if (sendProcess != null)
        {
            sendProcess.Abort();
            //sendProcess.Join();
        }

    }

    public void SendMsg(string msg)
    {
        sendQueue.Enqueue(msg);
    }

    public void Close()
    {
        OnApplicationQuit();
    }
}
