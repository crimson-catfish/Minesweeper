using UnityEngine;

public static class MapGenerator
{
    // -1 represents a mine, 0-8 represents adjacent mines count
    public static int[,] Generate(int width, int height, int minesCount, int startX, int startY)
    {
        int[,] grid = new int[width, height];

        int placedMines = 0;

        while (placedMines < minesCount)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            if (x >= startX - 1 && x <= startX + 1 || y >= startY - 1 && y <= startY + 1 || grid[x, y] == -1)
                continue;

            grid[x, y] = -1;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx < 0 || nx >= width || ny < 0 || ny >= height || grid[nx, ny] == -1)
                        continue;

                    grid[nx, ny]++;
                }
            }

            placedMines++;
        }

        return grid;
    }
}