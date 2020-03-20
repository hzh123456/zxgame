using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets.Script;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class login : MonoBehaviour {

    private float time;

    public GameObject obj;

    public InputField Username;
    public InputField Password;
    public InputField port;
    public Text ErrorInfo;
    public Button loginbtn;

    private bool flag = false;

    private string errormsg="";

    private Thread th = null;

    void Start()
    {
        string str = Server.SendRequestGetResponse("http://hzhweb.picp.vip/","");
        if(!String.IsNullOrEmpty(str))
        {
            port.text = str;
        } 
    }

    void Update()
    {
        
        obj.SetActive(flag);
        if(flag)
        {
            ErrorInfo.text = errormsg;
        }
    }

    public void Quit()
    {
        try
        {
            if (Server.Connected())
            {
                Server.Close();
            }
        }
        catch
        {
        }
        finally
        {
            Application.Quit();
        }
        
    }

    private void ShowOrHide1(string msg)
    {
        flag = true;
        errormsg = msg;
        Toggle1();
    }

    private void Connect()
    {
        try
        {
            int ports=-1;
            bool flags = int.TryParse(port.text,out ports);
            if (!flags || ports<=0)
            {
                ShowOrHide1("端口号错误");
            }
            else
            {
                Server.Connect(ports);
            }
        }
        catch (MyException e)
        {
            ShowOrHide1("连接超时");
        }
        finally
        {
            if(th!=null)
            {
                th.Abort();
            }
            
        }
    }

    IEnumerator ShowOrHideMain(string msg)
    {
        flag = true;
        errormsg = msg;
        obj.SetActive(true);
        ErrorInfo.text = msg;
        yield return new WaitForSeconds(1.0f);
        obj.SetActive(false);
        flag = false;
    }

    public void Islogin() 
    {
        loginbtn.enabled = false;
        try
        {
            if (!Server.Connected())
            {
                th = new Thread(new ThreadStart(Connect));
                th.IsBackground = true;
                th.Start();
            }
            Thread.Sleep(100);
            if (Server.Connected())
            {
                string username = Username.text;
                string password = Password.text;
                if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                {
                    StartCoroutine(ShowOrHideMain("用户名或密码不能为空"));
                }
                else
                {
                    StartCoroutine(IsLoginIE(username,password));
                }
            }
        }
        catch(Exception e)
        {
            StartCoroutine(ShowOrHideMain(e.ToString()));
        }
        finally
        {
            loginbtn.enabled = true;
        }
    }

    IEnumerator IsLoginIE(string username,string password)
    {
        Server.socket.SendMsg(Command.IsLogin(username, password));
        while (true)
        {
            string msg = Server.socket.GetMsg();
            if (!string.IsNullOrEmpty(msg) && msg != "wait")
            {
                if (msg == "NO")
                {
                    StartCoroutine(ShowOrHideMain("用户名或密码错误"));
                }
                else if (msg == "Online")
                {
                    StartCoroutine(ShowOrHideMain("当前用户已登录！"));
                }
                else
                {
                    Server.lastname = msg;
                    Server.username = Username.text;
                    SceneManager.LoadScene(1);
                }
                break;
            }
        }
        yield return new WaitForSeconds(0);
    }

    private void Toggle1()
    {
        Thread.Sleep(1000);
        flag = false;
    }

    void OnApplicationQuit()
    {
        Quit();
    }
}
 