using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("--- Thông tin chung ---")]
    public string itemID;
    public string itemName;
    public int price;
    public Sprite icon;
    public GameObject prefab;
}
