using UnityEngine;

public enum FragmentTheme
{
    Food,       // Ẩm thực
    Landscape,  // Phong cảnh
    People,     // Con người
    DailyLife   // Đời thường
}

public enum FragmentRarity
{
    Common,
    Rare,
    Epic
}

[CreateAssetMenu(fileName = "NewFragmentData", menuName = "VloggerSimulator/Fragment Data")]
public class FragmentData : ScriptableObject
{
    [Header("Đặc tính Mảnh Cảnh Quay")]
    public string fragmentName;
    public FragmentTheme theme;
    public FragmentRarity rarity;

    public int GetBaseScore()
    {
        switch (rarity)
        {
            case FragmentRarity.Common:
                return 10;
            case FragmentRarity.Rare:
                return 30;
            case FragmentRarity.Epic:
                return 100;
            default:
                return 0;
        }
    }
}