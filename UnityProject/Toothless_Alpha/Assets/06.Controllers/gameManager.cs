using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    //플레이어 필살기
    public Slider Player_SpecialBar;
    public int Player_TotalSpecial;
    public int Player_NowSpecial;

    //드래곤
    public GameObject[] Dragons;

    //인게임 사운드 매니저
    public IG_SoundManager soundManager;

    //컷씬
    public GameObject cutScene;

    //공격 경고 라인
    public GameObject Alert_Left;
    public GameObject Alert_Center;
    public GameObject Alert_Right;

    //게임 시작 카운트 다운
    public GameObject ReadyImg;
    public GameObject GoImg;
    private float setTime = 3f;

    //드래곤 종류(기본 : 비활성화 상태)
    public GameObject Dragon_Blue;
    public GameObject Dragon_Green;
    public GameObject Dragon_Pink;
    public GameObject Dragon_Purple;
    public GameObject Dragon_Black;
    public GameObject Dragon_Red;
    public GameObject Dragon_Yellow;

    //오브젝트 매니저 객체 연결
    public ObjectManager objectManager;

    //오브젝트 생성, 공격 생성 위치를 저장할 배열 연결
    public Transform[] SpawnPoints;

    //스폰 딜레이 계산
    [SerializeField]
    private float fdt_Dragon;   //드래곤 스폰 딜레이

    [SerializeField]
    private float fdt;      //오브젝트 스폰 딜레이

    private float fdt_Anim;

    //스테이지 정보
    public int nowStage;


    [Header("Delay")]
    // 1. 장애물, 드래곤볼 생성 주기 함수 관련
    //장애물, 드래곤볼 최종 딜레이
    public float Obj_ATK_TotalDelay;        
    public float BasicDefaultObj_ATKDelay;  //기본_Default : 4
    public float BasicPlusObj_ATKDelay;     //기본_가중치 : 0.1
    public float EditDefaultObj_ATKDelay;   //보정값_Default : 0
    public float EditPlusObj_ATKDelay;      //보정값_가중치 : 0.5
    public float maxObj_ATKDelay;           //최소 : 1.2
    public int BasicCorStage_Obj;           //보정스테이지_기본 : 0
    public int EditCorStage_Obj;            //보정스테이지_보정값 : 5

    [Space (10f)]
    // 2. 장애물, 드래곤 볼 속도 함수 관련
    [Header("Obstacle Speed")]
    //장애물
    //최종 장애물 속도
    public float Total__ComObj_Speed;
    public float BasicDefault_ComObj_Speed; //기본_Default : 5
    public float BasicPlus_ComObj_Speed;    //기본_가중치 : 0
    public float EditDefault_ComObj_Speed;  //보정값_Default : 0
    public float EditPlus_ComObj_Speed;     //보정값_가중치 : 1.5
    public int BasicCorStage_ComObj_Speed;  //보정스테이지_기본 : 0
    public int EditCorStage_ComObj_Speed;   //보정스테이지_보정값 : 10
    public float max_ComObj_Speed;          //최대(or최소)값 : 15

    [Space (10f)]
    [Header("DragonBall Speed")]
    //드래곤 볼
    //최종 드래곤볼 속도
    public float Total__ComAtk_Speed;
    public float BasicDefault_ComAtk_Speed; //기본_Default : 5
    public float BasicPlus_ComAtk_Speed;    //기본_가중치 : 0
    public float EditDefault_ComAtk_Speed;  //보정값_Default : 0
    public float EditPlus_ComAtk_Speed;     //보정값_가중치 : 1.5
    public int BasicCorStage_ComAtk_Speed;  //보정스테이지_기본 : 0
    public int EditCorStage_ComAtk_Speed;   //보정스테이지_보정값 : 10
    public float max_ComAtk_Speed;          //최대(or최소)값 : 15

    //랜덤 생성
    private int ranPoint;    //위치 
    private int ranObj;      //장애물의 랜덤 좌표
    private int ranAtk;      //드래곤볼의 랜덤 좌표
    private int ranBall;     //드래곤볼 랜덤 종류

    //타겟위치로 자연스럽게 커지면서 이동하기 위해 타겟설정
    public GameObject[] ObjTargetPoints;

    //풀에서 꺼내오는 뉴 오브젝트
    private GameObject newAtkObj;
    private GameObject newObstacle;

    //폭발 효과
    public GameObject Explosion;

    //드래곤 사망정보
    public int isDragonDie;

    //애니메이션
    public GameObject Eye_Atk;
    public GameObject Mouse_Atk;
    public GameObject Eye_Hit;

    void Start()
    {
        //1초 후 카운트 다운 시작
        Invoke("OnReady", 1f);

        Eye_Atk.SetActive(false);
        Mouse_Atk.SetActive(false);
        Eye_Hit.SetActive(false);

        Obj_ATK_TotalDelay = BasicDefaultObj_ATKDelay;
        PlayerPrefs.SetInt("isDragonDie", 1);
        PlayerPrefs.SetInt("Stage", 1);
        PlayerPrefs.Save();

        //필살기 게이지 초기화
        
        Player_SpecialBar.value = 0f;
        Player_TotalSpecial = 10;
        Player_NowSpecial = 0;
    }

    void Update()
    {
        //필살기
        if(Player_NowSpecial == 10 && Player_SpecialBar.value == 1.0f)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                SpecialAtk();
            }
        }

        if(Eye_Atk.activeSelf || Mouse_Atk.activeSelf)
        {
            fdt_Anim += Time.deltaTime;

            if(fdt_Anim > 0.5f)
            {
                OffAtkAnim();
            }
        }

        if(isDragonDie == 1)
        {
            OffAtkAnim();
        }




        if (ReadyImg.activeSelf)
        {
            setTime -= Time.deltaTime;

            if (setTime < 1)
            {
                ReadyImg.SetActive(false);
                GoImg.SetActive(true);
                soundManager.PlayAudio("Go");
                Invoke("OffReady", 1f);
            }
        }


        isDragonDie = PlayerPrefs.GetInt("isDragonDie");
        //현 스테이지의 1의 자리를 받아와서 스폰할 드래곤 종류를 결정


        nowStage = PlayerPrefs.GetInt("Stage");

        Total__ComObj_SpeedCal();   //장애물 속도 함수 관련
        Total__ComAtk_SpeedCal();   //드래곤볼 속도 함수 관련                             

        //드래곤 공격 주기 함수 관련

        //드래곤 사망정보를 받아와서 1(사망)이면 드래곤 생성
        if (isDragonDie == 1)
        {
            Eye_Atk.SetActive(false);
            Eye_Hit.SetActive(false);
            fdt_Dragon += Time.deltaTime;

            //3초 후에 소환
            if (fdt_Dragon > 3.0f)
            {
                SpawnDragon();
                
                fdt = 3.0f;
               
                fdt_Dragon = 0;
            }
        }

        //드래곤 살아있을 때
        else
        {
            //피격 애니메이션 재생
            if (Dragon.isHit == true)
            {
                //폭발 효과 함수
                Eye_Hit.SetActive(true);
                ExplosionOn();
            }

            else
                Eye_Hit.SetActive(false);
  
            Obj_ATK_TotalDelayCal();    //장애물 등장 주기 함수 호출

            fdt += Time.deltaTime;

            if (fdt + 0.5f > Obj_ATK_TotalDelay)
            {
                ranBall = Random.Range(0, 4);
                ranObj = Random.Range(0, 3);

                switch (ranObj)
                {
                    case 0:
                        ranAtk = Random.Range(1, 3);
                        break;
                    case 1:
                        while (true)
                        {
                            ranAtk = Random.Range(0, 3);
                            if (ranObj != ranAtk)
                                break;
                        }
                        break;
                    case 2:
                        ranAtk = Random.Range(0, 2);
                        break;
                }
                //경고라인 표시
                //0라인
                if (fdt + 0.5f > Obj_ATK_TotalDelay && ranAtk == 0)
                {
                    Alert_Left.SetActive(true);
                }

                //1 라인
                if (fdt + 0.5f > Obj_ATK_TotalDelay && ranAtk == 1)
                {
                    Alert_Center.SetActive(true);
                }


                //2 라인
                if (fdt + 0.5f > Obj_ATK_TotalDelay && ranAtk == 2)
                {
                    Alert_Right.SetActive(true);
                }

                

                //장애물, 공격 소환
                Invoke("SpawnObjects",0.5f);
                //경고라인 끄기
                Invoke("OffAlert", 0.45f);
                fdt = 0;
            }
        }



    }

    public void SpawnDragon()
    {
        Debug.Log("nowStage : " + nowStage);
        //드래곤이 사망상태일 때
        if (isDragonDie == 1)
        {
            CancelInvoke("SpawnObjects");

            // 1 - 블루 드래곤
            if (nowStage % 10 == 1)
            {
                Dragon_Blue.SetActive(true);
            }
            // 2 - 그린 드래곤
            else if (nowStage % 10 == 2)
            {
                Dragon_Green.SetActive(true);
            }
            // 3 - 핑크 드래곤
            else if (nowStage % 10 == 3)
            {
                Dragon_Pink.SetActive(true);
            }
            // 4 - 퍼플 드래곤
            else if (nowStage % 10 == 4)
            {
                Dragon_Purple.SetActive(true);
            }
            // 5 - 블랙 드래곤
            else if (nowStage % 10 == 5)
            {
                Dragon_Black.SetActive(true);
            }
            // 6 - 레드 드래곤
            else if (nowStage % 10 == 6)
            {
                Dragon_Red.SetActive(true);
            }
            // 7 - 옐로우 드래곤
            else if (nowStage % 10 == 7)
            {
                Dragon_Yellow.SetActive(true);
            }
            // 8 - 옐로우 드래곤 2
            else if (nowStage % 10 == 8)
            {
                Dragon_Black.SetActive(true);
            }
            // 9 - 옐로우 드래곤 3
            else if (nowStage % 10 == 9)
            {
                Dragon_Blue.SetActive(true);
            }
            // 10 - 옐로우 드래곤 4
            else if (nowStage % 10 == 0)
            {
                Dragon_Purple.SetActive(true);
            }

        }
        //드래곤 리스폰 후에는 다시 사망 정보를 갱신!
        PlayerPrefs.SetInt("isDragonDie", 0);
        PlayerPrefs.Save();
    }


    void SpawnObjects()
    {
        //애니메이션 재생
        Eye_Atk.SetActive(true);


        if (ranBall == 0)
        {
            //오브젝트 풀에서 꺼내기
            newAtkObj = objectManager.MakeObj("FireBall");
            newAtkObj.transform.position = SpawnPoints[ranAtk].transform.position;
            soundManager.PlayAudio("Pyro");
        }

        else if (ranBall == 1)
        {
            //오브젝트 풀에서 꺼내기
            newAtkObj = objectManager.MakeObj("IceBall");
            newAtkObj.transform.position = SpawnPoints[ranAtk].transform.position;
            soundManager.PlayAudio("Ice");
        }

        else if (ranBall == 2)
        {
            //오브젝트 풀에서 꺼내기
            newAtkObj = objectManager.MakeObj("WaterBall");
            newAtkObj.transform.position = SpawnPoints[ranAtk].transform.position;
            soundManager.PlayAudio("Water");
        }

        else if (ranBall == 3)
        {
            //오브젝트 풀에서 꺼내기
            newAtkObj = objectManager.MakeObj("ElectricBall");
            newAtkObj.transform.position = SpawnPoints[ranAtk].transform.position;
            soundManager.PlayAudio("Electro");
        }

        //오브젝트가 생성되고 떨어질 때 일자로 떨어지지 않고 기울기에 맞게 비스듬하게 떨어지게 하기

        //왼쪽 스폰인 경우
        if (ranAtk == 0)
        {
            Vector3 dirVec = ObjTargetPoints[0].transform.position - newAtkObj.transform.position;
            newAtkObj.GetComponent<Rigidbody2D>().AddForce(dirVec * Total__ComAtk_Speed * 0.1f, ForceMode2D.Impulse);
        }
        //가운데 스폰인 경우
        else if (ranAtk == 1)
        {
            Mouse_Atk.SetActive(true);
            Vector3 dirVec = ObjTargetPoints[1].transform.position - newAtkObj.transform.position;
            newAtkObj.GetComponent<Rigidbody2D>().AddForce(dirVec * Total__ComAtk_Speed * 0.1f, ForceMode2D.Impulse);
        }
        //오른쪽 스폰인 경우
        else if (ranAtk == 2)
        {
            Vector3 dirVec = ObjTargetPoints[2].transform.position - newAtkObj.transform.position;
            newAtkObj.GetComponent<Rigidbody2D>().AddForce(dirVec * Total__ComAtk_Speed * 0.1f, ForceMode2D.Impulse);
        }

        //오브젝트 풀에서 꺼내기
        newObstacle = objectManager.MakeObj("Obstacle");
        newObstacle.transform.position = SpawnPoints[ranObj].transform.position;

        //오브젝트가 생성되고 떨어질 때 일자로 떨어지지 않고 기울기에 맞게 비스듬하게 떨어지게 하기

        //왼쪽 스폰인 경우
        if (ranObj == 0)
        {
            Vector3 dirVec = ObjTargetPoints[0].transform.position - newObstacle.transform.position;
            newObstacle.GetComponent<Rigidbody2D>().AddForce(dirVec * Total__ComObj_Speed * 0.1f, ForceMode2D.Impulse);
        }
        //가운데 스폰인 경우
        else if (ranObj == 1)
        {
            Vector3 dirVec = ObjTargetPoints[1].transform.position - newObstacle.transform.position;
            newObstacle.GetComponent<Rigidbody2D>().AddForce(dirVec * Total__ComObj_Speed * 0.1f, ForceMode2D.Impulse);
        }
        //오른쪽 스폰인 경우
        else if (ranObj == 2)
        {
            Vector3 dirVec = ObjTargetPoints[2].transform.position - newObstacle.transform.position;
            newObstacle.GetComponent<Rigidbody2D>().AddForce(dirVec * Total__ComObj_Speed * 0.1f, ForceMode2D.Impulse);
        }
    }

    void ExplosionOn()
    {
        //soundManager.PlayAudio("Explosion");
        Explosion.SetActive(true);
        Invoke("ExplosionOff", 0.5f);
        if (isDragonDie == 1)
            Invoke("ExplosionOff", 1.2f);
    }

    void ExplosionOff()
    {
        Explosion.SetActive(false);
        Dragon.isHit = false;
    }

    //장애물, 드래곤볼 생성 주기 함수
    void Obj_ATK_TotalDelayCal()
    {

        if (((BasicDefaultObj_ATKDelay - ((nowStage - 1) * BasicPlusObj_ATKDelay)) - 
                EditDefaultObj_ATKDelay - Mathf.FloorToInt((nowStage - 1) / (float)EditCorStage_Obj) * EditPlusObj_ATKDelay) <= maxObj_ATKDelay)
            Obj_ATK_TotalDelay = maxObj_ATKDelay;
        else
            Obj_ATK_TotalDelay = ((BasicDefaultObj_ATKDelay - ((nowStage - 1) * BasicPlusObj_ATKDelay)) -
                EditDefaultObj_ATKDelay - Mathf.FloorToInt((nowStage - 1) / (float)EditCorStage_Obj) * EditPlusObj_ATKDelay);
}

    // 4. 장애물 속도
    void Total__ComObj_SpeedCal()
    {
        if (((BasicDefault_ComObj_Speed + ((nowStage - 1) * BasicPlus_ComObj_Speed)) +
                            EditDefault_ComObj_Speed + Mathf.FloorToInt((nowStage - 1) / EditCorStage_ComObj_Speed) * EditPlus_ComObj_Speed) >= max_ComObj_Speed)
            Total__ComObj_Speed = max_ComObj_Speed;
        else
            Total__ComObj_Speed = ((BasicDefault_ComObj_Speed + ((nowStage - 1) * BasicPlus_ComObj_Speed)) +
                            EditDefault_ComObj_Speed + Mathf.FloorToInt((nowStage - 1) / EditCorStage_ComObj_Speed) * EditPlus_ComObj_Speed);
    }

    // 5. 드래곤볼 속도
    void Total__ComAtk_SpeedCal()
    {
        if (((BasicDefault_ComAtk_Speed + ((nowStage - 1) * BasicPlus_ComAtk_Speed)) +
                            EditDefault_ComAtk_Speed + Mathf.FloorToInt((nowStage - 1) / EditCorStage_ComAtk_Speed) * EditPlus_ComAtk_Speed) >= max_ComAtk_Speed)
            Total__ComAtk_Speed = max_ComAtk_Speed;
        else
            Total__ComAtk_Speed = ((BasicDefault_ComAtk_Speed + ((nowStage - 1) * BasicPlus_ComAtk_Speed)) +
                            EditDefault_ComAtk_Speed + Mathf.FloorToInt((nowStage - 1) / EditCorStage_ComAtk_Speed) * EditPlus_ComAtk_Speed);
    }

    //애니메이션 끄기
    void OffAtkAnim()
    {
        Eye_Atk.SetActive(false);
        Mouse_Atk.SetActive(false);
        fdt_Anim = 0;
    }

    //카운트다운 표시
    void OnReady()
    {
        soundManager.PlayAudio("Ready");
        ReadyImg.SetActive(true);
    }

    //카운트다운 끄기
    void OffReady()
    {
        GoImg.SetActive(false);
    }

    //경고 라인 끄기
    void OffAlert()
    {
        Alert_Left.SetActive(false);
        Alert_Center.SetActive(false);
        Alert_Right.SetActive(false);
    }

    //플레이어 필살기
    void SpecialAtk()
    {
        for(int i = 0; i < 30; i++)
        {
            if (newAtkObj.activeSelf == true || newObstacle.activeSelf == true)
            {
                newAtkObj.SetActive(false);
                newObstacle.SetActive(false);
            }
        }
        Player_NowSpecial = 0;
        Player_SpecialBar.value = 0f;
    }
}
