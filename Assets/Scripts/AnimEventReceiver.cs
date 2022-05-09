using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventReceiver : MonoBehaviour
{
    [SerializeField] GameObject target;

    public void OnSendMessage(string funtion)
    {
        // SendMessage(�ش� �̸��� ���� �Լ��� ȣ���Ѵ�)
        target.SendMessage(funtion);
    }
}
