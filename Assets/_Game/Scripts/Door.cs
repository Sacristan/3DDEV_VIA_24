using UnityEngine;

public class Door : PlayerTrigger
{
    [SerializeField] Transform doorTransform;
    Vector3 openDoorRot = Vector3.up * -90;  //new Vector3(0, -90, 0); 
    Vector3 closedDoorRot = Vector3.zero; //new Vector3(0, 0, 0);

    protected override void UpdatePlayerInZone(bool result)
    {
        base.UpdatePlayerInZone(result);
        OpenDoor();
    }

    void OpenDoor()
    {
        Vector3 rot;

        if (isPlayerInTrigger)
        {
            rot = openDoorRot;
        }
        else
        {
            rot = closedDoorRot;
        }

        doorTransform.localRotation = Quaternion.Euler(rot);
    }
}
