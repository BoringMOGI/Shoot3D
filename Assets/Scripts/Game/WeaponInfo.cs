using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "GameData/WeaponInfo")]
public class WeaponInfo : ScriptableObject
{
    [Header("Weapon Info")]
    public float rateTime;            // ���� �ӵ�.
    public float bulletSpeed;         // �Ѿ� �ӵ�.
    public float power;               // ���ݷ�.
    public int maxBullet;             // �ִ� ���� ��.
    public int maxHaveBullet;         // �ִ� ���� ź�� ��.

    [Header("Collection")]
    public float addCollection;       // ������.
    public float delCollection;       // ���ҷ�.
    public float minCollection;       // �ּ� ��ź��.
    public float maxCollection;       // �ִ� ��ź��.

    [Header("Etc")]
    public Vector2 recoil;            // �ݵ� ��ġ.
    public LayerMask ignoreLayer;     // üũ ���� ���� ���̾�.
}
