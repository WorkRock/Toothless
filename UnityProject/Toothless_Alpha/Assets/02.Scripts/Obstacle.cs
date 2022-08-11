using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //오브젝트의 타입 설정
    public string type;

    void OnEnable()
    {
        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }

    void Start()
    {
        type = "Obstacle";
    }


    void Update()
    {     
        if (Time.timeScale > 0)
        {
            //스케일 점점 커지게
            transform.localScale += new Vector3(0.002f, 0.002f, 0.002f);
            if (transform.localScale.x >= 0.75f)
                transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("BorderBottom"))
            gameObject.SetActive(false);
    }
}
