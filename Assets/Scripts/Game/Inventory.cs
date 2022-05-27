using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    [SerializeField] Transform eyePivot;

    Item[] items;
    const int MAX_INVENTORY = 20;
 
    protected new void Awake()
    {
        base.Awake();                       // 상위 클래스의 Awake 실행.
        items = new Item[MAX_INVENTORY];      
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                PickupUI.Instance.PickupItem(item);
                break;
            }
        }

        UpdateUI();
    }
    public void MoveItem(int start, int end)
    {
        if (start == end)
            return;

        Item startItem = items[start];
        Item endItem = items[end];

        items[start] = endItem;
        items[end] = startItem;

        UpdateUI();
    }
    public void DropItem(int index)
    {
        Item drop = items[index];
        items[index] = null;

        if(drop != null)
        {
            // 아이템 매니저에게서 drop을 가진 오브젝트 가져옴.
            // 그 후 내 정면 방향으로 던진다.
            ItemObject dropObject = ItemManager.Instance.GetItemObject(drop);
            dropObject.transform.position = eyePivot.position + (eyePivot.forward * 0.5f);
            dropObject.Throw(eyePivot.forward, 2f);
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        InventoryUI.Instance.UpdateItems(items);
    }
}
