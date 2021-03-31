using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Delirium.Events;
using Delirium.Lore;

namespace Delirium{
public class Compass : MonoBehaviour
{
    [SerializeField] private Transform southernIsland;
    [SerializeField] private Transform ferry;

    Vector3 NorthDirection;
    public Transform player;
    public Quaternion MissionDirection;

    private Transform MissionPlace;
    public Transform CompassImage;
    public Transform MissionPointer;

    private void Start()
    {
        EventCollection.Instance.LoreScrollFoundEvent.AddListener(OnLoreScrollFound);
        MissionPointer.gameObject.SetActive(false);
    }
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

    public void ChangeMissionPointer()
    {
        if (MissionPlace == null) { return; }

        Vector3 dir = transform.position - MissionPlace.position;

        MissionDirection = Quaternion.LookRotation(dir);

        MissionDirection.z = -MissionDirection.y;
        MissionDirection.x = 0;
        MissionDirection.y = 0;

        MissionPointer.localRotation = MissionDirection * Quaternion.Euler(NorthDirection);
    }
    
    private void OnLoreScrollFound(LoreScrollData data, Player player)
    {
        if (data.Number == 8)
        {
            MissionPointer.gameObject.SetActive(true);
            MissionPlace = southernIsland;
        }
        if (data.Number == 12)
        {
            ferry.gameObject.SetActive(true);
            MissionPlace = ferry;
        }
    }
}
}