using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image ImgHPBar;
    public int HPMax;
    public int HPMin;
    int currentHPValue;
    float currentHPPercent;
    
    public ClaytonCont player;

    public void SetHP(int HP)
    {
        if (HP != currentHPValue)
        {
            if (HPMax - HPMin == 0)
            {
                currentHPValue = 0;
                currentHPPercent = 0;
            }
            else
            {
                currentHPValue = HP;
                currentHPPercent = (float)currentHPValue / (float)(HPMax - HPMin);
            }
            ImgHPBar.fillAmount = currentHPPercent;
        }

    }

    public int getCurrentHPValue
    {
        get { return currentHPValue; }
    }
    int regenAmount = 1;
    float regenTime = 2f;
    bool isRegen;
    void HPRegen()
    {
        if (currentHPValue != HPMax  && !isRegen)
        {
            StartCoroutine(hpRegen());
        }

    }

    IEnumerator hpRegen()

    {
        isRegen = true;
        yield return new WaitForSeconds(regenTime);
        isRegen = false;

        SetHP(currentHPValue + regenAmount);
    }
    void Awake()
    {
        SetHP(HPMax);
    }

    // Update is called once per frame
    void Update()
    {
        HPRegen();
    }
}
