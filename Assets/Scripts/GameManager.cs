using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Tetromino Settings")]
    public GameObject[] tetrominoPrefabs;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI highScoreText;
    private int highScore = 0;
    public Transform nextSpawnPoint; // Gắn vị trí hiển thị next
    private GameObject nextTetromino;

    [Header("Speed Settings")]
    public float baseFallTime = 0.8f;
    private int score = 0;
    private bool isGameOver = false;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
        SpawnRandomTetromino();
        UpdateScore(0); // Khởi tạo điểm
        gameOverPanel.SetActive(false);
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateScore(score);
    }

    void UpdateScore(int current)
    {
        scoreText.text = "Score: " + current;
        
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            highScoreText.text = "High Score: " + highScore;
        }
        
        // Tăng tốc theo mốc điểm (mỗi 1000 điểm giảm 0.2s)
        int milestone = score / 1000;
        float newFallTime = baseFallTime - (milestone * 0.2f);

        // Giới hạn tốc độ tối thiểu
        newFallTime = Mathf.Max(newFallTime, 0.1f);

        Tetromino.fallTimeGlobal = newFallTime;
    }

    public void SpawnRandomTetromino()
    {
        if (isGameOver) return;

        // Nếu chưa có nextTetromino thì tạo ngẫu nhiên
        if (nextTetromino == null)
        {
            int rand = Random.Range(0, tetrominoPrefabs.Length);
            nextTetromino = Instantiate(tetrominoPrefabs[rand], nextSpawnPoint.position, Quaternion.identity);
            nextTetromino.transform.localScale = Vector3.one * 0.7f;
            nextTetromino.GetComponent<Tetromino>().enabled = false;
        }

        // Kiểm tra xem nextTetromino có va chạm khi spawn không
        GameObject realTetromino = Instantiate(nextTetromino, new Vector3(5, 19, 0), Quaternion.identity);
        realTetromino.transform.localScale = Vector3.one;
        realTetromino.GetComponent<Tetromino>().enabled = true;

        foreach (Transform block in realTetromino.transform)
        {
            Vector2 pos = GridManager.Round(block.position);
            int x = (int)pos.x;
            int y = (int)pos.y;

            if (x >= 0 && x < GridManager.width && y >= 0 && y < GridManager.height)
            {
                if (GridManager.grid[x, y] != null)
                {
                    Destroy(realTetromino);
                    GameOver();
                    return;
                }
            }
        }

        // Tạo khối kế tiếp mới
        int nextRand = Random.Range(0, tetrominoPrefabs.Length);
        Destroy(nextTetromino);
        nextTetromino = Instantiate(tetrominoPrefabs[nextRand], nextSpawnPoint.position, Quaternion.identity);
        nextTetromino.transform.localScale = Vector3.one * 0.7f;
        nextTetromino.GetComponent<Tetromino>().enabled = false;
    }
    public void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Debug.Log("Game Over!");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
