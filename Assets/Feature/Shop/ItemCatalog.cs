using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCatalog", menuName = "Scriptable Objects/ItemCatalog")]
public class ItemCatalog : ScriptableObject
{
    // Chứa cả ItemData (Decor) VÀ CameraItemData
    public List<ItemData> allItems;

    // Lọc vật phẩm dựa trên Class của nó (Sử dụng Generics)
    public List<T> GetItemsOfType<T>() where T : ItemData
    {
        List<T> filteredList = new List<T>();
        foreach (ItemData item in allItems)
        {
            // Kiểm tra xem item có phải là kiểu T (ví dụ CameraItemData) không
            if (item is T typedItem)
            {
                filteredList.Add(typedItem);
            }
        }
        return filteredList;
    }
}
