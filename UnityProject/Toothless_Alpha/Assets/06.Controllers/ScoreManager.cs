using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("PlayerInfo")]
    protected int exp = PlayerPrefs.GetInt("Exp");
    protected int coin = PlayerPrefs.GetInt("Coin");
    protected int atkUG = PlayerPrefs.GetInt("AtkUG");
    protected int playerLevel = PlayerPrefs.GetInt("Level");
    protected bool isLobby = false;
    protected bool isSoundOn = true;
    
    
    [Header("Stage")]
    protected int stage = 1;
    protected int bStage = PlayerPrefs.GetInt("BStage");
    // Start is called before the first frame update

    public ScoreManager()
    {
        // Debug.Log("게임 매니저 초기화");
        // 초기화
    }

    public static int GetExp()
    {
        return GetInstance().exp;
    }

    public static int GetCoin()
    {
        return GetInstance().coin;
    }

     public static int GetAtkUG()
    {
        return GetInstance().atkUG;
    }

    public static int GetPlayerLevel()
    {
       return GetInstance().playerLevel;
    }

    public static int GetBStage()
    {
       return GetInstance().bStage;
    }

    public static bool GetIsLobby()
    {
        return GetInstance().isLobby;
    }

    public static bool GetIsSoundOn()
    {
        return GetInstance().isSoundOn;
    }

    public static void SetExp(int _exp)
    {
        GetInstance().exp += _exp;
        PlayerPrefs.SetInt("Exp",GetInstance().exp);
        PlayerPrefs.Save();
    }

    public static void SetCoin(int _coin)
    {
        GetInstance().coin += _coin;
        PlayerPrefs.SetInt("Coin",GetInstance().coin);
        PlayerPrefs.Save();
    }

    public static void SetAtkUG(int _atkUG)
    {
        GetInstance().atkUG += _atkUG;
        PlayerPrefs.SetInt("AtkUG",GetInstance().atkUG);
        PlayerPrefs.Save();
    }

    public static void SetPlayerLevel(int _playerLevel)
    {
        GetInstance().playerLevel += _playerLevel;
        PlayerPrefs.SetInt("Level",GetInstance().playerLevel);
        PlayerPrefs.Save();
    }

    public static void SetBStage(int _bStage)
    {
        GetInstance().bStage = _bStage;
        PlayerPrefs.SetInt("Level",GetInstance().bStage);
        PlayerPrefs.Save();
    }

    public static void SetIsLobby(bool _isLobby)
    {
        GetInstance().isLobby = _isLobby;
    }

    public static void SetIsSoundOn(bool _isSoundOn)
    {
        GetInstance().isSoundOn = _isSoundOn;
    }

}
