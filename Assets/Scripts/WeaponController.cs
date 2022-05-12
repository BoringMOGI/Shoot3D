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

    [Header("Object")]
    [SerializeField] Transform eye;             // ī�޶� (��)
    [SerializeField] Transform muzzle;          // �ѱ�.
    [SerializeField] Transform casingPivot;     // ź�� ���ⱸ.
    [SerializeField] Bullet bulletPrefab;       // �Ѿ� ������.
    [SerializeField] Transform casingPrefab;    // ź�� ������.

    [Header("Weapon Info")]
    [SerializeField] float rateTime;            // ���� �ӵ�.
    [SerializeField] float bulletSpeed;         // �Ѿ� �ӵ�.
    [SerializeField] float power;               // ���ݷ�.
    [SerializeField] int maxBullet;             // �ִ� ���� ��.
    [SerializeField] int haveBullet;            // ���� ���� ź�� ��.

    [Header("Etc")]
    [SerializeField] Vector2 recoil;
    [SerializeField] LayerMask ignoreLayer;     // üũ ���� ���� ���̾�.

    int currentBullet;      // ���� ź�� ��. 
    float nextFireTime;     // ���� ���� �� �� �ִ� �ð�.
    bool isReload;          // ���� ���ΰ�?
    bool isFire;            // �ݹ��ϰ� �ִ°�?

    FIRE_TYPE fireType;        // �߻� ���
    //LayerMask bulletRayMask;   // �Ѿ��� ���ư��� ���� ����ũ.

    bool isEmpty => currentBullet <= 0;   // ������ �Ѿ��� ���°�?

    // Start is called before the first frame update
    void Start()
    {
        // ignore : �������ϴ� ���� Player ���̾��.
        // int.MaxValue(��� ��)�� ignore�� XOR���Ѽ� ��Ʈ�� �����ߴ�.
        // LayerMask ignore = 1 << LayerMask.NameToLayer("Player");
        // bulletRayMask = int.MaxValue ^ ignore;

        // ������ ź�� ����.
        currentBullet = maxBullet;
        UpdateUI();
    }

    private Vector3 GetBulletDirection()
    {
        // '�ü�'�� '�ѱ�'�� ���� ���̸� �������ֱ� ���� Ray�� �̿��� �Ѿ��� ������.
        Vector3 destination = eye.position + eye.forward * 1000f;
        RaycastHit hit;                     // ray�� �浹�� ������ ����.

        // �����κ��� �������� 1000m���� rayMask�� ������ �浹ü�� ����.

        LayerMask mask = int.MaxValue ^ ignoreLayer;
        Debug.Log(mask.ToString());
        if (Physics.Raycast(eye.position, eye.forward, out hit, 1000f, mask))
            destination = hit.point;

        // �ѱ����� ������ ����. (����ȭ)
        Vector3 direction = destination - muzzle.position;
        return direction.normalized;
    }        

    // �߻�
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

        nextFireTime = Time.time + rateTime;           // ���� �� �� �ִ� �ð�.
        AudioManager.Instance.PlaySE("shoot", 0.1f);   // ȿ���� ���.
        currentBullet -= 1;                            // �Ѿ� �ϳ� ����.

        // UI�� ����
        UpdateUI();

        // ���� �Ѿ� ������Ʈ ����.
        Vector3 direction = GetBulletDirection();
        Bullet bullet = Instantiate(bulletPrefab);
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = Quaternion.LookRotation(direction);
        bullet.Shoot(bulletSpeed, direction);

        // ź�� ���� �� ����.
        CreateCasing();

        // �ѱ� �ݵ�.
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
        if (isReload || haveBullet <= 0 || currentBullet >= maxBullet)
            return false;

        AudioManager.Instance.PlaySE("reload");
        isReload = true;
        return true;
    }
    private void OnEndReload()
    {
        isReload = false;

        int need = maxBullet - currentBullet;   // ������ �ʿ��� ź�� ��.
        if (haveBullet < need)                  // ���� �ʿ��� �纸�� �������� �� ���� ���.
        {
            currentBullet += haveBullet;        // ���� ������ ��ŭ ���� ź�࿡ ���ϰ�
            haveBullet = 0;                     // �������� 0�� �ȴ�.
        }
        else                                    // �ʿ䷮��ŭ ����� �����ϰ� �����ϱ�.
        {
            currentBullet = maxBullet;          // �ִ�� ����.
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
}
