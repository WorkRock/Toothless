using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    //이동할 위치 배열로 선언
    public GameObject[] targetPos;

    //이동 속도
    public float playerSpeed;
    //이동 방향(-1 or 1)
    public float fHor;

    //위치 인덱스 : 3
    private int minPos; //0
    private int nowPos; //1
    private int maxPos; //2

    // Start is called before the first frame update
    void Start()
    {
        //인덱스 초기화
        minPos = 0;
        nowPos = 1;
        maxPos = targetPos.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        //키보드 좌우 입력했을 때
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            //방향 값(-1 or 1)
            fHor = Input.GetAxisRaw("Horizontal");

            // 0 <= nowPos <= 2 이면
            if(nowPos + (int)fHor <= maxPos && nowPos + (int)fHor >= minPos)
            {
                //현재 인덱스에 방향 값 +
                nowPos += (int)fHor;
            }
        }
        //현 위치 -> 이동할 위치 (반드시 if문 밖에서)
        transform.position = Vector3.MoveTowards(transform.position, targetPos[nowPos].transform.position, playerSpeed);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Dragon_Atk_Fire"))
        {
            gameObject.SetActive(false);
            Time.timeScale = 0;
        }

        else if(collision.gameObject.tag.Equals("Obstacle"))
        {
            gameObject.SetActive(false);
            Time.timeScale = 0;
        }
    }
}
