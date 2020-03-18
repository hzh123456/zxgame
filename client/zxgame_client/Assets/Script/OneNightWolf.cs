using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OneNightWolf : MonoBehaviour
{

    public Text roomid;

    public Button[] players;

    public Sprite[] img;

    public GameObject startGame;

    public Text content;

    public InputField SendMsg;

    public void SendTalkMsg()
    {
        string msg = SendMsg.text;
        if(!String.IsNullOrEmpty(msg))
        {
            StartCoroutine(SendTalkMsgIE(msg));
        }
        
    }

    IEnumerator SendTalkMsgIE(string msg)
    {
        try
        {
            string sendmsg = Command.Talk(Server.lastname, msg, Server.roomid);
            Server.socket.SendMsg(sendmsg);
        }
        catch { }
        yield return new WaitForSeconds(0);
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(ShowView(Server.playernum));
        if(Server.IsFangZhu)
        {
            content.text += Server.lastname + "加入房间\r\n";
        }
    }

    IEnumerator ShowView(int playernum)
    {
        roomid.text = "房间号：" + Server.roomid;
        for (int i = players.Length - 1; i >= playernum; i--)
        {
            players[i].image.sprite = img[2];
            players[i].enabled = false;
            players[i].GetComponentInChildren<Text>().text = "";
        }
        startGame.SetActive(Server.IsFangZhu);
        StartCoroutine(updateroom());
        yield return new WaitForSeconds(0);
    }

    IEnumerator updateroom()
    {
        try
        {
            Server.socket.SendMsg(Command.RoomInfo(Server.roomid));
            bool IsQuit = false;
            while (true)
            {
                string msg = Server.socket.GetMsg();

                if (!String.IsNullOrEmpty(msg) && msg != "wait")
                {
                    if (msg == "QuitRoom")
                    {
                        IsQuit = true;
                        break;
                    }
                    else if (msg.Contains("roomInfo"))
                    {
                        string[] roomInfos = msg.Split('|');

                        int playnum = int.Parse(roomInfos[roomInfos.Length - 1]);
                        for (int i = 0; i < playnum; i++)
                        {
                            players[i].enabled = true;
                            players[i].image.sprite = img[1];
                            players[i].GetComponentsInChildren<Text>()[0].text = "等待中";
                            players[i].GetComponentsInChildren<Text>()[1].text = "";
                            
                        }
                        for (int i = 0; i < roomInfos.Length - 2; i++)
                        {
                            string[] user = roomInfos[i].Split(',');
                            players[i].image.sprite = img[0];
                            players[i].GetComponentsInChildren<Text>()[0].text = user[1];
                            players[i].GetComponentsInChildren<Text>()[1].text = (i+1).ToString();
                        }
                        break;
                    }
                }
            }
            if (IsQuit)
            {
                QuitRoom();
            }
        }
        catch
        {
            
        }
        yield return new WaitForSeconds(0);
    }

    void OnApplicationQuit()
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

    private float time = 1f;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //在Update中每1s的时候同步一次  
        if (time > 1f)
        {
            StartCoroutine(FlushView());
            time = 0;
        }
    }

    IEnumerator FlushView()
    {
        try
        {
            string msg = Server.socket.GetMsg();
            if (!String.IsNullOrEmpty(msg) && msg == "QuitRoom")
            {
                SceneManager.LoadScene(1);
            }
            else if (!String.IsNullOrEmpty(msg) && msg.Contains("msg"))
            {
                StartCoroutine(updatetalk(msg));
            }
            else if (!String.IsNullOrEmpty(msg) && msg=="wait")
            {
                StartCoroutine(updateroom());
            }
            else if (!String.IsNullOrEmpty(msg) && msg.Contains("Quit"))
            {
                StartCoroutine(updatetalk(msg));
            }
            else if (!String.IsNullOrEmpty(msg) && msg.Contains("Join"))
            {
                StartCoroutine(updatetalk(msg));
            }
        }
        catch
        { }
        yield return new WaitForSeconds(0);
    }

    IEnumerator updatetalk(string msg)
    {
        try
        {
            string[] data = msg.Split('|')[1].Split(',');
            if (msg.Contains("msg"))
            {
                content.text += data[0] + "：" + data[1] + "\r\n";
            }
            else if (msg.Contains("Quit"))
            {
                content.text += data[1] + "退出房间\r\n";
            }
            else if (msg.Contains("Join"))
            {
                content.text += data[1] + "加入房间\r\n";
            }
            
        }
        catch
        {
        }

        yield return new WaitForSeconds(0);
    }

    public void QuitRoom()
    {
        StartCoroutine(QuitRoomIE());
    }

    IEnumerator QuitRoomIE()
    {
        try
        {
            Server.socket.SendMsg(Command.QuitRoom(Server.username, int.Parse(Server.roomid)));
        }
        catch
        { }
        finally
        {
            SceneManager.LoadScene(1);
        }
        yield return new WaitForSeconds(0);
    }
}
