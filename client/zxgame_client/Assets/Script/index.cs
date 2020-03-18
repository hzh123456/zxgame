using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class index : MonoBehaviour {

    public Text info;

    public GameObject[] Panels;

    public GameObject[] CreatePanels;

    public GameObject[] JoinPanels;

	// Use this for initialization
	void Start () {
        info.text = "用户：" + Server.lastname;
	}
	
	// Update is called once per frame
	void Update () {
		
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


    public void back()
    {
        SceneManager.LoadScene(0);
    }

    public void OneNightWolf()
    {
        int index = 0;
        for (int i = 0; i < Panels.Length;i++ )
        {
            if (i == index)
            {
                Panels[i].SetActive(true);
            }
            else
            {
                Panels[i].SetActive(false);
            }
        }
    }

    public void ShanziWolf()
    {
        int index = 1;
        for (int i = 0; i < Panels.Length; i++)
        {
            if (i == index)
            {
                Panels[i].SetActive(true);
            }
            else
            {
                Panels[i].SetActive(false);
            }
        }
    }

    public void newGame1()
    {
        int index = 2;
        for (int i = 0; i < Panels.Length; i++)
        {
            if (i == index)
            {
                Panels[i].SetActive(true);
            }
            else
            {
                Panels[i].SetActive(false);
            }
        }
    }

    public void newGame2()
    {
        int index = 3;
        for (int i = 0; i < Panels.Length; i++)
        {
            if (i == index)
            {
                Panels[i].SetActive(true);
            }
            else
            {
                Panels[i].SetActive(false);
            }
        }
    }

    public void CreateOpen(int index)
    {
        for (int i = 0; i < CreatePanels.Length; i++)
        {
            if (i == index)
            {
                CreatePanels[i].SetActive(true);
            }
            else
            {
                CreatePanels[i].SetActive(false);
            }
        }
    }

    public void CreateBack(int index)
    {
        CreatePanels[index].SetActive(false);
    }

    public void JoinBack(int index)
    {
        JoinPanels[index].SetActive(false);
    }

    public void JoinOpen(int index)
    { 
        for (int i = 0; i < JoinPanels.Length; i++)
        {
            if (i == index)
            {
                JoinPanels[i].SetActive(true);
            }
            else
            {
                JoinPanels[i].SetActive(false);
            }
        }
    }

    public void PanelHide() 
    {
        for (int i = 0; i < Panels.Length; i++)
        { 
            Panels[i].SetActive(false);
        }
    }

}
