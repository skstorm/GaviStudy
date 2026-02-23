using UnityEngine;

public class Board : MonoBehaviour
{
    public static readonly int Width = 10;
    public static readonly int Height = 20;

    private Transform[,] grid = new Transform[Width, Height];

    public bool IsValidPosition(Tetromino piece)
    {
        foreach (Transform block in piece.transform)
        {
            Vector2Int pos = WorldToGrid(block.position);

            if (pos.x < 0 || pos.x >= Width) return false;
            if (pos.y < 0 || pos.y >= Height) return false;

            if (grid[pos.x, pos.y] != null) return false;
        }
        return true;
    }

    public void PlacePiece(Tetromino piece)
    {
        foreach (Transform block in piece.transform)
        {
            Vector2Int pos = WorldToGrid(block.position);
            if (pos.y >= 0 && pos.y < Height && pos.x >= 0 && pos.x < Width)
                grid[pos.x, pos.y] = block;
        }
    }

    public int ClearLines()
    {
        int linesCleared = 0;
        for (int y = 0; y < Height; y++)
        {
            if (IsLineFull(y))
            {
                DeleteLine(y);
                MoveAllRowsDown(y);
                y--;
                linesCleared++;
            }
        }
        return linesCleared;
    }

    private bool IsLineFull(int y)
    {
        for (int x = 0; x < Width; x++)
            if (grid[x, y] == null) return false;
        return true;
    }

    private void DeleteLine(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }

    private void MoveAllRowsDown(int clearedY)
    {
        for (int y = clearedY; y < Height - 1; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (grid[x, y + 1] != null)
                {
                    grid[x, y] = grid[x, y + 1];
                    grid[x, y + 1] = null;
                    grid[x, y].position += Vector3.down;
                }
            }
        }
    }

    // 월드 좌표 → 보드 그리드 인덱스 변환 (Board 오브젝트 위치 기준 로컬 좌표)
    private Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 local = worldPos - transform.position;
        return new Vector2Int(Mathf.RoundToInt(local.x), Mathf.RoundToInt(local.y));
    }
}
