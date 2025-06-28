using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TposeFixer : MonoBehaviour
{
    public string defaultAnimationName = "Idle";

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator�� �����ϴ�!");
            return;
        }

        // �ִϸ��̼� �̸��� ������� ������ ���
        if (!string.IsNullOrEmpty(defaultAnimationName))
        {
            animator.Play(defaultAnimationName);
        }
    }
}
