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
    //3. 플레이어 공격(하트)
    public GameObject Player_AtkPrefs;
    //4. 드래곤들
    public GameObject Dragon_BluePrefs;
    public GameObject Dragon_GreenPrefs;
    public GameObject Dragon_PinkPrefs;
    public GameObject Dragon_PurplePrefs;
    public GameObject Dragon_BlackPrefs;
    public GameObject Dragon_RedPrefs;
    public GameObject Dragon_YellowPrefs;

    //오브젝트 풀 생성(public X)
    //1. 장애물
    GameObject[] ObstaclePool;
    //2. 드래곤 공격
    GameObject[] FireBallPool;
    GameObject[] IceBallPool;
    GameObject[] WaterBallPool;
    //3. 플레이어 공격
    GameObject[] Player_AtkPool;
    //4. 드래곤
    GameObject Dragon_Blue;
    GameObject Dragon_Green;
    GameObject Dragon_Pink;
    GameObject Dragon_Purple;
    GameObject Dragon_Black;
    GameObject Dragon_Red;
    GameObject Dragon_Yellow;
    //임시 3마리
    GameObject Dragon_Yellow_2;
    GameObject Dragon_Yellow_3;
    GameObject Dragon_Yellow_4;



    //타겟풀 생성
    GameObject[] targetPool;
    //타겟 드래곤
    GameObject targetDragon;

    public int nowStage;

    void Awake()
    {
        //한번에 등장할 갯수만큼 오브젝트 풀 크기 설정
        //1. 장애물 풀
        ObstaclePool = new GameObject[10];
        //2. 공격 풀
        FireBallPool = new GameObject[10];
        IceBallPool = new GameObject[10];
        WaterBallPool = new GameObject[10];
        //3. 플레이어 공격 풀
        Player_AtkPool = new GameObject[10];
        //4. 드래곤 (10마리)
        Dragon_Blue = new GameObject();
        Dragon_Green = new GameObject();
        Dragon_Pink = new GameObject();
        Dragon_Purple = new GameObject();
        Dragon_Black = new GameObject();
        Dragon_Red = new GameObject();
        Dragon_Yellow = new GameObject();
        //임시 3마리
        Dragon_Yellow_2 = new GameObject();
        Dragon_Yellow_3 = new GameObject();
        Dragon_Yellow_4 = new GameObject();




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
        //플레이어 공격 풀
        for (int i = 0; i < Player_AtkPool.Length; i++)
        {
            Player_AtkPool[i] = Instantiate(Player_AtkPrefs);
            Player_AtkPool[i].SetActive(false);
        }

        //드래곤
        //1. 블루
        Dragon_Blue = Instantiate(Dragon_BluePrefs);
        Dragon_Blue.SetActive(false);
        //2. 그린
        Dragon_Green = Instantiate(Dragon_GreenPrefs);
        Dragon_Green.SetActive(false);
        //3. 핑크
        Dragon_Pink = Instantiate(Dragon_PinkPrefs);
        Dragon_Pink.SetActive(false);
        //4. 퍼플
        Dragon_Purple = Instantiate(Dragon_PurplePrefs);
        Dragon_Purple.SetActive(false);
        //5. 블랙
        Dragon_Black = Instantiate(Dragon_BlackPrefs);
        Dragon_Black.SetActive(false);
        //6. 레드
        Dragon_Red = Instantiate(Dragon_RedPrefs);
        Dragon_Red.SetActive(false);
        //7. 옐로우
        Dragon_Yellow = Instantiate(Dragon_YellowPrefs);
        Dragon_Yellow.SetActive(false);
        //임시 3마리
        //8. 옐로우2
        Dragon_Yellow_2 = Instantiate(Dragon_YellowPrefs);
        Dragon_Yellow_2.SetActive(false);
        //9. 옐로우3
        Dragon_Yellow_3 = Instantiate(Dragon_YellowPrefs);
        Dragon_Yellow_3.SetActive(false);
        //10. 옐로우4
        Dragon_Yellow_4 = Instantiate(Dragon_YellowPrefs);
        Dragon_Yellow_4.SetActive(false);
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

    public GameObject MakeDragon()
    {  
        if (PlayerPrefs.GetInt("isDragonDie") == 1)
        {
            // 1 - 블루 드래곤
            if(nowStage % 10 == 1)
            {
                Dragon_Blue.SetActive(true);
                return Dragon_Blue;
            }
            // 2 - 그린 드래곤
            else if(nowStage % 10 == 2)
            {
                Dragon_Green.SetActive(true);
                return Dragon_Green;
            }
            // 3 - 핑크 드래곤
            else if (nowStage % 10 == 3)
            {
                Dragon_Pink.SetActive(true);
                return Dragon_Pink;
            }
            // 4 - 퍼플 드래곤
            else if (nowStage % 10 == 4)
            {
                Dragon_Purple.SetActive(true);
                return Dragon_Purple;
            }
            // 5 - 블랙 드래곤
            else if (nowStage % 10 == 5)
            {
                Dragon_Black.SetActive(true);
                return Dragon_Black;
            }
            // 6 - 레드 드래곤
            else if (nowStage % 10 == 6)
            {
                Dragon_Red.SetActive(true);
                return Dragon_Red;
            }
            // 7 - 옐로우 드래곤
            else if (nowStage % 10 == 7)
            {
                Dragon_Yellow.SetActive(true);
                return Dragon_Yellow;
            }
            // 8 - 옐로우 드래곤 2
            else if (nowStage % 10 == 8)
            {
                Dragon_Yellow_2.SetActive(true);
                return Dragon_Yellow_2;
            }
            // 9 - 옐로우 드래곤 3
            else if (nowStage % 10 == 9)
            {
                Dragon_Yellow_3.SetActive(true);
                return Dragon_Yellow_3;
            }
            // 10 - 옐로우 드래곤 4
            else if (nowStage % 10 == 0)
            {
                Dragon_Yellow_4.SetActive(true);
                return Dragon_Yellow_4;
            }

        }
          
        return null;    //모두 비활성화 상태인 경우 null 리턴
       
    }

    void Update()
    {
        nowStage = PlayerPrefs.GetInt("Stage");
    }
}
