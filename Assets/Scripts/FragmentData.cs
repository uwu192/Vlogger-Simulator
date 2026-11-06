using UnityEngine;

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
    public FragmentRarity rarity;
    public Sprite commonImage;
    public Sprite rareImage;
    public Sprite epicImage;

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
    public Sprite GetImage()
    {
        switch (rarity)
        {
            case FragmentRarity.Common:
                return commonImage;
            case FragmentRarity.Rare:
                return rareImage;
            case FragmentRarity.Epic:
                return epicImage;
            default:
                return null;
        }
    }
}