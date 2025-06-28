using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CityAutoCollider : MonoBehaviour
{
    void Start()
    {
        // 도시 모델의 모든 Renderer 찾기
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogWarning("Renderer가 없습니다!");
            return;
        }

        // 전체 Bounds 계산
        Bounds bounds = renderers[0].bounds;
        foreach (Renderer rend in renderers)
        {
            bounds.Encapsulate(rend.bounds);
        }

        // Box Collider 가져오기 (없으면 자동 추가됨)
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        // 로컬 공간으로 변환해서 Collider에 적용
        Vector3 localCenter = transform.InverseTransformPoint(bounds.center);
        Vector3 localSize = transform.InverseTransformVector(bounds.size);

        boxCollider.center = localCenter;
        boxCollider.size = localSize;

        Debug.Log("도시에 자동 Box Collider 적용 완료!");
    }
}
