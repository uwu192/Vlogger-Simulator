using UnityEngine;

[CreateAssetMenu(fileName = "NewCameraData", menuName = "VloggerSimulator/Camera Data")]
public class CameraData : ItemData
{
    [Header("Thông tin chỉ số")]
    public int resolutionBonus;
    public int viewMultiplier;

}