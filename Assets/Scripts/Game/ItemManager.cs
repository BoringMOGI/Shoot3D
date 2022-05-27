using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] ItemObject prefab;
    [SerializeField] ItemData[] itemDatas;

    private ItemData GetItemData(ItemData.ITEM_TYPE type)
    {
        foreach (ItemData data in itemDatas)
        {
            if (data.type == type)
                return data;
        }

        return null;
    }
    public Item GetItem(ItemData.ITEM_TYPE type, int count)
    {
        // ������ �����͸� �˻��Ѵ�.
        ItemData data = GetItemData(type);
        if (data == null)
        {
            Debug.Log(type + "�� �����ϴ�.");
            return null;
        }

        // �ش� �����͸� Item ��ü�� ���� �� ��ȯ.
        return new Item(data, count);
    }

    // �ΰ����� ���.
    public ItemObject GetItemObject(ItemData.ITEM_TYPE itemType, int count)
    {
        return GetItemObject(GetItem(itemType, count));
    }
    public ItemObject GetItemObject(Item item)
    {
        // ������Ʈ �������� �����ϰ� ���ο� ���� ������ �����͸� �����Ѵ�.
        ItemObject newObject = Instantiate(prefab);
        newObject.Setup(item);
        return newObject;
    }


}
