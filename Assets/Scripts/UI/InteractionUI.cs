using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractionUI : Singleton<InteractionUI>
{
    [SerializeField] GameObject panel;
    [SerializeField] Text keyText;              // ����Ű �ؽ�Ʈ.
    [SerializeField] Text interactionText;      // ��ȣ�ۿ� ����.

    public void Setup(KeyCode key, IInteraction interaction)
    {
        // ��ȣ �ۿ� ��ü�� Ű�� �޾Ƽ� UI�� ���.
        keyText.text = key.ToString();
        interactionText.text = interaction.GetContext();
        panel.SetActive(true);
    }
    public void Close()
    {
        panel.SetActive(false);
    }
}
