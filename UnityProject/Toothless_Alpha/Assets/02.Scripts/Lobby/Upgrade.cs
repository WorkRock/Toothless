using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public Text UGLevel;
    public Text NeedCoin;

    public Button UGBtn;

    public int playerLevel;
    public int atkUGLevel;


    private bool isCanUG;

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
    // Start is called before the first frame update
    void Start()
    {
        // 테스트를 위한 초기화
        PlayerPrefs.SetInt("AtkUG", 1);
        PlayerPrefs.Save();
        atkUGLevel = PlayerPrefs.GetInt("AtkUG");
        playerLevel = PlayerPrefs.GetInt("Level");
        totalCoin = PlayerPrefs.GetInt("Coin");

        calCoin = 0;

        playerNeedCoin = BasicDefaultNeedCoin;

        for (int i = 1; i < playerLevel + 1; i++)
            needCoinCal(i);
    }

    // Update is called once per frame
    void Update()
    {
        totalCoin = PlayerPrefs.GetInt("Coin");
        UGLevel.text = atkUGLevel.ToString();
        NeedCoin.text = playerNeedCoin.ToString();

        checkUGPossible();

        UGBtn.interactable = isCanUG;
    }

    void checkUGPossible()
    {
        if (totalCoin > playerNeedCoin)
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
        UpgradeATK();
    }

    void UpgradeATK()
    {
        totalCoin -= playerNeedCoin;
        atkUGLevel++;
        PlayerPrefs.SetInt("Coin", totalCoin);
        PlayerPrefs.Save();
        needCoinCal(atkUGLevel);

    }


    void needCoinCal(int atkUGLevel)
    {
        if (atkUGLevel == 1)
            return;

        else
        {
            totalNeedCoinFormula(atkUGLevel);
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
        calCoin = Mathf.FloorToInt((atkUGLevel / BasicCorUGLevel) * BasicPlusNeedCoin)
                    + Mathf.FloorToInt(EditDefaultNeedCoin + (atkUGLevel / EditCorUGLevel) * EditPlusNeedCoin);
    }
}
