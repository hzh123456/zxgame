using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Assets.Script;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Server {

    public static Sockets socket = null;

    public static string username;

    public static string lastname;

    public static string roomid;

    public static int playernum;

    public static bool IsFangZhu = false;

    public static string shenfen;

    public static int ZuoWei;

    public static void Connect(int port)
    {
        try
        {
            socket = Sockets.GetInstance(port);
        }
        catch
        {
            throw new Exception();
        }
    }

    public static bool Connected()
    {
        if (socket == null)
        {
            return false;
        }
        else
        {
            return socket.Connected();
        }
    }

    public static void Close()
    {
        socket.Close();
    }

    public static string SendRequestGetResponse(string RequestUrl, string Params)
    {
        HttpWebRequest HWRequest = null;
        string NewUrl = string.Empty;
        string RetCode = string.Empty;

        try
        {
            HWRequest = (HttpWebRequest)WebRequest.Create(RequestUrl);
            HWRequest.KeepAlive = true;
            HWRequest.Timeout = 30000;
            HWRequest.Method = "POST";
            HWRequest.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse HWResponse = null;
            byte[] reqParams = System.Text.Encoding.ASCII.GetBytes(Params);
            Stream st = HWRequest.GetRequestStream();
            st.Write(reqParams, 0, reqParams.Length);
            st.Flush();
            st.Close();

            HWResponse = (HttpWebResponse)HWRequest.GetResponse();
            Stream ResSt = HWResponse.GetResponseStream();
            StreamReader sr = new StreamReader(ResSt, System.Text.Encoding.UTF8);
            RetCode = sr.ReadToEnd();
            ResSt.Close();
            ResSt.Close();
            HWResponse.Close();
            ResSt.Dispose();
            sr.Dispose();
            st.Dispose();
        }
        catch
        {
            RetCode = string.Empty;
        }

        return RetCode;
    }

}
