using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public Text UGLevel;
    public Text NeedCoin;

    public int playerLevel;
    public int playerNextLevel;
    public int atkUGLevel;
    public int totalCoin;
    public int playerNeedCoin;
    // Start is called before the first frame update
    void Start()
    {
        // 테스트를 위한 초기화
        PlayerPrefs.SetInt("AtkUG",1);

        atkUGLevel = PlayerPrefs.GetInt("AtkUG");
        playerLevel = PlayerPrefs.GetInt("Level");
        totalCoin = PlayerPrefs.GetInt("Coin");
        playerNextLevel = playerLevel + 1;
    }

    // Update is called once per frame
    void Update()
    {
        UGLevel.text = atkUGLevel.ToString();
        NeedCoin.text = playerNeedCoin.ToString();
    }

    void needCoinCal()
    {
        /*
        // 플레이어의 다음 레벨까지 I가 더해짐
        for(int i = 1; i < playerNextLevel; i++)
        {   
            // i가 플레이어 레벨보다 작을 시에는 contiune
            if(i < playerLevel)
                continue;

            // total 경험치 계산 : 기본 경험치 + 보정 경험치
            // 기본 경험치 : (이전 레벨 경험치 + (현재 레벨 / 기본 경험치 보정 레벨)*(기본 경험치 가중치))
            // 보정 경험치 : (이전 레벨 보정 경험치 + (현재 레벨 / 보정 경험치 보정 레벨)*(보정 경험치 가중치))
            totalExp += 
                ((i / BasicCorLevel) * BasicPlusExp)
                + (EditDefaultExp + (i / EditCorLevel) * EditPlusExp);
            Debug.Log("i :" + i);
            Debug.Log("BasicCal : " + (i / BasicCorLevel) * BasicPlusExp);
            Debug.Log("EditCal : " + EditDefaultExp + (i / EditCorLevel) * EditPlusExp);
            Debug.Log("+Total Exp : " + totalExp);
        }

        // 만약 max로 잡아놓은 경험치 값보다 높아질 시 max로 통일
        if(totalExp >= maxExp)
        {
            totalExp = maxExp;
        }

        Debug.Log("Total Exp : " + totalExp);
        */
    }
}
