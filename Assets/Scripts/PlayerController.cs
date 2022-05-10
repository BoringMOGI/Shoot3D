using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class Weapon
{
    [SerializeField] int maxHaveBullet;       // 내가 최대로 소지할 수 있는 탄약 수.
    [SerializeField] int maxCurretBullet;     // 내가 최대로 장전할 수 있는 탄약 수.
    [SerializeField] float rateTime;          // 연사 속도.
    [SerializeField] public int power;        // 데미지.

    private int haveBullet;          // 내가 소지하고 있는 탄약 수.
    private int currentBullet;       // 내가 장전하고 있는 탄약 수.

    public bool IsEmptyCurrent => currentBullet <= 0;       // 현재 장전되어 있는 탄약이 없다.
    public bool IsAmmoOut => haveBullet <= 0;               // 가지고 있는 탄약이 없다.

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
        int need = maxCurretBullet - currentBullet;     // 장전에 필요한 탄약 수.
        if(haveBullet < need)                           // 내가 필요한 양보다 소지량이 더 적을 경우.
        {
            currentBullet += haveBullet;        // 남은 소지량 만큼 현재 탄약에 더하고
            haveBullet = 0;                     // 소지량은 0이 된다.
        }
        else                                    // 필요량만큼 충분히 소지하고 있으니까.
        {
            currentBullet = maxCurretBullet;    // 최대로 충전.
            haveBullet -= need;                 // 소지량에서 필요량만큼 감소.
        }
    }
}

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] Animator anim;             // 애니메이터.

    [SerializeField] Transform muzzle;          // 총구.
    [SerializeField] Bullet bulletPrefab;       // 총알 프리팹.
    [SerializeField] Weapon weapon;             // 무기 데이터.

    public int MaxBullet => (weapon!= null) ? weapon.HavaBullet : 0;
    public int CurrentBullet => (weapon != null) ? weapon.CurretBullet : 0;

    float nextFireTime;     // 다음 총을 쏠 수 있는 시간.
    bool isReload;          // 장전 중인가?

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
        if (isReload)       // 장전 중에는 공격을 못한다.
            return;

        // Time.time : 게임이 시작하고 몇 초가 흘렀는가?
        if (Input.GetMouseButton(0) && !weapon.IsEmptyCurrent && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + weapon.RateTime;             // 다음 쏠 수 있는 시간.
            AudioManager.Instance.PlaySE("shoot", 0.1f);            // 효과음 재생.
            anim.SetTrigger("onFire");                              // 애니메이션 트리거.
            weapon.Shoot();                                         // 무기 정보에서 총알 하나 제거.

            // 실제 총알 오브젝트 생성.
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
