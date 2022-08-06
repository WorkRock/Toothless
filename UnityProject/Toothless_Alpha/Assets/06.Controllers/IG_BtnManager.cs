using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IG_BtnManager : MonoBehaviour
{
    public GameObject Func;

    private int isSoundOn;
    
    public GameObject SoundOn;
    public GameObject SoundOff;

    private bool isFuncOn;

    // Start is called before the first frame update
    void Start()
    {
        isSoundOn = PlayerPrefs.GetInt("isSoundOn");
    }

    // Update is called once per frame
    void Update()
    {
        if(isFuncOn)
        {
            if (isSoundOn == 0)
            {
                SoundOff.SetActive(true);
                SoundOn.SetActive(false);
            }

            else
            {
                SoundOff.SetActive(false);
                SoundOn.SetActive(true);
            }
        }
        
    }
    public void FuncOn()
    {
        Func.SetActive(true);
        isFuncOn = true;
        Time.timeScale = 0.0f;
    }

    public void FuncExit()
    {
        Func.SetActive(false);
        isFuncOn = false;
        Time.timeScale = 1.0f;
    }

    public void optionFunc()
    {
        if (isSoundOn == 1)
        {
            isSoundOn = 0;
        }

        else
        {
            isSoundOn = 1;
        }

        PlayerPrefs.SetInt("isSoundOn", isSoundOn);
        PlayerPrefs.Save();
    }

}
