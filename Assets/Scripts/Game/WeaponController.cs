using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public enum FIRE_TYPE
    {
        Single,     // �ܹ�.
        //Burst,      // ����.
        Auto,       // ����.

        Count,
    }

    [Header("Trnasform")]
    [SerializeField] Transform eye;             // ī�޶� (��)
    [SerializeField] Transform muzzle;          // �ѱ�.
    [SerializeField] Transform casingPivot;     // ź�� ���ⱸ.
    [SerializeField] Transform grenadePivot;    // ����ź ���� ��ġ.

    [Header("Prefab")]
    [SerializeField] Bullet bulletPrefab;       // �Ѿ� ������.
    [SerializeField] Transform casingPrefab;    // ź�� ������.

    [Header("Info")]
    [SerializeField] WeaponInfo weaponInfo;

    int currentBullet;      // ���� ź�� ��. 
    int haveBullet;         // ���� ���� ź�� ��.
    float nextFireTime;     // ���� ���� �� �� �ִ� �ð�.

    [SerializeField]
    float collectionRate;   // ��ź��

    public bool isReload;   // ���� ���ΰ�?
    bool isFire;            // �ݹ��ϰ� �ִ°�?

    FIRE_TYPE fireType;     // �߻� ���

    bool isEmpty => currentBullet <= 0;   // ������ �Ѿ��� ���°�?

    // Start is called before the first frame update
    void Start()
    {
        // ignore : �������ϴ� ���� Player ���̾��.
        // int.MaxValue(��� ��)�� ignore�� XOR���Ѽ� ��Ʈ�� �����ߴ�.
        // LayerMask ignore = 1 << LayerMask.NameToLayer("Player");
        // bulletRayMask = int.MaxValue ^ ignore;

        // ������ ź�� ����.
        currentBullet = weaponInfo.maxBullet;
        haveBullet = weaponInfo.maxHaveBullet;
        UpdateUI();
    }
    private void Update()
    {
        float decrese = collectionRate - weaponInfo.delCollection * Time.deltaTime;
        float min = weaponInfo.minCollection;
        float max = weaponInfo.maxCollection;

        collectionRate = Mathf.Clamp(decrese, min, max);            // ��ź�� ��ȭ.
        CrossHairUI.Instance.UpdateCrosshair(collectionRate);       // UI�� �׸���.
    }

    private Vector3 GetBulletDirection()
    {
        // '�ü�'�� '�ѱ�'�� ���� ���̸� �������ֱ� ���� Ray�� �̿��� �Ѿ��� ������.
        Vector3 collectionSphere = Random.onUnitSphere * collectionRate * 0.1f;   // ��ź�� �������� ������ �������� ������ ��ġ.
        Vector3 eyePosition = eye.position + collectionSphere;                    // ���� �� ��ġ + ������ ��ġ.
        Vector3 destination = eyePosition + eye.forward * 1000f;                  // ��ź���� ���� ������.
        RaycastHit hit;           
        
        // �����κ��� �������� 1000m���� rayMask�� ������ �浹ü�� ����.
        LayerMask mask = int.MaxValue ^ weaponInfo.ignoreLayer;
        if (Physics.Raycast(eyePosition, eye.forward, out hit, 1000f, mask))
            destination = hit.point;

        // �ѱ����� ������ ����. (����ȭ)
        Vector3 direction = destination - muzzle.position;
        return direction.normalized;
    }        

    // �߻�
    public bool StartFire(bool isAim)
    {
        if (isReload || isEmpty || Time.time < nextFireTime)
            return false;

        switch(fireType)
        {
            case FIRE_TYPE.Single:
                if (!isFire)
                {
                    isFire = true;
                    Fire(isAim);
                }
                else
                    return false;
                break;
            //case FIRE_TYPE.Burst:
            //    break;
            case FIRE_TYPE.Auto:
                Fire(isAim);
                break;
        }

        return true;
    }    
    public void EndFire()
    {
        isFire = false;
    }
    private void Fire(bool isAim)
    {
        isFire = true;

        collectionRate = collectionRate * 1.1f + weaponInfo.addCollection;  // ��ź�� �϶�.

        nextFireTime = Time.time + weaponInfo.rateTime;  // ���� �� �� �ִ� �ð�.
        AudioManager.Instance.PlaySE("shoot", 0.1f);     // ȿ���� ���.
        currentBullet -= 1;                              // �Ѿ� �ϳ� ����.

        // UI�� ����
        UpdateUI();

        // ���� �Ѿ� ������Ʈ ����.
        Vector3 direction = GetBulletDirection();
        Bullet bullet = Instantiate(bulletPrefab);
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = Quaternion.LookRotation(direction);
        bullet.Shoot(weaponInfo.bulletSpeed, direction);

        // ź�� ���� �� ����.
        CreateCasing();

        // �ѱ� �ݵ�.
        Recoil(isAim);
    }
    private void CreateCasing()
    {
        Transform casing = Instantiate(casingPrefab, casingPivot.position, casingPivot.rotation);
        casing.Rotate(casing.forward * Random.Range(-10f, 10f));
        casing.GetComponent<Rigidbody>().AddForce(casing.right * Random.Range(1f, 2.5f), ForceMode.Impulse);
    }
    private void Recoil(bool isAim)
    {
        float ratio = isAim ? 0.5f : 1.0f;
        float recoilX = Random.Range(-weaponInfo.recoil.x, weaponInfo.recoil.x) * ratio;
        float recoilY = Random.Range(0, weaponInfo.recoil.y) * ratio;
        CameraRotate.Instance.AddRecoil(new Vector2(recoilX, recoilY));
    }

    // ������.
    public void OnChangeType()
    {
        fireType += 1;
        if (fireType == FIRE_TYPE.Count)
            fireType = 0;

        UpdateUI();
    }
    public bool Reload()
    {
        // ������ ���̰ų�, ���� ź���� ���ų�, �̹� Ǯ�����̶�� ���� �ʴ´�.
        if (isReload || haveBullet <= 0 || currentBullet >= weaponInfo.maxBullet)
            return false;

        AudioManager.Instance.PlaySE("reload");
        isReload = true;
        return true;
    }
    private void OnEndReload()
    {
        isReload = false;

        int need = weaponInfo.maxBullet - currentBullet;   // ������ �ʿ��� ź�� ��.
        if (haveBullet < need)                  // ���� �ʿ��� �纸�� �������� �� ���� ���.
        {
            currentBullet += haveBullet;        // ���� ������ ��ŭ ���� ź�࿡ ���ϰ�
            haveBullet = 0;                     // �������� 0�� �ȴ�.
        }
        else                                    // �ʿ䷮��ŭ ����� �����ϰ� �����ϱ�.
        {
            currentBullet = weaponInfo.maxBullet;          // �ִ�� ����.
            haveBullet -= need;                 // ���������� �ʿ䷮��ŭ ����.
        }

        // UI�� ����
        UpdateUI();
    }

    // ������ ������Ʈ.
    static string[] typeKorea = new string[] { "�ܹ�", "����" };
    private void UpdateUI()
    {
        // UI�� ����
        WeaponInfoUI.Instance.UpdateBulletText(currentBullet, haveBullet);
        WeaponInfoUI.Instance.UpdateFireType(typeKorea[(int)fireType]);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (transform.forward * 2f), collectionRate * 0.1f);
    }
}
