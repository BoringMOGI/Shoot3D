using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventoryUI : Singleton<InventoryUI>
{
    [SerializeField] ItemSlotUI[] itemSlots;     // ��� ������ ���Ե�.
    [SerializeField] UnityEvent OnOpenEvent;
    [SerializeField] UnityEvent OnCloseEvent;

    Transform[] allChilds;      // ��� �ڽ� ������Ʈ. (Ȱ��/��Ȱ��ȭ�� ��� �ȴ�)

    public bool isOpen;

    private void Start()
    {
        // ��� ���� �ڽĵ��� �����´�.
        allChilds = new Transform[transform.childCount];
        for (int i = 0; i < allChilds.Length; i++)
            allChilds[i] = transform.GetChild(i);

        SwitchInventory(false);
    }

    public bool SwitchInventory()
    {
        return SwitchInventory(!isOpen);
    }
    public bool SwitchInventory(bool isOpen)
    {
        this.isOpen = isOpen;

        if (isOpen)
        {
            OnOpen();
        }
        else
        {
            OnClose();
        }

        for (int i = 0; i < allChilds.Length; i++)
            allChilds[i].gameObject.SetActive(isOpen);

        return isOpen;
    }

    private void OnOpen()
    {
        AudioManager.Instance.PlaySE("paper");
        OnOpenEvent?.Invoke();
    }
    private void OnClose()
    {
        OnCloseEvent?.Invoke();
    }

    public void UpdateItems(Item[] items)
    {
        for(int i= 0; i<items.Length; i++)
        {
            itemSlots[i].Setup(items[i]);
        }
    }
}
