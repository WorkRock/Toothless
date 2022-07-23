using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //생성할 오브젝트 프리팹 연결
    //1. 장애물
    public GameObject ObstaclePrefs;
    //2. 드래곤 공격
    public GameObject FireBallPrefs;

    //오브젝트 풀 생성(public X)
    //1. 장애물
    GameObject[] ObstaclePool;
    //2. 드래곤 공격
    GameObject[] FireBallPool;

    //타겟풀 생성
    GameObject[] targetPool;

    void Awake()
    {
        //한번에 등장할 갯수만큼 오브젝트 풀 크기 설정
        //1. 장애물 풀
        ObstaclePool = new GameObject[10];
        //2. 공격 풀
        FireBallPool = new GameObject[10];

        //오브젝트 생성(로딩시간)
        Generate();
    }

    public void Generate()
    {
        //오브젝트 풀에 오브젝트를 1.생성만 해놓고 2.비활성화
        for(int i = 0; i < ObstaclePool.Length; i++)
        {
            ObstaclePool[i] = Instantiate(ObstaclePrefs);
            ObstaclePool[i].SetActive(false);
        }

        for(int i = 0; i < FireBallPool.Length; i++)
        {
            FireBallPool[i] = Instantiate(FireBallPrefs);
            FireBallPool[i].SetActive(false);
        }
    }

    //외부에서 오브젝트 풀에 접근할 함수 생성
    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            //타입명에 따라 타겟풀에 해당 오브젝트 풀 지정
            case "Obstacle":
                targetPool = ObstaclePool;
                break;

            case "FireBall":
                targetPool = FireBallPool;
                break;
          
        }

        //타겟풀의 크기만큼 오브젝트를 검사, 비활성화 상태이면 1.활성화시킨 후 2. 리턴
        for(int i = 0; i < targetPool.Length; i++)
        {
            if(!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }

        return null;    //모두 비활성화 상태인 경우 null 리턴
    }

    //해당 오브젝트들을 전부 가져오는 함수
    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "Obstacle":
                targetPool = ObstaclePool;
                break;
        }

        return targetPool;
    }
}
