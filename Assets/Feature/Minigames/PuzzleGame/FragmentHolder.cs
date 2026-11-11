using System.Collections.Generic;
using UnityEngine;

public class FragmentHolder : MonoBehaviour
{
    public static FragmentHolder Instance { get; private set; }

    [Header("Cấu hình Kho Fragment")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject pickUpPrefab;

    [Header("Danh sách mảnh trong Inspector (Tạm thời)")]
    [SerializeField] private List<FragmentData> initialFragments = new List<FragmentData>();

    private List<PickUpFragment> activeFragments = new List<PickUpFragment>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Debug.Log("[FragmentHolder] Bắt đầu khởi tạo kho Fragment...");
        PopulateHolder();
    }

    private void PopulateHolder()
    {
        if (initialFragments == null || initialFragments.Count == 0)
        {
            Debug.LogWarning("[FragmentHolder] Danh sách 'initialFragments' trong Inspector đang TRỐNG! Hãy kéo Asset FragmentData vào.");
            return;
        }

        int createdCount = 0;
        foreach (var data in initialFragments)
        {
            if (data != null)
            {
                AddFragmentToHolder(data);
                createdCount++;
            }
            else
            {
                Debug.LogWarning("[FragmentHolder] Phát hiện 1 ô FragmentData bị Null trong danh sách Inspector!");
            }
        }

        Debug.Log($"[FragmentHolder] Khởi tạo xong! Đã tạo <color=green>{createdCount}</color> mảnh Fragment vào kho.");
    }

    public PickUpFragment AddFragmentToHolder(FragmentData data)
    {
        if (pickUpPrefab == null)
        {
            Debug.LogError("[FragmentHolder] Thiếu 'pickUpPrefab'! Hãy kéo Prefab PickUpFragment vào Inspector của FragmentHolder.");
            return null;
        }

        Transform parent = contentParent != null ? contentParent : transform;
        if (contentParent == null)
        {
            Debug.LogWarning("[FragmentHolder] Chưa gán 'contentParent'! Đang đặt tạm item làm con của FragmentHolder.");
        }

        GameObject obj = Instantiate(pickUpPrefab, parent);
        PickUpFragment item = obj.GetComponent<PickUpFragment>();
        if (item == null) item = obj.AddComponent<PickUpFragment>();

        item.SetupFragment(data);
        activeFragments.Add(item);

        Debug.Log($"[FragmentHolder] Đã thêm mảnh '{data.fragmentName}' vào Holder UI.");
        return item;
    }

    public void RemoveFromHolder(PickUpFragment fragment)
    {
        if (activeFragments.Contains(fragment))
        {
            activeFragments.Remove(fragment);
            Debug.Log($"[FragmentHolder] Đã xóa 1 mảnh khỏi kho UI.");
        }
    }
}