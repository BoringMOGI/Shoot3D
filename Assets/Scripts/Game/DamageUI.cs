using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour, IObjectPool<DamageUI>
{
    [SerializeField] Text damageText;
    [SerializeField] float showTime;

    float countdown;
    Transform cam;

    public void SetDamage(Vector3 position, int damage)
    {
        transform.position = position;
        damageText.text = damage.ToString();
        countdown = 0f;
    }
    void Update()
    {
        Vector3 dir = (transform.position - cam.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);

        countdown += Time.deltaTime;
        if(countdown >= showTime)
        {
            countdown = 0f;
            onReturn(this);
        }
    }

    // 풀링 인터페이스 구현.
    private ReturnPoolEvent<DamageUI> onReturn;
    public void Setup(ReturnPoolEvent<DamageUI> onReturn)
    {
        this.onReturn = onReturn;
        cam = Camera.main.transform;
    }
}
