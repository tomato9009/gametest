using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    void Start()
    {                    //string name,int id,string desc, int damage, int speed, int manacost, int stamcost, int range, level req, heal amount, ItemType type
        items.Add(new Item("White Shirt", 0, "A White Shirt", 2, 0, 0, 0, 0, 0, 0, Item.ItemType.BodySlot));
        items.Add(new Item("Bronze Ring", 1, "Why Does This Go On Neck?", 5, 0, 0, 0, 0, 0, 0, Item.ItemType.NeckSlot));
        items.Add(new Item("Apple", 2, "Heals 5HP", 0, 0, 1, 5, 0, 0, 5, Item.ItemType.Consumable));
        items.Add(new Item("Blunt Sword", 3, "My First Sword", 5, 2, 0, 5, 2, 0, 0, Item.ItemType.MeleeWeapon));


    }  
    

}
