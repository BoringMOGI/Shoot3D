using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject holePrefab;

    Rigidbody rigid;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hole = Instantiate(holePrefab);
        hole.transform.position = transform.position;
        hole.transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);

        Destroy(gameObject);        // 내 게임 오브젝트를 삭제하겠다.
    }

    private void OnTriggerEnter(Collider other)
    {
        //Instantiate(holePrefab, transform.position, transform.rotation);
        //Destroy(gameObject);        // 내 게임 오브젝트를 삭제하겠다.
    }

    public void Shoot(float bulletSpeed)
    {
        // velocity(속력):Vector3
        // = Vector3.forward(월드상 정면) * 속도 = 벡터.
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * bulletSpeed;
    }


}
