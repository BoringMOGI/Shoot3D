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
    
    float totalPersent = 0f;    // �� ���� Ȯ��.

    private void Start()
    {
        for (int i = 0; i < dropTables.Length; i++)
            totalPersent += dropTables[i].persent;
    }

    public void DropRandomItem()
    {
        // �� Ȯ���� ������ ���� ���ϴ� ��ġ�� ����.
        float pick = totalPersent * Random.value;
        float category = 0;
        Item dropItem = null;

        // ��� ���̺��� ���鼭 ��ġ�� �ش��ϴ� ������ ����.
        for(int i = 0; i<dropTables.Length; i++)
        {
            category += dropTables[i].persent;
            if(pick < category)
            {
                dropItem = dropTables[i].GetItem();
                break;
            }
        }

        // ���� ������ ������Ʈ�� ����.
        ItemObject io = ItemManager.Instance.GetItemObject(dropItem);
        Transform itemBox = io.transform;

        itemBox.transform.position = transform.position;
    }

}
