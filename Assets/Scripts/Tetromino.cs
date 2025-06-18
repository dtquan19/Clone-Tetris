using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public static float fallTimeGlobal = 1f;
    private float previousTime;
    private GameManager gameManager;

    void Start()
    {
        // Tìm 1 lần duy nhất lúc khởi tạo, tránh gọi FindObjectOfType nhiều lần
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Time.time - previousTime > fallTimeGlobal)
        {
            MoveDown();
            previousTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Move(Vector3.left);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            Move(Vector3.right);
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Rotate();
        if (Input.GetKeyDown(KeyCode.DownArrow))
            MoveDown();
    }

    void MoveDown()
    {
        transform.position += Vector3.down;

        if (!IsValidPosition())
        {
            transform.position += Vector3.up;
            GridManager.AddToGrid(transform);
            GridManager.CheckForLines();
            // Dùng biến đã lưu, không gọi FindObjectOfType nữa
            if (gameManager != null)
            {
                gameManager.SpawnRandomTetromino();
            }
            else
            {
                Debug.LogWarning("GameManager not found!");
            }

            enabled = false;
        }
    }

    void Move(Vector3 direction)
    {
        transform.position += direction;
        if (!IsValidPosition()) transform.position -= direction;
    }

    void Rotate()
    {
        transform.Rotate(0, 0, -90);
        if (!IsValidPosition()) transform.Rotate(0, 0, 90);
    }

    bool IsValidPosition()
    {
        foreach (Transform block in transform)
        {
            Vector2 pos = GridManager.Round(block.position);
            if (!GridManager.InsideBorder(pos)) return false;

            if (GridManager.grid[(int)pos.x, (int)pos.y] != null)
                return false;
        }
        return true;
    }
}