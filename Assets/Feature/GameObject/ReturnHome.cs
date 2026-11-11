using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnHome : MonoBehaviour
{
    [Header("Tên chính xác của Scene chính")]
    [SerializeField] private string mainSceneName = "PrototypeIsometricRoom";

    [Header("Nút bấm (Kéo Button vào đây hoặc gán sự kiện OnClick)")]
    [SerializeField] private Button returnButton;

    private void Start()
    {
        // Tự động gán sự kiện nếu bạn kéo Button vào ô Return Button trên Inspector
        if (returnButton != null)
        {
            returnButton.onClick.AddListener(LoadMainScene);
        }
    }

    // Hàm gọi chuyển cảnh (dùng để gán vào sự kiện OnClick() của Button)
    public void LoadMainScene()
    {
        if (!string.IsNullOrEmpty(mainSceneName))
        {
            SceneManager.LoadScene(mainSceneName);
        }
        else
        {
            Debug.LogWarning("[ReturnHome] Chưa nhập tên Scene chính trong Inspector!");
        }
    }
}