using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Connect : MonoBehaviour {

    private float time;

    public Dropdown dropDown;

    public GameObject ShowPanel;
    public Text Msg;

    public Toggle[] toggles;

    public InputField[] roomids;

    public Button[] creates;
    public Button[] joins;

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

    public void JoinGame(int index)
    {
        joins[index].enabled = false;
        int roomid;
        string roomidstr = roomids[index].text;
        bool f = int.TryParse(roomidstr, out roomid);
        if (string.IsNullOrEmpty(roomidstr) || !f)
        {
            ShowMsg("请输入正确的房间号！");
        }
        else
        {
            try
            {

                Server.socket.SendMsg(Command.IsJoinGame(roomid, index));
                string IsJoin = "";
                while (true)
                {
                    string msg = Server.socket.GetMsg();
                    if (msg != "wait" && !string.IsNullOrEmpty(msg) && ( msg=="False" || msg=="True" || msg=="Playing" ))
                    {
                        IsJoin = msg;
                        break;
                    }
                }
                if (IsJoin == "False")
                {
                    ShowMsg("房间不存在！");
                }
                else if (IsJoin == "Playing")
                {
                    ShowMsg("房间正在游戏中！");
                }
                else if (IsJoin == "True")
                {
                    switch (index)
                    {
                        case 0:
                            string msg = Command.JoinGame(Server.username, roomid, Server.lastname);
                            StartCoroutine(SendMsg(msg));
                            Server.roomid = roomid.ToString();
                            Server.IsFangZhu = false;
                            SceneManager.LoadScene(2);
                            break;
                        case 1:
                            ShowMsg("还未开发，敬请期待");
                            break;
                        case 2:
                            ShowMsg("还未开发，敬请期待");
                            break;
                        case 3:
                            ShowMsg("还未开发，敬请期待");
                            break;
                    }
                }
            }
            catch
            {
                return;
            }
        }
        joins[index].enabled = true;
    }

    IEnumerator SendMsg(string msg)
    {
        Server.socket.SendMsg(msg);
        yield return new WaitForSeconds(0);
    }

    public void CreateGame(int index)
    {
        creates[index].enabled = false;
        try
        {
            switch (index)
            {
                case 0:
                    int num = -5;
                    List<Toggle> npcs = new List<Toggle>();
                    foreach (Toggle toggle in toggles)
                    {
                        if (toggle.isOn)
                        {
                            npcs.Add(toggle);
                        }
                    }
                    bool flag = int.TryParse(dropDown.value.ToString(), out num);
                    num += 4;
                    if (!flag)
                    {
                        ShowMsg("人数转换错误！");
                    }
                    else if (!toggles[9].isOn && !toggles[10].isOn && !toggles[11].isOn)
                    {
                        ShowMsg("必须至少选择1个平民！");
                    }
                    else if (!toggles[0].isOn && !toggles[1].isOn && !toggles[7].isOn && !toggles[8].isOn)
                    {
                        ShowMsg("必须至少选择1个狼人！");
                    }
                    else if (npcs.Count != num + 3)
                    {
                        ShowMsg("角色与玩家不匹配！");
                    }
                    else if (npcs.Count == num + 3)
                    {
                        StartCoroutine(CreateGameIE(Server.username, num, index, Server.lastname));
                    }
                    else
                    {
                        ShowMsg("创建房间错误，联系管理员！");
                    }

                    break;
                case 1:
                    ShowMsg("还未开发，敬请期待");
                    break;
                case 2:
                    ShowMsg("还未开发，敬请期待");
                    break;
                case 3:
                    ShowMsg("还未开发，敬请期待");
                    break;
            }
        }
        catch
        {
            return;
        }
        creates[index].enabled = true;
    }

    IEnumerator CreateGameIE(string username,int num,int index,string lastname)
    {
        string sendmsg = Command.CreateRoom(username, num, index, 0, lastname);
        Server.socket.SendMsg(sendmsg);
        bool f = false;
        while (true)
        {
            string msg = Server.socket.GetMsg();
            if (msg != "wait" && !string.IsNullOrEmpty(msg))
            {
                Server.roomid = msg;
                Server.playernum = num;
                Server.IsFangZhu = true;
                f = true;
                break;
            }
        }
        if (f)
        {
            SceneManager.LoadScene(2);
        }
        yield return new WaitForSeconds(0);
    }

    private void ShowMsg(string msg)
    {
        Msg.text = msg;
        ShowPanel.SetActive(true);
        StartCoroutine(Toggle());
    }

    private IEnumerator Toggle()
    {
        yield return new WaitForSeconds(1.0f);
        ShowPanel.SetActive(false);
    }

}
