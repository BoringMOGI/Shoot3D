using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class Weapon
{
    [SerializeField] int maxHaveBullet;       // ���� �ִ�� ������ �� �ִ� ź�� ��.
    [SerializeField] int maxCurretBullet;     // ���� �ִ�� ������ �� �ִ� ź�� ��.
    [SerializeField] float rateTime;          // ���� �ӵ�.
    [SerializeField] public int power;        // ������.

    private int haveBullet;          // ���� �����ϰ� �ִ� ź�� ��.
    private int currentBullet;       // ���� �����ϰ� �ִ� ź�� ��.

    public bool IsEmptyCurrent => currentBullet <= 0;       // ���� �����Ǿ� �ִ� ź���� ����.
    public bool IsAmmoOut => haveBullet <= 0;               // ������ �ִ� ź���� ����.

    public int HavaBullet => haveBullet;
    public int CurretBullet => currentBullet;
    public float RateTime => rateTime;
    public int Power => power;

    public void Init()
    {
        haveBullet = maxHaveBullet;
        currentBullet = maxCurretBullet;
    }
    public void Shoot()
    {
        currentBullet -= 1;
    }
    public void Reload()
    {
        int need = maxCurretBullet - currentBullet;     // ������ �ʿ��� ź�� ��.
        if(haveBullet < need)                           // ���� �ʿ��� �纸�� �������� �� ���� ���.
        {
            currentBullet += haveBullet;        // ���� ������ ��ŭ ���� ź�࿡ ���ϰ�
            haveBullet = 0;                     // �������� 0�� �ȴ�.
        }
        else                                    // �ʿ䷮��ŭ ����� �����ϰ� �����ϱ�.
        {
            currentBullet = maxCurretBullet;    // �ִ�� ����.
            haveBullet -= need;                 // ���������� �ʿ䷮��ŭ ����.
        }
    }
}

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] Animator anim;             // �ִϸ�����.

    [SerializeField] Transform muzzle;          // �ѱ�.
    [SerializeField] Bullet bulletPrefab;       // �Ѿ� ������.
    [SerializeField] Weapon weapon;             // ���� ������.

    public int MaxBullet => (weapon!= null) ? weapon.HavaBullet : 0;
    public int CurrentBullet => (weapon != null) ? weapon.CurretBullet : 0;

    float nextFireTime;     // ���� ���� �� �� �ִ� �ð�.
    bool isReload;          // ���� ���ΰ�?

    private void Start()
    {
        weapon.Init();
    }


    void Update()
    {
        Fire();
        Reload();
    }

    private void Fire()
    {
        if (isReload)       // ���� �߿��� ������ ���Ѵ�.
            return;

        // Time.time : ������ �����ϰ� �� �ʰ� �귶�°�?
        if (Input.GetMouseButton(0) && !weapon.IsEmptyCurrent && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + weapon.RateTime;             // ���� �� �� �ִ� �ð�.
            AudioManager.Instance.PlaySE("shoot", 0.1f);            // ȿ���� ���.
            anim.SetTrigger("onFire");                              // �ִϸ��̼� Ʈ����.
            weapon.Shoot();                                         // ���� �������� �Ѿ� �ϳ� ����.

            // ���� �Ѿ� ������Ʈ ����.
            Bullet bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
            bullet.Shoot();
        }
    }
    private void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R) && !isReload && !weapon.IsAmmoOut)
        {
            isReload = true;
            anim.SetTrigger("onReload");
            AudioManager.Instance.PlaySE("reload");
        }
    }


    private void OnEndReload()
    {
        isReload = false;
        weapon.Reload();
    }
}
