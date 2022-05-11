using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Transform eye;             // ī�޶� (��)
    [SerializeField] Transform muzzle;          // �ѱ�.
    [SerializeField] Bullet bulletPrefab;       // �Ѿ� ������.

    [SerializeField] float rateTime;            // ���� �ӵ�.
    [SerializeField] float bulletSpeed;         // �Ѿ� �ӵ�.
    [SerializeField] float power;               // ���ݷ�.
    [SerializeField] int maxBullet;             // �ִ� ���� ��.
    [SerializeField] int haveBullet;            // ���� ���� ź�� ��.

    int currentBullet;        // ���� ź�� ��. 
    float nextFireTime;     // ���� ���� �� �� �ִ� �ð�.
    bool isReload;          // ���� ���ΰ�?

    LayerMask bulletRayMask;   // �Ѿ��� ���ư��� ���� ����ũ.

    bool isEmpty => currentBullet <= 0;   // ������ �Ѿ��� ���°�?

    // Start is called before the first frame update
    void Start()
    {
        // ignore : �������ϴ� ���� Player ���̾��.
        // int.MaxValue(��� ��)�� ignore�� XOR���Ѽ� ��Ʈ�� �����ߴ�.
        LayerMask ignore = 1 << LayerMask.NameToLayer("Player");
        bulletRayMask = int.MaxValue ^ ignore;

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
        if (Physics.Raycast(eye.position, eye.forward, out hit, 1000f, bulletRayMask))
            destination = hit.point;

        // �ѱ����� ������ ����. (����ȭ)
        Vector3 direction = destination - muzzle.position;
        return direction.normalized;
    }

    public bool Fire()
    {
        if (isReload || isEmpty || Time.time < nextFireTime)
            return false;

        nextFireTime = Time.time + rateTime;           // ���� �� �� �ִ� �ð�.
        AudioManager.Instance.PlaySE("shoot", 0.1f);   // ȿ���� ���.
        currentBullet -= 1;                            // �Ѿ� �ϳ� ����.

        // UI�� ����
        UpdateUI();

        // ���� �Ѿ� ������Ʈ ����.
        Bullet bullet = Instantiate(bulletPrefab);
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = Quaternion.LookRotation(GetBulletDirection());
        bullet.Shoot(bulletSpeed);

        return true;
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

    private void UpdateUI()
    {
        // UI�� ����
        WeaponInfoUI.Instance.UpdateBulletText(currentBullet, haveBullet);
    }
}
