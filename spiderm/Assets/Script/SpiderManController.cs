using UnityEngine;

public class SpiderManController : MonoBehaviour
{
    public float moveSpeed = 5f; // ĳ���� �̵� �ӵ�
    public float jumpForce = 8f; // ĳ���� ���� ��

    private Rigidbody rb; // Rigidbody ������Ʈ ����
    private bool isGrounded; // ĳ���Ͱ� ���� ����ִ��� Ȯ���ϴ� ����

    private Animator animator; // Animator ������Ʈ ����
    public Transform modelTransform; // ���� 3D ���� Transform (�ν����Ϳ��� ����)

    void Start()
    {
        // ���� ���� ������Ʈ���� Rigidbody ������Ʈ ��������
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("ERROR: Rigidbody component not found on this GameObject (SpiderMan)! Please add a Rigidbody.");
            // Rigidbody�� ������ ��ũ��Ʈ ������ ������ ���� �ֽ��ϴ�.
            // enabled = false; 
            return;
        }

        // Rigidbody ȸ�� ���� (ĳ���Ͱ� �Ѿ����� �ʵ���)
        rb.freezeRotation = true;

        // modelTransform�� �Ҵ�Ǿ����� Ȯ���ϰ� Animator ������Ʈ ��������
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

        // ���� ���� �� �ʱ� isGrounded ���¸� �ֿܼ� ���
        Debug.Log("Game Started. Initial isGrounded state: " + isGrounded);
    }

    void Update()
    {
        // === 1. �̵� �Է� ó�� ===
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D �Ǵ� ��/�� ȭ��ǥ
        float moveVertical = Input.GetAxis("Vertical");     // W/S �Ǵ� ��/�� ȭ��ǥ

        // �̵� ���� ��� (Y���� �߷°� ������ ����ϹǷ� 0)
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        // *** ����� �α�: �Է� �� Ȯ�� ***
        Debug.Log("Input: H=" + moveHorizontal + ", V=" + moveVertical + " | Movement Vector: " + movement);

        // === 2. ĳ���� ȸ�� ó�� (�𵨸� ȸ��) ===
        // �̵� �Է��� ���� ���� ȸ�� (���� ���� ���� ����)
        if (movement.magnitude > 0.1f)
        {
            if (modelTransform != null)
            {
                // �̵� �������� ���� �ٶ󺸵��� ȸ�� ��ǥ ����
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                // ���� �� ȸ������ ��ǥ ȸ������ �ε巴�� ����
                modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        // === 3. Rigidbody�� �̿��� �̵� ó�� ===
        if (rb != null)
        {
            // �̵� ������ ����ȭ�Ͽ� ������ �ӵ� ����
            // Time.deltaTime�� ���Ͽ� ������ �ӵ��� �������� �̵�
            Vector3 targetPosition = rb.position + movement.normalized * moveSpeed * Time.deltaTime;
            rb.MovePosition(targetPosition);

            // *** ����� �α�: Rigidbody �̵� ��ǥ ��ġ Ȯ�� ***
            Debug.Log("Rigidbody moving to: " + targetPosition);
        }

        // === 4. �ִϸ��̼� �Ķ���� ������Ʈ (�̵� �ӵ�) ===
        if (animator != null)
        {
            // �̵� ������ ũ�⸦ �ִϸ������� "Speed" �Ķ���Ϳ� ����
            // �������� ������ 0, �������� ������ 1�� ����� ��
            float currentSpeed = movement.magnitude;
            animator.SetFloat("Speed", currentSpeed); // Animator Controller�� "Speed" Float �Ķ���� ���
            // *** ����� �α�: �ִϸ��̼� ���ǵ� �Ķ���� �� Ȯ�� ***
            Debug.Log("Animator Speed Parameter: " + currentSpeed);
        }

        // === 5. ���� �Է� ó�� ===
        // �����̽��ٸ� ������, ĳ���Ͱ� ���� ����ִ��� Ȯ��
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            if (rb != null)
            {
                // �������� ���� ���� ���� (ForceMode.Impulse�� �������� ��)
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false; // ���������� ���� ���� ���� ���·� ����
                // *** ����� �α�: ���� ���� Ȯ�� ***
                Debug.Log("Jump Triggered! isGrounded set to false.");
            }
            if (animator != null)
            {
                animator.SetTrigger("Jump"); // Animator Controller�� "Jump" Trigger �Ķ���� ���
            }
        }
    }

    // === �浹 ���� (���� ��Ҵ��� Ȯ��) ===
    // ĳ������ �ݶ��̴��� �ٸ� �ݶ��̴��� �����ϱ� ������ �� ȣ��
    void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� �±װ� "Ground"���� Ȯ��
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // ���� �����
            // *** ����� �α�: �� �浹 ���� Ȯ�� ***
            Debug.Log("OnCollisionEnter: Touched Ground! isGrounded set to true.");

            if (animator != null)
            {
                animator.SetBool("IsGrounded", true); // Animator Controller�� "IsGrounded" Bool �Ķ���� ���
            }
        }
        else
        {
            // �ٸ� ������Ʈ�� �浹���� �� (�ɼ�)
            Debug.Log("OnCollisionEnter: Collided with non-Ground object: " + collision.gameObject.name);
        }
    }

    // ĳ������ �ݶ��̴��� �ٸ� �ݶ��̴����� ������ �� ȣ��
    void OnCollisionExit(Collision collision)
    {
        // �浹�� ���� ������Ʈ�� �±װ� "Ground"���� Ȯ��
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // ������ ��������
            // *** ����� �α�: ������ ������ Ȯ�� ***
            Debug.Log("OnCollisionExit: Left Ground! isGrounded set to false.");

            if (animator != null)
            {
                animator.SetBool("IsGrounded", false); // Animator Controller�� "IsGrounded" Bool �Ķ���� ���
            }
        }
    }
}