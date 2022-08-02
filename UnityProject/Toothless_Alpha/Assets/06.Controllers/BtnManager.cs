using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    /*
    private static BtnManager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            instance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            //gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
            //나는 헷갈림 방지를 위해 this를 붙여주기도 한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
            Destroy(this.gameObject);
        }
    }

    public static BtnManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    */

    // public SoundManager soundManager;

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


    private int isSoundOn;
    public bool isSoundPlay;
    public bool isFuncOn;
    public bool isInfoOn;
    public bool isShopOn;
    public bool isCreditOn;
    public bool isOptionOn;


    public string sceneName;

    public GameObject Func;


    void Start()
    {
        isSoundOn = PlayerPrefs.GetInt("isSoundOn");
    }

    // Update is called once per frame
    void Update()
    {
        switch (checkSceneName())
        {
            case "Lobby":
                if (!isLobby)
                {
                    inLobby();
                }

                else
                {
                    funcOn();
                }
                break;

            case "Ingame":
                isLobby = false;
                break;

            case "Result":
                isLobby = false;
                break;
        }
        if (checkSceneName() == "Lobby")
        {

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
            PlayerPrefs.SetInt("Stage",-3);
            PlayerPrefs.Save();
            isLobby = true;
            IntroBG.SetActive(false);
            IntroLogo.SetActive(false);
            OutLobbyText.SetActive(false);
            UI_Lobby.SetActive(true);
            
            //Debug.Log("isLobby = -3");
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

            if (isSoundOn == 0)
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
        PlayerPrefs.SetInt("isDragonDie",-2);
        PlayerPrefs.Save();
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
        PlayerPrefs.SetInt("isShop", 0);
        PlayerPrefs.Save();
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
        if (isSoundOn == 1)
        {
            isSoundOn = 0;
        }

        else
        {
            isSoundOn = 1;
        }

        PlayerPrefs.SetInt("isSoundOn", isSoundOn);
        PlayerPrefs.Save();
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
        PlayerPrefs.SetInt("Stage",-2);
        PlayerPrefs.SetInt("isDragonDie", -3);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Lobby");
    }

}
