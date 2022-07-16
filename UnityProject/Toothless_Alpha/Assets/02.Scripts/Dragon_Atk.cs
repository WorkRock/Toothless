using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_Atk : MonoBehaviour
{
    //드래곤 공격 타입
    public string type;

    //발사 스피드
    public float speed = 1f;

    Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //발사
        rigid.AddForce(transform.position.x, transform.position.y + speed, transform.position.z, ForceMode.Impulse);
    }
}
