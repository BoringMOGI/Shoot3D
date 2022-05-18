using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] Image hpImage;
    [SerializeField] Text hpText;

    Camera cam;

    public void OnUpdatePosition(Vector3 worldPosition)
    {
        // ���� ��ǥ�� ��ũ�� ��ǥ�� ����.
        // Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        // transform.position = screenPosition;
    }

    private void Start()
    {
        cam = Camera.main;
        
    }
    private void Update()
    {
        //transform.LookAt(cam.transform.position);
        Vector3 dir = (transform.position - cam.transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public void OnUpdateHp(float current, float max)
    {
        float ratio = current / max;        // ü�� ����.
        hpImage.fillAmount = ratio;

        hpText.text = string.Format("{0}/{1}", current, max);
    }

}
