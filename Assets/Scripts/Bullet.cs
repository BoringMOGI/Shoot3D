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

        Destroy(gameObject);        // �� ���� ������Ʈ�� �����ϰڴ�.
    }

    private void OnTriggerEnter(Collider other)
    {
        //Instantiate(holePrefab, transform.position, transform.rotation);
        //Destroy(gameObject);        // �� ���� ������Ʈ�� �����ϰڴ�.
    }

    public void Shoot(float bulletSpeed)
    {
        // velocity(�ӷ�):Vector3
        // = Vector3.forward(����� ����) * �ӵ� = ����.
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * bulletSpeed;
    }


}
