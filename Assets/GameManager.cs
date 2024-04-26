using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event System.Action OnGameWon;
    public event System.Action OnGameLost;

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
    }

    public void PlayerInHouse()
    {
        OnGameWon?.Invoke();
        Invoke(nameof(RestartGame), 5f);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
