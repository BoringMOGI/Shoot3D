using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteraction
{
    [SerializeField] Animation anim;
    [SerializeField] bool isOpen;

    public string GetContext()
    {
        return isOpen ? "문 닫기" : "문 열기";
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
