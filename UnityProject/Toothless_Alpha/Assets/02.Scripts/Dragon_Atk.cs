using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_Atk : MonoBehaviour
{
    //드래곤 공격 타입
    public string type;

    //발사 스피드
    public float speed = 1f;

    void OnEnable()
    {
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    void Start()
    {
        type = "FireBall";
    }

    void Update()
    {
        if(Time.timeScale > 0)
        {
            //스케일 점점 커지게
            transform.localScale += new Vector3(0.002f, 0.002f, 0.002f);
            if (transform.localScale.x >= 1f)
                transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("BorderBottom"))
            gameObject.SetActive(false);
    }
}
