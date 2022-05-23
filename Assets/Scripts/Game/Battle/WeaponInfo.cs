using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "GameData/WeaponInfo")]
public class WeaponInfo : ScriptableObject
{
    [Header("Weapon Info")]
    public float rateTime;            // 연사 속도.
    public float bulletSpeed;         // 총알 속도.
    public float power;               // 공격력.
    public int maxBullet;             // 최대 장전 수.
    public int maxHaveBullet;         // 최대 소지 탄약 수.

    [Header("Collection")]
    public float addCollection;       // 증가량.
    public float delCollection;       // 감소량.
    public float minCollection;       // 최소 집탄율.
    public float maxCollection;       // 최대 집탄율.

    [Header("Etc")]
    public Vector2 recoil;            // 반동 수치.
    public LayerMask ignoreLayer;     // 체크 하지 않을 레이어.
}
