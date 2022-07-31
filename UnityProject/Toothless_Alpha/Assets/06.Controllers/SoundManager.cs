using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip Lobby_BtnClick;
    public AudioClip Lobby_BGM;
    public AudioClip Lobby_FuncOut;
    public AudioClip Lobby_Func_Upgrade;

    public AudioSource audioSource;
    public AudioSource gameBgm;
    public Upgrade upgrade;
    public BtnManager btnManager;
    public string sceneName;
    public bool isSoundPlay;

    // Start is called before the first frame update
    void Start()
    {
        isSoundPlay = false;
        Invoke("playBgm",4.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!btnManager.isSoundOn)
        {
            gameBgm.mute = true;
            audioSource.mute = true;
            return;
        }

        else
        {
            gameBgm.mute = false;
            audioSource.mute = false;
            checkSceneSound();
        }
    }

    
    void checkSceneSound()
    {
        switch (btnManager.checkSceneName())
        {
            case "Lobby":
                checkLobbyBtn();
                break;

            case "Ingame":
                break;

            case "Result":
                break;

        }
    }
    

    void checkLobbyBtn()
    {
        if (btnManager.isSoundOn && btnManager.isLobby)
        {
            if(btnManager.isSoundPlay && btnManager.isFuncOn)
            {
                audioSource.PlayOneShot(Lobby_BtnClick);
                btnManager.isSoundPlay = false;
            }

            else if(btnManager.isSoundPlay && !btnManager.isFuncOn)
            {
                audioSource.PlayOneShot(Lobby_FuncOut);
                btnManager.isSoundPlay = false;
            }
        }

        if(btnManager.isShopOn && btnManager.isLobby)
        {
            upgradeBtn();
        }
    }

    void upgradeBtn()
    {
        if(upgrade.isBtnClicked)
        {
            Debug.Log("Upgrade");
            audioSource.PlayOneShot(Lobby_Func_Upgrade);
            upgrade.isBtnClicked = false;
        }
    }

    void playBgm()
    {
        gameBgm.Play();
        gameBgm.loop = true;
    }

}
