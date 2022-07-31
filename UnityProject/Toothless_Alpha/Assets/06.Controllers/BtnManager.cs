using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    public GameObject UI_Lobby_Info;
    public GameObject UI_Lobby_Shop;
    public GameObject UI_Lobby_Credit;
    public GameObject UI_Lobby_Option;

    public bool isInfoOn;
    public bool isShopOn;
    public bool isCreditOn;
    public bool isOptionOn;

    public GameObject Func;

    // Update is called once per frame
    void Update()
    {
        funcOn();
    }

    public void funcOn()
    {
        if(isInfoOn)
        {
            UI_Lobby_Info.SetActive(true);
        }

        else if(isShopOn)
        {
            UI_Lobby_Shop.SetActive(true);
        }

        else if(isCreditOn)
        {
            UI_Lobby_Credit.SetActive(true);
        }

        else if(isOptionOn)
        {
            UI_Lobby_Option.SetActive(true);
        }

        else
        {
            return;
        }
    }

    public void FuncExit()
    {
        isInfoOn = false;
        isShopOn = false;
        isCreditOn = false;
        isOptionOn = false;
        UI_Lobby_Info.SetActive(false);
        UI_Lobby_Shop.SetActive(false);
        UI_Lobby_Credit.SetActive(false);
        UI_Lobby_Option.SetActive(false);
        Func.SetActive(false);
    }

    public void EnterInfo()
    {
        isInfoOn = true;
        Func.SetActive(true);
    }

    public void EnterShop()
    {
        isShopOn = true;
        Func.SetActive(true);
    }
    public void EnterCredit()
    {
        isCreditOn = true;
        Func.SetActive(true);
    }
    public void EnterOption()
    {
        isOptionOn = true;
        Func.SetActive(true);
    }

    public void UpgradeCom()
    {
        Debug.Log("Complete Upgrade");
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Ingame");
    }

    public void goLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
