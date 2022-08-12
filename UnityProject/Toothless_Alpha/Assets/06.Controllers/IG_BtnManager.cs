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

    private bool isSoundOn;

    public GameObject SoundOn;
    public GameObject SoundOff;

    private bool isFuncOn;

    public Player_Move player;

    public float shieldDelay;
    public float fdt;

    private void Start()
    {
        shieldDelay = player.maxShieldDelay;
    }

    // Update is called once per frame
    void Update()
    {
        fdt += Time.deltaTime;
        isSoundOn = ScoreManager.GetIsSoundOn();

        if (Input.GetKeyDown(KeyCode.Escape))
            FuncOn();

        if (isFuncOn)
        {
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

    }
    public void FuncOn()
    {
        SoundManager.Instance.PlaySound_01("Lobby_MenuIn");
        Func.SetActive(true);
        isFuncOn = true;
        Time.timeScale = 0.0f;
    }

    public void FuncExit()
    {
        SoundManager.Instance.PlaySound_01("Lobby_MenuOut");
        Func.SetActive(false);
        isFuncOn = false;
        Time.timeScale = 1.0f;
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
        EscapeGame.SetActive(true);
        //이 밑으로 추가한 부분
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
        EscapeGame.SetActive(false);
    }

    public void ShieldSwap()
    {
        player.ShieldSwapBtn();
    }

    public void playerLMove()
    {
        player.MovePlayerBtn(-1);
    }

    public void playerRMove()
    {
        player.MovePlayerBtn(1);
    }

    public void shieldOn()
    {
        if(fdt > shieldDelay)
        {
            player.isShieldBtnClicked = true;
            fdt = 0;
        }
    }
}
