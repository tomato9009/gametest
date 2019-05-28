using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item 
{
    public string  itemName;
    public int itemID;
    public string itemDesc;
    public Texture2D itemIcon;
    public int itemDamage;
    public int itemSpeed;
    public int itemManaCost;
    public int itemStamCost;
    public int itemRange;
    public int itemLevelReq;
    public int itemHealAmount;
    public ItemType itemType;

    public enum ItemType
    {
        MeleeWeapon,
        RangeWeapon,
        Consumable,
        HeadSlot,
        BodySlot,
        LegSlot,
        FeetSlot,
        NeckSlot,
        RingSlot,
        
    }
    public Item(string name,int id,string desc, int damage, int speed, int manacost, int stamcost, int range, int levelReq, int healAmount, ItemType type)
    {
        itemName = name;
        itemID = id;
        itemDesc = desc;
        itemDamage = damage;
        itemSpeed = speed;
        itemManaCost = manacost;
        itemStamCost = stamcost;
        itemRange = range;
        itemType = type;
        itemLevelReq = levelReq;
        itemHealAmount = healAmount;
        itemIcon = Resources.Load<Texture2D>("ItemIcons/" + name); 
    }
    public Item()
    {

    }
}
