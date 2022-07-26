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

    //타겟위치로 자연스럽게 커지면서 이동하기 위해 타겟설정
    //public Transform[] ObjTargetPoints;
    public GameObject[] ObjTargetPoints;

    //풀에서 꺼내오는 뉴 오브젝트
    GameObject newAtkObj;
    GameObject newObstacle;

    //드래곤 객체 생성
    //public Dragon dragon;

    void Start()
    {
        InvokeRepeating("SpawnObstacle", 1f, 1f);
        InvokeRepeating("SpawnFireBall", 2f, 2f);
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

    public void SpawnFireBall()
    {
        ranPoint = Random.Range(0, 3);

        //오브젝트 풀에서 꺼내기
        newAtkObj = objectManager.MakeObj(fireBall.type);
        newAtkObj.transform.position = SpawnPoints[ranPoint].transform.position;

        
        
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
