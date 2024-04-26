using System.Collections;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    const string GameStartText = "Avoid <b><color=red>Wolves</color></b>\nGet safe <b><color=green>Home</color></b>";
    const string GameWonText = "Congratz! You Survived!";
    const string GameLoseText = "You are eaten!";

    [SerializeField] TMPro.TextMeshProUGUI notificationText;

    IEnumerator Start()
    {
        notificationText.text = GameStartText;
        yield return new WaitForSeconds(3f);
        notificationText.text = string.Empty;
    }

}
