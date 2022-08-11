using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    [Header("LobbyAudioSource")]
    [SerializeField] private AudioSource[] audioSources;

    [Header("LobbyIntro")]
    // 로비 입장 전 인트로 사용 리소스
    public GameObject OutLobbyText;
    public GameObject IntroBG;
    // public GameObject IntroLogo;

    [Header("InLobby")]
    // 기본 로비 배경 
    public GameObject UI_Lobby;

    [Header("Lobby_Func")]
    // 로비 각 기능별 부모 객체
    public GameObject UI_Lobby_Info;
    public GameObject UI_Lobby_Shop;
    public GameObject UI_Lobby_Credit;
    public GameObject UI_Lobby_Option;

    [Header("Sound ON / OFF")]
    // Option : 소리 On / Off 아이콘
    public GameObject SoundOn;
    public GameObject SoundOff;

    [Header("Account Reset")]
    // 계정 초기화 관련
    public GameObject Warning;
    public GameObject WarnParent;
    public GameObject ResetGame;

    [Header("Escape Game")]
    // 게임 종료 관련
    public GameObject EscapeGame;

    // Lobby인지를 체크하는 bool 변수
    public bool isLobby;

    private bool isSoundOn;
    public bool isFuncOn;
    public bool isInfoOn;
    public bool isShopOn;
    public bool isCreditOn;
    public bool isOptionOn;

    private bool isExitOn;

    public string sceneName;

    public GameObject Func;

    // Update is called once per frame
    void Update()
    {
        isSoundOn = ScoreManager.GetIsSoundOn();

        switch (checkSceneName())
        {
            case "Lobby":

            if(isSoundOn)
            {
                for(int i = 0; i < audioSources.Length; i++)
                {
                    audioSources[i].mute = false;
                }
            }

            else
            {
                for(int i = 0; i < audioSources.Length; i++)
                {
                    audioSources[i].mute = true;
                }
                }
                if (!isLobby)
                {
                    inLobby();
                }

                else
                {
                    funcOn();
                    EnterOptionToEsc();
                }
                break;

            case "Ingame":
                isLobby = false;
                funcOn();
                break;

            case "Result":
                isLobby = false;
                break;
        }
    }

    public string checkSceneName()
    {
        sceneName = SceneManager.GetActiveScene().name;
        return sceneName;
    }

    public void inLobby()
    {
        if (Input.anyKeyDown)
        {
            SoundManager.Instance.PlaySound_01("NorBtn");
            ScoreManager.SetIsLobby(true);
            PlayerPrefs.Save();
            isLobby = true;
            IntroBG.SetActive(false);
            // IntroLogo.SetActive(false);
            OutLobbyText.SetActive(false);
            UI_Lobby.SetActive(true);
        }
    }




    public void funcOn()
    {
        if (isInfoOn)
        {
            
            UI_Lobby_Info.SetActive(true);
            PlayerPrefs.SetInt("isDragonDie",-1);
            PlayerPrefs.Save();
        }

        else if (isShopOn)
        {

            UI_Lobby_Shop.SetActive(true);
            PlayerPrefs.SetInt("isDragonDie",-1);
            PlayerPrefs.Save();
            
        }

        else if (isCreditOn)
        {

            UI_Lobby_Credit.SetActive(true);
            PlayerPrefs.SetInt("isDragonDie",-1);
            PlayerPrefs.Save();
        }

        else if (isOptionOn)
        {

            UI_Lobby_Option.SetActive(true);
            PlayerPrefs.SetInt("isDragonDie",-1);
            PlayerPrefs.Save();

            if (!isSoundOn)
            {
                SoundOff.SetActive(true);
                SoundOn.SetActive(false);
            }

            else
            {
                SoundOff.SetActive(false);
                SoundOn.SetActive(true);
            }
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
        isFuncOn = false;
        UI_Lobby_Info.SetActive(false);
        UI_Lobby_Shop.SetActive(false);
        UI_Lobby_Credit.SetActive(false);
        UI_Lobby_Option.SetActive(false);
        Func.SetActive(false);
        PlayerPrefs.SetInt("isDragonDie",-2);
        PlayerPrefs.Save();
    }

    public void EnterInfo()
    {
        isFuncOn = true;
        isInfoOn = true;
        Func.SetActive(true);

    }

    public void EnterShop()
    {
        PlayerPrefs.SetInt("isShop", 0);
        PlayerPrefs.Save();
        isFuncOn = true;
        isShopOn = true;
        Func.SetActive(true);

    }
    public void EnterCredit()
    {
        isFuncOn = true;
        isCreditOn = true;
        Func.SetActive(true);
    }
    public void EnterOption()
    {
        isFuncOn = true;
        isOptionOn = true;
        Func.SetActive(true);
    }

    public void EnterOptionToEsc()
    {
        if(!Input.GetKeyDown(KeyCode.Escape))
            return;
        
        SoundManager.Instance.PlaySound_02("Lobby_MenuIn");
        isFuncOn = true;
        isOptionOn = true;
        Func.SetActive(true);
    }

    public void optionFunc()
    {
        if (isSoundOn)
        {
            ScoreManager.SetIsSoundOn(false);
        }

        else
        {
            ScoreManager.SetIsSoundOn(true);
        }

    }


    public void GameStart()
    {
        //isLobby = false;
        Time.timeScale = 1.0f;

        PlayerPrefs.SetInt("Stage",1);
        PlayerPrefs.SetInt("isDragonDie",1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Ingame");
    }

    public void goLobby()
    {
        //isLobby = true;

        Time.timeScale = 1.0f;
        
        ScoreManager.SetIsLobby(false);

        SceneManager.LoadScene("Lobby");
    }


    public void resetAccount()
    {
        if(isOptionOn)
        {
            Warning.SetActive(true);
            WarnParent.SetActive(true);
            ResetGame.SetActive(false);
        }    
    }

    public void resetClicked()
    {
        if(isOptionOn)
        {
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.SetInt("AtkUG", 1);
            PlayerPrefs.SetInt("Exp", 0);
            PlayerPrefs.SetInt("Coin", 0);
            PlayerPrefs.SetInt("BStage",0);
            PlayerPrefs.Save();
            FuncExit();
            WarnParent.SetActive(false);
            ResetGame.SetActive(true);
        }
    }

    public void resetNotClicked()
    {
        if(isOptionOn)
        {
            Warning.SetActive(false);
        }
    }

    public void escapeGame()
    {
        if(!isExitOn)
        {
            isExitOn = true;
            Time.timeScale = 0.0f;
            EscapeGame.SetActive(true);
        }
    }

    public void escapeGame_Yes()
    {
        # if UNITY_EDITOR
            SoundManager.Instance.PlaySound_01("NorBtn");
            UnityEditor.EditorApplication.isPlaying = false;
        # else 
            SoundManager.Instance.PlaySound_01("NorBtn");
            Application.Quit();
        # endif
    }

    public void escapeGame_No()
    {  
        SoundManager.Instance.PlaySound_01("NorBtn");
        isExitOn = false;
        Time.timeScale = 1.0f;
        EscapeGame.SetActive(false);
    }
}