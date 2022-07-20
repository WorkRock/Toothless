using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExp : MonoBehaviour
{
    public Slider Exp;

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            Exp.value += 0.1f;

            if(Exp.value >= 1.0f)
            {
                Exp.value = 0;
            }
        }
    }
}
