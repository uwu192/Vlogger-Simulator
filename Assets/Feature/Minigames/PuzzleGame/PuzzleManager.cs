using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

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
    }

    private void Update()
    {
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            int score = ConfirmAndCalculateScore();
            Debug.Log(score);
        }
    }
    private void InitGrid()
    {
        // Tự động tìm tất cả PuzzleSlot ở các GameObject con nếu danh sách đang trống
        if (slots == null || slots.Count == 0)
        {
            slots = new List<PuzzleSlot>(GetComponentsInChildren<PuzzleSlot>());
        }

        if (slots.Count < columns * rows)
        {
            Debug.LogError($"[PuzzleManager] Tìm thấy {slots.Count} ô, chưa đủ {columns * rows} ô (5x3)!");
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

        Debug.Log($"[PuzzleManager] Khởi tạo thành công ma trận {columns}x{rows}!");
    }

    public void Select(PickUpFragment fragment)
    {
        selectedFragment = fragment;
    }

    public void OnSlotClicked(PuzzleSlot slot)
    {
        if (selectedFragment != null)
        {
            if (slot.IsFilled) slot.Clear();

            slot.Fill(selectedFragment);
            Destroy(selectedFragment.gameObject);
            selectedFragment = null;
        }
        else if (slot.IsFilled)
        {
            slot.Clear();
            Debug.Log($"Đã tháo mảnh tại ô ({slot.GridX}, {slot.GridY})");
        }
    }

    // Nút xác nhận gọi hàm này
    public int ConfirmAndCalculateScore()
    {
        int totalScore = PuzzleScorer.CalculateTotalScore(grid, columns, rows);
        return totalScore;
    }
}