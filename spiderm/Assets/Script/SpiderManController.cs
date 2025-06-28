using UnityEngine;

public class SpiderManController : MonoBehaviour
{
    public float moveSpeed = 5f; // 캐릭터 이동 속도
    public float jumpForce = 8f; // 캐릭터 점프 힘

    private Rigidbody rb; // Rigidbody 컴포넌트 참조
    private bool isGrounded; // 캐릭터가 땅에 닿아있는지 확인하는 변수

    private Animator animator; // Animator 컴포넌트 참조
    public Transform modelTransform; // 실제 3D 모델의 Transform (인스펙터에서 연결)

    void Start()
    {
        // 현재 게임 오브젝트에서 Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("ERROR: Rigidbody component not found on this GameObject (SpiderMan)! Please add a Rigidbody.");
            // Rigidbody가 없으면 스크립트 실행을 중지할 수도 있습니다.
            // enabled = false; 
            return;
        }

        // Rigidbody 회전 고정 (캐릭터가 넘어지지 않도록)
        rb.freezeRotation = true;

        // modelTransform이 할당되었는지 확인하고 Animator 컴포넌트 가져오기
        if (modelTransform != null)
        {
            animator = modelTransform.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("ERROR: Animator component not found on the Model Transform! Please add an Animator to " + modelTransform.name);
            }
        }
        else
        {
            Debug.LogError("ERROR: Model Transform is not assigned in SpiderManController! Please drag your 3D model into the 'Model Transform' slot in the Inspector.");
        }

        // 게임 시작 시 초기 isGrounded 상태를 콘솔에 출력
        Debug.Log("Game Started. Initial isGrounded state: " + isGrounded);
    }

    void Update()
    {
        // === 1. 이동 입력 처리 ===
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D 또는 좌/우 화살표
        float moveVertical = Input.GetAxis("Vertical");     // W/S 또는 상/하 화살표

        // 이동 벡터 계산 (Y축은 중력과 점프가 담당하므로 0)
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        // *** 디버깅 로그: 입력 값 확인 ***
        Debug.Log("Input: H=" + moveHorizontal + ", V=" + moveVertical + " | Movement Vector: " + movement);

        // === 2. 캐릭터 회전 처리 (모델만 회전) ===
        // 이동 입력이 있을 때만 회전 (아주 작은 값은 무시)
        if (movement.magnitude > 0.1f)
        {
            if (modelTransform != null)
            {
                // 이동 방향으로 모델이 바라보도록 회전 목표 설정
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                // 현재 모델 회전에서 목표 회전까지 부드럽게 보간
                modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        // === 3. Rigidbody를 이용한 이동 처리 ===
        if (rb != null)
        {
            // 이동 방향을 정규화하여 일정한 속도 유지
            // Time.deltaTime을 곱하여 프레임 속도에 독립적인 이동
            Vector3 targetPosition = rb.position + movement.normalized * moveSpeed * Time.deltaTime;
            rb.MovePosition(targetPosition);

            // *** 디버깅 로그: Rigidbody 이동 목표 위치 확인 ***
            Debug.Log("Rigidbody moving to: " + targetPosition);
        }

        // === 4. 애니메이션 파라미터 업데이트 (이동 속도) ===
        if (animator != null)
        {
            // 이동 벡터의 크기를 애니메이터의 "Speed" 파라미터에 전달
            // 움직임이 없으면 0, 움직임이 있으면 1에 가까운 값
            float currentSpeed = movement.magnitude;
            animator.SetFloat("Speed", currentSpeed); // Animator Controller의 "Speed" Float 파라미터 사용
            // *** 디버깅 로그: 애니메이션 스피드 파라미터 값 확인 ***
            Debug.Log("Animator Speed Parameter: " + currentSpeed);
        }

        // === 5. 점프 입력 처리 ===
        // 스페이스바를 눌렀고, 캐릭터가 땅에 닿아있는지 확인
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            if (rb != null)
            {
                // 위쪽으로 힘을 가해 점프 (ForceMode.Impulse는 순간적인 힘)
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false; // 점프했으니 땅에 닿지 않은 상태로 변경
                // *** 디버깅 로그: 점프 상태 확인 ***
                Debug.Log("Jump Triggered! isGrounded set to false.");
            }
            if (animator != null)
            {
                animator.SetTrigger("Jump"); // Animator Controller의 "Jump" Trigger 파라미터 사용
            }
        }
    }

    // === 충돌 감지 (땅에 닿았는지 확인) ===
    // 캐릭터의 콜라이더가 다른 콜라이더와 접촉하기 시작할 때 호출
    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 태그가 "Ground"인지 확인
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // 땅에 닿았음
            // *** 디버깅 로그: 땅 충돌 감지 확인 ***
            Debug.Log("OnCollisionEnter: Touched Ground! isGrounded set to true.");

            if (animator != null)
            {
                animator.SetBool("IsGrounded", true); // Animator Controller의 "IsGrounded" Bool 파라미터 사용
            }
        }
        else
        {
            // 다른 오브젝트와 충돌했을 때 (옵션)
            Debug.Log("OnCollisionEnter: Collided with non-Ground object: " + collision.gameObject.name);
        }
    }

    // 캐릭터의 콜라이더가 다른 콜라이더에서 떨어질 때 호출
    void OnCollisionExit(Collision collision)
    {
        // 충돌을 멈춘 오브젝트의 태그가 "Ground"인지 확인
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // 땅에서 떨어졌음
            // *** 디버깅 로그: 땅에서 떨어짐 확인 ***
            Debug.Log("OnCollisionExit: Left Ground! isGrounded set to false.");

            if (animator != null)
            {
                animator.SetBool("IsGrounded", false); // Animator Controller의 "IsGrounded" Bool 파라미터 사용
            }
        }
    }
}