using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // 카메라가 따라갈 목표 (스파이더맨 캐릭터)
    public float smoothSpeed = 0.125f; // 카메라 움직임의 부드러움 정도
    public Vector3 offset; // 캐릭터로부터 카메라의 상대적인 위치

    void LateUpdate() // LateUpdate는 모든 Update 함수가 호출된 후에 호출됩니다.
                      // 캐릭터의 움직임이 완전히 계산된 후에 카메라를 움직이는 것이 좋습니다.
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target is not set!");
            return;
        }

        // 목표(캐릭터)의 현재 위치에 오프셋을 더한 것이 카메라의 목표 위치
        Vector3 desiredPosition = target.position + offset;

        // 현재 카메라 위치에서 목표 위치까지 부드럽게 이동
        // Lerp는 두 값 사이를 선형적으로 보간합니다.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // 카메라가 목표(캐릭터)를 바라보도록 설정
        transform.LookAt(target);
    }
}