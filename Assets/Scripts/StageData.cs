using UnityEngine;

// Định nghĩa Enum cho Quốc gia để dễ quản lý và tránh lỗi gõ phím
public enum Country
{
    Vietnam,
    SouthKorea
}

[CreateAssetMenu(fileName = "NewStageData", menuName = "VloggerSimulator/Stage Data")]
public class StageData : ScriptableObject
{
    [Header("Thông tin cơ bản")]
    public string stageName;
    public Country country;

    [Header("Đặc trưng khu vực")]
    [TextArea(2, 5)]
    public string landmarks;

    [TextArea(2, 5)]
    public string weatherDescription;
}