using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public Text StageText;

    public static int Stage;

    // Start is called before the first frame update
    void Start()
    {
        //현재 스테이지
        Stage = PlayerPrefs.GetInt("Stage", 1);
        StageText.text = "STAGE " + Stage.ToString("0");
    }

    // Update is called once per frame
    void Update()
    {
     
         Stage = PlayerPrefs.GetInt("Stage", Stage);
         StageText.text = "STAGE " + Stage.ToString("0");
    
    }
}
