﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    public GameObject UI_Lobby;

    public GameObject OutLobbyText;
    public GameObject IntroBG;
    public GameObject IntroLogo;


    public GameObject UI_Lobby_Info;
    public GameObject UI_Lobby_Shop;
    public GameObject UI_Lobby_Credit;
    public GameObject UI_Lobby_Option;

    public GameObject SoundOn;
    public GameObject SoundOff;

    public bool isLobby;

    public bool isSoundOn;
    public bool isSoundPlay;
    public bool isFuncOn;
    public bool isInfoOn;
    public bool isShopOn;
    public bool isCreditOn;
    public bool isOptionOn;


    public string sceneName;

    public GameObject Func;



    // Update is called once per frame
    void Update()
    {
        if(!isLobby)
        {
            inLobby();
        }
        
        else
        {
            funcOn();
        } 
    }

    public string checkSceneName()
    {
        sceneName = SceneManager.GetActiveScene().name;
        return sceneName;
    }

    public void inLobby()
    {
        if(Input.anyKeyDown)
        {
            isLobby = true;
            IntroBG.SetActive(false);
            IntroLogo.SetActive(false);
            OutLobbyText.SetActive(false);
            UI_Lobby.SetActive(true);
        }
        
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
        isSoundPlay = true;
        isInfoOn = false;
        isShopOn = false;
        isCreditOn = false;
        isOptionOn = false;
        isFuncOn = false;
        UI_Lobby_Info.SetActive(false);
        UI_Lobby_Shop.SetActive(false);
        UI_Lobby_Credit.SetActive(false);
        UI_Lobby_Option.SetActive(false);
        Func.SetActive(false);
    }

    public void EnterInfo()
    {
        isSoundPlay = true;
        isFuncOn = true;
        isInfoOn = true;
        Func.SetActive(true);

    }

    public void EnterShop()
    {
        isSoundPlay = true;
        isFuncOn = true;
        isShopOn = true;
        Func.SetActive(true);

    }
    public void EnterCredit()
    {
        isSoundPlay = true;
        isFuncOn = true;
        isCreditOn = true;
        Func.SetActive(true);
    }
    public void EnterOption()
    {
        isSoundPlay = true;
        isFuncOn = true;
        isOptionOn = true;
        Func.SetActive(true);
    }

    public void optionFunc()
    {
        if(isSoundOn)
        {
            isSoundOn = false;
            SoundOff.SetActive(true);
            SoundOn.SetActive(false);
        }

        else
        {
            isSoundOn = true;
            SoundOff.SetActive(false);
            SoundOn.SetActive(true);
        }
    }


    public void GameStart()
    {
        //isLobby = false;
        SceneManager.LoadScene("Ingame");
    }

    public void goLobby()
    {
        //isLobby = true;
        SceneManager.LoadScene("Lobby");
    }

}
