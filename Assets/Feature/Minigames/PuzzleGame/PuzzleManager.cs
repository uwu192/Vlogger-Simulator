using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    private PickUpFragment selectedFragment;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Select(PickUpFragment fragment)
    {
        selectedFragment = fragment;
    }

    public void TryPlace(PuzzleSlot slot)
    {
        if (selectedFragment == null || slot.IsFilled) return;

        slot.Fill(selectedFragment);
        Destroy(selectedFragment.gameObject); // Xóa mảnh cũ ở khay
        selectedFragment = null;
    }
}