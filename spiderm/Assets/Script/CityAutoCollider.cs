using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CityAutoCollider : MonoBehaviour
{
    void Start()
    {
        // ���� ���� ��� Renderer ã��
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogWarning("Renderer�� �����ϴ�!");
            return;
        }

        // ��ü Bounds ���
        Bounds bounds = renderers[0].bounds;
        foreach (Renderer rend in renderers)
        {
            bounds.Encapsulate(rend.bounds);
        }

        // Box Collider �������� (������ �ڵ� �߰���)
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        // ���� �������� ��ȯ�ؼ� Collider�� ����
        Vector3 localCenter = transform.InverseTransformPoint(bounds.center);
        Vector3 localSize = transform.InverseTransformVector(bounds.size);

        boxCollider.center = localCenter;
        boxCollider.size = localSize;

        Debug.Log("���ÿ� �ڵ� Box Collider ���� �Ϸ�!");
    }
}
