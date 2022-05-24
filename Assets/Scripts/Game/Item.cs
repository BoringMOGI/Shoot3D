using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemData data;
    public int count;

    public string itemName => data.itemName;
    public string description => data.description;
    public Sprite itemSprite => data.itemSprite;

    public Item(ItemData data, int count)
    {
        this.data = data;
        this.count = count;
    }

    public override string ToString()
    {
        return data.ToString();
    }
}
