using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    
    private static SoundManager instance = null;

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

    public static SoundManager Instance
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
    
    public AudioClip Lobby_Func_Upgrade;

    [System.Serializable]
    public struct BgmType
    {
        public string name;
        public AudioClip audio;
    }
    public BgmType[] BGMList;

    public BgmType[] SoundList;

    private string NowSoundname = "";
    private string NowBGMname = "";

    private AudioSource BGM;
    private AudioSource audioSource;


    private int isSoundOn;
    private int isFuncOn;
    private int isLobby;
    private int isShop;

    [SerializeField]
    private bool lobbyDelay;

    [SerializeField]
    private bool isIngame;

    [SerializeField]
    private float fdt;

    [SerializeField]
    private float lobbyDelayTime;

    [SerializeField]
    private float ingameDelayTime;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("isShop", 0);
        PlayerPrefs.SetInt("isLobby", 0);
        PlayerPrefs.SetInt("isDragonDie", -3);
        PlayerPrefs.Save();
        
        fdt = 0;
        
        isSoundOn = 1;
        isIngame = false;
        lobbyDelay = true;

        PlayerPrefs.SetInt("isSoundOn", 1);
        PlayerPrefs.Save();

        // Invoke("PlayBGM", 4.0f);

        // NowBGMname = SceneManager.GetActiveScene().name;
        audioSource = gameObject.AddComponent<AudioSource>();
        BGM = gameObject.AddComponent<AudioSource>();
        BGM.loop = true;
        audioSource.loop = false;
        // if (BGMList.Length > 0) PlayBGM(BGMList[0].name);
    }

    // Update is called once per frame
    void Update()
    {
        isFuncOn = PlayerPrefs.GetInt("isDragonDie");
        isSoundOn = PlayerPrefs.GetInt("isSoundOn");
        isLobby = PlayerPrefs.GetInt("isLobby");
        isShop = PlayerPrefs.GetInt("isShop");

        Debug.Log("isLobby : " + isLobby);
        Debug.Log("isFuncOn : " + isFuncOn);

        fdt += Time.deltaTime;

        if (isSoundOn == 1)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Lobby":
                    if(isIngame)
                    {
                        fdt = 0;
                        isIngame = false;
                    }
                    
                    else
                    {
                        if (isLobby == 0)
                        {
                            if (lobbyDelay)
                            {
                                audioSource.mute = true;
                                BGM.mute = true;
                            }

                        }

                        else
                        {
                            lobbyDelay = true;
                            BGM.mute = false;
                            audioSource.volume = 1.0f;
                            audioSource.mute = false;
                            PlayBGM("Lobby");
                            fdt = 0;
                        }

                        if (fdt > lobbyDelayTime)
                        {
                            BGM.mute = false;
                            lobbyDelay = false;
                            PlayBGM("Lobby");
                            fdt = 0;
                        }

                        checkLobbyBtn();
                    }
                    
                    break;

                case "Ingame":
                    isIngame = true;
                    lobbyDelay = true;

                    if (fdt > ingameDelayTime)
                    {
                        PlayBGM(BGMList[1].name);
                        BGM.mute = false;
                        BGM.volume = 0.5f;
                        audioSource.mute = false;
                    }


                    else
                    {
                        BGM.mute = true;
                    }                   
                    
                    
                    break;

                case "Result":
                    fdt = 0;
                    BGM.mute = false;
                    lobbyDelay = true;
                    PlayBGM(BGMList[2].name);
                    BGM.volume = 1.0f;
                    audioSource.mute = false;
                    break;

            }
        }

        else
        {
            PlayerPrefs.SetInt("isShop", 0);
            PlayerPrefs.Save();

            BGM.mute = true;
            audioSource.mute = true;
        }


    }

    public void PlayBGM(string name)
    {
        if (NowBGMname.Equals(name)) return;

        for (int i = 0; i < BGMList.Length; ++i)
            if (BGMList[i].name.Equals(name))
            {
                BGM.clip = BGMList[i].audio;
                BGM.Play();
                NowBGMname = name;
            }
    }

    public void PlaySound(string name)
    {
        if (NowSoundname.Equals(name)) return;

        for (int i = 0; i < SoundList.Length; ++i)
            if (SoundList[i].name.Equals(name))
            {
                audioSource.clip = SoundList[i].audio;
                audioSource.Play();
                NowSoundname = name;
            }
    }



    void checkLobbyBtn()
    {
        if (isLobby == 1)
        {
            if (isFuncOn == (-1))
            {
                PlaySound(SoundList[0].name);
            }

            else if (isFuncOn == (-2))
            {
                PlaySound(SoundList[1].name);
            }


            if (isFuncOn == (-1) && isShop == 1)
            {
                upgradeBtn();
            }
        }
    }

    void upgradeBtn()
    {
        // Debug.Log("Upgrade");
        audioSource.PlayOneShot(Lobby_Func_Upgrade);

        PlayerPrefs.SetInt("isShop", 0);
        PlayerPrefs.Save();
    }
}
