using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Move : MonoBehaviour
{
    //체력바 ui 관련
    //플레이어 체력바
    public Slider Player_HPBar;
    //Dragon_Atk 클래스 객체 생성
    public Dragon_Atk dragon;
    //Obstacle 클래스 객체 생성
    public Obstacle obstacle;


    //플레이어 체력
    public int Player_NowHP;
    public int Player_TotalHP;

    //이동할 위치 배열로 선언
    public GameObject[] targetPos;

    //플레이어 쉴드 오브젝트 연결
    public GameObject playerShield;

    //플레이어의 콜라이더 연결
    public CapsuleCollider2D capsuleCollider2D;

    //이동 속도
    public float playerSpeed;
    //이동 방향(-1 or 1)
    public float fHor;

    //위치 인덱스 : 3
    private int minPos; //0
    private int nowPos; //1
    private int maxPos; //2

    //쉴드 딜레이
    public float curDelay = 0f;
    public float maxDelay = 1f;
    public float curShieldDelay;
    public float maxShieldDelay;
    public bool isShieldOn;

    //함수 관련(플레이어 HP 증가)
    public int nowLevel;
    public int BasicDefaultHp;
    public int BasicPlusHp;
    public int EditDefaultHp;
    public int EditPlusHp;
    public int BasicCorLevel;
    public int EditCorLevel;
    public int maxHp;

    // Start is called before the first frame update
    void Start()
    {
        //레벨
        //nowLevel = PlayerPrefs.GetInt("Level");    
        //totalHpCal(nowLevel - 1);
        totalHpCal(nowLevel - 1);
        Debug.Log("체력 : " + Player_TotalHP);

        //체력 초기화
        Player_NowHP = Player_TotalHP;

        //인덱스 초기화
        minPos = 0;
        nowPos = 1;
        maxPos = targetPos.Length - 1;
        playerShield.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        ShieldOn();            
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

    public void ShieldOn()
    {
        curDelay += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isShieldOn = true;
            if (curDelay > maxDelay)
            {
                //쉴드 킬때 플레이어 콜라이더 잠시 끄기(옆으로 이동할때 이동속도가 공격 꺼지는 속도보다 빨라서 죽는판정남)
                capsuleCollider2D.enabled = false;
                playerShield.SetActive(true);
                curDelay = 0;
                Debug.Log("ShieldOn");
            }  
        }
        if (isShieldOn)
            curShieldDelay += Time.deltaTime;

        if(curShieldDelay > maxShieldDelay)
        {
            ShieldOff();
            isShieldOn = false;
            curShieldDelay = 0;
        }
    }

    public void ShieldOff()
    {
        //쉴드 꺼지면 플레이어 콜라이더 다시 켜기
        capsuleCollider2D.enabled = true;
        playerShield.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Dragon_Atk_Fire"))
        {
            collision.gameObject.SetActive(false);
            //맞을때마다 현재 체력에서 드래곤 공격력만큼 뺌
            Player_NowHP -= dragon.Dragon_Atk_Power;

            // 체력바 조정(슬라이더 밸류값으로 조정)
            Player_HPBar.value = Player_NowHP / (float)Player_TotalHP;

            Debug.Log($"플레이어 체력 : {Player_NowHP}");
            //0이하로 떨어지면 플레이어 비활성화
            if (Player_NowHP <= 0)
                gameObject.SetActive(false);
        }

        else if (collision.gameObject.tag.Equals("Obstacle"))
        {
            collision.gameObject.SetActive(false);
            //맞을때마다 현재 체력에서 장애물 데미지만큼 뺌
            Player_NowHP -= obstacle.damage;

            // 체력바 조정(슬라이더 밸류값으로 조정)
            Player_HPBar.value = Player_NowHP / (float)Player_TotalHP;

            Debug.Log($"플레이어 체력 : {Player_NowHP}");
            //0이하로 떨어지면 플레이어 비활성화
            if (Player_NowHP <= 0)
                gameObject.SetActive(false);
        }
    }

    void totalHpCal(int nowLevel)
    {
        Player_TotalHP = BasicDefaultHp + Mathf.FloorToInt((nowLevel / BasicCorLevel) * BasicPlusHp)
                    + Mathf.FloorToInt(EditDefaultHp + (nowLevel / EditCorLevel) * EditPlusHp);

        // 만약 max로 잡아놓은 체력보다 높아질 시 max로 통일
        if (Player_TotalHP >= maxHp)
        {
            Player_TotalHP = maxHp;
        }
    }
}
