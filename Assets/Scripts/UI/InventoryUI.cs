using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventoryUI : Singleton<InventoryUI>
{
    [SerializeField] GameObject uiPanel;
    [SerializeField] Transform slotParent;
    [SerializeField] ItemSlotUI dragSlot;

    [Header("Event")]
    [SerializeField] UnityEvent OnOpenEvent;
    [SerializeField] UnityEvent OnCloseEvent;

    ItemSlotUI[] itemSlots;     // ��� ������ ���Ե�.

    public bool isOpen;

    protected new void Awake()
    {
        base.Awake();
        itemSlots = slotParent.GetComponentsInChildren<ItemSlotUI>();
    }
    private void Start()
    {
        SwitchInventory(false);
    }

    public bool SwitchInventory()
    {
        return SwitchInventory(!isOpen);
    }
    public bool SwitchInventory(bool isOpen)
    {
        this.isOpen = isOpen;
        uiPanel.SetActive(isOpen);
        
        DescriptionUI.Instance.Close();             // ������ ������ ��Ȱ��ȭ.
        dragSlot.gameObject.SetActive(false);       // ������ ������ ��Ȱ��ȭ.

        if (isOpen)
            OnOpen();
        else
            OnClose();

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
    public void OnEndSlotDrag(int start, int end, bool isInside)
    {
        dragSlot.gameObject.SetActive(false);

        if (isInside)
            Inventory.Instance.MoveItem(start, end);
        else
            Inventory.Instance.DropItem(start);
    }
}
