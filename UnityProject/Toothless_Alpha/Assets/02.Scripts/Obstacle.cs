using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //오브젝트의 타입 설정
    public string type;

    //오브젝트 스피드
    public float objSpeed;
    
    void OnEnable()
    {
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }
    
    void Start()
    {
        type = "Obstacle";
    }

    void Update()
    {
        if(Time.timeScale > 0)
        {
            //스케일 점점 커지게
            transform.localScale += new Vector3(0.001f, 0.001f, 0.001f);
            if (transform.localScale.x >= 0.35f)
                transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("BorderBottom"))
            gameObject.SetActive(false);
    }

}
