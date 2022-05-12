using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public enum FIRE_TYPE
    {
        Single,     // 단발.
        //Burst,      // 점사.
        Auto,       // 연발.

        Count,
    }

    [Header("Object")]
    [SerializeField] Transform eye;             // 카메라 (눈)
    [SerializeField] Transform muzzle;          // 총구.
    [SerializeField] Transform casingPivot;     // 탄피 배출구.
    [SerializeField] Bullet bulletPrefab;       // 총알 프리팹.
    [SerializeField] Transform casingPrefab;    // 탄피 프리팹.

    [Header("Weapon Info")]
    [SerializeField] float rateTime;            // 연사 속도.
    [SerializeField] float bulletSpeed;         // 총알 속도.
    [SerializeField] float power;               // 공격력.
    [SerializeField] int maxBullet;             // 최대 장전 수.
    [SerializeField] int haveBullet;            // 나의 소지 탄약 수.

    [Header("Etc")]
    [SerializeField] Vector2 recoil;
    [SerializeField] LayerMask ignoreLayer;     // 체크 하지 않을 레이어.

    int currentBullet;      // 현재 탄약 수. 
    float nextFireTime;     // 다음 총을 쏠 수 있는 시간.
    bool isReload;          // 장전 중인가?
    bool isFire;            // 격발하고 있는가?

    FIRE_TYPE fireType;        // 발사 방식
    //LayerMask bulletRayMask;   // 총알이 날아가는 광선 마스크.

    bool isEmpty => currentBullet <= 0;   // 장전된 총알이 없는가?

    // Start is called before the first frame update
    void Start()
    {
        // ignore : 빠져야하는 값은 Player 레이어다.
        // int.MaxValue(모든 값)에 ignore를 XOR시켜서 비트를 제거했다.
        // LayerMask ignore = 1 << LayerMask.NameToLayer("Player");
        // bulletRayMask = int.MaxValue ^ ignore;

        // 최초의 탄약 대입.
        currentBullet = maxBullet;
        UpdateUI();
    }

    private Vector3 GetBulletDirection()
    {
        // '시선'과 '총구'의 각도 차이를 보상해주기 위해 Ray를 이용한 총알의 목적지.
        Vector3 destination = eye.position + eye.forward * 1000f;
        RaycastHit hit;                     // ray와 충돌한 지점의 정보.

        // 눈으로부터 정면으로 1000m까지 rayMask를 포함한 충돌체만 검출.

        LayerMask mask = int.MaxValue ^ ignoreLayer;
        Debug.Log(mask.ToString());
        if (Physics.Raycast(eye.position, eye.forward, out hit, 1000f, mask))
            destination = hit.point;

        // 총구에서 목적지 방향. (정규화)
        Vector3 direction = destination - muzzle.position;
        return direction.normalized;
    }        

    // 발사
    public bool StartFire()
    {
        if (isReload || isEmpty || Time.time < nextFireTime)
            return false;

        switch(fireType)
        {
            case FIRE_TYPE.Single:
                if (!isFire)
                {
                    isFire = true;
                    Fire();
                }
                else
                    return false;
                break;
            //case FIRE_TYPE.Burst:
            //    break;
            case FIRE_TYPE.Auto:
                Fire();
                break;
        }

        return true;
    }    
    public void EndFire()
    {
        isFire = false;
    }
    private void Fire()
    {
        isFire = true;

        nextFireTime = Time.time + rateTime;           // 다음 쏠 수 있는 시간.
        AudioManager.Instance.PlaySE("shoot", 0.1f);   // 효과음 재생.
        currentBullet -= 1;                            // 총알 하나 제거.

        // UI에 전달
        UpdateUI();

        // 실제 총알 오브젝트 생성.
        Vector3 direction = GetBulletDirection();
        Bullet bullet = Instantiate(bulletPrefab);
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = Quaternion.LookRotation(direction);
        bullet.Shoot(bulletSpeed, direction);

        // 탄피 생성 후 배출.
        CreateCasing();

        // 총기 반동.
        Recoil();
    }
    private void CreateCasing()
    {
        Transform casing = Instantiate(casingPrefab, casingPivot.position, casingPivot.rotation);
        casing.Rotate(casing.forward * Random.Range(-10f, 10f));
        casing.GetComponent<Rigidbody>().AddForce(casing.right * Random.Range(1f, 2.5f), ForceMode.Impulse);
    }
    private void Recoil()
    {
        float recoilX = Random.Range(-recoil.x, recoil.x);
        float recoilY = Random.Range(0, recoil.y);
        CameraRotate.Instance.AddRecoil(new Vector2(recoilX, recoilY));
    }

    // 재장전.
    public void OnChangeType()
    {
        fireType += 1;
        if (fireType == FIRE_TYPE.Count)
            fireType = 0;

        UpdateUI();
    }
    public bool Reload()
    {
        // 재장전 중이거나, 소지 탄약이 없거나, 이미 풀충전이라면 하지 않는다.
        if (isReload || haveBullet <= 0 || currentBullet >= maxBullet)
            return false;

        AudioManager.Instance.PlaySE("reload");
        isReload = true;
        return true;
    }
    private void OnEndReload()
    {
        isReload = false;

        int need = maxBullet - currentBullet;   // 장전에 필요한 탄약 수.
        if (haveBullet < need)                  // 내가 필요한 양보다 소지량이 더 적을 경우.
        {
            currentBullet += haveBullet;        // 남은 소지량 만큼 현재 탄약에 더하고
            haveBullet = 0;                     // 소지량은 0이 된다.
        }
        else                                    // 필요량만큼 충분히 소지하고 있으니까.
        {
            currentBullet = maxBullet;          // 최대로 충전.
            haveBullet -= need;                 // 소지량에서 필요량만큼 감소.
        }

        // UI에 전달
        UpdateUI();
    }

    // 유아이 업데이트.
    static string[] typeKorea = new string[] { "단발", "연사" };
    private void UpdateUI()
    {
        // UI에 전달
        WeaponInfoUI.Instance.UpdateBulletText(currentBullet, haveBullet);
        WeaponInfoUI.Instance.UpdateFireType(typeKorea[(int)fireType]);
    }
}
