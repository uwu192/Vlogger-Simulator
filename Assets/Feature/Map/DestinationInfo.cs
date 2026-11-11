using UnityEngine;

public enum Countries
{
    VietNam,
    Korea
}

[CreateAssetMenu(fileName = "DestinationInfo", menuName = "Scriptable Objects/DestinationInfo")]
public class DestinationInfo : ScriptableObject
{
    public Sprite image;
    public string destinationName;
    public Countries country;

    [Header("Cấu hình Mở Khóa & Chuyển Scene")]
    public int unlockCost = 100;       // Số tiền cần để mở khóa
    public string sceneToLoad;         // Tên chính xác của Scene 2D muốn chuyển đến
}