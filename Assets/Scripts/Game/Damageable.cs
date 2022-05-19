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

        stat.hp = Mathf.Clamp(stat.hp - power, 0, stat.maxHP);          // 실제 hp감소.
        onDamageEvent?.Invoke(power);                                   // 피격 이벤트 발생.
        if(stat.hp <= 0)                                                // 사망시.
            onDeadEvent?.Invoke();                                      // 죽음 이벤트 발생.
    }
    
}
