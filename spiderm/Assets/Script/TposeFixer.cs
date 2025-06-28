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
            Debug.LogError("Animator가 없습니다!");
            return;
        }

        // 애니메이션 이름이 비어있지 않으면 재생
        if (!string.IsNullOrEmpty(defaultAnimationName))
        {
            animator.Play(defaultAnimationName);
        }
    }
}
