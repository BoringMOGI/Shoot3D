using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickupUI : Singleton<PickupUI>
{
    [SerializeField] Image iconImage;
    [SerializeField] Text itemText;
    [SerializeField] Animation anim;
    [SerializeField] GameObject panel;

    float appearTime;
    bool isAppear;

    private void Start()
    {
        panel.SetActive(false);
    }
    private void Update()
    {
        // 등장 중이고 등장 시간이 지났을 경우.
        if(isAppear && (appearTime -= Time.deltaTime) <= 0.0f)
        {
            isAppear = false;
            anim.Play("Pickup_Disappear");
        }
    }

    public void PickupItem(Item item)
    {
        isAppear = true;
        appearTime = 1.5f;

        iconImage.sprite = item.itemSprite;
        itemText.text = string.Format("{0} (x{1})", item.itemName, item.count);
        anim.Play("Pickup_Appear");
    }
}
