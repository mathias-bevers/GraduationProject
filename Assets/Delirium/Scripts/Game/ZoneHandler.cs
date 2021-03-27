﻿using System;
using System.Collections;
using Delirium.Events;
using UnityEngine;

namespace Delirium
{
	public class ZoneHandler : MonoBehaviour
	{
		private const float INTERACTION_COOLDOWN = 0.5f;

		private bool canInteract = true;
		private InteractionZone interactionZone;
		private Inventory playerInventory;
		private Transform interactableObject;

		private void Awake() { playerInventory = GetComponent<Player>().Inventory; }

		private void Update()
		{
			if (Input.GetAxis("Interact") <= 0 || !canInteract) { return; }

			switch (interactionZone)
			{
				case InteractionZone.None: break;
				case InteractionZone.Campfire:
					StartCoroutine(InteractionCooldown());

					if (interactableObject.GetChild(0).gameObject.activeInHierarchy)
					{
						EventCollection.Instance.OpenPopupEvent.Invoke("This campfire is already lit", PopupMenu.PopupLevel.Waring);
						return;
					}

					if (playerInventory.GetItemValueByName("Torch") > 0)
					{
						foreach (Transform child in interactableObject.transform) { child.gameObject.SetActive(true); }
					}
					else if (playerInventory.GetItemValueByName("Flint") > 0)
					{
						foreach (Transform child in interactableObject.transform) { child.gameObject.SetActive(true); }
						
						playerInventory.RemoveItems(playerInventory.GetItemKeyByName("Flint"));
					}
					else { EventCollection.Instance.OpenPopupEvent.Invoke("You need flint or a torch to light a campfire", PopupMenu.PopupLevel.Error); }

					break;

				case InteractionZone.Ritual:
					StartCoroutine(InteractionCooldown());

					if (playerInventory.GetItemValueByName("Skull") < 3 || playerInventory.GetItemValueByName("Tongue") < 1 || LoreScrollManager.Instance.ScrollsFound < 9)
					{
						EventCollection.Instance.OpenPopupEvent.Invoke("You don't have all the required times for the ritual", PopupMenu.PopupLevel.Error);
						return;
					}
					//TODO: perform ritual.
					
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Campfire"))
			{
				interactionZone = InteractionZone.Campfire;
				interactableObject = other.transform;
				EventCollection.Instance.EnteredInteractionZoneEvent.Invoke(interactionZone);
			}

			if (!other.CompareTag("Altar")) { return; }

			interactionZone = InteractionZone.Ritual;
			interactableObject = other.transform;
			EventCollection.Instance.EnteredInteractionZoneEvent.Invoke(interactionZone);
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.CompareTag("Campfire") && !other.CompareTag("Altar")) { return; }

			interactionZone = InteractionZone.None;
			interactableObject = null;
			EventCollection.Instance.DisableInteractTextEvent.Invoke();
		}

		private IEnumerator InteractionCooldown()
		{
			canInteract = false;
			yield return new WaitForSeconds(INTERACTION_COOLDOWN);
			canInteract = true;
		}

		public enum InteractionZone { None, Campfire, Ritual }
	}
}