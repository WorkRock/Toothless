using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Move : MonoBehaviour
{
    //인게임 사운드 매니저
    public IG_SoundManager soundManager;

    [Header ("UI")]
    public GameObject GameOverImg;              //게임오버
    public Slider Player_HPBar;                 //플레이어 체력바
    public GameObject[] shieldImgs;             //쉴드 이미지 UI
    public Image ShieldCoolTime;                //쉴드 쿨타임 이미지
    public Text ShieldCoolTimeText;             //쉴드 쿨타임 텍스트 표시

    [Space(10f)]
    [Header("Player")] 
    public GameObject[] targetPos;              //이동할 위치 배열로 선언
    public CapsuleCollider2D capsuleCollider2D; //플레이어의 콜라이더 연결
    public SpriteRenderer spriteRenderer;       //무적상태일 때 플레이어 흐리게

    private float playerSpeed = 0.2f;           //이동 속도(고정)
    private float fHor;                         //이동 방향(-1 or 1)
    //위치 인덱스
    private int minPos; //0
    private int nowPos; //현 위치
    private int maxPos; //2

    [Space(10f)]
    [Header("Shield")] 
    public GameObject[] playerShield;           //플레이어 쉴드 오브젝트 연결
    //쉴드 딜레이
    public float curDelay = 0f;                 // 현재 쉴드 유지 시간
    public float maxDelay = 1f;                 // 쉴드 유지 시간 max
    public float curShieldDelay;                // 현재 쉴드 쿨타임
    public float maxShieldDelay;                // 쉴드 쿨타임 max
    private bool isShieldOn;                    // 쉴드 ON/OFF 체크

    //쉴드 업그레이드(업그레이드 비례)
    private float BasicDefaultShieldDelay = 3;  //기본 쉴드 딜레이(3초)
    private float EditPlusShieldDelay = 0.45f;  //보정값 가중치(0.45초)
    private float EditCorAtkUGSD = 10;          
    
    private int playerShieldNum;                //쉴드 번호
    private int onShieldNum;                    //현재 활성화된 쉴드 번호

    [Space(10f)]
    [Header ("Player Func")]
    // 플레이어 함수 공통
    public int nowLevel;

    [Header("Player HP")]
    // 1. 플레이어 HP
    //플레이어 체력
    public int Player_TotalHP;                  //HP(분모)
    public int Player_NowHP;                    //HP(분자)
    
    [Space(10f)]
    public int BasicDefaultHp;                  //기본_Default : 100
    public int BasicPlusHp;                     //기본_가중치 : 3
    
    [Space(10f)]
    public int EditDefaultHp;                   //보정값_Default : 0
    public int EditPlusHp;                      //보정값_가중치 : 20

    [Space(10f)]
    public int BasicCorLevel_HP;                //보정레벨_기본 : 0
    public int EditCorLevel_HP;                 //보정레벨_보정값 : 10

    [Space(10f)]
    public int maxHp;                           //최댓값 : 2000

    [Space(10f)]
    [Header("Player Atk")]
    // 2. 플레이어 공격력
    public float Player_TotalAtk;               //최종 공격력
    public int BasicDefaultPlayer_Atk;          //기본_Default : 30
    public int BasicPlusPlayer_Atk;             //기본_가중치 : 0

    [Space(10f)]
    public int EditDefaultPlayer_Atk;           //보정값_Default : 0
    public int EditPlusPlayer_Atk;              //보정값_가중치 : 15

    [Space(10f)]
    public int BasicCorLevel_Atk;               //보정레벨_기본 : 0
    public int EditCorLevel_Atk;                //보정레벨_보정값 : 10

    [Space(10f)]
    public int maxPlayer_Atk;                   //최대(or최소)값 : 500

    [Space(10f)]
    [Header("AtkUG")]
    // 3. 플레이어 공격력 업그레이드
    public float totalUGDMG;                    //최종 업그레이드 데미지
    public int atkUGLevel;                      //공격력 업그레이드 레벨

    [Space(10f)]
    public float BasicDefaultUGDMG;             //기본_Default : 0
    public float BasicPlusUGDMG;                //기본_가중치 : 0.037

    [Space(10f)]
    public float EditDefaultUGDMG;              //보정값_Default : 0
    public float EditPlusUGDMG;                 //보정값_가중치 : 0.075

    [Space(10f)]
    public int BasicCorUGDMGLevel;              //보정레벨_기본 : 1
    public int EditCorUGDMGLevel;               //보정레벨_가중치 : 5
    [Space(10f)]
    public float maxUGDMG;                      //업그레이드 데미지 최댓값 : 3

    [Space(10f)]
    [Header("Obj Damage Func")]
    // 오브젝트 함수 공통
    public int nowStage;

    [Space(10f)]
    [Header("Obstacle Damage")]
    // 4. 장애물 공격력 함수  
    public int Total_ComObj_Atk;                //최종 장애물 데미지
    public int BasicDefault_ComObj_Atk;         //기본_Default : 10
    public int BasicPlus_ComObj_Atk;            //기본_가중치 : 2

    [Space(10f)]
    public int EditDefault_ComObj_Atk;          //보정값_Default : 0
    public int EditPlus_ComObj_Atk;             //보정값_가중치 : 20

    [Space(10f)]
    public int BasicCorStage_ComObj_Atk;        //보정스테이지_기본 : 0
    public int EditCorStage_ComObj_Atk;         //보정스테이지_보정값 : 10
    [Space(10f)]
    public int max_ComObj_Atk;                  //최대(or최소)값 : 99999

    [Space(10f)]
    [Header("DragonBall Damage")]
    // 5. 드래곤 공격력 함수
    public int Total_ComAtk_Atk;                //최종 드래곤볼 데미지
    public int BasicDefault_ComAtk_Atk;         //기본_Default : 10
    public int BasicPlus_ComAtk_Atk;            //기본_가중치 : 2

    [Space(10f)]
    public int EditDefault_ComAtk_Atk;          //보정값_Default : 0
    public int EditPlus_ComAtk_Atk;             //보정값_가중치 : 20

    [Space(10f)]
    public int BasicCorStage_ComAtk_Atk;        //보정스테이지_기본 : 0
    public int EditCorStage_ComAtk_Atk;         //보정스테이지_보정값 : 10
    [Space(10f)]
    public int max_ComAtk_Atk;                  //최대(or최소)값 : 99999

    // Start is called before the first frame update
    void Start()
    {
        //쉴드 딜레이 초기화
        curShieldDelay = 0;
        //시작 시 쉴드 번호는 0번(Pyro 쉴드)로 초기화
        playerShieldNum = 0;

        //플레이어 체력, 공격력(업그레이드)는 로비에서 업그레이드 하여 게임중에는 변하지 않으므로 Start에서 한번만 호출

        //PlayerPrefs에서 공격력 업그레이드 레벨 값 받아오기
        atkUGLevel = PlayerPrefs.GetInt("AtkUG");
        
        // 0. 쉴드 딜레이 계산
        if (atkUGLevel == 1)
            maxShieldDelay = BasicDefaultShieldDelay;
        else
            maxShieldDelay = BasicDefaultShieldDelay - (Mathf.FloorToInt(atkUGLevel / EditCorAtkUGSD) * EditPlusShieldDelay);

        //PlayerPrefs에서 현재 레벨 값 받아오기
        nowLevel = PlayerPrefs.GetInt("Level");

        // 1. 플레이어 체력 계산
        //체력 초기화
        Player_TotalHP = BasicDefaultHp;
        totalHpCal();
        Player_NowHP = Player_TotalHP;
     
        // 2. 플레이어 공격력 계산
        totalPlayer_AtkCal();
        totalUGDMG = BasicDefaultUGDMG;
        
        //업그레이드 레벨이 1이 아닐 때 실행
        if(atkUGLevel !=1)
            totalUGDMGCal(atkUGLevel - 1);
        
        //플레이어 최종 공격력 = 플레이어 최종 공격력 * 최종 업그레이드 데미지
        Player_TotalAtk *= totalUGDMG;
       
        //인덱스 초기화(시작은 가운데에서)
        minPos = 0;
        nowPos = 1;
        maxPos = targetPos.Length - 1;

        //쉴드는 시작할 때 비활성화 상태로 초기화
        for (int i = 0; i < playerShield.Length; i++)
        {
            playerShield[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어 이동 함수
        MovePlayer();

        //방향키 위, 아래를 눌러서 쉴드 스왑 함수 호출
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ShieldSwap();
        }

        //쉴드 켜기 함수
        ShieldOn();

        //장애물, 드래곤볼 데미지 계산을 위해 스테이지 값 지속적으로 받아오기
        nowStage = PlayerPrefs.GetInt("Stage");

        //장애물, 드래곤볼 공격력 지속적으로 계산
        Total_ComObj_AtkCal();
        Total_ComAtk_AtkCal();

        //쉴드 쿨타임 UI 이미지 표시
        ShieldCoolTime.fillAmount = 1.0f - Mathf.Lerp(0, 100, (curShieldDelay / maxShieldDelay) / 100);
        
        //쉴드딜레이가 max보다 커질 경우(쿨타임이 다 찼을 경우) 쿨타임 텍스트(남은 시간)을 비활성화
        if(curShieldDelay >= maxShieldDelay)
        {
            ShieldCoolTimeText.enabled = false;    
        }
        
        //쿨타임이 덜 찼을 경우 남은시간을 활성화, 텍스트엔 남은시간을 소수 첫째 자리까지 표시
        else
        {
            ShieldCoolTimeText.enabled = true;
            ShieldCoolTimeText.text = (maxShieldDelay - curShieldDelay).ToString("F1");
        }

        //플레이어 체력바 조정(슬라이더 밸류값으로 조정)
        Player_HPBar.value = Player_NowHP / (float)Player_TotalHP;
        
        //쉴드 이미지가 지속적으로 현재 플레이어 쉴드 번호에 맞게 활성화 & 비활성화 됨
        for (int i = 0; i < shieldImgs.Length; i++)
        {
            if (i == playerShieldNum)
            {
                shieldImgs[i].SetActive(true);
            }

            else
                shieldImgs[i].SetActive(false);
        }

        //플레이어 쉴드의 활성화 여부를 검사하여 쉴드 위치에 플레이어의 위치를 대입, 이동할 때 쉴드 끊김 현상을 방지
        for (int i = 0; i < playerShield.Length; i++)
        {
            if(playerShield[i].activeSelf)
            {
                playerShield[i].transform.position = gameObject.transform.position;
            }
        }
    }

    //플레이어 이동 함수
    public void MovePlayer()
    {
        //키보드 좌우 입력했을 때
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            //방향 값(-1 or 1)을 fHor에 대입
            fHor = Input.GetAxisRaw("Horizontal");

            //0 <= nowPos <= 2 이면 (0~2사이에서만 이동 가능)
            if (nowPos + (int)fHor <= maxPos && nowPos + (int)fHor >= minPos)
            {
                //현재 인덱스에 방향 값 +
                nowPos += (int)fHor;
            }
        }
        //현 위치 -> 이동할 위치 (반드시 if문 밖에서)
        transform.position = Vector3.MoveTowards(transform.position, targetPos[nowPos].transform.position, playerSpeed);
    }

    //쉴드 스왑 함수
    public void ShieldSwap()
    {
        float fVer = Input.GetAxisRaw("Vertical");
        //현 쉴드 번호에 1 or -1 을 누적
        playerShieldNum += (int)fVer;
        
        //쉴드 번호가 쉴드 개수를 넘어갈 경우 0번(Pyro)으로 초기화
        if(playerShieldNum > playerShield.Length-1)
        {
            playerShieldNum = 0;
        }
        //쉴드 번호가 음수로 갈 경우 거꾸로 스왑
        else if(playerShieldNum < 0)
        {
            playerShieldNum = playerShield.Length - 1;
        }
    }

    //쉴드 활성화 함수
    public void ShieldOn()
    {
        //쉴드 쿨타임이 max보다 작으면 쉴드쿨타임에 시간을 누적
        if (curShieldDelay < maxShieldDelay)
        {
            curShieldDelay += Time.deltaTime;
        }

        //쿨타임이 max보다 커지면 쉴드 쿨타임이 max(쿨타임 다 참)
        else 
        {
            curShieldDelay = maxShieldDelay;
        }
            
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //쉴드 쿨타임이 다 찼을 때 Space를 누르면
            if (curShieldDelay >= maxShieldDelay)
            {
                soundManager.PlayAudio("ShieldOn");             //쉴드 활성화 소리 재생
                isShieldOn = true;                              //쉴드활성화 상태를 true로
                playerShield[playerShieldNum].SetActive(true);  //현재 플레이어의 쉴드 번호에 맞는 쉴드를 활성화
                onShieldNum = playerShieldNum;                  
                
                // 0 - pyro, 1 - ice, 2 - water, 3 - electro
                //onShieldNum에 맞는 태그로 플레이어의 태그를 변경
                switch (onShieldNum)
                {
                    case 0:
                        gameObject.tag = "PyroShield";
                        break;
                    case 1:
                        gameObject.tag = "IceShield";
                        break;
                    case 2:
                        gameObject.tag = "WaterShield";
                        break;
                    case 3:
                        gameObject.tag = "ElectroShield";
                        break;
                }        
                //쉴드 키고 나면 쉴드 쿨타임을 다시 0초로 초기화
                curShieldDelay = 0;
            }
        }

        //쉴드 지속 시간 검사 : 쉴드 활성화 상태가 true인 동안에는 curDelay에 시간을 누적
        if (isShieldOn)
        {
            curDelay += Time.deltaTime;
        }

        //curDelay(쉴드 지속시간)이 max보다 커지면
        if (curDelay > maxDelay)
        {
            ShieldOff();            //쉴드 비활성화 함수 호출
            isShieldOn = false;     //쉴드 활성화 상태를 false로
            curDelay = 0;           //쉴드 지속시간을 0으로 초기화
        }
    }

    //쉴드 비활성화 함수
    public void ShieldOff()
    {
        //다시 플레이어의 태그를 Player로 바꿔주고, 현재 활성화된 쉴드를 비활성화 시켜줌
        gameObject.tag = "Player";
        playerShield[onShieldNum].SetActive(false);
    }

    //플레이어 충돌 관련 계산
    void OnTriggerEnter2D(Collider2D collision)
    {
        //같은 속성의 공격만 방어 가능, 공격과 일치하는 쉴드가 아니면 방어 불가하도록 태그를 검사
        if (collision.gameObject.tag.Equals("Dragon_Atk_Fire")
            || collision.gameObject.tag.Equals("Dragon_Atk_Ice")
            || collision.gameObject.tag.Equals("Dragon_Atk_Water")
            || collision.gameObject.tag.Equals("Dragon_Atk_Electric"))
        {
            if (collision.gameObject.tag.Equals("Dragon_Atk_Fire") && gameObject.tag.Equals("PyroShield")
                ||  collision.gameObject.tag.Equals("Dragon_Atk_Ice") && gameObject.tag.Equals("IceShield")
                ||  collision.gameObject.tag.Equals("Dragon_Atk_Water") && gameObject.tag.Equals("WaterShield")
                ||  collision.gameObject.tag.Equals("Dragon_Atk_Electric") && gameObject.tag.Equals("ElectroShield"))
            {
                //드래곤볼과 플레이어의 태그가 속성이 일치할 경우, 리턴
                return;
            }
                
            //맞았을 때 사운드 재생(3개중에 랜덤 재생)
            int ranHitSound = Random.Range(0, 3);
            if (ranHitSound == 0)
                soundManager.PlayAudio2("Hit_1");
            else if(ranHitSound == 1)
                soundManager.PlayAudio2("Hit_2");
            else if (ranHitSound == 2)
                soundManager.PlayAudio2("Hit_3");

            //충돌한 상대방(드래곤볼) 비활성화
            collision.gameObject.SetActive(false);

            //맞을때마다 현재 체력에서 드래곤 공격력만큼 뺌
            Player_NowHP -= Total_ComAtk_Atk;

            //무적상태 함수 호출
            OnSuperEffect();
            //1초 후 무적상태 해제 함수 호출
            Invoke("OffSuperEffect", 1f);

            Debug.Log($"플레이어 체력 : {Player_NowHP}");

            //체력이 0이하로 떨어지면
            if (Player_NowHP <= 0)
            {
                Player_NowHP = 0;                
                Player_HPBar.value = 0;
                
                //쉴드 비활성화
                ShieldOff();

                //사망 사운드 재생(2개중에 랜덤 재생)
                int ranDieSound = Random.Range(0, 2);
                if (ranDieSound == 0)
                    soundManager.PlayAudio("Die_1");
                else
                    soundManager.PlayAudio("Die_2");

                //플레이어 비활성화
                gameObject.SetActive(false);

                //게임오버 텍스트 활성화
                GameOverImg.SetActive(true);

                //3초 후 Result 씬으로 이동하는 함수 호출
                Invoke("ShowResult", 3f);         
            }
        }

        //장애물과 충돌 시
        else if (collision.gameObject.tag.Equals("Obstacle"))
        {
            //맞았을 때 사운드 재생(3개중에 랜덤 재생)
            int ranHitSound = Random.Range(0, 3);
            if (ranHitSound == 0)
                soundManager.PlayAudio2("Hit_1");
            else if(ranHitSound == 1)
                soundManager.PlayAudio2("Hit_2");
            else if (ranHitSound == 2)
                soundManager.PlayAudio2("Hit_3");

            //장애물 비활성화
            collision.gameObject.SetActive(false);

            //맞을때마다 현재 체력에서 장애물 데미지만큼 뺌
            Player_NowHP -= Total_ComObj_Atk;

            //무적상태 함수 호출
            OnSuperEffect();
            //1초 후 무적상태 해제 함수 호출
            Invoke("OffSuperEffect", 1f);

            Debug.Log($"플레이어 체력 : {Player_NowHP}");

            //체력이 0이하로 떨어지면
            if (Player_NowHP <= 0)
            {
                Player_NowHP = 0;
                Player_HPBar.value = 0;

                //쉴드 비활성화
                ShieldOff();

                //사망 사운드 재생(2개중에 랜덤 재생)
                int ranDieSound = Random.Range(0, 2);
                if (ranDieSound == 0)
                    soundManager.PlayAudio("Die_1");
                else
                    soundManager.PlayAudio("Die_2");

                //플레이어 비활성화
                gameObject.SetActive(false);

                //게임오버 텍스트 활성화
                GameOverImg.SetActive(true);

                //3초 후 Result 씬으로 이동하는 함수 호출
                Invoke("ShowResult", 3f);
            }
        }
    }

    //무적상태 함수
    void OnSuperEffect()
    {
        //플레이어의 콜라이더를 끄고, 색깔을 흐리게
        capsuleCollider2D.enabled = false;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
    }

    //무적상태 해제 함수
    void OffSuperEffect()
    {
        //콜라이더를 다시 켜고, 색깔을 원래대로
        capsuleCollider2D.enabled = true;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    //플레이어 관련 함수
    // 1. 플레이어 HP
    void totalHpCal()
    {
        if (nowLevel == 1)
            return;
        else if ((BasicDefaultHp + ((nowLevel - 1) * BasicPlusHp)) +
                           EditDefaultHp + Mathf.FloorToInt(((nowLevel - 1) / (float)EditCorLevel_HP) * EditPlusHp) >= maxHp)
            Player_TotalHP = maxHp;
        else
        {
            Player_TotalHP = (BasicDefaultHp + ((nowLevel - 1) * BasicPlusHp)) +
                           EditDefaultHp + Mathf.FloorToInt((nowLevel - 1) / (float)EditCorLevel_HP) * EditPlusHp;
        }
    }

    // 2. 플레이어 공격력
    void totalPlayer_AtkCal()
    {

        if ((BasicDefaultPlayer_Atk + ((nowLevel - 1) * BasicPlusPlayer_Atk)) +
                            EditDefaultPlayer_Atk + Mathf.FloorToInt((nowLevel - 1) / (float)EditCorLevel_Atk) * EditPlusPlayer_Atk >= maxPlayer_Atk)
            Player_TotalAtk = maxPlayer_Atk;
        else
            Player_TotalAtk = (BasicDefaultPlayer_Atk + ((nowLevel - 1) * BasicPlusPlayer_Atk)) +
                            EditDefaultPlayer_Atk + Mathf.FloorToInt((nowLevel - 1) / (float)EditCorLevel_Atk) * EditPlusPlayer_Atk;
    }

    // 3. 플레이어 업그레이드 공격력
    void totalUGDMGCal(int atkUGLevel)
    {
        if (this.atkUGLevel == 1)
            return;

        else
        {
            totalUGDMG = totalUGDMGFormula(this.atkUGLevel);
            Debug.Log("추가한 값 : " + totalUGDMG);
            Debug.Log("this.atkUGLevel-1 : " + (this.atkUGLevel));
        }

        // 만약 max값보다 높아질 시 max로 통일
        if (totalUGDMG >= maxUGDMG)
        {
            totalUGDMG = maxUGDMG;
        }
    }

    float totalUGDMGFormula(int toAtkUGLevel)
    {
        float calUGDMG;
        Debug.Log("this.atkUGLevel : " + (toAtkUGLevel - 1));
        Debug.Log("atkUGLevel : " + atkUGLevel);
        Debug.Log("1 : " + Mathf.FloorToInt(((toAtkUGLevel - 1) / BasicCorUGDMGLevel)));
        Debug.Log("2 : " + Mathf.FloorToInt(((toAtkUGLevel - 1) / BasicCorUGDMGLevel)) * BasicPlusUGDMG);
        Debug.Log("3 : " + Mathf.FloorToInt((toAtkUGLevel - 1) / EditCorUGDMGLevel));
        Debug.Log("4 : " + Mathf.FloorToInt((toAtkUGLevel - 1) / EditCorUGDMGLevel) * EditPlusUGDMG);
        calUGDMG = BasicDefaultUGDMG + Mathf.FloorToInt(((toAtkUGLevel - 1) / BasicCorUGDMGLevel)) * BasicPlusUGDMG
                    + EditDefaultUGDMG + Mathf.FloorToInt((toAtkUGLevel) / EditCorUGDMGLevel) * EditPlusUGDMG;

        Debug.Log("업그레이드 시 추가되는 값 : " + calUGDMG);
        return calUGDMG;
    }

    // 4. 장애물 공격력
    void Total_ComObj_AtkCal()
    {
        if (((BasicDefault_ComObj_Atk + ((nowStage - 1) * BasicPlus_ComObj_Atk)) +
                            EditDefault_ComObj_Atk + Mathf.FloorToInt((nowStage - 1) / EditCorStage_ComObj_Atk) * EditPlus_ComObj_Atk) >= max_ComObj_Atk)
            Total_ComObj_Atk = max_ComObj_Atk;
        else
            Total_ComObj_Atk = ((BasicDefault_ComObj_Atk + ((nowStage - 1) * BasicPlus_ComObj_Atk)) +
                            EditDefault_ComObj_Atk + Mathf.FloorToInt((nowStage - 1) / EditCorStage_ComObj_Atk) * EditPlus_ComObj_Atk);
    }

    // 5. 드래곤 공격력
    void Total_ComAtk_AtkCal()
    {
        if (((BasicDefault_ComAtk_Atk + ((nowStage - 1) * BasicPlus_ComAtk_Atk)) +
                            EditDefault_ComAtk_Atk + Mathf.FloorToInt((nowStage - 1) / EditCorStage_ComAtk_Atk) * EditPlus_ComAtk_Atk) >= max_ComAtk_Atk)
            Total_ComAtk_Atk = max_ComAtk_Atk;
        else
            Total_ComAtk_Atk = ((BasicDefault_ComAtk_Atk + ((nowStage - 1) * BasicPlus_ComAtk_Atk)) +
                            EditDefault_ComAtk_Atk + Mathf.FloorToInt((nowStage - 1) / EditCorStage_ComAtk_Atk) * EditPlus_ComAtk_Atk);
    }

    //결과 화면 표출 함수
    void ShowResult()
    {
        //시간 멈추고, 결과 씬으로 이동, PlayerPrefs "Stage"의 value 값을 nowStage로 저장
        Time.timeScale = 0;
        SceneManager.LoadScene("Result");
        PlayerPrefs.SetInt("Stage", nowStage);
    }
}
