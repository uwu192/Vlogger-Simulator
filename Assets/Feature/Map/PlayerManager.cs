using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public int CurrentMoney { get; private set; }
    private HashSet<string> unlockedDestinations = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Cộng tiền khi hoàn thành minigame
    public void AddMoney(int amount)
    {
        CurrentMoney += amount;
        SaveData();
    }

    // Trừ tiền khi mở khóa
    public bool UseMoney(int amount)
    {
        if (CurrentMoney >= amount)
        {
            CurrentMoney -= amount;
            SaveData();
            return true;
        }
        return false;
    }

    // Kiểm tra xem địa điểm đã được mở khóa chưa
    public bool IsDestinationUnlocked(string destName)
    {
        return unlockedDestinations.Contains(destName);
    }

    // Đánh dấu mở khóa địa điểm
    public void UnlockDestination(string destName)
    {
        if (!unlockedDestinations.Contains(destName))
        {
            unlockedDestinations.Add(destName);
            SaveData();
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("PlayerMoney", CurrentMoney);
        string dataStr = string.Join(",", unlockedDestinations);
        PlayerPrefs.SetString("UnlockedDestinations", dataStr);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        CurrentMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        unlockedDestinations.Clear();

        if (PlayerPrefs.HasKey("UnlockedDestinations"))
        {
            string data = PlayerPrefs.GetString("UnlockedDestinations");
            if (!string.IsNullOrEmpty(data))
            {
                string[] names = data.Split(',');
                foreach (string name in names)
                {
                    unlockedDestinations.Add(name);
                }
            }
        }
    }
}