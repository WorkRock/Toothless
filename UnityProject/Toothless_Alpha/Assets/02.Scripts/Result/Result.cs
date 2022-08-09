using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public Text LastStage;
    public Text BestStage;
    public Text GetCoin;
    public Text GetExp;

    public int totalCoin;

    public int playerLastStage;
    public int playerBestStage;
    public int playerNowExp;


    // 획득 경험치 관련 변수들
    public int realTotalGetExp;
    public int totalGetExp;
    public int calGetExp;
    public int BasicDefaultGetExp;
    public int BasicPlusGetExp;
    public int EditDefaultGetExp;
    public int EditPlusGetExp;
    public int BasicCorGetExpStage;
    public int EditCorGetExpStage;
    public int maxGetExp;


    // 획득 코인 관련 변수들
    public int realTotalGetCoin;
    public int totalGetCoin;
    public int calGetCoin;
    public int BasicDefaultGetCoin;
    public int BasicPlusGetCoin;
    public int EditDefaultGetCoin;
    public int EditPlusGetCoin;
    public int BasicCorGetCoinStage;
    public int EditCorGetCoinStage;
    public int maxGetCoin;


    // Start is called before the first frame update
    void Start()
    {
        playerLastStage = PlayerPrefs.GetInt("Stage");
        playerBestStage = PlayerPrefs.GetInt("BStage");
        playerNowExp = PlayerPrefs.GetInt("Exp");
        totalCoin = PlayerPrefs.GetInt("Coin");
        
        playerLastStage -= 1;
        
        Debug.Log("playerLastStage : " + playerLastStage);
        Debug.Log("playerBestStage : " + playerBestStage);

        checkBStage();

        totalGetExp = BasicDefaultGetExp;
        totalGetCoin = BasicDefaultGetCoin;

        for(int i = 1; i < playerLastStage; i++)
        {
            totalGetExpCal(i);
            realTotalGetExp += totalGetExp;

            totalGetCoinCal(i);
            realTotalGetCoin += totalGetCoin;
        }
        
        LastStage.text = playerLastStage.ToString();
        BestStage.text = playerBestStage.ToString();
        
        //Debug.Log("획득 경험치 : " + realTotalGetExp);
        //Debug.Log("획득 코인 : " + realTotalGetCoin);
        GetExp.text = realTotalGetExp.ToString();
        GetCoin.text = realTotalGetCoin.ToString();

        playerNowExp += realTotalGetExp;
        totalCoin += realTotalGetCoin;

        
        PlayerPrefs.SetInt("Exp",playerNowExp);
        PlayerPrefs.SetInt("Coin",totalCoin);
        PlayerPrefs.SetInt("BStage",playerBestStage);
        
    }

    void checkBStage()
    {
        if(playerBestStage < playerLastStage)
        {
            playerBestStage = playerLastStage;
        }
    }

    void totalGetExpCal(int playerLastStage)
    {
        if (playerLastStage == 1)
            return;

        else
        {
            calGetExp = Mathf.FloorToInt((playerLastStage / BasicCorGetExpStage) * BasicPlusGetExp)
            + Mathf.FloorToInt(EditDefaultGetExp + (playerLastStage / EditCorGetExpStage) * EditPlusGetExp);
        }

        totalGetExp += calGetExp;
        //Debug.Log("totalGetExpCal : " + calGetExp);
        // 만약 max로 잡아놓은 경험치 값보다 높아질 시 max로 통일
        if (totalGetExp >= maxGetExp)
        {
            totalGetExp = maxGetExp;
        }
    }

    void totalGetCoinCal(int playerLastStage)
    {
        if (playerLastStage == 1)
            return;

        else
        {
            calGetCoin = Mathf.FloorToInt((playerLastStage / BasicCorGetCoinStage) * BasicPlusGetCoin)
            + Mathf.FloorToInt(EditDefaultGetCoin + (playerLastStage / EditCorGetCoinStage) * EditPlusGetCoin);
        }

        totalGetCoin += calGetCoin;
        //Debug.Log("totalGetCoinCal : " + calGetCoin);
        // 만약 max로 잡아놓은 경험치 값보다 높아질 시 max로 통일
        if (totalGetCoin >= maxGetCoin)
        {
            totalGetCoin = maxGetCoin;
        }
    }
}
