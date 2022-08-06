using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Text Level;
    public Text Coin;
    public Slider Exp;


    public Text InfoLevel;
    public Text InfoAtk;
    public Text InfoHP;
    public Text InfoExp;


    public GameObject UI_Lobby_Player;
    public BtnManager btnManager;
    public Upgrade upgrade;

    // UI_Lobby_Player 좌 우 움직임 값
    public float rot;

    public float fdt;
    public float maxT;

    // Player 레벨 및 경험치 계산용 변수들
    public int playerLevel;

    public int playerUGLevel;

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


    // 1. 플레이어 HP(레벨 & 업그레이드)
    //플레이어 체력
    public int Player_TotalHP;
    public int Player_NowHP;
    public int BasicDefaultHp;
    public int BasicPlusHp;
    public int EditDefaultHp;
    public int EditPlusHp;
    public int BasicCorLevel_HP;               //보정레벨_기본 : 0
    public int EditCorLevel_HP;                //보정레벨_보정값 : 10
    public int maxHp;


     // 2. 플레이어 공격력(레벨 & 업그레이드)
    public float Player_TotalAtk;
    public int BasicDefaultPlayer_Atk;      //기본_Default : 30
    public int BasicPlusPlayer_Atk;         //기본_가중치 : 0
    public int EditDefaultPlayer_Atk;       //보정값_Default : 0
    public int EditPlusPlayer_Atk;          //보정값_가중치 : 15
    public int BasicCorLevel_Atk;           //보정레벨_기본 : 0
    public int EditCorLevel_Atk;            //보정레벨_보정값 : 10
    public int maxPlayer_Atk;               //최대(or최소)값 : 500


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
        playerUGLevel = PlayerPrefs.GetInt("AtkUG");

        calExp = 0;

        totalExp = BasicDefaultExp;

        Player_TotalHP = BasicDefaultHp;
        Player_TotalAtk = BasicDefaultPlayer_Atk;

        totalHpCal();
        totalPlayer_AtkCal();

        for (int i = 1; i < playerLevel + 1; i++)
            totalExpCal(i);
    }

    // Update is called once per frame
    void Update()
    {
        UIPlayerMove();

        totalCoin = PlayerPrefs.GetInt("Coin");
        // 레벨 및 코인 텍스트 출력
        Level.text = playerLevel.ToString();
        Coin.text = totalCoin.ToString();
        
        levelEdit();
        funcTest();

        if(btnManager.isInfoOn)
        {
            InfoLevel.text = playerLevel.ToString();
            InfoAtk.text = Player_TotalAtk.ToString("F2");
            InfoHP.text = Player_TotalHP.ToString();
            InfoExp.text = "<color=green>"+curExp.ToString() + "</color>" + " / " + totalExp.ToString();
        }
    }

    void UIPlayerMove()
    {
        fdt += Time.deltaTime;

            if (fdt > maxT)
            {
                rot *= (-1);
                fdt = 0;
            }
        /*
        if (!btnManager.isLobby)
        {
            fdt += Time.deltaTime;

            if (fdt > maxT)
            {
                rot *= (-1);
                fdt = 0;
            }
        }

        else
        {
            rot = 0;
        }
        */
        
        UI_Lobby_Player.transform.rotation = Quaternion.Euler(0, 0, rot);
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
            totalHpCal();
            totalPlayer_AtkCal();
            Player_TotalAtk *= upgrade.totalUGDMG;
            upgrade.isBtnClicked = false;
        }
        markExp();

        // Lobby Test를 위한 추가
        
        // totalPlayer_AtkCal();

        if(upgrade.isBtnClicked)
        {
            Debug.Log("BtnClicked");
            totalPlayer_AtkCal();
            Player_TotalAtk *= upgrade.totalUGDMG;
            Debug.Log("Player_TotalAtk : " + Player_TotalAtk);
            upgrade.isBtnClicked = false;
        }
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
        // Debug.Log("TotalExp : " + totalExp);
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



    // 플레이어 체력 계산
    void totalHpCal()
    {
        if (playerLevel == 1)
            return;
        else if ((BasicDefaultHp + ((playerLevel - 1) * BasicPlusHp)) +
                           EditDefaultHp + Mathf.FloorToInt(((playerLevel - 1) / (float)EditCorLevel_HP) * EditPlusHp) >= maxHp)
            Player_TotalHP = maxHp;
        else
        {
            Player_TotalHP = (BasicDefaultHp + ((playerLevel - 1) * BasicPlusHp)) +
                           EditDefaultHp + Mathf.FloorToInt((playerLevel - 1) / (float)EditCorLevel_HP) * EditPlusHp;
        }

    }


    // 플레이어 공격력(레벨 비례)
    void totalPlayer_AtkCal()
    {
        if ((BasicDefaultPlayer_Atk + ((playerLevel - 1) * BasicPlusPlayer_Atk)) +
                            EditDefaultPlayer_Atk + Mathf.FloorToInt((playerLevel - 1) / (float)EditCorLevel_Atk) * EditPlusPlayer_Atk >= maxPlayer_Atk)
            Player_TotalAtk = maxPlayer_Atk;
        else
            Player_TotalAtk = (BasicDefaultPlayer_Atk + ((playerLevel - 1) * BasicPlusPlayer_Atk)) +
                            EditDefaultPlayer_Atk + Mathf.FloorToInt((playerLevel - 1) / (float)EditCorLevel_Atk) * EditPlusPlayer_Atk;
    }
}
