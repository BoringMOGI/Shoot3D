using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] Animator anim;             // 애니메이터.
    [SerializeField] WeaponController weapon;   // 무기.

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
