using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{
    //인게임 사운드 매니저 연결
    public IG_SoundManager soundManager;

    [Space(10f)]
    //게임 매니저 연결
    public gameManager gameManager;

    [Space(10f)]
    //Player_Move 스크립트의 객체 생성
    public Player_Move player;

    [Space(10f)]
    //드래곤 HP 슬라이더 바 연결
    public Slider Dragon_HPBar;

    [Space(10f)]
    [Header("Dragon HP")]
    //드래곤 체력
    public float Dragon_NowHP;          //현재 HP(분자)
    public float Dragon_TotalHP;        //총 HP(분모)

    [Space(10f)]
    //함수 관련 (드래곤 HP 증가)
    public int nowStage;

    [Space(10f)]
    public int BasicDefaultHp;
    public int BasicPlusHp;

    [Space(10f)]
    public int EditDefaultHp;
    public int EditPlusHp;

    [Space(10f)]
    public int BasicCorStage;
    public int EditCorStage;

    [Space(10f)]
    public int maxHp;

    [Space(10f)]
    //드래곤 피격 판정을 저장할 변수
    public static bool isHit;

    [Space(10f)]
    public GameObject Eye_Hit;

    //드래곤 체력바 연결하는 방법 : 활성화될때 프리팹의 자식에서 슬라이더를 Dragon_HPBar 컴포넌트에 연결시킨다.
    void OnEnable()
    {
        //현 스테이지 정보 받아오기
        nowStage = PlayerPrefs.GetInt("Stage");

        //토탈 HP에 디폴트 값 저장
        Dragon_TotalHP = BasicDefaultHp;

        //HP계산 함수 호출
        totalHpCal(nowStage - 1);

        //현재 HP에 계산한 토탈HP를 저장
        Dragon_NowHP = Dragon_TotalHP;

        Debug.Log("드래곤 전체 체력 : " + Dragon_TotalHP);

        //슬라이더에 현 드래곤의 자식으로 있는 슬라이더를 연결
        Dragon_HPBar = gameObject.GetComponentInChildren<Slider>();

        //다시 활성화 될때 hp바는 만땅으로
        Dragon_HPBar.value = 1.0f;
    }

    void Update()
    {
        // 체력바 조정
        Dragon_HPBar.value = Dragon_NowHP / Dragon_TotalHP;
            
        // 필살기를 맞아서 죽을 수 있도록 업데이트에서 검사
        if (Dragon_NowHP <= 0f)
        {
            //DragonDieSound();
            
            //사망시에는 Eye_Hit 상태인채로 사망하게 한다
            Eye_Hit.SetActive(true);
            Invoke("Eye_HitOff", 0.6f);

            Invoke("Disappear", 0.6f);
        }
    }

    void OnDisable()
    {
        //스테이지 값 + 1
        nowStage++;
        PlayerPrefs.SetInt("Stage", nowStage);

        //드래곤 사망 정보 저장 0-생존 1-사망
        PlayerPrefs.SetInt("isDragonDie", 1);
        PlayerPrefs.Save();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player_Atk"))
        {
            //필살기 게이지 스택 + 1
            gameManager.Player_NowSpecial += 1;
            
            //폭발 효과 사운드 재생
            soundManager.PlayAudio3("Explosion");

            //피격 상태 true
            isHit = true;

            //플레이어 공격 비활성화
            collision.gameObject.SetActive(false);

            //현 HP에서 플레이어 공격력을 뺌
            Dragon_NowHP -= player.Player_TotalAtk;

            Debug.Log($"드래곤 체력 : {Dragon_NowHP}");

            
            //드래곤 HP가 0보다 낮아지면 죽는 소리 재생, 0.5초 후 드래곤 비활성화
            if (Dragon_NowHP <= 0)
            {
                DragonDieSound();
                Invoke("Disappear", 0.6f);
            }
        }
    }

    //드래곤 HP 계산 함수
    void totalHpCal(int nowStage)
    {
        Dragon_TotalHP = BasicDefaultHp +  Mathf.FloorToInt((nowStage / BasicCorStage) * BasicPlusHp)
                    + Mathf.FloorToInt(EditDefaultHp + (nowStage / EditCorStage) * EditPlusHp);

        // 만약 max로 잡아놓은 경험치 값보다 높아질 시 max로 통일
        if (Dragon_TotalHP >= maxHp)
        {
            Dragon_TotalHP = maxHp;
        }
    }

    void Disappear()
    {
        gameObject.SetActive(false);
    }

    void DragonDieSound()
    {
        soundManager.PlayAudio2("DragonDie");
    }

    //플레이어 필살기 함수 : 매개변수로 필살기 데미지를 받아서 드래곤 HP에서 그만큼 깎음
    public void Special_Atk(float Special_Atk_Dmg)
    {
        Dragon_NowHP -= Special_Atk_Dmg;
    }

    void Eye_HitOff()
    {
        Eye_Hit.SetActive(false);
    }
}
