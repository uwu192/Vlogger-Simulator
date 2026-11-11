using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        // Hãy chắc chắn nhập đúng tên Scene gameplay của bạn vào đây
        SceneManager.LoadScene("Room");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game đã thoát!"); // Chỉ hiện trên Editor
    }
}