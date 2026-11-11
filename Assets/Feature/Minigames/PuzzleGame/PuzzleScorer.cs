using UnityEngine;

public static class PuzzleScorer
{
    public static int CalculateTotalScore(PuzzleSlot[,] grid, int columns, int rows)
    {
        int totalScore = 0;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                PuzzleSlot slot = grid[x, y];

                // Bỏ qua các ô trống hoặc ô đang chứa mảnh phụ trợ (Catalyst không có điểm gốc)
                if (slot == null || !slot.IsFilled || slot.IsPlacedCatalyst || slot.PlacedFragmentData == null)
                    continue;

                totalScore += CalculateSlotScore(grid, slot, columns, rows);
            }
        }

        return totalScore;
    }

    private static int CalculateSlotScore(PuzzleSlot[,] grid, PuzzleSlot slot, int columns, int rows)
    {
        FragmentData data = slot.PlacedFragmentData;

        int adjacentCatalystsCount = 0;
        int cameraCatalystCount = 0;
        bool hasSoundCatalyst = false;

        // Tọa độ 4 hướng lân cận: Trái, Phải, Dưới, Trên
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        // 1. Kiểm tra 4 ô xung quanh
        for (int i = 0; i < 4; i++)
        {
            int nx = slot.GridX + dx[i];
            int ny = slot.GridY + dy[i];

            if (nx >= 0 && nx < columns && ny >= 0 && ny < rows)
            {
                PuzzleSlot neighbor = grid[nx, ny];
                if (neighbor != null && neighbor.IsFilled && neighbor.IsPlacedCatalyst && neighbor.PlacedCatalystData != null)
                {
                    adjacentCatalystsCount++;

                    if (neighbor.PlacedCatalystData.catalystType == CatalystType.Sound)
                    {
                        hasSoundCatalyst = true;
                    }
                    else if (neighbor.PlacedCatalystData.catalystType == CatalystType.Camera)
                    {
                        cameraCatalystCount++;
                    }
                }
            }
        }

        // 2. Tính điểm dựa trên độ hiếm và điều kiện
        int slotScore = 0;

        switch (data.rarity)
        {
            case FragmentRarity.Common:
                slotScore = data.GetBaseScore(); // Mặc định 10
                break;

            case FragmentRarity.Rare:
                // Thỏa mãn nếu có Sound Catalyst HOẶC có ít nhất 1 Catalyst bất kỳ
                bool rareConditionMet = hasSoundCatalyst || (adjacentCatalystsCount >= 1);
                slotScore = rareConditionMet ? data.GetBaseScore() : 5; // 30 hoặc 5

                // Cộng bonus từ Camera Catalyst
                slotScore += (cameraCatalystCount * 3);
                break;

            case FragmentRarity.Epic:
                // Thỏa mãn nếu có Sound Catalyst HOẶC có ít nhất 2 Catalyst bất kỳ
                bool epicConditionMet = hasSoundCatalyst || (adjacentCatalystsCount >= 2);
                slotScore = epicConditionMet ? data.GetBaseScore() : 10; // 100 hoặc 10

                // Cộng bonus từ Camera Catalyst
                slotScore += (cameraCatalystCount * 3);
                break;
        }

        return slotScore;
    }
}