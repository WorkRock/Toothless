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
    public GameObject IceBallPrefs;
    public GameObject WaterBallPrefs;
    public GameObject ElectricBallPrefs;
    //3. 플레이어 공격(하트)
    public GameObject Player_AtkPrefs;
   
    //오브젝트 풀 생성(public X)
    //1. 장애물
    GameObject[] ObstaclePool;
    //2. 드래곤 공격
    GameObject[] FireBallPool;
    GameObject[] IceBallPool;
    GameObject[] WaterBallPool;
    GameObject[] ElectricBallPool;
    //3. 플레이어 공격
    GameObject[] Player_AtkPool;
   
    //타겟풀 생성
    GameObject[] targetPool;
   
    void Awake()
    {
        //한번에 등장할 갯수만큼 오브젝트 풀 크기 설정
        //1. 장애물 풀
        ObstaclePool = new GameObject[30];
        //2. 공격 풀
        FireBallPool = new GameObject[30];
        IceBallPool = new GameObject[30];
        WaterBallPool = new GameObject[30];
        ElectricBallPool = new GameObject[30];
        //3. 플레이어 공격 풀
        Player_AtkPool = new GameObject[10];
       
        //오브젝트 생성(로딩시간)
        Generate();
    }

    public void Generate()
    {
        //오브젝트 풀에 오브젝트를 1.생성만 해놓고 2.비활성화
        //장애물 풀
        for(int i = 0; i < ObstaclePool.Length; i++)
        {
            ObstaclePool[i] = Instantiate(ObstaclePrefs);
            ObstaclePool[i].SetActive(false);
        }
        //파이어볼 풀
        for(int i = 0; i < FireBallPool.Length; i++)
        {
            FireBallPool[i] = Instantiate(FireBallPrefs);
            FireBallPool[i].SetActive(false);
        }
        //아이스볼 풀
        for (int i = 0; i < IceBallPool.Length; i++)
        {
            IceBallPool[i] = Instantiate(IceBallPrefs);
            IceBallPool[i].SetActive(false);
        }
        //워터볼 풀
        for (int i = 0; i < WaterBallPool.Length; i++)
        {
            WaterBallPool[i] = Instantiate(WaterBallPrefs);
            WaterBallPool[i].SetActive(false);
        }
        //일렉트릭볼 풀
        for(int i = 0; i < ElectricBallPool.Length; i++)
        {
            ElectricBallPool[i] = Instantiate(ElectricBallPrefs);
            ElectricBallPool[i].SetActive(false);
        }
        //플레이어 공격 풀
        for (int i = 0; i < Player_AtkPool.Length; i++)
        {
            Player_AtkPool[i] = Instantiate(Player_AtkPrefs);
            Player_AtkPool[i].SetActive(false);
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

            case "IceBall":
                targetPool = IceBallPool;
                break;

            case "WaterBall":
                targetPool = WaterBallPool;
                break;

            case "ElectricBall":
                targetPool = ElectricBallPool;
                break;

            case "Player_Atk":
                targetPool = Player_AtkPool;
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
}
