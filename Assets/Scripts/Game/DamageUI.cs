using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour, IObjectPool<DamageUI>
{
    private enum SHOW_TYPE
    {
        Fix,            // 고정.
        MoveUp,         // 위로 이동.
        Volcano,        // 화산 폭발.
    }

    [SerializeField] Text damageText;
    [SerializeField] Rigidbody rigid;
    [SerializeField] float showTime;
    [SerializeField] float fadeTime;
    [SerializeField] SHOW_TYPE showType;

    bool isShow;
    bool isStartShow;
    float countdown;
    Transform cam;

    public void SetDamage(Vector3 position, int damage)
    {
        isShow = true;
        isStartShow = false;

        countdown = 0f;
        transform.position = position;
        damageText.text = damage.ToString();
        rigid.velocity = Vector3.zero;
        rigid.isKinematic = true;

        ChangeAlpha(1f);
    }
    void Update()
    {
        LookCamera();
        ShowType();

        // 보여지는 시간.
        if (isShow)
        {
            CountDown();
        }
        else
        {
            Fade();
        }
    }

    private void LookCamera()
    {
        Vector3 dir = (transform.position - cam.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void ShowType()
    {
        switch(showType)
        {
            case SHOW_TYPE.Fix:
                break;
            case SHOW_TYPE.MoveUp:
                MoveUp();
                break;
            case SHOW_TYPE.Volcano:
                if(isStartShow == false)
                    MoveVolcano();
                break;
        }

        isStartShow = true;
    }

    private void MoveUp()
    {
        transform.position += Vector3.up * 0.2f * Time.deltaTime;
    }
    private void MoveVolcano()
    {
        rigid.isKinematic = false;

        Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        rigid.AddForce(dir * rigid.mass * 2f, ForceMode.Impulse);
    }


    private void CountDown()
    {
        countdown += Time.deltaTime;
        if (countdown >= showTime)
        {
            countdown = fadeTime;
            isShow = false;
        }
    }
    private void Fade()
    {
        countdown -= Time.deltaTime;                // 시간을 타임 값으로 감소.
        if (countdown <= 0.0f)                      // 0이하가 되면
        {
            onReturn(this);                         // 오브젝트 풀 반납.
        }
        else
        {
            ChangeAlpha(countdown / fadeTime);      // 알파 값 변경.
        }
    }
    private void ChangeAlpha(float alpha)
    {
        Color textColor = damageText.color;     // 텍스트에서 색상 값을 대입.
        textColor.a = alpha;                    // 색상 값 중에서 알파 값만 변경 (직접 대입이 불가능해서)
        damageText.color = textColor;           // 변경한 색상을 대입.
    }


    


    // 풀링 인터페이스 구현.
    private ReturnPoolEvent<DamageUI> onReturn;
    public void Setup(ReturnPoolEvent<DamageUI> onReturn)
    {
        this.onReturn = onReturn;
        cam = Camera.main.transform;
    }
}
