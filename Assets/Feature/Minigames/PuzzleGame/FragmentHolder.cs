using System.Collections.Generic;
using UnityEngine;

public class FragmentHolder : MonoBehaviour
{
    public static FragmentHolder Instance { get; private set; }

    [Header("Cấu hình Kho Fragment")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject pickUpPrefab;

    private List<PickUpFragment> activeFragments = new List<PickUpFragment>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Load lại mảnh mỗi khi UI Holder này được bật lên
    private void OnEnable()
    {
        RefreshHolderFromInventory();
    }

    // Đọc danh sách từ PlayerInventory và tạo UI Prefab tương ứng
    public void RefreshHolderFromInventory()
    {
        // 1. Xóa các UI Item cũ nếu có
        foreach (var item in activeFragments)
        {
            if (item != null) Destroy(item.gameObject);
        }
        activeFragments.Clear();

        // 2. Lấy danh sách mảnh từ PlayerInventory
        if (PlayerInventory.Instance == null)
        {
            Debug.LogWarning("[FragmentHolder] Chưa tìm thấy PlayerInventory Instance trong Scene!");
            return;
        }

        List<FragmentData> collectedList = FragmentInventory.Instance.GetCollectedFragments();

        // 3. Tạo UI cho từng mảnh thu thập được
        foreach (var data in collectedList)
        {
            AddFragmentToHolder(data);
        }

        Debug.Log($"[FragmentHolder] Đã nạp thành công <color=green>{activeFragments.Count}</color> mảnh từ Inventory lên UI!");
    }

    public PickUpFragment AddFragmentToHolder(FragmentData data)
    {
        if (data == null || pickUpPrefab == null) return null;

        Transform parent = contentParent != null ? contentParent : transform;
        GameObject obj = Instantiate(pickUpPrefab, parent);

        PickUpFragment item = obj.GetComponent<PickUpFragment>();
        if (item == null) item = obj.AddComponent<PickUpFragment>();

        item.SetupFragment(data);
        activeFragments.Add(item);
        return item;
    }

    public void RemoveFromHolder(PickUpFragment fragment)
    {
        if (activeFragments.Contains(fragment))
        {
            activeFragments.Remove(fragment);
        }
    }
}