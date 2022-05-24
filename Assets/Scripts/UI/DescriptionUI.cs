using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionUI : Singleton<DescriptionUI>
{
    [SerializeField] Text descriptionText;
    [SerializeField] Image boxImage;

    bool isShow;

    private void Start()
    {
        Close();
    }
    private void Update()
    {
        if (!isShow)
            return;

        transform.position = Input.mousePosition;
    }
    public void SetText(string text)
    {
        descriptionText.text = text;
        Switch(true);
    }
    public void Close()
    {
        Switch(false);
    }

    private void Switch(bool isOn)
    {
        isShow = isOn;
        boxImage.enabled = isOn;
        descriptionText.enabled = isOn;
    }
}
