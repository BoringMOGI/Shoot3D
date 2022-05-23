using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] ParticleSystem explodeEffect;

    [Header("Range")]
    [SerializeField] float delay;
    [SerializeField] float explodeRadius;   // 폭발 반경.
    [SerializeField] float explodeForce;    // 힘.

    [Header("Damage")]
    [SerializeField] int damagePower;       // 데미지.
    [SerializeField] LayerMask damageMask;  // 폭발 데미지 마스크.

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

    // 슈류탄을 던지는 함수.
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
        // 폭발 반경에 부서지는 오브젝트가 있다면 활성화.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (Collider collider in colliders)
        {
            DestructObject destruct = collider.GetComponent<DestructObject>();
            if (destruct != null)
                destruct.OnDestruct();
        }

        // 폭발 반경의 오브젝트의 움직임.
        colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rigid = collider.GetComponent<Rigidbody>();
            if (rigid != null)
            {
                // 폭발에 의한 힘.
                // 힘의 강도, 폭발 지점, 폭발 범위.
                rigid.AddExplosionForce(explodeForce, transform.position, explodeRadius);
            }
        }
    }
    private void Damage()
    {
        // 폭발 반경에 데미지 받는 존재가 있다면 데미지 받기.
        Collider[] damages = Physics.OverlapSphere(transform.position, explodeRadius, damageMask);
        foreach (Collider collider in damages)
        {
            Damageable target = collider.GetComponent<Damageable>();
            if (target != null)
            {
                // 폭발 지점과 대상의 거리 비율을 계산.
                float distance = Vector3.Distance(transform.position, target.transform.position);
                float distanceRatio = distance / explodeRadius;
                float ratio = 1f;

                // 거리에 따른 데미지 비율 계산.
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

                // 실제 데미지 전달.
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
