using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void TryPurchaseItem(ItemData item)
    {
        if (PlayerInventory.Instance.HasItem(item.itemID))
        {
            Debug.LogWarning($"Bạn đã sở hữu vật phẩm: {item.itemName}");
            return;
        }

        if (PlayerInventory.Instance.Currency < item.price)
        {
            Debug.LogWarning($"Không đủ tiền mua: {item.itemName}");
            return;
        }

        PlayerInventory.Instance.DeductCurrency(item.price);
        PlayerInventory.Instance.AddItem(item);

        Debug.Log($"Giao dịch thành công: {item.itemName}!");
    }
}
