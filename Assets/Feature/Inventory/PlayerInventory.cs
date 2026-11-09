using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory: MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public int Currency { get; private set; }
    public List<string> OwnedItems { get; private set; } = new List<string>();

    public event Action<int> OnCurrencyChanged;
    public event Action<ItemData> OnItemPurchased;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        LoadData();
    }

    public bool HasItem(string itemID)
    {
        return OwnedItems.Contains(itemID);
    }

    public void DeductCurrency(int amount)
    {
        Currency -= amount;
        SaveCurrency();
        OnCurrencyChanged?.Invoke(Currency);
    }

    public void AddItem(ItemData item)
    {
        if (!OwnedItems.Contains(item.itemID))
        {
            OwnedItems.Add(item.itemID);
            SaveItems();
            OnItemPurchased?.Invoke(item);
        }
    }

    private void LoadData()
    {
        Currency = PlayerPrefs.GetInt("PLAYER_CURRENCY", 1000);
        string itemsCSV = PlayerPrefs.GetString("PLAYER_ITEMS", "");
        if (!string.IsNullOrEmpty(itemsCSV))
        {
            OwnedItems = itemsCSV.Split(',').ToList();
        }
    }

    private void SaveCurrency() => PlayerPrefs.SetInt("PLAYER_CURRENCY", Currency);
    private void SaveItems() => PlayerPrefs.SetString("PLAYER_ITEMS", string.Join(",", OwnedItems));
}
