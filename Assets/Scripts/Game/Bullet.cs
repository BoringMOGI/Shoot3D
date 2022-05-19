using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject holePrefab;

    Rigidbody rigid;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌 : " + collision.gameObject.name);

        // 충돌한 충돌체에게서 Damgeable 컴포넌트 검색.
        Damageable target = collision.gameObject.GetComponent<Damageable>();
        if(target != null)
        {
            target.OnDamaged(10);
            DamageManager.Instance.ShowDamageText(transform.position, 10);
        }
        else
        {
            GameObject hole = Instantiate(holePrefab);
            hole.transform.position = transform.position;
            hole.transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);
        }

        Destroy(gameObject);        // 내 게임 오브젝트를 삭제하겠다.
    }
    public void Shoot(float bulletSpeed, Vector3 direction)
    {
        // velocity(속력):Vector3
        // = Vector3.forward(월드상 정면) * 속도 = 벡터.
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = direction * bulletSpeed;
    }


}
