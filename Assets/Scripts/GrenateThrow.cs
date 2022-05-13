using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenateThrow : MonoBehaviour
{
    [SerializeField] Grenade grenadePrefab;
    [SerializeField] float throwPower;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            Vector3 position = transform.position + (transform.forward * 1f);
            Grenade grenade = Instantiate(grenadePrefab, position, Quaternion.identity);
            grenade.Throw(transform.forward, throwPower);
        }
    }

}
