using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    public GUISkin skin;
    int hbSlots = 8;
    private ItemDatabase database;
    public List<Item> hotbar = new List<Item>();
    public List<Item> hotbarSlots = new List<Item>();
    public bool hotbarfull;
    public Inventory inv;
    public bool showInventory;
    public bool showTooltip;
    public string tooltip;
    public bool draggingItem;
    public Item draggedItem;
    public int prevIndex;
    public ClaytonCont clayton;
    public int slotSelected;
    public int holdingID;

    void OnGUI()
    {
        
        tooltip = "";
        GUI.skin = skin;
        DrawHotbar();
        DrawSelector();
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

    //draw the squares for the hotbar
    void DrawHotbar()
    { Event e = Event.current;
        int x = 0;
        for(int i = 0; i < hbSlots; i++)
        {
            Rect hbSlotRect = new Rect(Screen.width / 4 + i * 100 , Screen.height - 108, Screen.width / 20, Screen.height / 20);
            GUI.Box(new Rect(hbSlotRect),"", skin.GetStyle("Slot" ));
          
            hotbarSlots[x] = hotbar[x];
            if (hotbarSlots[x].itemName != null)
            {
                GUI.DrawTexture(hbSlotRect, hotbarSlots[x].itemIcon);

                if (inv.showInventory)
                {
                    if (hbSlotRect.Contains(e.mousePosition))
                    {
                        tooltip = CreatTooltip(hotbarSlots[x]);
                        showTooltip = true;
                        if (e.button == 0 && e.type == EventType.MouseDrag && (!draggingItem || !inv.draggingItem))
                        {
                            draggingItem = true;
                            prevIndex = x;
                            draggedItem = hotbarSlots[x];
                            hotbar[x] = new Item();

                        }
                        if (e.type == EventType.MouseUp && (draggingItem || inv.draggingItem))
                        {
                            hotbar[prevIndex] = hotbar[x];
                            hotbar[x] = draggedItem;
                            draggingItem = false;
                            draggedItem = null;
                        }
                        if (e.isMouse && e.type == EventType.MouseDown && e.button == 1)
                        {
                            if (hotbarSlots[x].itemType == Item.ItemType.Consumable)
                            {

                                clayton.HPBar.SetHP(clayton.HPBar.getCurrentHPValue + hotbarSlots[x].itemHealAmount);
                                clayton.sBar.SetStam(clayton.sBar.getCurrentStamValue + hotbarSlots[x].itemStamCost);
                                clayton.MPBar.SetMP(clayton.MPBar.getCurrentMPValue + hotbarSlots[x].itemManaCost);
                                hotbar[i] = new Item();

                            }
                        }
                        if (e.isMouse && e.type == EventType.MouseDown && e.button == 2 && !inv.invFull)
                        {
                            inv.AddItem(hotbar[x].itemID);
                            hotbar[i] = new Item();
                        }
                    }
                }
            }
            else
            {//drop into slot
                if (hbSlotRect.Contains(Event.current.mousePosition))
                {
                    if (e.type == EventType.MouseUp && draggingItem)
                    {
                        hotbar[x] = draggedItem;
                        draggingItem = false;
                        draggedItem = null;
                    }
                }

            }
            if (tooltip == "")
            {
                showTooltip = false;
            }
            x++;
            
        }
        


    }
   



    //selecting items on the hotbar
    
    void HotbarSelection()
    {
        if(slotSelected < 0)
        {
            slotSelected = 7;
        }
        if(slotSelected > 7)
        {
            slotSelected = 0;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 )
        {
            slotSelected++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            slotSelected--;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            slotSelected = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            slotSelected = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            slotSelected = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            slotSelected = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            slotSelected = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            slotSelected = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            slotSelected = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            slotSelected = 7;
        }
    }
    void HotbarClicked() //use item when not in inv with left click
    {
        if (Input.GetMouseButtonDown(0) && !inv.showInventory)
        { if (hotbar[slotSelected].itemName != null)
            {
                if (hotbar[slotSelected].itemType == Item.ItemType.Consumable)
                {
                    Debug.Log("done");
                    clayton.HPBar.SetHP(clayton.HPBar.getCurrentHPValue + hotbarSlots[slotSelected].itemHealAmount);
                    clayton.sBar.SetStam(clayton.sBar.getCurrentStamValue + hotbarSlots[slotSelected].itemStamCost);
                    clayton.MPBar.SetMP(clayton.MPBar.getCurrentMPValue + hotbarSlots[slotSelected].itemManaCost);
                    hotbar[slotSelected] = new Item();
                }
            }
            else return;
        }
    }
    


    string CreatTooltip(Item item)
    {
        tooltip = "<color=#ffffff>" + item.itemName + "</Color>\n\n" + "<color=#D3D3D3>" + item.itemDesc + "\n" + "Level Required: " + item.itemLevelReq + "\n" +
                   "Damage: " + item.itemDamage + "\n" + "Attack Speed: " + item.itemSpeed + "\n" + "Range: " + item.itemRange + "\n" +
                   "Mana Cost: " + item.itemManaCost + "\n" + "Stamina Cost: " + item.itemStamCost + "\n" + item.itemType + "</Color>";
        return tooltip;

    }
    //check if hotbar is full, if so change a public bool to let the pick up script know
    void CheckHotbarFull()
    {
        for(int i = 0; i < hotbar.Count; i++)
        {
            if (hotbar[i].itemName == null)
            {
                hotbarfull = false;
                
                break;
            }
            else
                hotbarfull = true;
            
        }
    }
    public void AddItemHotbar(int id)
    {
        for(int i = 0; i < hotbar.Count; i++)
        {
            if (hotbar[i].itemName == null)
            {
                for(int j = 0; j < database.items.Count; j++)
                {
                    if(database.items[j].itemID == id)
                    {
                        hotbar[i] = database.items[j];
                    }
                }
                break;
            }
        }
    }

    
    
       
    




//be able to drag and drop items

//highlight selected item
void DrawSelector()
    {
        
        
            Rect selectorRect = new Rect(Screen.width / 4 + slotSelected * 100, Screen.height - 108, 87, 87);
            GUI.Box(new Rect(selectorRect), "", skin.GetStyle("Selector"));
    }
    





// Start is called before the first frame update
void Start()
    {
        // change this to load data when save load is completed
        for(int i = 0; i < hbSlots; i++)
        {
            hotbarSlots.Add(new Item());
            hotbar.Add(new Item());
        }
        database = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<ItemDatabase>();
        slotSelected = 1;
    }

    // Update is called once per frame
    void Update()
    {
        holdingID = hotbar[slotSelected].itemID;
        HotbarClicked();
        HotbarSelection();
      
            CheckHotbarFull();
        
        
    }
}
