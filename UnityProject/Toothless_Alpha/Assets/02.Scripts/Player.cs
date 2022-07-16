using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //플레이어 좌우 이동 속도(고정값)
    float moveSpeed = 2;

    // Update is called once per frame
    void Update()
    {    
        //플레이어 좌우 이동
        transform.position =  new Vector3(Input.GetAxis("Horizontal") * moveSpeed, -3, 0);
    }
}
