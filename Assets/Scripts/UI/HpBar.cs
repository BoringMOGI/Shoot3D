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
        // 월드 좌표를 스크린 좌표로 변경.
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
        float ratio = current / max;        // 체력 비율.
        hpImage.fillAmount = ratio;

        hpText.text = string.Format("{0}/{1}", current, max);
    }

}
