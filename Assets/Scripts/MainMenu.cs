using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadLevel1(string sceneName)
    {
        SceneManager.LoadScene("EnemyAI");
    }

    public void LoadCredits(string sceneName)
    {
        SceneManager.LoadScene("Credits");
    }
    public void LoadMainMenu(string sceneName)
    {
        SceneManager.LoadScene("MainMenu");
    }



    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}


