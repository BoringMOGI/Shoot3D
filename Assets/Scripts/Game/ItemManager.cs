using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] ItemData[] itemDatas;

    public Item GetItem(string itemName, int count)
    {
        // ������ �����͸� �˻��Ѵ�.
        ItemData data = GetItemData(itemName);
        if (data == null)
        {
            Debug.Log(itemName + "�� �����ϴ�.");
            return null;
        }

        // �ش� �����͸� Item ��ü�� ���� �� ��ȯ.
        return new Item(data, count);
    }
    private ItemData GetItemData(string name)
    {
        foreach(ItemData data in itemDatas)
        {
            if (data.name == name)
                return data;
        }

        return null;
    }
}
