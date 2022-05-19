using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] HpBar hpBar;
    [SerializeField] Status stat;

    private void Start()
    {
        UpdateHp();
    }
    public void UpdateHp()
    {
        hpBar.OnUpdateHp(stat.hp, stat.maxHP);
    }
}
