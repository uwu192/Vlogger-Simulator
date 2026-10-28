using UnityEngine;

[CreateAssetMenu(fileName = "NewCameraData", menuName = "VloggerSimulator/Camera Data")]
public class CameraData : ScriptableObject
{
    [Header("Thông tin Thiết bị")]
    public string cameraName;

    [Header("Chỉ số Nâng cấp")]
    public int resolutionBonus;
    public int viewMultiplier;

}