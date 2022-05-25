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
    int slotIndex;      // 내 슬롯 번호.

    static int currentSlotIndex;  // 현재 선택중인 번호.

    private void OnEnable()
    {
        selectedImage.enabled = false;
        slotIndex = transform.GetSiblingIndex();    // 부모로부터 몇번째 자식인가?
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
        // 아이템 슬롯이 선택 되었을 때.
        selectedImage.enabled = true;
        currentSlotIndex = slotIndex;

        if(item != null)
            DescriptionUI.Instance.SetText(item.ToString());
    }
    public void OnDeSelected()
    {
        // 아이템 슬롯 선택이 풀렸을 때.
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
        // 내가 어떤 슬롯에서 드래그를 시작해 어떤 슬롯에서 끝냈는가?
        InventoryUI.Instance.OnEndSlotDrag(slotIndex, currentSlotIndex);
    }
}
