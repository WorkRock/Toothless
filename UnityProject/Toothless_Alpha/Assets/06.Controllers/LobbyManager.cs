using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Text Level;
    public Text Coin;
    public Slider Exp;


    // Player 레벨 및 경험치 계산용 변수들
    public int playerLevel;
    public int curExp;
    public int totalExp;

    public int calExp;

    public int BasicDefaultExp;
    public int BasicPlusExp;
    public int EditDefaultExp;
    public int EditPlusExp;
    public int BasicCorLevel;
    public int EditCorLevel;
    public int maxExp;


    public int totalCoin;

    // Start is called before the first frame update
    void Start()
    {
        // 테스트를 위한 레벨 및 경험치값 초기화
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetInt("Exp", 0);
        PlayerPrefs.SetInt("Coin", 15000);
        PlayerPrefs.Save();

        // 획득한 경험치 및 코인을 불러옴
        
        curExp = PlayerPrefs.GetInt("Exp");
        playerLevel = PlayerPrefs.GetInt("Level");
        //playerNextLevel = playerLevel+1;

        calExp = 0;

        totalExp = BasicDefaultExp;

        for (int i = 1; i < playerLevel+1; i++)
            totalExpCal(i);
    }

    // Update is called once per frame
    void Update()
    {
        totalCoin = PlayerPrefs.GetInt("Coin");
        // 레벨 및 코인 텍스트 출력
        Level.text = playerLevel.ToString();
        Coin.text = totalCoin.ToString();
        levelEdit();
        funcTest();
    }

    // 레벨 변경값 테스트를 위한 메소드
    void funcTest()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            curExp += 100;
        }
    }

    // 슬라이더 조정을 이용한 경험치량 표기
    void markExp()
    {
        double s = (double)curExp / (double)totalExp;
        Exp.value = (float)s;
    }

    // 레벨값 변화 메소드
    void levelEdit()
    {
        // 획득 경험치가 토탈 경험치보다 높을 시 실행
        if (curExp >= totalExp)
        {
            playerLevel++;
            curExp -= totalExp;
            
            // 코인 값 변화 확인을 위한 테스트
            totalCoin += 100000;
            
            // 토탈 경험치를 빼주어도 0보다 클 시 재귀
            if (curExp > 0)
            {
                totalExpCal(playerLevel);
                levelEdit();
            }

            // 0보다 작아질 시 0으로 초기화
            else if (curExp <= 0)
            {
                curExp = 0;
                totalExpCal(playerLevel);
            }

            // 변경된 레벨 값 및 경험치 획득량 변화
            PlayerPrefs.SetInt("Level", playerLevel);
            PlayerPrefs.SetInt("Exp", curExp);
            PlayerPrefs.SetInt("Coin", totalCoin);
            PlayerPrefs.Save();

        }
        markExp();
    }


    // 레벨업에 필요한 총 경험치량 계산
    void totalExpCal(int playerLevel)
    {
        if (playerLevel == 1)
            return;

        else
        {
            totalExpFormula(playerLevel);
        }

        totalExp += calExp;
        Debug.Log("TotalExp : " + totalExp);
        // 만약 max로 잡아놓은 경험치 값보다 높아질 시 max로 통일
        if (totalExp >= maxExp)
        {
            totalExp = maxExp;
        }
    }

    // 계산 공식
    void totalExpFormula(int playerLevel)
    {
        calExp = Mathf.FloorToInt((playerLevel / BasicCorLevel) * BasicPlusExp)
                    + Mathf.FloorToInt(EditDefaultExp + (playerLevel / EditCorLevel) * EditPlusExp);
    }

}
