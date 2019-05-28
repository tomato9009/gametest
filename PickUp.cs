using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public int itemID;
    public Inventory inv;
    public Transform clayton;
    bool inRange;
    public Hotbar hbar;
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            inRange = true;
           
        }
    }

    
    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            transform.GetChild(0).rotation = Camera.main.transform.rotation;

        }
    }
    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            inRange = false;
        }
    }
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (inRange == true && Input.GetKey(KeyCode.E))
        {
            if (!inv.invFull)
            {
                inv.AddItem(itemID);

                Destroy(gameObject);
            }
           else if (!hbar.hotbarfull)
            {
                hbar.AddItemHotbar(itemID);
                Destroy(gameObject);
                
            }
            if(hbar.hotbarfull && inv.invFull)
            {
                print("full");
            }
               
        }
       
    }
}
