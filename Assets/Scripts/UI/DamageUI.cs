using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour, IObjectPool<DamageUI>
{
    private enum SHOW_TYPE
    {
        Fix,            // ����.
        MoveUp,         // ���� �̵�.
        Volcano,        // ȭ�� ����.
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

        // �������� �ð�.
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
        countdown -= Time.deltaTime;                // �ð��� Ÿ�� ������ ����.
        if (countdown <= 0.0f)                      // 0���ϰ� �Ǹ�
        {
            onReturn(this);                         // ������Ʈ Ǯ �ݳ�.
        }
        else
        {
            ChangeAlpha(countdown / fadeTime);      // ���� �� ����.
        }
    }
    private void ChangeAlpha(float alpha)
    {
        Color textColor = damageText.color;     // �ؽ�Ʈ���� ���� ���� ����.
        textColor.a = alpha;                    // ���� �� �߿��� ���� ���� ���� (���� ������ �Ұ����ؼ�)
        damageText.color = textColor;           // ������ ������ ����.
    }


    


    // Ǯ�� �������̽� ����.
    private ReturnPoolEvent<DamageUI> onReturn;
    public void Setup(ReturnPoolEvent<DamageUI> onReturn)
    {
        this.onReturn = onReturn;
        cam = Camera.main.transform;
    }
}
