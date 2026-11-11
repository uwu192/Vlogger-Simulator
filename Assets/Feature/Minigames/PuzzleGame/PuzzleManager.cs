using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }
    [SerializeField] Button SubmitBtt;
    [Header("Cấu hình Ma trận")]
    public int columns = 5;
    public int rows = 3;
    [SerializeField] private List<PuzzleSlot> slots = new List<PuzzleSlot>();

    private PuzzleSlot[,] grid;
    private PickUpFragment selectedFragment;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitGrid();
        SubmitBtt.onClick.AddListener(OnSubmitPressed);
    }

    private void OnSubmitPressed() 
    {
        FinishMinigame();
    }

    private void InitGrid()
    {
        if (slots == null || slots.Count == 0)
        {
            slots = new List<PuzzleSlot>(GetComponentsInChildren<PuzzleSlot>());
        }

        if (slots.Count < columns * rows)
        {
            Debug.LogError($"[PuzzleManager] Tìm thấy {slots.Count} ô, CHƯA ĐỦ {columns * rows} ô cho ma trận (5x3)!");
            return;
        }

        grid = new PuzzleSlot[columns, rows];

        for (int i = 0; i < slots.Count; i++)
        {
            int x = i % columns;
            int y = i / columns;
            slots[i].InitCoords(x, y);
            grid[x, y] = slots[i];
        }
    }

    public void Select(PickUpFragment fragment)
    {
        selectedFragment = fragment;
    }

    public void OnSlotClicked(PuzzleSlot slot)
    {
        if (selectedFragment != null)
        {
            if (slot.IsFilled)
            {
                ReturnSlotItemToHolder(slot);
            }

            slot.Fill(selectedFragment);

            if (selectedFragment.IsCatalyst)
            {
                CatalystHolder.Instance.ConsumeCatalyst(selectedFragment.catalystData.catalystType);
            }
            else
            {
                FragmentHolder.Instance.RemoveFromHolder(selectedFragment);
                Destroy(selectedFragment.gameObject);
            }

            selectedFragment = null;
        }
        else if (slot.IsFilled)
        {
            ReturnSlotItemToHolder(slot);
            slot.Clear();
        }
    }

    private void ReturnSlotItemToHolder(PuzzleSlot slot)
    {
        if (slot.IsPlacedCatalyst)
        {
            CatalystHolder.Instance.AddCatalystToHolder(slot.PlacedCatalystData);
        }
        else if (slot.PlacedFragmentData != null)
        {
            FragmentHolder.Instance.AddFragmentToHolder(slot.PlacedFragmentData);
        }
    }

    // Kiểm tra xem tất cả các ô trên ma trận đã được lấp đầy chưa
    public bool IsGridFullyFilled()
    {
        int filledCount = 0;
        int totalRequired = columns * rows; // 5 x 3 = 15 ô

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (grid[x, y] != null && grid[x, y].IsFilled)
                {
                    filledCount++;
                }
            }
        }

        return filledCount == totalRequired;
    }

    // Nút HOÀN THÀNH (Nút Upload) gọi hàm này
    public void FinishMinigame()
    {
        if (!IsGridFullyFilled()) return;

        int totalViews = PuzzleScorer.CalculateTotalScore(grid, columns, rows);
        int earnedMoney = totalViews / 50;

        // CỘNG TIỀN CHO NGƯỜI CHƠI
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.AddMoney(earnedMoney);
        }

        ClearAllGridSlots();

        if (PuzzleResultUI.Instance != null)
        {
            PuzzleResultUI.Instance.DisplayResults(totalViews, earnedMoney);
        }
    }

    // Xóa tất cả mảnh trên ma trận
    private void ClearAllGridSlots()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (grid[x, y] != null && grid[x, y].IsFilled)
                {
                    grid[x, y].Clear();
                }
            }
        }
        selectedFragment = null;
        Debug.Log("[PuzzleManager] Đã dọn sạch toàn bộ ma trận 5x3!");
    }
}