using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] Animator anim;
    [SerializeField] float moveSpeed;       // 이동속도.
    [SerializeField] float jumpHeight;      // 점프 높이.

    [Range(1.0f, 3.0f)]
    [SerializeField] float gravityScale;    // 중력 배수.

    [Header("Ground")]
    [SerializeField] Transform groundPivot; // 지면 체크 중심점.
    [SerializeField] float groundRadius;    // 지면 체크 구의 반지름.
    [SerializeField] LayerMask groundMask;  // 지면 레이어 마스크.

    CharacterController controller;         // 캐릭터 컨트롤러 클래스.
    bool isGrounded;                        // 땅에 있는가?
    Vector3 velocity;                       // 현재 나의 속도.

    float gravity => -9.81f * gravityScale; // 실제 중력 가속도 * 중력 배수.

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
        // 지면에 도달했지만 여전히 속도가 하강하고 있을 때.
        if (isGrounded && velocity.y < 0f)
        {
            // 작은 값을 줘서 착지할 수 있도록 한다.
            velocity.y = -2f;
        }

        Move();

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // (H*-2f*G)^2
            // 물리 공식에 의해 Vector.up방향으로 속도를 가한다.
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetTrigger("onJump");
        }

        // 우리는 계속 중력을 받고 있기 때문에 중력 값을 더한다.
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");   // 오른쪽:1, 안누르면:0, 왼쪽:-1.
        float z = Input.GetAxis("Vertical");     // 위쪽:1, 안누르면:0, 아래쪽:-1.
        bool isRun = Input.GetKey(KeyCode.LeftShift);

        // Vector3.right : 월드 좌표를 기준으로 오른쪽 방향 값.
        // transform.right : 나를 기준으로 오른쪽 방향 값.

        // direction = 내 키 입력에 따라 가고자 하는 "방향"
        // movement  = 이동량.
        // 벡터의 특징 : 벡터(방향)에 -1을 곱하면 반대 방향이 된다.
        Vector3 direction = (transform.right * x) + (transform.forward * z);
        Vector3 movement = direction * moveSpeed * Time.deltaTime;
        movement *= isRun ? 1.5f : 1.0f;

        controller.Move(movement);

        // 방향이 0이 아닐 경우.
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
