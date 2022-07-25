using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    //쉴드가 플레이어 따라 움직이게 하기

    public Transform player;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = player.position;
        transform.position = targetPos;
    }
}
