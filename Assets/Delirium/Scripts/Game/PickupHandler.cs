﻿using System;
using Delirium.AbstractClasses;
using Delirium.Exceptions;
using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	[RequireComponent(typeof(Player))]
	public class PickupHandler : MonoBehaviour
	{
		[SerializeField] private float pickupReach = 1.5f;
		private Pickupable highlightedObject;

		private Player player;
		private Transform cameraTransform;

		public void Start()
		{
			player = GetComponent<Player>();
			cameraTransform = GetComponentInChildren<Camera>().transform;
		}

		private void Update()
		{
			if (MenuManager.Instance.IsAnyOpen) { return; }

			RaycastCheck();
		}

		private void RaycastCheck()
		{
			if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, pickupReach))
			{
				if (highlightedObject == null) { return; }

				highlightedObject.InReach = false;
				highlightedObject = null;
				return;
			}

			var pickupable = hit.transform.GetComponent<Pickupable>();

			if (pickupable == null)
			{
				if (highlightedObject == null) { return; }

				highlightedObject.InReach = false;
				highlightedObject = null;
				return;
			}

			highlightedObject = pickupable;
			highlightedObject.InReach = true;

			if (Input.GetAxis("Interact") <= 0) { return; }

			AddToInventory(highlightedObject);
		}

		private void AddToInventory(Pickupable pickupable)
		{
			switch (pickupable)
			{
				case InventoryWorldItem inventoryItem:
					try
					{
						player.Inventory.AddItem(inventoryItem.Data);
						Destroy(inventoryItem.gameObject);
						highlightedObject = null;
					}
					catch (AddingInventoryItemFailed exception) { MenuManager.Instance.GetMenu<PopupMenu>()?.ShowPopup(exception.Message, PopupMenu.PopupLevel.Waring); }

					break;

				case WorldCraftingRecipe craftingRecipe:
					try { player.Inventory.AddCraftingRecipe(craftingRecipe.Data); }
					catch (AddingCraftingRecipeFailed exception) { MenuManager.Instance.GetMenu<PopupMenu>()?.ShowPopup(exception.Message, PopupMenu.PopupLevel.Waring); }

					Destroy(craftingRecipe.gameObject);
					highlightedObject = null;
					break;

				default: throw new NotSupportedException($"{pickupable.GetType().FullName} does not inherit from {typeof(Pickupable).FullName}");
			}
		}
	}
}