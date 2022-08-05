﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Move : MonoBehaviour
{
    //UI 관련
    //플레이어 체력바
    public Slider Player_HPBar;

    //플레이어 쉴드 오브젝트 연결
    public GameObject[] playerShield;

    //쉴드 쿨타임
    public Image ShieldCoolTime;

    //쉴드 딜레이
    public float curDelay = 0f;
    public float maxDelay = 1f;
    public float curShieldDelay;
    public float maxShieldDelay;
    public bool isShieldOn;

    //Dragon_Atk 클래스 객체 생성
    public Dragon_Atk dragon;
    //Obstacle 클래스 객체 생성
    public Obstacle obstacle;
    //게임매니저
    public gameManager gameManager;

    //이동할 위치 배열로 선언
    public GameObject[] targetPos;

    //쉴드 이미지 UI
    public GameObject[] shieldImgs;

    //플레이어의 콜라이더 연결
    public CapsuleCollider2D capsuleCollider2D;

    //무적상태일 때 플레이어 흐리게
    public SpriteRenderer spriteRenderer;

    public int playerShieldNum;
    private int onShieldNum;

    //이동 속도
    public float playerSpeed;
    //이동 방향(-1 or 1)
    public float fHor;

    //위치 인덱스 : 3
    private int minPos; //0
    private int nowPos; //1
    private int maxPos; //2

    //함수 관련
    // 플레이어 공통
    public int nowLevel;

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
    public int Player_TotalAtk;
    public int BasicDefaultPlayer_Atk;      //기본_Default : 30
    public int BasicPlusPlayer_Atk;         //기본_가중치 : 0
    public int EditDefaultPlayer_Atk;       //보정값_Default : 0
    public int EditPlusPlayer_Atk;          //보정값_가중치 : 15
    public int BasicCorLevel_Atk;           //보정레벨_기본 : 0
    public int EditCorLevel_Atk;            //보정레벨_보정값 : 10
    public int maxPlayer_Atk;               //최대(or최소)값 : 500


    // 3. 장애물 공격력 함수
    //최종 공격력
    public int Total_ComObj_Atk;

    public int BasicDefault_ComObj_Atk;     //기본_Default : 10
    public int BasicPlus_ComObj_Atk;        //기본_가중치 : 2
    public int EditDefault_ComObj_Atk;      //보정값_Default : 0
    public int EditPlus_ComObj_Atk;         //보정값_가중치 : 20
    public int BasicCorStage_ComObj_Atk;    //보정스테이지_기본 : 0
    public int EditCorStage_ComObj_Atk;     //보정스테이지_보정값 : 10
    public int max_ComObj_Atk;              //최대(or최소)값 : 99999

    // 4. 드래곤 공격력 함수
    //최종 공격력
    public int Total_ComAtk_Atk;

    public int BasicDefault_ComAtk_Atk;     //기본_Default : 10
    public int BasicPlus_ComAtk_Atk;        //기본_가중치 : 2
    public int EditDefault_ComAtk_Atk;      //보정값_Default : 0
    public int EditPlus_ComAtk_Atk;         //보정값_가중치 : 20
    public int BasicCorStage_ComAtk_Atk;    //보정스테이지_기본 : 0
    public int EditCorStage_ComAtk_Atk;     //보정스테이지_보정값 : 10
    public int max_ComAtk_Atk;              //최대(or최소)값 : 99999

    public int nowStage;


    // Start is called before the first frame update
    void Start()
    {
        playerShieldNum = 0;
        nowLevel = PlayerPrefs.GetInt("Level");
        //플레이어 체력, 공격력은 로비에서 업그레이드 하여 게임중에는 변하지 않으므로 Start에서 한번만 호출
        // 1. 플레이어 체력 계산 함수
        totalHpCal();
        //체력 초기화
        Player_NowHP = Player_TotalHP;
        // 2. 플레이어 공격력 계산 함수
        totalPlayer_AtkCal();

        //인덱스 초기화
        minPos = 0;
        nowPos = 1;
        maxPos = targetPos.Length - 1;

        //쉴드 비활성화
        for (int i = 0; i < playerShield.Length; i++)
        {
            playerShield[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ShieldSwap();
        }
        ShieldOn();
        nowStage = PlayerPrefs.GetInt("Stage");
        Total_ComObj_AtkCal();
        Total_ComAtk_AtkCal();

        // 쉴드 쿨타임
        ShieldCoolTime.fillAmount = Mathf.Lerp(0, 100, (curShieldDelay / maxShieldDelay) / 100);

        // 체력바 조정(슬라이더 밸류값으로 조정)
        Player_HPBar.value = Player_NowHP / (float)Player_TotalHP;

        for (int i = 0; i < shieldImgs.Length; i++)
        {
            if (i == playerShieldNum)
            {
                shieldImgs[i].SetActive(true);
            }

            else
                shieldImgs[i].SetActive(false);
        }

        //이동할 때 쉴드 끊김 방지
        for (int i = 0; i < playerShield.Length; i++)
        {
            if(playerShield[i].activeSelf)
            {
                playerShield[i].transform.position = gameObject.transform.position;
            }
        }

    }

    public void MovePlayer()
    {
        //키보드 좌우 입력했을 때
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            //방향 값(-1 or 1)
            fHor = Input.GetAxisRaw("Horizontal");

            // 0 <= nowPos <= 2 이면
            if (nowPos + (int)fHor <= maxPos && nowPos + (int)fHor >= minPos)
            {
                //현재 인덱스에 방향 값 +
                nowPos += (int)fHor;
            }
        }
        //현 위치 -> 이동할 위치 (반드시 if문 밖에서)
        transform.position = Vector3.MoveTowards(transform.position, targetPos[nowPos].transform.position, playerSpeed);
    }

    public void ShieldSwap()
    {
        float fVer = Input.GetAxisRaw("Vertical");
        Debug.Log("fVer : " + fVer);
        playerShieldNum += (int)fVer;
    
        if(playerShieldNum > playerShield.Length-1)
        {
            playerShieldNum = 0;
        }

        else if(playerShieldNum < 0)
        {
            playerShieldNum = playerShield.Length - 1;
        }
    }

    public void ShieldOn()
    {
        curDelay += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if (curDelay > maxDelay)
            {
                //gameObject.tag = "Player_OnShield";
                isShieldOn = true;
                playerShield[playerShieldNum].SetActive(true);
                onShieldNum = playerShieldNum;
                //
                // 0 - pyro, 1 - ice, 2 - water, 3 - electro
                switch (onShieldNum)
                {
                    case 0:
                        gameObject.tag = "PyroShield";
                        break;
                    case 1:
                        gameObject.tag = "IceShield";
                        break;
                    case 2:
                        gameObject.tag = "WaterShield";
                        break;
                    case 3:
                        gameObject.tag = "ElectroShield";
                        break;
                }                  
                //
                curDelay = 0;
                Debug.Log("ShieldOn");
            }
        }
        if (isShieldOn)
        {
            curShieldDelay += Time.deltaTime;
        }


        if (curShieldDelay > maxShieldDelay)
        {
            ShieldOff();
            isShieldOn = false;
            curShieldDelay = 0;
        }
    }

    public void ShieldOff()
    {
        gameObject.tag = "Player";
        playerShield[onShieldNum].SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //같은 속성의 공격만 방어 가능, 공격과 일치하는 쉴드가 아니면 방어 불가
        if (collision.gameObject.tag.Equals("Dragon_Atk_Fire")
            || collision.gameObject.tag.Equals("Dragon_Atk_Ice")
            || collision.gameObject.tag.Equals("Dragon_Atk_Water")
            || collision.gameObject.tag.Equals("Dragon_Atk_Electric"))
        {
            if (collision.gameObject.tag.Equals("Dragon_Atk_Fire") && gameObject.tag.Equals("PyroShield")
                ||  collision.gameObject.tag.Equals("Dragon_Atk_Ice") && gameObject.tag.Equals("IceShield")
                ||  collision.gameObject.tag.Equals("Dragon_Atk_Water") && gameObject.tag.Equals("WaterShield")
                ||  collision.gameObject.tag.Equals("Dragon_Atk_Electric") && gameObject.tag.Equals("ElectroShield"))
                return;

            collision.gameObject.SetActive(false);
            //맞을때마다 현재 체력에서 드래곤 공격력만큼 뺌
            Player_NowHP -= Total_ComAtk_Atk;

            //무적상태 시 색깔 흐리게
            OnSuperEffect();
            //무적상태(1)
            capsuleCollider2D.enabled = false;
            Invoke("OffSuper", 1f);

            Debug.Log($"플레이어 체력 : {Player_NowHP}");
            //0이하로 떨어지면 플레이어 비활성화, 시간 정지,  결과 씬 전환
            if (Player_NowHP <= 0)
            {
                gameObject.SetActive(false);
                Time.timeScale = 0;
                SceneManager.LoadScene("Result");
                PlayerPrefs.SetInt("Stage", nowStage);
            }
        }

        else if (collision.gameObject.tag.Equals("Obstacle"))
        {
            collision.gameObject.SetActive(false);
            //맞을때마다 현재 체력에서 장애물 데미지만큼 뺌
            Player_NowHP -= Total_ComObj_Atk;

            //무적상태 시 색깔 흐리게
            OnSuperEffect();
            //무적상태(1)
            capsuleCollider2D.enabled = false;
            Invoke("OffSuper", 1f);

            Debug.Log($"플레이어 체력 : {Player_NowHP}");
            //0이하로 떨어지면 플레이어 비활성화, 시간 정지, 결과 씬 전환
            if (Player_NowHP <= 0)
            {
                gameObject.SetActive(false);
                Time.timeScale = 0;
                SceneManager.LoadScene("Result");
                PlayerPrefs.SetInt("Stage", nowStage);
            }
        }
    }

    //무적상태 끄기
    void OffSuper()
    {
        capsuleCollider2D.enabled = true;
    }

    void OnSuperEffect()
    {
        //피격시 스프라이트 흐리게
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        Invoke("OffSuperEffect", 1f);
    }

    void OffSuperEffect()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    //@플레이어 관련 함수@
    // 1. 플레이어 체력(레벨 비례)
    void totalHpCal()
    {
        if (nowLevel == 1)
            return;
        else if ((BasicDefaultHp + ((nowLevel - 1) * BasicPlusHp)) +
                           EditDefaultHp + Mathf.FloorToInt(((nowLevel - 1) / (float)EditCorLevel_HP) * EditPlusHp) >= maxHp)
            Player_TotalHP = maxHp;
        else
        {
            Player_TotalHP = (BasicDefaultHp + ((nowLevel - 1) * BasicPlusHp)) +
                           EditDefaultHp + Mathf.FloorToInt((nowLevel - 1) / (float)EditCorLevel_HP) * EditPlusHp;
        }

    }

    // 2. 플레이어 공격력(레벨 비례)
    void totalPlayer_AtkCal()
    {

        if ((BasicDefaultPlayer_Atk + ((nowLevel - 1) * BasicPlusPlayer_Atk)) +
                            EditDefaultPlayer_Atk + Mathf.FloorToInt((nowLevel - 1) / (float)EditCorLevel_Atk) * EditPlusPlayer_Atk >= maxPlayer_Atk)
            Player_TotalAtk = maxPlayer_Atk;
        else
            Player_TotalAtk = (BasicDefaultPlayer_Atk + ((nowLevel - 1) * BasicPlusPlayer_Atk)) +
                            EditDefaultPlayer_Atk + Mathf.FloorToInt((nowLevel - 1) / (float)EditCorLevel_Atk) * EditPlusPlayer_Atk;
    }



    // 3. 장애물 공격력
    void Total_ComObj_AtkCal()
    {
        if (((BasicDefault_ComObj_Atk + ((nowStage - 1) * BasicPlus_ComObj_Atk)) +
                            EditDefault_ComObj_Atk + Mathf.FloorToInt((nowStage - 1) / EditCorStage_ComObj_Atk) * EditPlus_ComObj_Atk) >= max_ComObj_Atk)
            Total_ComObj_Atk = max_ComObj_Atk;
        else
            Total_ComObj_Atk = ((BasicDefault_ComObj_Atk + ((nowStage - 1) * BasicPlus_ComObj_Atk)) +
                            EditDefault_ComObj_Atk + Mathf.FloorToInt((nowStage - 1) / EditCorStage_ComObj_Atk) * EditPlus_ComObj_Atk);
    }

    // 4. 드래곤 공격력
    void Total_ComAtk_AtkCal()
    {
        if (((BasicDefault_ComAtk_Atk + ((nowStage - 1) * BasicPlus_ComAtk_Atk)) +
                            EditDefault_ComAtk_Atk + Mathf.FloorToInt((nowStage - 1) / EditCorStage_ComAtk_Atk) * EditPlus_ComAtk_Atk) >= max_ComAtk_Atk)
            Total_ComAtk_Atk = max_ComAtk_Atk;
        else
            Total_ComAtk_Atk = ((BasicDefault_ComAtk_Atk + ((nowStage - 1) * BasicPlus_ComAtk_Atk)) +
                            EditDefault_ComAtk_Atk + Mathf.FloorToInt((nowStage - 1) / EditCorStage_ComAtk_Atk) * EditPlus_ComAtk_Atk);
    }
}
