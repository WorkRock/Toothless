using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_Atk : MonoBehaviour
{
    //드래곤 공격 타입
    public string[] type;

    //공격력
    public int Dragon_Atk_Power;

    //발사 스피드
    public float speed;

    void OnEnable()
    {
        //생성될 때 스케일 초기화
        if (gameObject.tag.Equals("Dragon_Atk_Fire"))
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        else if (gameObject.tag.Equals("Dragon_Atk_Ice"))
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else if (gameObject.tag.Equals("Dragon_Atk_Water"))
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    void Start()
    {
        type = new string[] { "FireBall", "IceBall", "WaterBall" };
    }

    void Update()
    {
        if(Time.timeScale > 0)
        {
            //스케일 점점 커지게
            if (gameObject.tag.Equals("Dragon_Atk_Fire"))
            {
                transform.localScale += new Vector3(0.002f, 0.002f, 0.002f);
                if (transform.localScale.x >= 1f)
                    transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (gameObject.tag.Equals("Dragon_Atk_Ice"))
            {
                transform.localScale += new Vector3(0.002f, 0.002f, 0.002f);
                if (transform.localScale.x >= 1.3f)
                    transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
            else if (gameObject.tag.Equals("Dragon_Atk_Water"))
            {
                transform.localScale += new Vector3(0.001f, 0.001f, 0.001f);
                if (transform.localScale.x >= 0.5f)
                    transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("BorderBottom"))
            gameObject.SetActive(false);
    }
}
