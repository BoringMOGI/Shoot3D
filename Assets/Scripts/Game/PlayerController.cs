using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] Animator anim;             // �ִϸ�����.
    [SerializeField] WeaponController weapon;   // ����.
    [SerializeField] GrenadeThrow grenadeThrow; // ����ź.

    [Header("Eye")]
    [SerializeField] Camera eye;                // ��.
    [SerializeField] Transform normalCamera;    // �Ϲ� �þ� ��ġ.
    [SerializeField] Transform aimCamera;       // ���� �þ� ��ġ.

    bool isAim;

    void Update()
    {
        if (weapon != null && !weapon.isReload)
        {
            Fire();
            Reload();
            Grenade();
        }

        ChangeFireType();
        Aim();
    }

    private void Fire()
    {
        if (Input.GetMouseButton(0))
        {
            if(weapon.StartFire(isAim))
                anim.SetTrigger("onFire");
        }
        else if (Input.GetMouseButtonUp(0))
            weapon.EndFire();
    }
    private void Aim()
    {
        if(Input.GetMouseButtonDown(1) && !weapon.isReload)
        {
            anim.SetTrigger("onAim");
        }

        isAim = Input.GetMouseButton(1) && !weapon.isReload;
        anim.SetBool("isAim", isAim);
        eye.transform.position = isAim ? aimCamera.position : normalCamera.position;
        eye.fieldOfView = isAim ? 45 : 60;
        CrossHairUI.Instance.SwitchCrosshair(!isAim);
    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && weapon.Reload())
            anim.SetTrigger("onReload");
    }
    private void Grenade()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            grenadeThrow.OnThrowGrenabe();
        }
    }
    private void ChangeFireType()
    {
        if (Input.GetKeyDown(KeyCode.B))
            weapon.OnChangeType();
    }

   
}