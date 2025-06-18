using UnityEngine;

public static class GridManager
{
    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height];

    public static Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    public static bool InsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0);
    }

    public static void AddToGrid(Transform tetromino)
    {
        foreach (Transform block in tetromino)
        {
            Vector2 pos = Round(block.position);

            int x = (int)pos.x;
            int y = (int)pos.y;

            // 🔒 Kiểm tra trước khi ghi vào grid để tránh lỗi
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                grid[x, y] = block;
            }
            else
            {
                Debug.LogWarning($"Block position ({x},{y}) is out of bounds!");
            }
        }
    }
    public static void CheckForLines()
    {
        for (int y = 0; y < height; ++y)
        {
            if (IsFullLine(y))
            {
                DeleteLine(y);
                MoveAllRowsDown(y + 1);
                GameManager.Instance.AddScore(100);
                y--; // Kiểm lại dòng mới sau khi dịch xuống
            }
        }
    }

    private static bool IsFullLine(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] == null)
                return false;
        }
        return true;
    }

    private static void DeleteLine(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            GameObject.Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    private static void MoveAllRowsDown(int startY)
    {
        for (int y = startY; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }

}