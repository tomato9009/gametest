using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    public int slotsX, slotsY;
    public GUISkin skin;
    public List<Item> inventory = new List<Item>();
    public List<Item> slots = new List<Item>();
    private ItemDatabase database;
    public bool showInventory;
    public bool showTooltip;
    public string tooltip;
    public bool draggingItem;
    public Item draggedItem;
    public int prevIndex;
    public bool invFull;
    public ClaytonCont clayton;
    public Hotbar hbar;
    

    
    void Start()
    {
        for (int i = 0; i < (slotsX*slotsY); i++) // calculate amount of slots + add empty items
        {
            slots.Add(new Item());
            inventory.Add(new Item());
        }
        database = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<ItemDatabase>();

        

       
    }
    void OnGUI()
    {
        
        tooltip = "";
        GUI.skin = skin;
        if (showInventory)
        {
            DrawInventory();
            if (showTooltip) // draw tooltip
            {
                GUI.Box(new Rect(Event.current.mousePosition.x + 15f, Event.current.mousePosition.y, 200, 200), tooltip, skin.GetStyle("Tooltip"));
            }
            if (draggingItem) //draw item icons if dragging
            {
                showTooltip = false;
                GUI.DrawTexture(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 50, 50), draggedItem.itemIcon);
            }
        }
       

      
    }
    void CheckInventoryFull()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemName == null)
            {
                invFull = false;
               
                break;
            }
            else
                invFull = true;
            
        }
    }
    void DrawInventory()
    {
        Event e = Event.current;
        int i = 0;
        for (int y = 0 ; y < slotsY; y++) //slots on y
        {
            for (int x = 0 ; x < slotsX; x++) //slots on x
            {
                Rect slotRect = new Rect  (Screen.width/3 + x * 60, Screen.height /4 + y * 60, 50, 50)  ; // slot location and size
                
                GUI.Box(new Rect(slotRect), "", skin.GetStyle("Slot")); // draw box
                slots[i] = inventory[i]; // list of slots is now equal to the amount of newly drawn invslots
                if (slots[i].itemName != null) // if the slot does have an item in it, based off of name, empty item doesnt have a name
                {
                    GUI.DrawTexture(slotRect, slots[i].itemIcon); //draw the icon of the item that is in the slots
                   if (slotRect.Contains(Event.current.mousePosition)) // if your cursor is is the box
                    {
                       tooltip = CreatTooltip(slots[i]); //show tooltip 
                        showTooltip = true;
                        if (e.button == 0 && e.type == EventType.MouseDrag && !draggingItem ) //drag item and empty slot
                        {
                            draggingItem = true;
                            prevIndex = i;
                            draggedItem = slots[i];
                            inventory[i] = new Item();
                        }
                        if (e.type == EventType.MouseUp && draggingItem )
                        {
                            inventory[prevIndex] = inventory[i];
                            inventory[i] = draggedItem;
                            draggingItem = false;
                            draggedItem = null;
                        }
                       
                        if (e.isMouse && e.type == EventType.MouseDown && e.button==1)
                        {
                            if (slots[i].itemType == Item.ItemType.Consumable)
                            {
                                
                                clayton.HPBar.SetHP(clayton.HPBar.getCurrentHPValue + slots[i].itemHealAmount);
                                clayton.sBar.SetStam(clayton.sBar.getCurrentStamValue + slots[i].itemStamCost);
                                clayton.MPBar.SetMP(clayton.MPBar.getCurrentMPValue + slots[i].itemManaCost);
                                inventory[i] = new Item();

                            }
                            
                        }// middle mouse auto equipts to hotbar
                        if (e.isMouse && e.type == EventType.MouseDown && e.button == 2 && hbar.hotbarfull == false)
                        {
                            hbar.AddItemHotbar(inventory[i].itemID);
                            inventory[i] = new Item();
                        }


                    }
                }
                else
                {//drop into slot
                    if (slotRect.Contains(Event.current.mousePosition))
                    {
                        if (e.type == EventType.MouseUp && draggingItem)
                        {
                            inventory[i] = draggedItem;
                            draggingItem = false;
                            draggedItem = null;
                        }
                    }

                }
                if (tooltip == "")
                {
                    showTooltip = false;
                }
                i++;
            }
        }   
    }
    string CreatTooltip(Item item)
    {
        tooltip ="<color=#ffffff>" + item.itemName + "</Color>\n\n" + "<color=#D3D3D3>" + item.itemDesc + "\n" + "Level Required: " + item.itemLevelReq + "\n" + 
                   "Damage: " + item.itemDamage +"\n"  + "Attack Speed: " + item.itemSpeed + "\n" + "Range: " + item.itemRange+ "\n" +
                   "Mana Cost: " + item.itemManaCost + "\n" + "Stamina Cost: " + item.itemStamCost + "\n" + item.itemType + "</Color>" ;
        return tooltip;
        
    }
    //use this to remove items from inv
   public void RemoveItem(int id)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemID == id)
            {                   //this is an empyt item constructor use it to make slots empty
                inventory[i] = new Item();
                break;

            }
        }
    }
    //use this in pick up script to add items to inventory AddItem(ItemID);
   public void AddItem(int id)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemName == null)
            {
                for(int j = 0; j < database.items.Count; j++)
                {
                    if(database.items[j].itemID == id)
                    {
                        inventory[i] = database.items[j];
                    }
                }
                break;

            }
        }
    }

    //use to check if any specific items are in inv, uses ID
    bool InventoryContains(int id)
    {
        bool result = false;
        for(int i = 0; i < inventory.Count; i++)
        {
            result = inventory[i].itemID == id;
            if (result)
            {
                break;
            }
        }
        return result;
            
    }

    
   
    void Update()
    {
        CheckInventoryFull();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showInventory = !showInventory;
        }
        if (showInventory)
        {
           
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (!showInventory)
        {

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
