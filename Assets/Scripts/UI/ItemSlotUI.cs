using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Image selectedImage;

    Item item;

    private void OnEnable()
    {
        selectedImage.enabled = false;              // 선택 이미지 비활성화.
    }

    public void Setup(Item item)
    {
        Debug.Log(item);


        this.item = item;
        if (item == null)
        {
            iconImage.enabled = false;
        }
        else
        {
            iconImage.enabled = true;
            iconImage.sprite = item.itemSprite;
        }
    }

    public void OnSelected()
    {
        // 아이템 슬롯이 선택 되었을 때.
        selectedImage.enabled = true;
    }
    public void OnDeSelected()
    {
        // 아이템 슬롯 선택이 풀렸을 때.
        selectedImage.enabled = false;
    }
}
