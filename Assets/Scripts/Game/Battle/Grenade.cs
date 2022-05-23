using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] ParticleSystem explodeEffect;

    [Header("Range")]
    [SerializeField] float delay;
    [SerializeField] float explodeRadius;   // ���� �ݰ�.
    [SerializeField] float explodeForce;    // ��.

    [Header("Damage")]
    [SerializeField] int damagePower;       // ������.
    [SerializeField] LayerMask damageMask;  // ���� ������ ����ũ.

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
        AudioManager.Instance.PlaySE("grenade", 0.7f);
        ParticleSystem effect = Instantiate(explodeEffect, transform.position, Quaternion.identity);
        effect.Play();

        Destruct();
        Damage();
    }
    private void Destruct()
    {
        // ���� �ݰ濡 �μ����� ������Ʈ�� �ִٸ� Ȱ��ȭ.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (Collider collider in colliders)
        {
            DestructObject destruct = collider.GetComponent<DestructObject>();
            if (destruct != null)
                destruct.OnDestruct();
        }

        // ���� �ݰ��� ������Ʈ�� ������.
        colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rigid = collider.GetComponent<Rigidbody>();
            if (rigid != null)
            {
                // ���߿� ���� ��.
                // ���� ����, ���� ����, ���� ����.
                rigid.AddExplosionForce(explodeForce, transform.position, explodeRadius);
            }
        }
    }
    private void Damage()
    {
        // ���� �ݰ濡 ������ �޴� ���簡 �ִٸ� ������ �ޱ�.
        Collider[] damages = Physics.OverlapSphere(transform.position, explodeRadius, damageMask);
        foreach (Collider collider in damages)
        {
            Damageable target = collider.GetComponent<Damageable>();
            if (target != null)
            {
                // ���� ������ ����� �Ÿ� ������ ���.
                float distance = Vector3.Distance(transform.position, target.transform.position);
                float distanceRatio = distance / explodeRadius;
                float ratio = 1f;

                // �Ÿ��� ���� ������ ���� ���.
                if(distanceRatio <= 0.15f)
                {
                    ratio = 1.2f;
                }
                else if(distanceRatio <= 0.7f)
                {
                    ratio = 1.0f;
                }
                else
                {
                    ratio = 0.5f;
                }

                // ���� ������ ����.
                target.OnDamaged(Mathf.RoundToInt(damagePower * ratio));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explodeRadius * 0.15f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explodeRadius * 0.7f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explodeRadius);
    }
}
