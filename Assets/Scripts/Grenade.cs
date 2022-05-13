using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] ParticleSystem explodeEffect;

    [SerializeField] float delay;
    [SerializeField] float explodeRadius;   // 폭발 반경.
    [SerializeField] float explodeForce;    // 힘.

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
        ParticleSystem effect = Instantiate(explodeEffect, transform.position, Quaternion.identity);
        effect.Play();

        AudioManager.Instance.PlaySE("grenade", 0.7f);

        // 폭발 반경에 부서지는 오브젝트가 있다면 활성화.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach(Collider collider in colliders)
        {
            DestructObject destruct = collider.GetComponent<DestructObject>();
            if (destruct != null)
                destruct.OnDestruct();
        }


        // 폭발 반경의 오브젝트의 움직임.
        colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach(Collider collider in colliders)
        {
            Rigidbody rigid = collider.GetComponent<Rigidbody>();
            if(rigid != null)
            {
                // 폭발에 의한 힘.
                // 힘의 강도, 폭발 지점, 폭발 범위.
                rigid.AddExplosionForce(explodeForce, transform.position, explodeRadius);
            }
        }
    }
}
