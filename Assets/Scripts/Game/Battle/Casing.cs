using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour
{
    bool isPlaySE;

    private void OnCollisionEnter(Collision collision)
    {
        if (isPlaySE)
            return;

        int random = Random.Range(1, 5);
        AudioManager.Instance.PlaySE(string.Concat("casing", random));
        isPlaySE = true;
    }
}
