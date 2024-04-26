using UnityEngine;

public class HouseTrigger : PlayerTrigger
{
    protected override void UpdatePlayerInZone(bool result)
    {
        base.UpdatePlayerInZone(result);
        HandleMusic();

        if (isPlayerInTrigger)
        {
            GameManager.Instance.PlayerInHouse();
        }
    }

    void HandleMusic()
    {
        MusicManager.MusicState musicState;

        if (isPlayerInTrigger) musicState = MusicManager.MusicState.Shelter;
        else musicState = MusicManager.MusicState.OutsideAmbient;

        MusicManager.instance.PlayMusicState(musicState);
    }
}
