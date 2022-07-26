using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Atk : MonoBehaviour
{
    //오브젝트 타입
    public string type;

    //최종 공격력(반사 데미지)
    public int Player_Atk_Power;

    //함수 관련(플레이어 공격력 증가)
    public int nowLevel;
    public int BasicDefaultAtk;
    public int BasicPlusAtk;
    public int EditDefaultAtk;
    public int EditPlusAtk;
    public int BasicCorLevel;
    public int EditCorLevel;
    public int maxAtk;


    // Start is called before the first frame update
    void Start()
    {
        type = "Player_Atk";

    }

    void totalAtkCal(int nowLevel)
    {
        Player_Atk_Power = BasicDefaultAtk + Mathf.FloorToInt((nowLevel / BasicCorLevel) * BasicPlusAtk)
                    + Mathf.FloorToInt(EditDefaultAtk + (nowLevel / EditCorLevel) * EditPlusAtk);

        // 만약 max로 잡아놓은 공격력보다 높아질 시 max로 통일
        if (Player_Atk_Power >= maxAtk)
        {
            Player_Atk_Power = maxAtk;
        }
    }
}
