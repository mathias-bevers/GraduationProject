using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    Vector3 NorthDirection;
    public Transform player;
    public Quaternion MissionDirection;

    public Transform MissionPlace;
    public Transform CompassImage;
    public Transform MissionPointer;

    // Update is called once per frame
    void Update()
    {
        ChangeCompass();
        ChangeMissionPointer();
    }
    public void ChangeCompass()
    {
        NorthDirection.z = player.eulerAngles.y;
        CompassImage.localEulerAngles = NorthDirection;
    }

    public void MissionTrigger()
    {

    }

    public void ChangeMissionPointer()
    {
        Vector3 dir = transform.position - MissionPlace.position;

        MissionDirection = Quaternion.LookRotation(dir);

        MissionDirection.z = -MissionDirection.y;
        MissionDirection.x = 0;
        MissionDirection.y = 0;

        MissionPointer.localRotation = MissionDirection * Quaternion.Euler(NorthDirection);
    }
}
