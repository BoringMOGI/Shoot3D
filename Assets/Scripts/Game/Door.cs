using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteraction
{
    [SerializeField] Animation anim;
    [SerializeField] bool isOpen;

    public string GetContext()
    {
        return isOpen ? "�� �ݱ�" : "�� ����";
    }

    public void OnInteracation()
    {
        if (isOpen)
            anim.Play("Door_Close");
        else
            anim.Play("Door_Open");

        isOpen = !isOpen;
    }
}
