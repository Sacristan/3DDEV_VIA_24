using UnityEngine;

public abstract class PlayerTrigger : MonoBehaviour
{
    protected bool isPlayerInTrigger = false;

    void OnTriggerEnter(Collider other)
    {
        HandleIfPlayer(true, other);
    }

    void OnTriggerExit(Collider other)
    {
        HandleIfPlayer(false, other);
    }

    void HandleIfPlayer(bool isPlayerInZone, Collider other)
    {
        if (isPlayerInTrigger != isPlayerInZone && other.gameObject.CompareTag("Player"))
        {
            UpdatePlayerInZone(isPlayerInZone);
        }
    }

    protected virtual void UpdatePlayerInZone(bool result)
    {
        isPlayerInTrigger = result;
    }

}
