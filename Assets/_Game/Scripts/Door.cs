using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform doorTransform;
    Vector3 openDoorRot = Vector3.up * -90;  //new Vector3(0, -90, 0); 
    Vector3 closedDoorRot = Vector3.zero; //new Vector3(0, 0, 0);

    bool isOpen = false;

    void OnTriggerEnter(Collider other)
    {
        HandleDoorIfPlayer(true, other);
    }

    void OnTriggerExit(Collider other)
    {
        HandleDoorIfPlayer(false, other);
    }

    void HandleDoorIfPlayer(bool openDoor, Collider other)
    {
        if (isOpen != openDoor && other.gameObject.CompareTag("Player"))
        {
            OpenDoor(openDoor);
        }
    }

    void OpenDoor(bool flag)
    {
        isOpen = flag;

        Vector3 rot;

        if (isOpen)
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
