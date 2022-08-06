using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    //인게임 사운드 매니저
    public IG_SoundManager soundManager;

    //쉴드가 플레이어 따라 움직이게 하기
    public Transform player;

    //오브젝트 매니저 객체 생성
    public ObjectManager objectManager;

    //드래곤한테 발사하기 위해 드래곤 위치받아오기
    public Transform dragonPos;

    //컷씬 연결
    public GameObject CutScene;

    public bool isSamePos;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = player.position;
        transform.position = targetPos;
    }

    void ShieldTrigger()
    {
        soundManager.PlayAudio2("Parrying");
        Time.timeScale = 0.1f;
        CutScene.SetActive(true);
        Invoke("OnRealTime", 0.06f);
        Invoke("CutSceneOff", 0.3f);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (gameObject.tag)
        {
            case "PyroShield":
                if (collision.gameObject.tag.Equals("Dragon_Atk_Fire"))
                {
                    ShieldTrigger();
                    //1. 드래곤 공격 오브젝트 비활성화
                    collision.gameObject.SetActive(false);
                }
                break;
            case "IceShield":
                if (collision.gameObject.tag.Equals("Dragon_Atk_Ice"))
                {
                    ShieldTrigger();
                    //1. 드래곤 공격 오브젝트 비활성화
                    collision.gameObject.SetActive(false);
                }
                break;
            case "WaterShield":
                if (collision.gameObject.tag.Equals("Dragon_Atk_Water"))
                {
                    ShieldTrigger();
                    //1. 드래곤 공격 오브젝트 비활성화
                    collision.gameObject.SetActive(false);
                }
                break;
            case "ElectroShield":
                if (collision.gameObject.tag.Equals("Dragon_Atk_Electric"))
                {
                    ShieldTrigger();
                    //1. 드래곤 공격 오브젝트 비활성화
                    collision.gameObject.SetActive(false);
                }
                break;


        }
    }

    void OnRealTime()
    {
        Time.timeScale = 1f;

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

        else if (transform.position.x > 0)
        {
            newAtk.transform.rotation = Quaternion.Euler(0f, 0f, 20f);
            newAtk.GetComponent<Rigidbody2D>().AddForce(dirVec * 1.5f, ForceMode2D.Impulse);
        }

        else
        {
            newAtk.GetComponent<Rigidbody2D>().AddForce(dirVec * 1.5f, ForceMode2D.Impulse);
        }
    }

    void CutSceneOff()
    {
        CutScene.SetActive(false);
    }
}
