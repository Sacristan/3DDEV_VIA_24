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
        if (!isOpen && other.gameObject.CompareTag("Player"))
        {
            OpenDoor(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isOpen && other.gameObject.CompareTag("Player"))
        {
            OpenDoor(false);
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
