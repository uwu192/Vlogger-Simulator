using UnityEngine;
public enum CatalystType
{
    Sound,
    Camera
}
[CreateAssetMenu(fileName = "NewScriptableObjectScript", menuName = "Scriptable Objects/NewScriptableObjectScript")]
public class CatalystData : ScriptableObject
{
    [Header("Đặc tính mảnh phụ trợ")]
    public CatalystType catalystType;
    public Sprite icon;
}
