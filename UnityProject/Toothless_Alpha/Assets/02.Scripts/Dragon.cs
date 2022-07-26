using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{
    //Player_Atk 클래스 객체 생성
    public Player_Atk player;

    public Slider Dragon_HPBar;

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
    
    /*
    private void OnEnable()
    {
        Dragon_NowHP = Dragon_TotalHP;
    }
    */

    void Start()
    {
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
