using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventoryUI : Singleton<InventoryUI>
{
    [SerializeField] Transform slotParent;
    [SerializeField] ItemSlotUI dragSlot;
    [SerializeField] UnityEvent OnOpenEvent;
    [SerializeField] UnityEvent OnCloseEvent;

    Transform[] allChilds;      // ��� �ڽ� ������Ʈ. (Ȱ��/��Ȱ��ȭ�� ��� �ȴ�)
    ItemSlotUI[] itemSlots;     // ��� ������ ���Ե�.

    public bool isOpen;

    private void Start()
    {
        // ��� ���� �ڽĵ��� �����´�.
        allChilds = new Transform[transform.childCount];
        for (int i = 0; i < allChilds.Length; i++)
            allChilds[i] = transform.GetChild(i);

        // ��� ���� �ڽ��� ItemsSlotUI�� �����´�.
        /*
        itemSlots = new ItemSlotUI[slotParent.childCount];
        for(int i= 0; i< itemSlots.Length; i++)
            itemSlots[i] = transform.GetChild(i).GetComponent<ItemSlotUI>();
        */

        itemSlots = slotParent.GetComponentsInChildren<ItemSlotUI>();

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

    // ������ ���� �巡��.
    public void OnBeginDrag(Item item)
    {
        dragSlot.gameObject.SetActive(true);
        dragSlot.Setup(item);
    }
    public void OnSlotDrag()
    {
        dragSlot.transform.position = Input.mousePosition;
    }
    public void OnEndSlotDrag()
    {
        dragSlot.gameObject.SetActive(false);
    }
}
