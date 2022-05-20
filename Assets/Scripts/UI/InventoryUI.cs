using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : Singleton<InventoryUI>
{
    Transform[] allChilds;
    bool isOpen;

    private void Start()
    {
        // 모든 하위 자식들을 가져온다.
        allChilds = new Transform[transform.childCount];
        for (int i = 0; i < allChilds.Length; i++)
            allChilds[i] = transform.GetChild(i);

        SwitchInventory(false);
    }

    public void SwitchInventory()
    {
        SwitchInventory(!isOpen);
    }
    public void SwitchInventory(bool isOpen)
    {
        this.isOpen = isOpen;

        if (isOpen)
            AudioManager.Instance.PlaySE("paper");

        for (int i = 0; i < allChilds.Length; i++)
            allChilds[i].gameObject.SetActive(isOpen);
    }
}
