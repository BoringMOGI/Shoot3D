using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    [SerializeField] Grenade grenadePrefab;
    [SerializeField] Transform eye;
    [SerializeField] Transform createPivot;
    [SerializeField] float throwPower;

    [Header("count")]
    [SerializeField] int grenadeCount;

    private void Start()
    {
        WeaponInfoUI.Instance.UpdateGrenadeCount(grenadeCount);
    }

    public void OnThrowGrenabe()
    {
        if (grenadeCount <= 0)
            return;

        grenadeCount--;
        Vector3 position = createPivot.position;
        Grenade grenade = Instantiate(grenadePrefab, position, Quaternion.identity);
        grenade.Throw(eye.forward, throwPower);

        WeaponInfoUI.Instance.UpdateGrenadeCount(grenadeCount);
    }
}
