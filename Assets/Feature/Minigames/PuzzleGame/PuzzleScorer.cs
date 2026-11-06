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
        int adjacentCatalysts = CountAdjacentCatalysts(grid, slot.GridX, slot.GridY, columns, rows);

        switch (data.rarity)
        {
            case FragmentRarity.Common:
                return data.GetBaseScore(); // 10 điểm

            case FragmentRarity.Rare:
                return (adjacentCatalysts >= 1) ? data.GetBaseScore() : 5;

            case FragmentRarity.Epic:
                bool isCorner = IsCornerSlot(slot.GridX, slot.GridY, columns, rows);
                int requiredCatalysts = isCorner ? 2 : 3;
                return (adjacentCatalysts >= requiredCatalysts) ? data.GetBaseScore() : 10;

            default:
                return 0;
        }
    }

    private static int CountAdjacentCatalysts(PuzzleSlot[,] grid, int x, int y, int columns, int rows)
    {
        int count = 0;
        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (nx >= 0 && nx < columns && ny >= 0 && ny < rows)
            {
                PuzzleSlot neighbor = grid[nx, ny];
                if (neighbor != null && neighbor.IsFilled && neighbor.IsPlacedCatalyst)
                {
                    count++;
                }
            }
        }
        return count;
    }

    private static bool IsCornerSlot(int x, int y, int columns, int rows)
    {
        return (x == 0 || x == columns - 1) && (y == 0 || y == rows - 1);
    }
}