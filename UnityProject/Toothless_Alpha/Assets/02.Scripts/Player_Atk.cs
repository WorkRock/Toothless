using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Atk : MonoBehaviour
{
    //오브젝트 타입
    public string type;

    // Start is called before the first frame update
    void Start()
    {
        type = "Player_Atk";
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("BorderTop"))
            gameObject.SetActive(false);
    }
}
