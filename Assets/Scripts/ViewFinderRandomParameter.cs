using UnityEngine;

public class ViewfinderManager : MonoBehaviour
{
    [Header("Target Parameters (Hệ thống sinh ra)")]
    [SerializeField] private float targetFocus;
    [SerializeField] private float targetExpose;

    [Header("Player Inputs (UI Slider truyền vào)")]
    [Range(0f, 100f)] public float playerFocus;
    [Range(0f, 100f)] public float playerExpose;

    public void GenerateNewTargets(float cameraFocus, float cameraExpose)
    {
        // Máy quay sẽ sinh ngẫu nhiên target cho focus (lấy nét) và expose (độ sáng) từ 0 đến tối đa cho phép dựa vào đầu vào thiết bị đang sử dụng
        targetFocus = Random.Range(0f, cameraFocus);
        targetExpose = Random.Range(0f, cameraExpose);

        Debug.Log($"Target mới đã được thiết lập -> Focus: {targetFocus:F1}, Expose: {targetExpose:F1}");
    }

    public float CalculateCameraAccuracy()
    {
        float focusDifference = Mathf.Abs(targetFocus - playerFocus);
        float exposeDifference = Mathf.Abs(targetExpose - playerExpose);

        float averageDifference = (focusDifference + exposeDifference) / 2f;

        float accuracyPercentage = 100f - averageDifference;

        return Mathf.Max(0f, accuracyPercentage);
    }
}