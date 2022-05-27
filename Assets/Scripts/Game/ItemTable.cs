using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTable : MonoBehaviour
{
    [System.Serializable]
    public struct DropTable
    {
        public ItemData.ITEM_TYPE itemType;
        public int itemCount;
        public float persent;

        public Item GetItem()
        {
            return ItemManager.Instance.GetItem(itemType, itemCount);
        }
    }

    [SerializeField] DropTable[] dropTables;
    
    float totalPersent = 0f;    // 총 등장 확률.

    private void Start()
    {
        for (int i = 0; i < dropTables.Length; i++)
            totalPersent += dropTables[i].persent;
    }

    public void DropRandomItem()
    {
        // 총 확률에 비율을 곱해 원하는 위치를 지정.
        float pick = totalPersent * Random.value;
        float category = 0;
        Item dropItem = null;

        // 모든 테이블을 돌면서 위치에 해당하는 아이템 추출.
        for(int i = 0; i<dropTables.Length; i++)
        {
            category += dropTables[i].persent;
            if(pick < category)
            {
                dropItem = dropTables[i].GetItem();
                break;
            }
        }

        // 실제 아이템 오브젝트로 생성.
        ItemObject io = ItemManager.Instance.GetItemObject(dropItem);
        Transform itemBox = io.transform;

        itemBox.transform.position = transform.position;
    }

}
