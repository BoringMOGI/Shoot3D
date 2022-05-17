using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairUI : Singleton<CrossHairUI>
{
    [SerializeField] float hairOffset;

    Transform[] hairs;
    float screenUnit;

    private void Start()
    {
        // ���� ��ǥ�� Unit(1M)�� Screen�󿡼� �� Pixel�ΰ�?
        Camera cam = Camera.main;
        Vector3 p1 = cam.WorldToScreenPoint(Vector3.zero);
        Vector3 p2 = cam.WorldToScreenPoint(Vector3.right);
        screenUnit = Vector3.Distance(p1, p2);

        // �ڽ� ������Ʈ �˻�.
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
        float offset = collectionRate * screenUnit * hairOffset;
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