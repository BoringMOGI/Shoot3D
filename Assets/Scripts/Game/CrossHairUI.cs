using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairUI : Singleton<CrossHairUI>
{
    Transform[] hairs;
    float screenUnit;

    private void Start()
    {
        // 월드 좌표상 Unit(1M)가 Screen상에서 몇 Pixel인가?
        Camera cam = Camera.main;
        Vector3 onePosition = cam.transform.position + cam.transform.forward * 5f;

        Vector3 p1 = cam.WorldToScreenPoint(onePosition);
        Vector3 p2 = cam.WorldToScreenPoint(onePosition + cam.transform.right);
        screenUnit = Vector3.Distance(p1, p2);

        // 자식 오브젝트 검색.
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
    public void UpdateCrosshair(float collectionRate)
    {
        float offset = collectionRate * screenUnit;
        SetHairPosition(0, 0, offset);
        SetHairPosition(1, 0, -offset);
        SetHairPosition(2, -offset, 0);
        SetHairPosition(3, offset, 0);
    }

    private void SetHairPosition(int index, float x, float y)
    {   
        hairs[index].localPosition = new Vector3(x, y, 0);
    }
}
