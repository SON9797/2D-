using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] private Transform _target;

    [Header("카메라 설정")]
    [SerializeField] private float _smoothSpeed = 5f;
    [SerializeField] private float _yOffset = 2f;

    [Header("카메라 이동 제한 구역")]
    [SerializeField] private float _minCameraPos;
    [SerializeField] private float _maxCameraPos;

    void LateUpdate()
    {
        if (_target == null) return;

        Vector3 targetPosition = new Vector3(_target.position.x, _target.position.y + _yOffset, transform.position.z);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed * Time.deltaTime);

        float clampedX = Mathf.Clamp(smoothedPosition.x, _minCameraPos, _maxCameraPos);

        transform.position = new Vector3(clampedX, smoothedPosition.y, smoothedPosition.z);
    }
}
