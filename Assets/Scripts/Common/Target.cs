using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] HpBar hpBar;
    [SerializeField] float maxHP;
    [SerializeField] float hp;


    private void Start()
    {
        UpdateHp();
    }
    private void UpdateHp()
    {
        hpBar.OnUpdateHp(hp, maxHP);
    }
}
