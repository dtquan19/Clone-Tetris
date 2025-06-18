using UnityEngine;
using UnityEngine.Tilemaps;

public class GridPainter : MonoBehaviour
{
    public Tilemap tilemap;       // Tham chiếu tới Tilemap
    public TileBase tile;         // Tile bạn muốn vẽ (bubble)
    public int width = 10;        // Số cột
    public int height = 20;       // Số dòng

    void Start()
    {
        PaintGrid();
    }

    void PaintGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }
}