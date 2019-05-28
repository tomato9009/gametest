using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Image ImgStamBar;
    public int stamMax;
    public int stamMin;
    int currentStamValue;
    float currentStamPercent;
    bool usingStam;
    public ClaytonCont player;

    public void SetStam(int Stam)
    { if (Stam != currentStamValue)
        {
            if (stamMax - stamMin == 0)
            {
                currentStamValue = 0;
                currentStamPercent = 0;
            }
            else
            {
                currentStamValue = Stam;
                currentStamPercent = (float)currentStamValue / (float)(stamMax - stamMin);
            }
            ImgStamBar.fillAmount = currentStamPercent;
        }

    }

    public int getCurrentStamValue
    {
        get { return currentStamValue; }
    }

    int regenAmount = 1;
    float regenTime = 1f;
    bool isRegen;
    void StamRegen()
    {
        if( currentStamValue != stamMax &&!usingStam &&! isRegen)
        {
            StartCoroutine(stamRegen());
        }
        
    }

    IEnumerator stamRegen()

    {
        isRegen = true;
        yield return new WaitForSeconds(regenTime);
        isRegen = false;
       
        SetStam(currentStamValue + regenAmount);
    }
    void Awake()
    {
        SetStam(stamMax);
    }

    // Update is called once per frame
    void Update()
    {
        if(player.drainingStam == true)
        {
            usingStam = true;
        }
        else
        {
            usingStam = false;

        }

        StamRegen();
    }
}
