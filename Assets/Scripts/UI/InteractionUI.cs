using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractionUI : Singleton<InteractionUI>
{
    [SerializeField] GameObject panel;
    [SerializeField] Text keyText;              // 단축키 텍스트.
    [SerializeField] Text interactionText;      // 상호작용 내용.

    public void Setup(KeyCode key, IInteraction interaction)
    {
        // 상호 작용 물체와 키를 받아서 UI로 출력.
        keyText.text = key.ToString();
        interactionText.text = interaction.GetContext();
        panel.SetActive(true);
    }
    public void Close()
    {
        panel.SetActive(false);
    }
}
