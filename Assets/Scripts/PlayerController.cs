using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class Weapon
{
    public int maxBullet;           // �ִ� ź�� ��.
    public int currentBullet;       // ���� ź�� ��.
    public float rateTime;          // ���� �ӵ�.
    public int power;               // ������.

    public bool isEmpty => currentBullet <= 0;

    public Weapon(int maxBullet)
    {
        this.maxBullet = maxBullet;
        currentBullet = maxBullet;
    }
}

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] Animator anim;
    [SerializeField] Weapon weapon;

    public int MaxBullet => (weapon!= null) ? weapon.maxBullet : 0;
    public int CurrentBullet => (weapon != null) ? weapon.currentBullet : 0;

    float nextFireTime;     // ���� ���� �� �� �ִ� �ð�.

    void Update()
    {
        // Time.time : ������ �����ϰ� �� �ʰ� �귶�°�?
        if(Input.GetMouseButton(0) && !weapon.isEmpty && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + weapon.rateTime;
            weapon.currentBullet -= 1;
            anim.SetTrigger("onFire");
            AudioManager.Instance.PlaySE("shoot");
        }
    }
}
