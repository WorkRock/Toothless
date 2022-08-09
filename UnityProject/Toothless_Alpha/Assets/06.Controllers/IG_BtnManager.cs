using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IG_BtnManager : MonoBehaviour
{
    public GameObject Func;
    public GameObject EscapeGame;

    public GameObject GoLobby;
    public GameObject GoLobbyParent;


    private int isSoundOn;

    public GameObject SoundOn;
    public GameObject SoundOff;

    private bool isFuncOn;

    // Start is called before the first frame update
    void Start()
    {
        isSoundOn = PlayerPrefs.GetInt("isSoundOn");
    }

    // Update is called once per frame
    void Update()
    {
        escapeGame();
        if (isFuncOn)
        {
            
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
        
    }
    public void FuncOn()
    {
        Func.SetActive(true);
        isFuncOn = true;
        Time.timeScale = 0.0f;
    }

    public void FuncExit()
    {
        Func.SetActive(false);
        isFuncOn = false;
        Time.timeScale = 1.0f;
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

    public void goLobby()
    {
        GoLobby.SetActive(true);
        GoLobbyParent.SetActive(true);
    }


    public void goLobbyClicked()
    {
        Time.timeScale = 1.0f;
        PlayerPrefs.SetInt("isLobby", 0);
        PlayerPrefs.Save();

        
        SceneManager.LoadScene("Lobby");
    }

    public void goLobbyNotClicked()
    {
        GoLobby.SetActive(false);
    }

    public void escapeGame()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        Time.timeScale = 0.0f;
        EscapeGame.SetActive(true);

    }

    public void escapeGame_Yes()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void escapeGame_No()
    {

        Time.timeScale = 1.0f;
        EscapeGame.SetActive(false);
    }
}
