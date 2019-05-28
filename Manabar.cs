using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manabar : MonoBehaviour
{

    public Image ImgMPBar;
    public int MPMax;
    public int MPMin;
    int currentMPValue;
    float currentMPPercent;

    public ClaytonCont player;
    public void SetMP(int MP)
    {
        if (MP != currentMPValue)
        {
            if (MPMax - MPMin == 0)
            {
                currentMPValue = 0;
                currentMPPercent = 0;
            }
            else
            {
                currentMPValue = MP;
                currentMPPercent = (float)currentMPValue / (float)(MPMax - MPMin);
            }
            ImgMPBar.fillAmount = currentMPPercent;
        }

    }

    public int getCurrentMPValue
    {
        get { return currentMPValue; }
    }
    int regenAmount = 1;
    float regenTime = 2f;
    bool isRegen;
    void MPRegen()
    {
        if (currentMPValue != MPMax && !isRegen)
        {
            StartCoroutine(mpRegen());
        }

    }

    IEnumerator mpRegen()

    {
        isRegen = true;
        yield return new WaitForSeconds(regenTime);
        isRegen = false;

        SetMP(currentMPValue + regenAmount);
    }
    void Awake()
    {
        SetMP(MPMax);
    }

    // Update is called once per frame
    void Update()
    {
        MPRegen();
    }
}

