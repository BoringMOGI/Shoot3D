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
        selectedImage.enabled = false;              // ���� �̹��� ��Ȱ��ȭ.
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
        // ������ ������ ���� �Ǿ��� ��.
        selectedImage.enabled = true;
    }
    public void OnDeSelected()
    {
        // ������ ���� ������ Ǯ���� ��.
        selectedImage.enabled = false;
    }
}
