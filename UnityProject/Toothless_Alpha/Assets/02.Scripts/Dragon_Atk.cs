using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_Atk : MonoBehaviour
{
    //드래곤 공격 타입
    public string[] type;

    void OnEnable()
    {
        //생성될 때 스케일 초기화
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    void Start()
    {
        type = new string[] { "FireBall", "IceBall", "WaterBall", "ElectricBall" };
    }

    void Update()
    {
        if(Time.timeScale > 0)
        {
            //스케일 점점 커지게
            gameObject.transform.localScale += new Vector3(0.008f, 0.008f, 0.008f);
            if (gameObject.transform.localScale.x >= 1.29f)
                gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("BorderBottom"))
            gameObject.SetActive(false);
    }
}
