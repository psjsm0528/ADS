using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // ī�޶� ���� ��ǥ (�����̴��� ĳ����)
    public float smoothSpeed = 0.125f; // ī�޶� �������� �ε巯�� ����
    public Vector3 offset; // ĳ���ͷκ��� ī�޶��� ������� ��ġ

    void LateUpdate() // LateUpdate�� ��� Update �Լ��� ȣ��� �Ŀ� ȣ��˴ϴ�.
                      // ĳ������ �������� ������ ���� �Ŀ� ī�޶� �����̴� ���� �����ϴ�.
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target is not set!");
            return;
        }

        // ��ǥ(ĳ����)�� ���� ��ġ�� �������� ���� ���� ī�޶��� ��ǥ ��ġ
        Vector3 desiredPosition = target.position + offset;

        // ���� ī�޶� ��ġ���� ��ǥ ��ġ���� �ε巴�� �̵�
        // Lerp�� �� �� ���̸� ���������� �����մϴ�.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // ī�޶� ��ǥ(ĳ����)�� �ٶ󺸵��� ����
        transform.LookAt(target);
    }
}