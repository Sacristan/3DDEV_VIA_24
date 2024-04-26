using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event System.Action OnGameWon;
    public event System.Action OnGameLost;
    public event System.Action OnPlayerDied;
    public event System.Action<float> OnPlayerHealthChanged;

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    Player _player;
    public Player Player => _player;

    void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
    }

    public void PlayerInHouse()
    {
        OnGameWon?.Invoke();
        Invoke(nameof(RestartGame), 5f);
    }

    public void PlayerDied()
    {
        OnGameLost?.Invoke();
        Invoke(nameof(RestartGame), 3f);
    }

    public void PlayerHealthChanged(float health)
    {
        OnPlayerHealthChanged?.Invoke(health);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
