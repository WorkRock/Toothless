using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    //인게임 사운드 매니저
    public IG_SoundManager soundManager;

    //오브젝트 매니저 객체 연결
    public ObjectManager objectManager;

    [Space(10f)]
    [Header("Player Special ATK")]
    //플레이어 필살기
    public Slider Player_SpecialBar;            //필살기 게이지
    public int Player_TotalSpecial;             //필살기 필요 스택(분모)
    public int Player_NowSpecial;               //필살기 현재 스택(분자)
    public GameObject WhiteBG;                  //화면 하얗게 하는 효과

    [Space(10f)]
    [Header("Dragons")]
    //드래곤들(기본 : 비활성화 상태)
    public GameObject[] Dragons; 
   
    [Space(10f)]
    //애니메이션
    public GameObject Eye_Atk;
    public GameObject Mouse_Atk;
    public GameObject Eye_Hit;

    [Space(10f)]
    //드래곤 사망정보
    public int isDragonDie;

    [Space(10f)]
    [Header("UI Event")]
    //폭발 효과
    public GameObject Explosion;

    [Space(10f)]
    //공격 경고 라인 : 드래곤 볼이 나오기 전 빨간 줄로 잠시 동안 표시
    public GameObject Alert_Left;
    public GameObject Alert_Center;
    public GameObject Alert_Right;

    [Space(10f)]
    //게임 시작 카운트 다운
    public GameObject ReadyImg;
    public GameObject GoImg;
    private float setTime = 3f;

    [Space(10f)]
    [Header("Delay Time")]
    //오브젝트 생성, 공격 생성 위치를 저장할 배열 연결
    public Transform[] SpawnPoints;

    //스폰 딜레이 계산
    [SerializeField]
    private float fdt_Dragon;               //드래곤 스폰 딜레이

    [SerializeField]
    private float fdt;                      //오브젝트 스폰 딜레이

    [SerializeField]
    private float fdt_Anim;                 //드래곤 애니메이션 딜레이                              

    [Space(10f)]
    [Header("Spawn Delay")]
    //스테이지 정보(공통)
    public int nowStage;
    [Space(10f)]
    // 1. 장애물, 드래곤볼 생성 주기 함수 관련
    //장애물, 드래곤볼 최종 딜레이
    public float Obj_ATK_TotalDelay;        
    public float BasicDefaultObj_ATKDelay;  //기본_Default : 4
    public float BasicPlusObj_ATKDelay;     //기본_가중치 : 0.1

    [Space(10f)]
    public float EditDefaultObj_ATKDelay;   //보정값_Default : 0
    public float EditPlusObj_ATKDelay;      //보정값_가중치 : 0.5

    [Space(10f)]
    public int BasicCorStage_Obj;           //보정스테이지_기본 : 0
    public int EditCorStage_Obj;            //보정스테이지_보정값 : 5

    [Space(10f)]
    public float maxObj_ATKDelay;           //최소 : 1.2

    [Space (10f)]
    // 2. 장애물, 드래곤 볼 속도 함수 관련
    [Header("Obstacle Speed")]
    //장애물
    //최종 장애물 속도
    public float Total__ComObj_Speed;
    public float BasicDefault_ComObj_Speed; //기본_Default : 5
    public float BasicPlus_ComObj_Speed;    //기본_가중치 : 0

    [Space(10f)]
    public float EditDefault_ComObj_Speed;  //보정값_Default : 0
    public float EditPlus_ComObj_Speed;     //보정값_가중치 : 1.5

    [Space(10f)]
    public int BasicCorStage_ComObj_Speed;  //보정스테이지_기본 : 0
    public int EditCorStage_ComObj_Speed;   //보정스테이지_보정값 : 10

    [Space(10f)]
    public float max_ComObj_Speed;          //최대(or최소)값 : 15

    [Space (10f)]
    [Header("DragonBall Speed")]
    //드래곤 볼
    //최종 드래곤볼 속도
    public float Total__ComAtk_Speed;
    public float BasicDefault_ComAtk_Speed; //기본_Default : 5
    public float BasicPlus_ComAtk_Speed;    //기본_가중치 : 0

    [Space(10f)]
    public float EditDefault_ComAtk_Speed;  //보정값_Default : 0
    public float EditPlus_ComAtk_Speed;     //보정값_가중치 : 1.5

    [Space(10f)]
    public int BasicCorStage_ComAtk_Speed;  //보정스테이지_기본 : 0
    public int EditCorStage_ComAtk_Speed;   //보정스테이지_보정값 : 10

    [Space(10f)]
    public float max_ComAtk_Speed;          //최대(or최소)값 : 15

    //랜덤 생성
    private int ranObj;      //장애물의 랜덤 좌표
    private int ranAtk;      //드래곤볼의 랜덤 좌표
    private int ranBall;     //드래곤볼 랜덤 종류

    [Space(10f)]
    //타겟위치로 자연스럽게 커지면서 이동하기 위해 타겟설정
    public GameObject[] ObjTargetPoints;

    //풀에서 꺼내오는 뉴 오브젝트
    private GameObject newAtkObj;
    private GameObject newObstacle;

    void Start()
    {
        //시작 후 1초 후에 카운트 다운 함수 호출
        Invoke("OnReady", 1f);

        //시작 시 드래곤 애니메이션 관련 오브젝트들 비활성화 상태로 초기화
        Eye_Atk.SetActive(false);
        Mouse_Atk.SetActive(false);
        Eye_Hit.SetActive(false);

        //드래곤 볼, 장애물 생성 딜레이를 기본 값으로 초기화
        Obj_ATK_TotalDelay = BasicDefaultObj_ATKDelay;

        //PlayerPrefs의 드래곤 사망 정보, 스테이지 값을 1로 초기화 후 저장
        PlayerPrefs.SetInt("isDragonDie", 1);
        PlayerPrefs.SetInt("Stage", 1);
        PlayerPrefs.Save();

        //필살기 게이지 초기화
        Player_SpecialBar.value = 0f;
        Player_TotalSpecial = 10;       //필요 스택
        Player_NowSpecial = 0;          //현재 스택
    }

    void Update()
    {
        //드래곤 사망 정보를 지속적으로 갱신
        isDragonDie = PlayerPrefs.GetInt("isDragonDie");

        //현 스테이지를 지속적으로 받아옴
        nowStage = PlayerPrefs.GetInt("Stage");

        //필살기 슬라이더 바를 지속적으로 초기화(업데이트에서 안하면 슬라이더 클릭시 value값이 증가하는 현상 발생)
        Player_SpecialBar.value = Player_NowSpecial / (float)Player_TotalSpecial;

        //현재 스택이 필요 스택과 같아졌을 때 z를 누르면 필살기 함수 호출
        //부등호 쓴 이유 : 스택이 max(10)를 넘어가면 실행이 안되므로 부등호 써야함
        if(Player_NowSpecial >= Player_TotalSpecial)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                SpecialAtk();
            }
        }

        //눈, 입 공격 애니메이션 재생 시간 관련
        //눈, 입 공격이 활성화 상태일 경우 fdt_Anim에 시간을 누적, 0.5초 보다 커지면 공격 애니메이션 끄는 함수 호출
        if(Eye_Atk.activeSelf || Mouse_Atk.activeSelf)
        {
            fdt_Anim += Time.deltaTime;

            if(fdt_Anim > 0.5f)
            {
                OffAtkAnim();
            }
        }

        //드래곤이 사망 상태인 경우에도 공격 애니메이션 끄는 함수 호출
        if(isDragonDie == 1)
        {
            OffAtkAnim();
        }

        //레디~고 이미지 표시 시간 관련
        //레디 이미지가 활성화 상태인 경우 setTime에 시간을 감소
        if (ReadyImg.activeSelf)
        {
            setTime -= Time.deltaTime;

            //setTime(3초)가 1 보다 작아지면
            if (setTime < 1)
            {
                ReadyImg.SetActive(false);      //레디 이미지 비활성화
                GoImg.SetActive(true);          //Go 이미지 활성화
                soundManager.PlayAudio("Go");   //Go 오디오 실행
                Invoke("OffReady", 1f);         //1초 후 레디 이미지 끄는 함수 호출
            }
        }

        //현 스테이지를 지속적으로 받아와서 속도 업데이트
        Total__ComObj_SpeedCal();   //장애물 속도 함수
        Total__ComAtk_SpeedCal();   //드래곤볼 속도 함수                        

        //드래곤 사망정보를 받아와서 1(사망)이면 드래곤 생성
        if (isDragonDie == 1)
        {
            //사망 시 공격 애니메이션 비활성화
            Eye_Atk.SetActive(false);
            Eye_Hit.SetActive(false);

            //드래곤 스폰 딜레이에 시간을 누적
            fdt_Dragon += Time.deltaTime;

            //드래곤 스폰 딜레이가 3초 보다 커지면 드래곤 소환 함수를 호출
            if (fdt_Dragon > 3.0f)
            {
                SpawnDragon();
                
                fdt = 3.0f;     
                fdt_Dragon = 0;
            }
        }

        //드래곤 살아있을 때 (isDragonDie == 0)
        else
        {
            //피격 상태가 true이면
            if (Dragon.isHit == true)
            {
                //피격 애니메이션 활성화
                Eye_Hit.SetActive(true);

                //폭발 효과 함수 호출
                ExplosionOn();
            }

            //피격 상태가 false이면
            else
                Eye_Hit.SetActive(false);

            //드래곤 볼, 장애물 주기 함수 드래곤 살아 있을 때 지속적으로 호출
            Obj_ATK_TotalDelayCal();   

            //오브젝트 스폰 딜레이에 시간 누적
            fdt += Time.deltaTime;

            //오브젝트 스폰 딜레이 + 0.5초 > 토탈 딜레이 이면
            if (fdt + 0.5f > Obj_ATK_TotalDelay)
            {
                //랜덤한 드래곤볼, 랜덤한 장애물 위치
                ranBall = Random.Range(0, 4);
                ranObj = Random.Range(0, 3);

                /*
                장애물의 랜덤 좌표에 따라서
                장애물이 0(왼쪽)이면 드래곤 볼은 1(중앙), 2(오른쪽) 중에 하나에서 생성 되고     
                장애물이 1(중앙)이면 드래곤 볼은 0(왼쪽), 2(오른쪽) 중에 하나에서 생성 되고
                장애물이 2(오른쪽)이면 드래곤 볼은 0(왼쪽), 1(중앙) 중에 하나에서 생성 된다.
                */
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

                //경고라인 표시(드래곤 공격만)
                //드래곤 볼이 0라인
                if (fdt + 0.5f > Obj_ATK_TotalDelay && ranAtk == 0)
                {
                    Alert_Left.SetActive(true);
                }

                //드래곤 볼이 1 라인
                if (fdt + 0.5f > Obj_ATK_TotalDelay && ranAtk == 1)
                {
                    Alert_Center.SetActive(true);
                }

                //드래곤 볼이 2 라인
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

    //드래곤 스폰 함수
    public void SpawnDragon()
    {
        //드래곤 스폰 시 오브젝트 스폰 함수 취소(?)
        CancelInvoke("SpawnObjects");

        //스테이지를 10으로 나눈 나머지에 따라 스폰할 드래곤 종류 결정
        switch (nowStage % 10)
        {
            case 1:
                Dragons[0].SetActive(true);     // 1 - 블루
                break;
            case 2:
                Dragons[1].SetActive(true);     // 2 - 그린
                break;
            case 3:
                Dragons[2].SetActive(true);     // 3 - 핑크
                break;
            case 4:
                Dragons[3].SetActive(true);     // 4 - 퍼플
                break;
            case 5:
                Dragons[4].SetActive(true);     // 5 - 블랙
                break;
            case 6:
                Dragons[5].SetActive(true);     // 6 - 레드
                break;
            case 7:
                Dragons[6].SetActive(true);     // 7 - 옐로우
                break;
            case 8:
                Dragons[0].SetActive(true);     // 8 - 블루
                break;
            case 9:
                Dragons[1].SetActive(true);     // 9 - 그린
                break;
            case 0:
                Dragons[2].SetActive(true);     // 10 - 핑크
                break;
        }

        //드래곤 리스폰 후에는 다시 사망 정보를 갱신!
        PlayerPrefs.SetInt("isDragonDie", 0);
        PlayerPrefs.Save();
    }

    //장애물, 드래곤 볼 생성 함수
    void SpawnObjects()
    {
        // 1. 드래곤 공격 생성
        //드래곤 공격(눈) 애니메이션 재생
        Eye_Atk.SetActive(true);

        //파이어 볼
        if (ranBall == 0)
        {
            //오브젝트 풀에서 꺼내기
            newAtkObj = objectManager.MakeObj("FireBall");
            newAtkObj.transform.position = SpawnPoints[ranAtk].transform.position;
            soundManager.PlayAudio("Pyro");
        }

        //아이스 볼
        else if (ranBall == 1)
        {
            //오브젝트 풀에서 꺼내기
            newAtkObj = objectManager.MakeObj("IceBall");
            newAtkObj.transform.position = SpawnPoints[ranAtk].transform.position;
            soundManager.PlayAudio("Ice");
        }
        
        //워터 볼
        else if (ranBall == 2)
        {
            //오브젝트 풀에서 꺼내기
            newAtkObj = objectManager.MakeObj("WaterBall");
            newAtkObj.transform.position = SpawnPoints[ranAtk].transform.position;
            soundManager.PlayAudio("Water");
        }

        //일렉트릭 볼
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
            //공격(입) 애니메이션 활성화
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

        // 2. 장애물 생성
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

    //폭발 효과 함수
    void ExplosionOn()
    {
        Explosion.SetActive(true);
        Invoke("ExplosionOff", 0.5f);

        //드래곤이 사망(1) 상태이면 폭발을 1.2 초 후에 끔(드래곤 애니메이션 잔상 방지)
        if (isDragonDie == 1)
            Invoke("ExplosionOff", 1.2f);
    }

    //폭발 효과 끄기 함수
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
        WhiteBG.SetActive(true);
        for(int i = 0; i < 30; i++)
        {
            if (newAtkObj.activeSelf == true || newObstacle.activeSelf == true)
            {
                newAtkObj.SetActive(false);
                newObstacle.SetActive(false);
            }
        }
        // 'Dragon_뭐시기.변수' 로 사용할 수 없는 이유 : Dragon을 Dragon스크립트의 객체로 생성했다면 가능하지만, 위에서 GameObject 형식으로 선언했기 때문에 참조 불가   
        for(int i = 0; i < 7; i++)
        {
            if(Dragons[i].activeSelf == true)
            {
                Dragons[i].GetComponent<Dragon>().Special_Atk(100);
            }
        }

        Player_NowSpecial = 0;
        Player_SpecialBar.value = 0f;
        Invoke("SpecialAtkOff", 0.5f);
    }

    void SpecialAtkOff()
    {
        WhiteBG.SetActive(false);
    }
}
