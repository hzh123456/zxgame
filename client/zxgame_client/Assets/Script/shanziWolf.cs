using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class shanziWolf : MonoBehaviour
{

    public Text roomid;

    public Button[] players;

    public Sprite[] img;

    public GameObject startGame;

    public Text content;

    public InputField SendMsg;

    private string[] shenfen = new string[12];

    private string[] lastname = new string[12];
     
    public void SendTalkMsg()
    {
        string msg = SendMsg.text;
        if(!String.IsNullOrEmpty(msg))
        {
            StartCoroutine(SendTalkMsgIE(msg));
            SendMsg.text = "";
        }
        
    }

    IEnumerator SendTalkMsgIE(string msg)
    {
        try
        {
            string sendmsg = Command.Talk(Server.lastname, msg, Server.roomid);
            Server.socket.SendMsg(sendmsg);
        }
        catch
        {
        }
        yield return new WaitForSeconds(0);
    }

    // Use this for initialization
    void Start()
    {
        try
        {
            StartCoroutine(ShowView(Server.playernum));
            content.text += "扇子狼，房间号：" + Server.roomid + "\r\n<color=#FFFFFF>" + Server.lastname + "加入房间</color>\r\n";
        }
        catch
        {
            return;
        }
    }

    IEnumerator ShowView(int playernum)
    {
        roomid.text = "房间号：" + Server.roomid;
        for (int i = players.Length - 1; i >= playernum; i--)
        {
            players[i].image.sprite = img[2];
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
                            players[i].image.sprite = img[1];
                            players[i].GetComponentsInChildren<Text>()[0].text = "等待中";
                            players[i].GetComponentsInChildren<Text>()[1].text = "";
                        }
                        realplayernum = roomInfos.Length - 2;
                        for (int i = 0; i < roomInfos.Length - 2; i++)
                        {
                            string[] user = roomInfos[i].Split(',');
                            players[i].image.sprite = img[0];
                            players[i].GetComponentsInChildren<Text>()[0].text = user[1];
                            players[i].GetComponentsInChildren<Text>()[1].text = (i + 1).ToString();
                            shenfen[i] = user[2];
                            lastname[i] = user[1];
                            players[i].enabled = true;
                            if (user[0] == Server.username)
                            {
                                Server.shenfen = user[2];
                                Server.ZuoWei = i + 1;
                            }
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

    private int realplayernum = 0;

    private int roomstatus = 0;

    private int SkillIsUser = 0;

    public GameObject FanPai;

    public GameObject Quit;

    public void startGames() 
    {

        if (realplayernum == Server.playernum)
        {
            startGame.SetActive(false);
            StartCoroutine(startGameIE());
        }
        else
        {
            content.text += "玩家人数不足，无法开始游戏！\r\n";
        }
    }

    public GameObject QuitBig;

    public void endGame()
    {
        StartCoroutine(QuitRoomIE());
    }

    private IEnumerator startGameIE()
    {
        Server.socket.SendMsg(Command.startGame(int.Parse(Server.roomid)));
        yield return new WaitForSeconds(0);
    }

    private int[] indexs = new int[2];

    public void Button(int index)
    {
        if(roomstatus==1)
        {
            if (SkillIsUser==0)
            {
                FanPai.SetActive(false);
                if (index != -1)
                {
                    players[index - 1].GetComponent<Image>().color = new Color(255f, 0, 0);
                    players[index - 1].enabled = false;
                }
                indexs[0] = index;
                Server.socket.SendMsg(Command.Game(Server.shenfen, Server.roomid, indexs));
                SkillIsUser++;
            }
        }
    }

    private float time = 0.5f;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //在Update中每0.5s的时候同步一次  
        if (time > 0.5f)
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
            else if (!String.IsNullOrEmpty(msg) && msg.Contains("Quit"))
            {
                StartCoroutine(updateroom());
                StartCoroutine(updatetalk(msg));
            }
            else if (!String.IsNullOrEmpty(msg) && msg.Contains("Join"))
            {
                StartCoroutine(updateroom());
                StartCoroutine(updatetalk(msg));
            }
            else if (!String.IsNullOrEmpty(msg) && msg.Contains("startGame"))
            {
                string str = msg.Split('|')[1];
                Quit.SetActive(false);
                FanPai.SetActive(true);
                StartCoroutine(startGameIE2(str));
            }
            else if (!String.IsNullOrEmpty(msg) && msg.Contains("FuPan"))
            {
                Quit.SetActive(true);
                string str = msg.Split('|')[1];
                content.text += str;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        yield return new WaitForSeconds(0);
    }

    private IEnumerator startGameIE2(string msg)
    {
        roomstatus = 1;
        content.text += "游戏开始\r\n";
        for (int i = 0; i < realplayernum; i++)
        {
            if(Server.ZuoWei-1!=i)
            {
                content.text += (i + 1) + "号玩家 "+ lastname[i] +" 的身份是：<color=#FF3030>" + shenfen[i] + "</color>\r\n";
            }
        }
        content.text += msg;
        yield return new WaitForSeconds(0);
    }

    IEnumerator updatetalk(string msg)
    {
        try
        {
            string[] data = msg.Split('|')[1].Split(',');
            if (msg.Contains("msg"))
            {
                content.text += "<color=#FFFFFF>" + data[0] + "：" + data[1] + "</color>\r\n";
            }
            else if (msg.Contains("Quit"))
            {
                content.text += "<color=#FFFFFF>" + data[1] + "退出房间</color>\r\n";
            }
            else if (msg.Contains("Join"))
            {
                content.text += "<color=#FFFFFF>" + data[1] + "加入房间</color>\r\n";
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
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        finally
        {
            SceneManager.LoadScene(1);
        }
        yield return new WaitForSeconds(0);
    }



}
