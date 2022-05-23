using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] Status stat;
    [SerializeField] UnityEvent<int> onDamageEvent;
    [SerializeField] UnityEvent onDeadEvent;

    public void OnDamaged(int power)
    {
        if (stat.hp <= 0)
            return;

        stat.hp = Mathf.Clamp(stat.hp - power, 0, stat.maxHP);          // ���� hp����.
        onDamageEvent?.Invoke(power);                                   // �ǰ� �̺�Ʈ �߻�.
        if(stat.hp <= 0)                                                // �����.
            onDeadEvent?.Invoke();                                      // ���� �̺�Ʈ �߻�.
    }
    
}
