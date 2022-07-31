using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    //오브젝트 매니저 객체 연결
    public ObjectManager objectManager;

    //오브젝트 프리팹 연결
    //1. 장애물
    public Obstacle obstacle;
    //2. 파이어 볼
    public Dragon_Atk fireBall;

    //오브젝트 생성, 공격 생성 위치를 저장할 배열 연결
    public Transform[] SpawnPoints;


    //랜덤 생성 위치
    public int ranPoint;

    //랜덤 생성 오브젝트(공격)
    public int ranBall;

    //타겟위치로 자연스럽게 커지면서 이동하기 위해 타겟설정
    public GameObject[] ObjTargetPoints;

    //풀에서 꺼내오는 뉴 오브젝트
    GameObject newAtkObj;
    GameObject newObstacle;

    //새로 스폰되는 드래곤
    GameObject newDragon;
    //현 스테이지의 1의 자리를 받아와서 스폰할 드래곤 종류를 결정
    public int nowStage;


    void Start()
    {
        PlayerPrefs.SetInt("Stage", 1);
        PlayerPrefs.Save();
        InvokeRepeating("SpawnObstacle", 1f, 1f);
        InvokeRepeating("SpawnDragonBall", 2f, 2f);
    }

    void Update()
    {
        //현 스테이지의 1의 자리를 받아와서 스폰할 드래곤 종류를 결정
        nowStage = PlayerPrefs.GetInt("Stage");
        //PlayerPrefs의 드래곤 사망정보를 받아와서 1(사망)이면 드래곤 생성
        if (PlayerPrefs.GetInt("isDragonDie") == 1)
            Invoke("SpawnDragon", 3f);
    }

    public void SpawnObstacle()
    {
        //0, 1, 2
        ranPoint = Random.Range(0, 3);

        //오브젝트 풀에서 꺼내기
        newObstacle = objectManager.MakeObj(obstacle.type);
        newObstacle.transform.position = SpawnPoints[ranPoint].transform.position;

        
        //오브젝트가 생성되고 떨어질 때 일자로 떨어지지 않고 기울기에 맞게 비스듬하게 떨어지게 하기
        
        //왼쪽 스폰인 경우
        if (ranPoint == 0)
        {
            Vector3 dirVec = ObjTargetPoints[0].transform.position - newObstacle.transform.position;
            newObstacle.GetComponent<Rigidbody2D>().AddForce(dirVec * obstacle.objSpeed, ForceMode2D.Impulse);
        }
        //가운데 스폰인 경우
        else if (ranPoint == 1)
        {
            Vector3 dirVec = ObjTargetPoints[1].transform.position - newObstacle.transform.position;
            newObstacle.GetComponent<Rigidbody2D>().AddForce(dirVec * obstacle.objSpeed, ForceMode2D.Impulse);
        }
        //오른쪽 스폰인 경우
        else if (ranPoint == 2)
        {
            Vector3 dirVec = ObjTargetPoints[2].transform.position - newObstacle.transform.position;
            newObstacle.GetComponent<Rigidbody2D>().AddForce(dirVec * obstacle.objSpeed, ForceMode2D.Impulse);
        }    
    }

    public void SpawnDragonBall()
    {
        //드래곤이 살아있을 때만 드래곤 공격을 생성
        if(PlayerPrefs.GetInt("isDragonDie") != 1)
        {
            ranPoint = Random.Range(0, 3);
            //0 : 파이어볼, 1 : 아이스볼, 2 : 워터볼
            ranBall = Random.Range(0, 3);

            if (ranBall == 0)
            {
                //오브젝트 풀에서 꺼내기
                newAtkObj = objectManager.MakeObj("FireBall");
                newAtkObj.transform.position = SpawnPoints[ranPoint].transform.position;
            }

            else if (ranBall == 1)
            {
                //오브젝트 풀에서 꺼내기
                newAtkObj = objectManager.MakeObj("IceBall");
                newAtkObj.transform.position = SpawnPoints[ranPoint].transform.position;
            }

            else if (ranBall == 2)
            {
                //오브젝트 풀에서 꺼내기
                newAtkObj = objectManager.MakeObj("WaterBall");
                newAtkObj.transform.position = SpawnPoints[ranPoint].transform.position;
            }

            //오브젝트가 생성되고 떨어질 때 일자로 떨어지지 않고 기울기에 맞게 비스듬하게 떨어지게 하기

            //왼쪽 스폰인 경우
            if (ranPoint == 0)
            {
                Vector3 dirVec = ObjTargetPoints[0].transform.position - newAtkObj.transform.position;
                newAtkObj.GetComponent<Rigidbody2D>().AddForce(dirVec * fireBall.speed, ForceMode2D.Impulse);
            }
            //가운데 스폰인 경우
            else if (ranPoint == 1)
            {
                Vector3 dirVec = ObjTargetPoints[1].transform.position - newAtkObj.transform.position;
                newAtkObj.GetComponent<Rigidbody2D>().AddForce(dirVec * fireBall.speed, ForceMode2D.Impulse);
            }
            //오른쪽 스폰인 경우
            else if (ranPoint == 2)
            {
                Vector3 dirVec = ObjTargetPoints[2].transform.position - newAtkObj.transform.position;
                newAtkObj.GetComponent<Rigidbody2D>().AddForce(dirVec * fireBall.speed, ForceMode2D.Impulse);
            }
        }      
    }

    public void SpawnDragon()
    {
        //드래곤이 사망상태일 때
        if (PlayerPrefs.GetInt("isDragonDie") == 1)
        {
            // 1 - 블루 드래곤
            if (nowStage % 10 == 1)
            {
                newDragon = objectManager.MakeDragon("Blue");
                newDragon.transform.position = new Vector3(0f, 4f, 0f);
            }
            // 2 - 그린 드래곤
            else if (nowStage % 10 == 2)
            {
                newDragon = objectManager.MakeDragon("Green");
                newDragon.transform.position = new Vector3(0f, 4f, 0f);
            }
            // 3 - 핑크 드래곤
            else if (nowStage % 10 == 3)
            {
                newDragon = objectManager.MakeDragon("Pink");
                newDragon.transform.position = new Vector3(0f, 4f, 0f);
            }
            // 4 - 퍼플 드래곤
            else if (nowStage % 10 == 4)
            {
                newDragon = objectManager.MakeDragon("Purple");
                newDragon.transform.position = new Vector3(0f, 4f, 0f);
            }
            // 5 - 블랙 드래곤
            else if (nowStage % 10 == 5)
            {
                newDragon = objectManager.MakeDragon("Black");
                newDragon.transform.position = new Vector3(0f, 4f, 0f);
            }
            // 6 - 레드 드래곤
            else if (nowStage % 10 == 6)
            {
                newDragon = objectManager.MakeDragon("Red");
                newDragon.transform.position = new Vector3(0f, 4f, 0f);
            }
            // 7 - 옐로우 드래곤
            else if (nowStage % 10 == 7)
            {
                newDragon = objectManager.MakeDragon("Yellow");
                newDragon.transform.position = new Vector3(0f, 4f, 0f);
            }
            // 8 - 옐로우 드래곤 2
            else if (nowStage % 10 == 8)
            {
                newDragon = objectManager.MakeDragon("Yellow");
                newDragon.transform.position = new Vector3(0f, 4f, 0f);
            }
            // 9 - 옐로우 드래곤 3
            else if (nowStage % 10 == 9)
            {
                newDragon = objectManager.MakeDragon("Yellow");
                newDragon.transform.position = new Vector3(0f, 4f, 0f);
            }
            // 10 - 옐로우 드래곤 4
            else if (nowStage % 10 == 0)
            {
                newDragon = objectManager.MakeDragon("Yellow");
                newDragon.transform.position = new Vector3(0f, 4f, 0f);
            }

        }
        //드래곤 리스폰 후에는 다시 사망 정보를 갱신!
        PlayerPrefs.SetInt("isDragonDie", 0);
    }    
}
