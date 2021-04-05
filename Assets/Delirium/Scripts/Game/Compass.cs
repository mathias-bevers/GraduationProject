using Delirium.Events;
using Delirium.Lore;
using UnityEngine;

namespace Delirium
{
	public class Compass : MonoBehaviour
	{
		[SerializeField] private Transform[] missionPlaces;

		public Transform player;
		public Transform CompassImage;
		public Transform MissionPointer;
		private Quaternion MissionDirection;

		private Transform MissionPlace;
		private Vector3 NorthDirection;

		private void Start()
		{
			LoreScrollManager.Instance.newScrollFoundEvent += OnNewScrollFound;
			MissionPlace = missionPlaces[0];
		}

		// Update is called once per frame
		private void Update()
		{
			ChangeCompass();
			ChangeMissionPointer();
		}

		private void OnNewScrollFound(int scrollNumber)
		{
			if (scrollNumber == 10) { return; }

			if (scrollNumber == 12) { missionPlaces[scrollNumber].gameObject.SetActive(true); }

			MissionPlace = missionPlaces[scrollNumber];
			EventCollection.Instance.OpenPopupEvent.Invoke("Mission waypoint updated", PopupMenu.PopupLevel.Info);
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
	}
}