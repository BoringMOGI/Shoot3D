using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] Animator anim;
    [SerializeField] float moveSpeed;       // �̵��ӵ�.
    [SerializeField] float jumpHeight;      // ���� ����.

    [Range(1.0f, 3.0f)]
    [SerializeField] float gravityScale;    // �߷� ���.

    [Header("Ground")]
    [SerializeField] Transform groundPivot; // ���� üũ �߽���.
    [SerializeField] float groundRadius;    // ���� üũ ���� ������.
    [SerializeField] LayerMask groundMask;  // ���� ���̾� ����ũ.

    CharacterController controller;         // ĳ���� ��Ʈ�ѷ� Ŭ����.
    bool isGrounded;                        // ���� �ִ°�?
    Vector3 velocity;                       // ���� ���� �ӵ�.

    float gravity => -9.81f * gravityScale; // ���� �߷� ���ӵ� * �߷� ���.

    [Range(0.0f, 2.0f)]
    [SerializeField] float timeScale = 1f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        
    }
    void Update()
    {
        Time.timeScale = timeScale;

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
        bool isRun = Input.GetKey(KeyCode.LeftShift);

        // Vector3.right : ���� ��ǥ�� �������� ������ ���� ��.
        // transform.right : ���� �������� ������ ���� ��.

        // direction = �� Ű �Է¿� ���� ������ �ϴ� "����"
        // movement  = �̵���.
        // ������ Ư¡ : ����(����)�� -1�� ���ϸ� �ݴ� ������ �ȴ�.
        Vector3 direction = (transform.right * x) + (transform.forward * z);
        Vector3 movement = direction * moveSpeed * Time.deltaTime;
        movement *= isRun ? 1.5f : 1.0f;

        controller.Move(movement);

        // ������ 0�� �ƴ� ���.
        anim.SetBool("isWalk", movement != Vector3.zero);
        anim.SetBool("isRun", isRun);
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
