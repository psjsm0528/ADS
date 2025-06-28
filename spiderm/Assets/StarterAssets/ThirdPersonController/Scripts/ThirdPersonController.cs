using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 6f;
    public float rotationSmoothTime = 0.1f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Animator animator;
    private Transform cameraTransform;

    private Vector3 velocity;
    private float rotationVelocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("메인 카메라를 찾을 수 없습니다! Camera에 'MainCamera' 태그를 지정하세요.");
        }
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float targetSpeed = 0f;
        if (direction.magnitude >= 0.1f)
        {
            // Shift키로 달리기
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            targetSpeed = isRunning ? runSpeed : walkSpeed;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * targetSpeed * Time.deltaTime);
        }

        // Animator에 전달할 Speed (0~1)
        float animationSpeed = direction.magnitude;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // 달릴 땐 범위를 확실히 1에 가깝게
            animationSpeed = Mathf.Clamp(animationSpeed * (runSpeed / walkSpeed), 0f, 1f);
        }
        animator.SetFloat("Speed", animationSpeed);

        // 점프 (옵션)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
