using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] float moveSpeed;       // �̵��ӵ�.
    [SerializeField] float jumpHeight;      // ���� ����.

    [Range(1.0f, 3.0f)]
    [SerializeField] float gravityScale;    // �߷� ���.

    [Header("Ground")]
    [SerializeField] Transform groundPivot; // ���� üũ �߽���.
    [SerializeField] float groundRadius;    // ���� üũ ���� ������.
    [SerializeField] LayerMask groundMask;  // ���� ���̾� ����ũ.

    CharacterController controller;         // ĳ���� ��Ʈ�ѷ� Ŭ����.
    Animator anim;
    bool isGrounded;                        // ���� �ִ°�?
    Vector3 velocity;                       // ���� ���� �ӵ�.

    float gravity => -9.81f * gravityScale; // ���� �߷� ���ӵ� * �߷� ���.

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundPivot.position, groundRadius, groundMask);
        anim.SetBool("isGrounded", isGrounded);

        // ���鿡 ���������� ������ �ӵ��� �ϰ��ϰ� ���� ��.
        if (isGrounded && velocity.y < 0f)
        {
            // ���� ���� �༭ ������ �� �ֵ��� �Ѵ�.
            velocity.y = -2f;
        }

        Move();

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // (H*-2f*G)^2
            // ���� ���Ŀ� ���� Vector.up�������� �ӵ��� ���Ѵ�.
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetTrigger("onJump");
        }

        // �츮�� ��� �߷��� �ް� �ֱ� ������ �߷� ���� ���Ѵ�.
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");   // ������:1, �ȴ�����:0, ����:-1.
        float z = Input.GetAxis("Vertical");     // ����:1, �ȴ�����:0, �Ʒ���:-1.

        // Vector3.right : ���� ��ǥ�� �������� ������ ���� ��.
        // transform.right : ���� �������� ������ ���� ��.

        // direction = �� Ű �Է¿� ���� ������ �ϴ� "����"
        // movement  = �̵���.
        // ������ Ư¡ : ����(����)�� -1�� ���ϸ� �ݴ� ������ �ȴ�.
        Vector3 direction = (transform.right * x) + (transform.forward * z);
        controller.Move(direction * moveSpeed * Time.deltaTime);

        anim.SetFloat("horizontal", x);
        anim.SetFloat("vertical", z);
    }
   
    private void OnDrawGizmos()
    {
        if (groundPivot == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(groundPivot.position, groundRadius);
    }
}
