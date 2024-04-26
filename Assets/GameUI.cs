using System.Collections;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    const string GameStartText = "Avoid <b><color=red>Wolves</color></b>\nGet safe <b><color=green>Home</color></b>";
    const string GameWonText = "Congratz! You Survived!";
    const string GameLostText = "You are eaten!";

    [SerializeField] TMPro.TextMeshProUGUI notificationText;
    [SerializeField] TMPro.TextMeshProUGUI healthText;

    IEnumerator Start()
    {
        GameManager.Instance.OnGameWon += GameWon;
        GameManager.Instance.OnGameLost += GameLost;
        GameManager.Instance.OnPlayerHealthChanged += PlayerHealthChanged;

        notificationText.text = GameStartText;
        yield return new WaitForSeconds(3f);
        notificationText.text = string.Empty;
    }

    void GameWon()
    {
        notificationText.text = GameWonText;
    }

    void GameLost()
    {
        notificationText.text = GameLostText;
    }

    void PlayerHealthChanged(float health)
    {
        healthText.text = $"Health: {health}";
    }
}
