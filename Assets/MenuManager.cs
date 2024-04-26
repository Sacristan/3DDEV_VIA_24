using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log(nameof(PlayGame));
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log(nameof(QuitGame));
        Application.Quit();
    }
}
