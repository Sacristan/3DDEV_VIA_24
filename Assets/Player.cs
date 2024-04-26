using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const float MaxHealth = 100f;
    float health = MaxHealth;
    bool isDead = false;

    IEnumerator Start()
    {
        yield return null;
        GameManager.Instance.PlayerHealthChanged(health);
    }

    public void AddDamage(float damage)
    {
        health -= damage;
        GameManager.Instance.PlayerHealthChanged(health);
        if (!isDead && health <= 0) Die();
    }

    void Die()
    {
        isDead = true;
        GameManager.Instance.PlayerDied();
    }
}