using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairUI : Singleton<CrossHairUI>
{
    Transform[] hairs;

    private void Start()
    {
        hairs = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            hairs[i] = transform.GetChild(i);
    }

    public void SwitchCrosshair(bool isOn)
    {
        foreach(Transform t in hairs)
        {
            t.gameObject.SetActive(isOn);
        }
    }
}
