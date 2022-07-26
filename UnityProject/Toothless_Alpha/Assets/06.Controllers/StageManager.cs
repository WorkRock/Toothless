using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public Text StageText;

    public int Stage;

    // Update is called once per frame
    void Update()
    {
        Stage = PlayerPrefs.GetInt("Stage");
        StageText.text = "STAGE " + Stage.ToString();
    }
}
