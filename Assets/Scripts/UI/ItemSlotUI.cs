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

    private void OnEnable()
    {
        selectedImage.enabled = false;
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

        if(item != null)
            DescriptionUI.Instance.SetText(item.ToString());
    }
    public void OnDeSelected()
    {
        // 아이템 슬롯 선택이 풀렸을 때.
        selectedImage.enabled = false;
        DescriptionUI.Instance.Close();
    }
}
