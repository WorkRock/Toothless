using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    //인게임 사운드 매니저
    public IG_SoundManager soundManager;

    //컷씬
    public GameObject cutScene;

    //게임 시작 카운트 다운
    public GameObject ReadyImg;
    public GameObject GoImg;
    public float setTime = 3f;

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

    //오브젝트 프리팹 연결
    //1. 장애물
    public Obstacle obstacle;
    //2. 파이어 볼
    public Dragon_Atk fireBall;

    //오브젝트 생성, 공격 생성 위치를 저장할 배열 연결
    public Transform[] SpawnPoints;

    //스폰 딜레이 계산
    [SerializeField]
    private float fdt_Dragon;   //드래곤 스폰 딜레이

    [SerializeField]
    private float fdt;      //오브젝트 스폰 딜레이

    //스테이지 정보
    public int nowStage;

    // 1. 장애물, 드래곤볼 생성 주기 함수 관련
    //드래곤볼 최종 딜레이
    public float Enemy_ATK_TotalDelay;
    //드래곤볼
    public float BasicDefaultEnemy_ATKDelay;
    public float BasicPlusEnemy_ATKDelay;
    public float EditDefaultEnemy_ATKDelay;
    public float EditPlusEnemy_ATKDelay;
    public float maxEnemy_ATKDelay;
    public int BasicCorStage_Enemy;
    public int EditCorStage_Enemy;
    //장애물 최종 딜레이
    public float Obj_ATK_TotalDelay;
    //장애물
    public float BasicDefaultObj_ATKDelay;
    public float BasicPlusObj_ATKDelay;
    public float EditDefaultObj_ATKDelay;
    public float EditPlusObj_ATKDelay;
    public float maxObj_ATKDelay;
    public int BasicCorStage_Obj;
    public int EditCorStage_Obj;

    // 2. 장애물, 드래곤 볼 속도 함수 관련
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
    public int ranPoint;    //위치 
    public int ranObj;      //장애물의 랜덤 좌표
    public int ranAtk;      //드래곤볼의 랜덤 좌표
    public int ranBall;     //드래곤볼 랜덤 종류

    //타겟위치로 자연스럽게 커지면서 이동하기 위해 타겟설정
    public GameObject[] ObjTargetPoints;

    //풀에서 꺼내오는 뉴 오브젝트
    public GameObject newAtkObj;
    public GameObject newObstacle;

    //새로 스폰되는 드래곤
    GameObject newDragon;

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
        Enemy_ATK_TotalDelay = BasicDefaultEnemy_ATKDelay;  //5초
        PlayerPrefs.SetInt("isDragonDie", isDragonDie);
        PlayerPrefs.SetInt("Stage", 1);
        PlayerPrefs.Save();
    }

    void Update()
    {
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

            Enemy_ATK_TotalDelayCal();  //드래곤 공격 주기 함수 호출    
            Obj_ATK_TotalDelayCal();    //장애물 등장 주기 함수 호출

            fdt += Time.deltaTime;

            if (fdt > Obj_ATK_TotalDelay)
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
                SpawnObjects();
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
                Dragon_Yellow.SetActive(true);
            }
            // 9 - 옐로우 드래곤 3
            else if (nowStage % 10 == 9)
            {
                Dragon_Yellow.SetActive(true);
            }
            // 10 - 옐로우 드래곤 4
            else if (nowStage % 10 == 0)
            {
                Dragon_Yellow.SetActive(true);
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
        Invoke("OffAtkAnim", 1.5f);

        //드래곤이 살아있을 때만 드래곤 공격과 장애물을 생성
        if (isDragonDie != 1)
        {
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

    //드래곤 공격 생성 주기 함수
    void Enemy_ATK_TotalDelayCal()
    {
        if (nowStage == 1)
            return;

        //최종 적 공격 딜레이
        if (BasicDefaultEnemy_ATKDelay - EditDefaultEnemy_ATKDelay <= maxEnemy_ATKDelay)
            Enemy_ATK_TotalDelay = maxEnemy_ATKDelay;
        else
            Enemy_ATK_TotalDelay = BasicDefaultEnemy_ATKDelay - (((nowStage - 1) * BasicPlusEnemy_ATKDelay) + (Mathf.FloorToInt((nowStage - 1) / (float)EditCorStage_Enemy) * EditPlusEnemy_ATKDelay));


    }

    //장애물 생성 주기 함수
    void Obj_ATK_TotalDelayCal()
    {
        if (nowStage == 1)
            return;
        //최종 장애물 딜레이
        if (BasicDefaultObj_ATKDelay - EditDefaultObj_ATKDelay <= maxObj_ATKDelay)
            Obj_ATK_TotalDelay = maxObj_ATKDelay;
        else
            Obj_ATK_TotalDelay = BasicDefaultObj_ATKDelay - (((nowStage - 1) * BasicPlusObj_ATKDelay) + (Mathf.FloorToInt((nowStage - 1) / (float)EditCorStage_Obj) * EditPlusObj_ATKDelay));

    }

    //장애물, 드래곤볼 속도 증가 함수
    void Total__ComObj_SpeedCal()
    {
        if (BasicDefault_ComObj_Speed + EditDefault_ComObj_Speed > max_ComObj_Speed)
            Total__ComObj_Speed = max_ComObj_Speed;
        else
            Total__ComObj_Speed = BasicDefault_ComObj_Speed + ((nowStage - 1) * EditDefault_ComObj_Speed);
    }

    void Total__ComAtk_SpeedCal()
    {
        if (BasicDefault_ComAtk_Speed + EditDefault_ComAtk_Speed > max_ComAtk_Speed)
            Total__ComAtk_Speed = max_ComAtk_Speed;
        else
            Total__ComAtk_Speed = BasicDefault_ComAtk_Speed + ((nowStage - 1) * EditDefault_ComAtk_Speed);
    }


    //애니메이션 끄기
    void OffAtkAnim()
    {
        Eye_Atk.SetActive(false);
        Mouse_Atk.SetActive(false);
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
}
