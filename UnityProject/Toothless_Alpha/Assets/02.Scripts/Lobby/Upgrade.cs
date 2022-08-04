using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public Text UGLevel;
    public Text NeedCoin;
    public Text UGText;
    public Button UGBtn;

    public Text UGDmgNow;
    public Text UGDmgNext;


    public GameObject FirsstUGSliderParent;
    public GameObject SecondUGSliderParent;
    public Slider FirsstUGSlider;
    public Slider SecondUGSlider;


    public int firstUGLevel;
    public int secondUGLevel;
    public int maxUGLevel;

    public int playerLevel;
    public int atkUGLevel;

    //public bool isBtnClicked;
    private bool isCanUG;
    private bool isFirstUG;
    private bool isSecondUG;


    public int totalCoin;
    public int calCoin;
    public int playerNeedCoin;


    public int BasicDefaultNeedCoin;
    public int BasicPlusNeedCoin;
    public int EditDefaultNeedCoin;
    public int EditPlusNeedCoin;
    public int BasicCorUGLevel;
    public int EditCorUGLevel;
    public int maxNeedCoin;


    public float totalUGDMG;
    public float totalUGDMGNext;

    public float BasicDefaultUGDMG;
    public float BasicPlusUGDMG;
    public float EditDefaultUGDMG;
    public float EditPlusUGDMG;
    public float BasicCorUGDMGLevel;
    public float EditCorUGDMGLevel;
    public float maxUGDMG;

    // Start is called before the first frame update
    void Start()
    {
        //isBtnClicked = false;

        // 테스트를 위한 초기화
        PlayerPrefs.SetInt("AtkUG", 1);
        PlayerPrefs.Save();

        atkUGLevel = PlayerPrefs.GetInt("AtkUG");
        playerLevel = PlayerPrefs.GetInt("Level");
        totalCoin = PlayerPrefs.GetInt("Coin");

        playerNeedCoin = BasicDefaultNeedCoin;
        totalUGDMG = BasicDefaultUGDMG;

        calCoin = 0;

        // playerNeedCoin = BasicDefaultNeedCoin + EditDefaultNeedCoin;

        if (playerLevel == 1)
        {
            needCoinCal(playerLevel);
            totalUGDMGCal(playerLevel);
            totalUGDMGNext += (totalUGDMG + totalUGDMGFormula(atkUGLevel+1));
        }

        else
        {
            for (int i = 1; i < playerLevel; i++)
            {
                needCoinCal(i);
                totalUGDMGCal(i);
            }
            totalUGDMGNext += (totalUGDMG + totalUGDMGFormula(atkUGLevel));
        }

    }

    // Update is called once per frame
    void Update()
    {
        totalCoin = PlayerPrefs.GetInt("Coin");

        UGLevel.text = atkUGLevel.ToString();

        UGDmgNow.text = totalUGDMG.ToString();
        UGDmgNext.text = totalUGDMGNext.ToString();


        coinTxt();
        checkUGPossible();

        ATKLogoCheck();
        ATKLogoSlider();

        UGBtn.interactable = isCanUG;
    }

    void coinTxt()
    {
        if (isCanUG)
        {
            NeedCoin.text = playerNeedCoin.ToString();
            UGText.text = "Upgrade";
        }

        else
        {
            if (atkUGLevel == maxUGLevel)
            {
                NeedCoin.text = "<color=green>Max Level</color>";

            }

            else
            {
                NeedCoin.text = "<color=red>" + playerNeedCoin.ToString() + "</color>";
            }
            UGText.text = "<color=grey>Upgrade</color>";
        }
    }

    void checkUGPossible()
    {
        if (totalCoin > playerNeedCoin && atkUGLevel < maxUGLevel)
        {
            isCanUG = true;
        }

        else
        {
            isCanUG = false;
        }
    }

    public void UGbtnClick()
    {
        // Debug.Log("BtnClick");
        //isBtnClicked = true;
        PlayerPrefs.SetInt("isShop", 1);
        PlayerPrefs.Save();
        UpgradeATK();
    }

    void UpgradeATK()
    {
        totalCoin -= playerNeedCoin;
        atkUGLevel++;

        PlayerPrefs.SetInt("AtkUG", atkUGLevel);
        PlayerPrefs.SetInt("Coin", totalCoin);
        PlayerPrefs.Save();

        needCoinCal(atkUGLevel);
        totalUGDMGCal(atkUGLevel);
        totalUGDMGNext += totalUGDMGFormula(atkUGLevel);
    }


    void needCoinCal(int atkUGLevel)
    {

        if (atkUGLevel == 1)
            return;

        else
        {
            totalNeedCoinFormula(this.atkUGLevel);
        }

        playerNeedCoin += calCoin;

        // 만약 max로 잡아놓은 경험치 값보다 높아질 시 max로 통일
        if (playerNeedCoin >= maxNeedCoin)
        {
            playerNeedCoin = maxNeedCoin;
        }
    }

    void totalNeedCoinFormula(int atkUGLevel)
    {
        calCoin = Mathf.FloorToInt((this.atkUGLevel / BasicCorUGLevel) * BasicPlusNeedCoin)
                    + Mathf.FloorToInt(EditDefaultNeedCoin + (this.atkUGLevel / EditCorUGLevel) * EditPlusNeedCoin);
    }


    void totalUGDMGCal(int atkUGLevel)
    {
        if (this.atkUGLevel == 1)
            return;

        else
        {
            totalUGDMG += totalUGDMGFormula(atkUGLevel);
        }


        // 만약 max로 잡아놓은 경험치 값보다 높아질 시 max로 통일
        if (totalUGDMG >= maxUGDMG)
        {
            totalUGDMG = maxUGDMG;
        }
    }

    float totalUGDMGFormula(int atkUGLevel)
    {
        float calUGDMG;
        calUGDMG = Mathf.FloorToInt((this.atkUGLevel / BasicCorUGDMGLevel) * BasicPlusUGDMG)
                    + Mathf.FloorToInt(EditDefaultUGDMG + (this.atkUGLevel / EditCorUGDMGLevel) * EditPlusUGDMG);
        return calUGDMG;
    }





    void ATKLogoCheck()
    {
        if (atkUGLevel < firstUGLevel)
        {
            isFirstUG = true;
            isSecondUG = false;
        }

        else
        {
            isFirstUG = false;
            isSecondUG = true;
        }

        FirsstUGSliderParent.SetActive(isFirstUG);
        SecondUGSliderParent.SetActive(isSecondUG);
    }

    void ATKLogoSlider()
    {
        if (isFirstUG)
        {
            if (atkUGLevel == 1)
            {
                FirsstUGSlider.value = 0 / (float)firstUGLevel;
            }

            else
            {
                FirsstUGSlider.value = atkUGLevel / (float)firstUGLevel;
            }


        }

        else if (isSecondUG)
        {
            float checkValue = (atkUGLevel - firstUGLevel) / (float)firstUGLevel;

            if (checkValue >= 1)
            {
                checkValue = 1;
            }

            SecondUGSlider.value = checkValue;
        }
    }
}
