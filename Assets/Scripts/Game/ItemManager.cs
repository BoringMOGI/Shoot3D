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
        // 아이템 데이터를 검색한다.
        ItemData data = GetItemData(type);
        if (data == null)
        {
            Debug.Log(type + "은 없습니다.");
            return null;
        }

        // 해당 데이터를 Item 객체로 생성 후 반환.
        return new Item(data, count);
    }

    // 두가지의 방법.
    public ItemObject GetItemObject(ItemData.ITEM_TYPE itemType, int count)
    {
        return GetItemObject(GetItem(itemType, count));
    }
    public ItemObject GetItemObject(Item item)
    {
        // 오브젝트 프리팹을 생성하고 내부에 실제 아이템 데이터를 세팅한다.
        ItemObject newObject = Instantiate(prefab);
        newObject.Setup(item);
        return newObject;
    }


}
