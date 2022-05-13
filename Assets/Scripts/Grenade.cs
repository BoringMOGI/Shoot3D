using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] ParticleSystem explodeEffect;

    [SerializeField] float delay;
    [SerializeField] float explodeRadius;   // ���� �ݰ�.
    [SerializeField] float explodeForce;    // ��.

    float countDown;

    private void Start()
    {
        countDown = 0f;
    }

    private void Update()
    {
        countDown += Time.deltaTime;
        if(countDown >= delay)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    // ����ź�� ������ �Լ�.
    public void Throw(Vector3 direction, float power)
    {
        rigid.AddForce(direction * power, ForceMode.Impulse);
    }
    private void Explode()
    {
        ParticleSystem effect = Instantiate(explodeEffect, transform.position, Quaternion.identity);
        effect.Play();

        AudioManager.Instance.PlaySE("grenade", 0.7f);

        // ���� �ݰ濡 �μ����� ������Ʈ�� �ִٸ� Ȱ��ȭ.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach(Collider collider in colliders)
        {
            DestructObject destruct = collider.GetComponent<DestructObject>();
            if (destruct != null)
                destruct.OnDestruct();
        }


        // ���� �ݰ��� ������Ʈ�� ������.
        colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach(Collider collider in colliders)
        {
            Rigidbody rigid = collider.GetComponent<Rigidbody>();
            if(rigid != null)
            {
                // ���߿� ���� ��.
                // ���� ����, ���� ����, ���� ����.
                rigid.AddExplosionForce(explodeForce, transform.position, explodeRadius);
            }
        }
    }
}
