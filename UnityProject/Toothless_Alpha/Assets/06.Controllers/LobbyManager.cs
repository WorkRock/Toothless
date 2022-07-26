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
    public int playerNextLevel;
    public int curExp;
    public int totalExp;

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
        PlayerPrefs.SetInt("Level",1);
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("Exp",0);
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("Coin",15000);

        // 획득한 경험치 및 코인을 불러옴
        totalCoin = PlayerPrefs.GetInt("Coin");
        curExp = PlayerPrefs.GetInt("Exp");
        playerLevel = PlayerPrefs.GetInt("Level");
        playerNextLevel = playerLevel+1;
        
        totalExp = BasicDefaultExp;
        totalExpCal();
    }

    // Update is called once per frame
    void Update()
    {
        // 레벨 및 코인 텍스트 출력
       Level.text = playerLevel.ToString();
       Coin.text = totalCoin.ToString();
       levelEdit();
       funcTest();
    }

    // 레벨 변경값 테스트를 위한 메소드
    void funcTest()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            curExp += 100;
        }
    }

    // 슬라이더 조정을 이용한 경험치량 표기
    void markExp()
    {
        double s = (double)curExp / (double) totalExp;
        Exp.value = (float)s;
    }

    // 레벨값 변화 메소드
    void levelEdit()
    {
        // 획득 경험치가 토탈 경험치보다 높을 시 실행
        if(curExp >= totalExp)
        {
            playerLevel++;
            playerNextLevel++;
            curExp -= totalExp;
            totalCoin += 1000;
            
            // 토탈 경험치를 빼주어도 0보다 클 시 재귀
            if(curExp > 0)
            {
                levelEdit();
            }
            
            // 0보다 작아질 시 0으로 초기화
            else if(curExp < 0)
                curExp = 0;
            
            // 변경된 레벨 값 및 경험치 획득량 변화
            PlayerPrefs.SetInt("Level",playerLevel);
            PlayerPrefs.SetInt("Exp",curExp);
            PlayerPrefs.Save();
            totalExpCal();
        }
        markExp();
    }


    // 레벨업에 필요한 총 경험치량 계산
    void totalExpCal()
    {
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
        
    }
}
