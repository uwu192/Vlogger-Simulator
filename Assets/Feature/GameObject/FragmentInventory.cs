using System.Collections.Generic;
using UnityEngine;

public class FragmentInventory : MonoBehaviour
{
    private static FragmentInventory _instance;

    public static FragmentInventory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<FragmentInventory>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("FragmentInventory (Auto Generated)");
                    _instance = go.AddComponent<FragmentInventory>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    [Header("Danh sách Asset Data Fragment trong game")]
    // Kéo tất cả các file FragmentData (Common, Rare, Epic...) từ Project vào danh sách này
    [SerializeField] private List<FragmentData> allAvailableFragments = new List<FragmentData>();

    private List<FragmentData> collectedFragments = new List<FragmentData>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoadInventory(); // Tự động load dữ liệu cũ khi game vừa chạy
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddFragment(FragmentData data)
    {
        if (data != null)
        {
            collectedFragments.Add(data);
            Debug.Log($"[FragmentInventory] Đã thêm mảnh: {data.fragmentName}");

            SaveInventory(); // Lưu ngay lập tức mỗi khi nhặt được mảnh mới
        }
    }

    public List<FragmentData> GetCollectedFragments()
    {
        return new List<FragmentData>(collectedFragments);
    }

    public void ClearInventory()
    {
        collectedFragments.Clear();
        PlayerPrefs.DeleteKey("SavedFragments"); // Xóa file lưu khi reset game
        PlayerPrefs.Save();
    }

    // ================= LƯU VÀ LOAD DỮ LIỆU VĨNH VIỄN =================

    // 1. Hàm Lưu dữ liệu xuống ổ cứng
    public void SaveInventory()
    {
        List<string> fragmentNames = new List<string>();
        foreach (var frag in collectedFragments)
        {
            if (frag != null)
            {
                fragmentNames.Add(frag.name); // Lưu tên của ScriptableObject
            }
        }

        // Chuyển danh sách tên thành chuỗi phân cách bởi dấu phẩy (VD: "CommonFrag,EpicFrag,RareFrag")
        string dataToSave = string.Join(",", fragmentNames);
        PlayerPrefs.SetString("SavedFragments", dataToSave);
        PlayerPrefs.Save();

        Debug.Log($"[FragmentInventory] Đã lưu dữ liệu vĩnh viễn: {dataToSave}");
    }

    // 2. Hàm Tải dữ liệu từ ổ cứng lên
    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("SavedFragments")) return;

        collectedFragments.Clear();
        string savedData = PlayerPrefs.GetString("SavedFragments");

        if (string.IsNullOrEmpty(savedData)) return;

        string[] names = savedData.Split(',');

        foreach (string fragName in names)
        {
            // Tìm ScriptableObject tương ứng trong danh sách allAvailableFragments
            FragmentData match = allAvailableFragments.Find(f => f != null && f.name == fragName);
            if (match != null)
            {
                collectedFragments.Add(match);
            }
        }

        Debug.Log($"[FragmentInventory] Đã nạp lại {collectedFragments.Count} mảnh từ bộ nhớ máy!");
    }
}