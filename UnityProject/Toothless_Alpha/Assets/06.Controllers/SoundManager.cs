using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource Lobby_BtnClick;
    public AudioSource Lobby_BGM;

    public BtnManager btnManager;
    public string sceneName;
    public bool isSoundPlay;

    // Start is called before the first frame update
    void Start()
    {
        isSoundPlay = false;
        Lobby_BGM.playOnAwake = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!btnManager.isSoundOn)
        {
            Lobby_BGM.mute = true;
            return;
        }

        else
        {
            Lobby_BGM.mute = false;
            checkLobbyBtnSoundPlay();

        }
    }


    void checkSceneSound()
    {
        switch (btnManager.checkSceneName())
        {
            case "Lobby":
                checkLobbyBtnSoundPlay();
                break;

            case "Ingame":
                break;

            case "Result":
                break;

        }
    }


    void checkLobbyBtnSoundPlay()
    {
        if (btnManager.isFuncOn && isSoundPlay == false)
        {
            Lobby_BtnClick.Play();
            isSoundPlay = true;
        }

        else if (btnManager.isFuncOn == false)
        {
            isSoundPlay = false;
        }
    }


}
