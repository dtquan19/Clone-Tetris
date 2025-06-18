using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); // hoặc tên scene bạn đang dùng
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit"); // sẽ hiển thị trong Editor
    }
}