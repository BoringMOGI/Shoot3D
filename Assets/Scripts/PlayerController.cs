using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] Animator anim;             // �ִϸ�����.
    [SerializeField] WeaponController weapon;   // ����.

    void Update()
    {
        if (weapon != null)
        {
            Fire();
            Reload();
        }
    }

    private void Fire()
    {
        if(Input.GetMouseButton(0) && weapon.Fire())
            anim.SetTrigger("onFire");
    }
    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && weapon.Reload())
            anim.SetTrigger("onReload");
    }


   
}
