using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    const string GameStartText = "Avoid <b><color=red>Wolves</color></b>\nGet safe <b><color=green>Home</color></b>";
    const string GameWonText = "Congratz! You Survived!";
    const string GameLostText = "You are eaten!";

    [SerializeField] TMPro.TextMeshProUGUI notificationText;
    [SerializeField] TMPro.TextMeshProUGUI healthText;
    [SerializeField] RectTransform loseContainer;
    [SerializeField] Image damageImage;
    [SerializeField] Color damageIndicationColor = new Color(255, 0, 0, 100);
    [SerializeField] float fadeOutDamageEffectTime = 1.5f;

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
        loseContainer.gameObject.SetActive(true);
    }

    void PlayerHealthChanged(float health)
    {
        healthText.text = $"Health: {health}";
        DamageEffect();
    }

    void DamageEffect()
    {
        if (fadeDamageRoutine != null) StopCoroutine(fadeDamageRoutine);
        fadeDamageRoutine = StartCoroutine(FadeDamageRoutine());
    }

    Coroutine fadeDamageRoutine = null;
    IEnumerator FadeDamageRoutine()
    {
        damageImage.color = damageIndicationColor;

        Color resultColor = damageIndicationColor;
        resultColor.a = 0;

        float t = 0;

        while (t <= 1f)
        {
            t += Time.deltaTime / fadeOutDamageEffectTime;
            damageImage.color = Color.Lerp(damageIndicationColor, resultColor, t);
            yield return null;
        }

        fadeDamageRoutine = null;
    }
}
