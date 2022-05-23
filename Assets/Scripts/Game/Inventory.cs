using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] items;
    const int MAX_INVENTORY = 20;

    [ContextMenu("HELP")]
    void TEST()
    {
        for (int i = 0; i < items.Length; i++)
            Debug.Log(items[i]);
    }
    void Start()
    {
        items = new Item[MAX_INVENTORY];        
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                break;
            }
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        InventoryUI.Instance.UpdateItems(items);
    }
}
