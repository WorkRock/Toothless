using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    //쉴드가 플레이어 따라 움직이게 하기
    public Transform player;

    //오브젝트 매니저 객체 생성
    public ObjectManager objectManager;

    //드래곤한테 발사하기 위해 드래곤 위치받아오기
    public Transform dragonPos;


    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = player.position;
        transform.position = targetPos;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Dragon_Atk_Fire") || collision.gameObject.tag.Equals("Dragon_Atk_Ice") || collision.gameObject.tag.Equals("Dragon_Atk_Water"))
        {
            //1. 드래곤 공격 오브젝트 비활성화
            collision.gameObject.SetActive(false);

            //2. 오브젝트 매니저에서 오브젝트 생성, 위치 지정
            GameObject newAtk = objectManager.MakeObj("Player_Atk");
            newAtk.transform.position = transform.position;

            //3. 드래곤과 쉴드의 벡터 계산
            Vector3 dirVec = dragonPos.position - newAtk.transform.position;
            //4. 기울기 초기화
            newAtk.transform.rotation = Quaternion.Euler(0, 0, 0);

            //5. 위치에 따라 기울여서 발사
            if (transform.position.x < 0)
            {
                newAtk.transform.rotation = Quaternion.Euler(0, 0f, -20f);
                newAtk.GetComponent<Rigidbody2D>().AddForce(dirVec * 1.5f, ForceMode2D.Impulse);
            }

            else if(transform.position.x > 0)
            {
                newAtk.transform.rotation = Quaternion.Euler(0f, 0f, 20f);
                newAtk.GetComponent<Rigidbody2D>().AddForce(dirVec * 1.5f, ForceMode2D.Impulse);
            }

            else
            {
                newAtk.GetComponent<Rigidbody2D>().AddForce(dirVec * 1.5f, ForceMode2D.Impulse);
            }
        }
    }
}
