using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : ObjectPool<DamageManager, DamageUI>
{
    public void ShowDamageText(Vector3 position, int damage)
    {
        DamageUI ui = GetPool();
        ui.SetDamage(position, damage);
    }
}
