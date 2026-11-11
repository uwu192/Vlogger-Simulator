using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [Header("UI Hiển thị Tiền")]
    [SerializeField] private TextMeshProUGUI totalMoneyText; // Kéo Text display tiền trên MapPanel vào đây

    [Header("Dữ liệu Địa điểm")]
    [SerializeField] private List<DestinationInfo> allDestinations;

    [Header("Cấu hình UI Prefab & ScrollView")]
    [SerializeField] private GameObject destinationCardPrefab;
    [SerializeField] private Transform contentParent;

    private Countries currentCountry = Countries.VietNam;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        UpdateMoneyUI();
        ShowDestinationsByCountry(currentCountry);
    }

    public void UpdateMoneyUI()
    {
        if (totalMoneyText != null && PlayerManager.Instance != null)
        {
            totalMoneyText.text = $"${PlayerManager.Instance.CurrentMoney:N0}";
        }
    }

    public void OnClickVietNam()
    {
        currentCountry = Countries.VietNam;
        ShowDestinationsByCountry(currentCountry);
    }

    public void OnClickKorea()
    {
        currentCountry = Countries.Korea;
        ShowDestinationsByCountry(currentCountry);
    }

    public void ShowDestinationsByCountry(Countries targetCountry)
    {
        if (contentParent == null || destinationCardPrefab == null) return;

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var dest in allDestinations)
        {
            if (dest != null && dest.country == targetCountry)
            {
                GameObject cardObj = Instantiate(destinationCardPrefab, contentParent);
                TakeInfo takeInfo = cardObj.GetComponent<TakeInfo>();
                if (takeInfo != null)
                {
                    takeInfo.SetDestination(dest);
                }
            }
        }
    }
}