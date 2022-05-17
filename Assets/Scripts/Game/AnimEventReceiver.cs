using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventReceiver : MonoBehaviour
{
    [SerializeField] GameObject target;

    public void OnSendMessage(string funtion)
    {
        // SendMessage(해당 이름을 가진 함수를 호출한다)
        target.SendMessage(funtion);
    }
}
