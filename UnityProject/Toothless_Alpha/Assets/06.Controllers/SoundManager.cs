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

    [System.Serializable]
    public struct AudioType
    {
        public string name;
        public AudioClip audio;
    }
    public AudioType[] BGMList;

    public AudioType[] SoundList;

    private string NowSoundname = "";
    private string NowBGMname = "";

    private AudioSource BGM;
    private AudioSource audioSource_01;
    private AudioSource audioSource_02;
    private AudioSource audioSource_03;
    private AudioSource audioSource_04;
    private AudioSource audioSource_05;

    private bool isDelay;
    private bool isIngame;

    private bool isSoundOn;

    private bool isLobby;

    private float fdt;
    [SerializeField] private float delayTime;

    private void Start()
    {
        BGM = gameObject.AddComponent<AudioSource>();
        audioSource_01 = gameObject.AddComponent<AudioSource>();
        audioSource_02 = gameObject.AddComponent<AudioSource>();
        audioSource_03 = gameObject.AddComponent<AudioSource>();
        audioSource_04 = gameObject.AddComponent<AudioSource>();
        audioSource_05 = gameObject.AddComponent<AudioSource>();

        BGM.loop = true;
        isIngame = false;

    }

    void Update()
    {
        fdt += Time.deltaTime;

        isLobby = ScoreManager.GetIsLobby();
        isSoundOn = ScoreManager.GetIsSoundOn();

        if (isSoundOn)
        {
            SoundManager.Instance.audioSourceOn();
            
            switch (SceneManager.GetActiveScene().name)
            {
                case "Lobby":
                    if (isIngame)
                    {
                        // isLobby = false;
                        isIngame = false;
                        fdt = 0;
                    }
                    
                    if (!isLobby)
                    {
                        if (fdt > delayTime)
                        {
                            Debug.Log("SoundON");
                            SoundManager.Instance.BGMOn();
                            SoundManager.Instance.PlayBGM("Lobby");
                            SoundManager.Instance.BGMVolume(1.0f);
                            ScoreManager.SetIsLobby(true);
                        }

                        else
                        {
                            Debug.Log("SoundOff");
                            SoundManager.Instance.BGMOff();
                        }
                    }

                    else
                    {
                        fdt = 0;
                        SoundManager.Instance.BGMOn();
                        SoundManager.Instance.PlayBGM("Lobby");
                        SoundManager.Instance.BGMVolume(1.0f);
                    }

                    break;

                case "Ingame":
                    if (!isIngame)
                    {
                        isIngame = true;
                        fdt = 0;
                    }


                    if (fdt > delayTime)
                    {
                        Debug.Log("SoundON");
                        SoundManager.Instance.BGMOn();
                        SoundManager.Instance.PlayBGM("Ingame");
                        SoundManager.Instance.BGMVolume(0.5f);

                    }

                    else
                    {
                        Debug.Log("SoundOff");
                        SoundManager.Instance.BGMOff();
                    }
                    break;

                case "Result":
                    SoundManager.Instance.PlayBGM("Result");
                    break;
            }
        }

        else
        {
            SoundManager.Instance.BGMOff();
            SoundManager.Instance.audioSourceOff();

        }

    }

    public void BGMOn()
    {
        BGM.mute = false;
    }

    public void BGMOff()
    {
        BGM.mute = true;
    }

    public void audioSourceOn()
    {
        SoundManager.Instance.audioSource_01.mute = false;
        SoundManager.Instance.audioSource_02.mute = false;
        SoundManager.Instance.audioSource_03.mute = false;
        SoundManager.Instance.audioSource_04.mute = false;
        SoundManager.Instance.audioSource_05.mute = false;
    }

    public void audioSourceOff()
    {
        SoundManager.Instance.audioSource_01.mute = true;
        SoundManager.Instance.audioSource_02.mute = true;
        SoundManager.Instance.audioSource_03.mute = true;
        SoundManager.Instance.audioSource_04.mute = true;
        SoundManager.Instance.audioSource_05.mute = true;
        
    }


    public void BGMVolume(float _value)
    {
        BGM.volume = _value;
    }


    // Start is called before the first frame update
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

    public void PlaySound_01(string name)
    {
        // if (NowSoundname.Equals(name)) return;

        for (int i = 0; i < SoundList.Length; ++i)
            if (SoundList[i].name.Equals(name))
            {
                audioSource_01.clip = SoundList[i].audio;
                audioSource_01.Play();
                NowSoundname = name;
            }
    }

    public void PlaySound_02(string name)
    {
        // if (NowSoundname.Equals(name)) return;

        for (int i = 0; i < SoundList.Length; ++i)
            if (SoundList[i].name.Equals(name))
            {
                audioSource_02.clip = SoundList[i].audio;
                audioSource_02.Play();
                NowSoundname = name;
            }
    }

    public void PlaySound_03(string name)
    {
        // if (NowSoundname.Equals(name)) return;

        for (int i = 0; i < SoundList.Length; ++i)
            if (SoundList[i].name.Equals(name))
            {
                audioSource_03.clip = SoundList[i].audio;
                audioSource_03.Play();
                NowSoundname = name;
            }
    }

    public void PlaySound_04(string name)
    {
        // if (NowSoundname.Equals(name)) return;

        for (int i = 0; i < SoundList.Length; ++i)
            if (SoundList[i].name.Equals(name))
            {
                audioSource_04.clip = SoundList[i].audio;
                audioSource_04.Play();
                NowSoundname = name;
            }
    }

     public void PlaySound_05(string name)
    {
        // if (NowSoundname.Equals(name)) return;

        for (int i = 0; i < SoundList.Length; ++i)
            if (SoundList[i].name.Equals(name))
            {
                audioSource_05.clip = SoundList[i].audio;
                audioSource_05.Play();
                NowSoundname = name;
            }
    }
}
