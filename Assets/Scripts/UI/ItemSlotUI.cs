using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Image selectedImage;

    [SerializeField] Image countImage;
    [SerializeField] Text countText;

    Item item;
    int slotIndex;                  // �� ���� ��ȣ.
    RectTransform slotLayout;       // ������ �θ� ���̾ƿ�.

    static int currentSlotIndex;  // ���� �������� ��ȣ.

    private void OnEnable()
    {
        selectedImage.enabled = false;

        if (slotLayout == null)
        {
            slotLayout = transform.parent.GetComponent<RectTransform>();
            slotIndex = transform.GetSiblingIndex();    // �θ�κ��� ���° �ڽ��ΰ�?
        }
    }

    private void SwitchSlot(bool isOn)
    {
        iconImage.enabled = isOn;
        countImage.enabled = isOn;
        countText.enabled = isOn;
    }

    public void Setup(Item item)
    {
        this.item = item;
        if (item == null)
        {
            SwitchSlot(false);

        }
        else
        {
            SwitchSlot(true);
            iconImage.sprite = item.itemSprite;
            countText.text = item.count.ToString();
        }
    }

    public void OnSelected()
    {
        // ������ ������ ���� �Ǿ��� ��.
        selectedImage.enabled = true;
        currentSlotIndex = slotIndex;

        if(item != null)
            DescriptionUI.Instance.SetText(item.ToString());
    }
    public void OnDeSelected()
    {
        // ������ ���� ������ Ǯ���� ��.
        selectedImage.enabled = false;
        DescriptionUI.Instance.Close();
    }
    public void OnBeginDrag()
    {
        InventoryUI.Instance.OnBeginDrag(item);
    }
    public void OnDrag()
    {
        InventoryUI.Instance.OnSlotDrag();
    }
    public void OnEndDrag()
    {
        // ���콺 �������� slotLayout ���ο� �ִ���?
        bool isInside = RectTransformUtility.RectangleContainsScreenPoint(slotLayout, Input.mousePosition);

        // ���� � ���Կ��� �巡�׸� ������ � ���Կ��� ���´°�?
        InventoryUI.Instance.OnEndSlotDrag(slotIndex, currentSlotIndex, isInside);
    }
}
