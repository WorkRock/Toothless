using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{
    //Player_Atk 클래스 객체 생성
    public Player_Atk player;

    public Slider Dragon_HPBar;

    public ObjectManager objectManager;

    //드래곤 타입
    public string type;

    //드래곤 체력
    public int Dragon_NowHP;
    public int Dragon_TotalHP;

    //함수 관련 (드래곤 HP 증가)
    public int nowStage;
    public int BasicDefaultHp;
    public int BasicPlusHp;
    public int EditDefaultHp;
    public int EditPlusHp;
    public int BasicCorStage;
    public int EditCorStage;
    public int maxHp;

    public static bool isHit;


    //드래곤 체력바 연결하는 방법 : 활성화될때 프리팹의 자식에서 슬라이더를 Dragon_HPBar 컴포넌트에 연결시킨다.
    void OnEnable()
    {
        Dragon_HPBar = gameObject.GetComponentInChildren<Slider>();
        //다시 활성화 될때 hp바는 만땅으로
        Dragon_HPBar.value = 1.0f;
        objectManager = GetComponent<ObjectManager>();
    }

    void Start()
    {
        objectManager = GetComponent<ObjectManager>();
        type = "Dragon";
        nowStage = PlayerPrefs.GetInt("Stage");
        Dragon_TotalHP = BasicDefaultHp;
        totalHpCal(nowStage-1);
        Dragon_NowHP = Dragon_TotalHP;
        Debug.Log("드래곤 전체 체력 : " + Dragon_TotalHP);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player_Atk"))
        {
            collision.gameObject.SetActive(false);
            //폭발 효과
            isHit = true;

            Dragon_NowHP -= player.Player_Atk_Power;

            Debug.Log($"hp value : {Dragon_NowHP / (float)Dragon_TotalHP}");

            // 체력바 조정
            Dragon_HPBar.value = Dragon_NowHP / (float)Dragon_TotalHP;

            Debug.Log($"드래곤 체력 : {Dragon_NowHP}");
            if (Dragon_NowHP <= 0)
            {
                gameObject.SetActive(false);
                nowStage++;
                PlayerPrefs.SetInt("Stage", nowStage);
                PlayerPrefs.Save();
                //드래곤 사망 정보 저장 0-생존 1-사망
                PlayerPrefs.SetInt("isDragonDie", 1);
            }
                
        }
    }


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
}
