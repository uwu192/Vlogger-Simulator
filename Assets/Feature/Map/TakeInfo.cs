using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TakeInfo : MonoBehaviour
{
    [Header("Dữ liệu Destination")]
    public DestinationInfo destination;

    [Header("Thành phần UI Hiển Thị")]
    [SerializeField] private Image displayImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI countryText;

    [Header("Nút Hành Động")]
    [SerializeField] private Button actionButton;            // Nút bấm trên thẻ
    [SerializeField] private TextMeshProUGUI buttonLabelText; // Chữ hiển thị trên nút ("Mở khóa $..." hoặc "Đi đến")

    private void Start()
    {
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(OnActionButtonClicked);
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (destination == null) return;

        if (displayImage != null) displayImage.sprite = destination.image;
        if (nameText != null) nameText.text = destination.destinationName;
        if (countryText != null) countryText.text = destination.country.ToString();

        RefreshButtonState();
    }

    public void RefreshButtonState()
    {
        if (destination == null || PlayerManager.Instance == null) return;

        bool isUnlocked = PlayerManager.Instance.IsDestinationUnlocked(destination.destinationName);

        if (isUnlocked)
        {
            if (buttonLabelText != null) buttonLabelText.text = "Đi đến";
        }
        else
        {
            if (buttonLabelText != null) buttonLabelText.text = $"Mở khóa (${destination.unlockCost})";
        }
    }

    private void OnActionButtonClicked()
    {
        if (destination == null || PlayerManager.Instance == null) return;

        bool isUnlocked = PlayerManager.Instance.IsDestinationUnlocked(destination.destinationName);

        if (isUnlocked)
        {
            // ĐÃ MỞ KHÓA -> CHUYỂN SCENE
            if (!string.IsNullOrEmpty(destination.sceneToLoad))
            {
                SceneManager.LoadScene(destination.sceneToLoad);
            }
            else
            {
                Debug.LogWarning($"[{destination.destinationName}] Chưa gán tên Scene trong ScriptableObject!");
            }
        }
        else
        {
            // CHƯA MỞ KHÓA -> TRỪ TIỀN VÀ MỞ KHÓA
            if (PlayerManager.Instance.UseMoney(destination.unlockCost))
            {
                PlayerManager.Instance.UnlockDestination(destination.destinationName);
                Debug.Log($"[TakeInfo] Đã mở khóa thành công {destination.destinationName}!");

                // Cập nhật lại UI tiền trên MapPanel và cập nhật trạng thái nút
                if (MapManager.Instance != null)
                {
                    MapManager.Instance.UpdateMoneyUI();
                }
                RefreshButtonState();
            }
            else
            {
                Debug.LogWarning("[TakeInfo] Bạn không đủ tiền để mở khóa địa điểm này!");
            }
        }
    }

    public void SetDestination(DestinationInfo newDestination)
    {
        destination = newDestination;
        UpdateUI();
    }
}