using System;
using System.Linq;
using Delirium.AI;
using Delirium.Audio;
using Delirium.Events;
using Delirium.Tools;
using UnityEngine;

namespace Delirium.Lore
{
	public class LoreScrollManager : Singleton<LoreScrollManager>
	{
		public int ScrollsFound { get; private set; }

		private int highestFoundLoreScrollNumber;
		private WorldLoreScroll[] worldLoreScrolls;

		private void Start()
		{
			EventCollection.Instance.LoreScrollFoundEvent.AddListener(OnLoreScrollFound);

			worldLoreScrolls = FindObjectsOfType<WorldLoreScroll>();

			foreach (WorldLoreScroll worldLoreScroll in worldLoreScrolls)
			{
				if (worldLoreScroll.Data.Number == 1) { continue; }

				worldLoreScroll.gameObject.SetActive(false);
			}
		}

		public event Action<int> newScrollFoundEvent;

		private void OnLoreScrollFound(LoreScrollData foundLoreScroll, Player invokingPlayer)
		{
			var openedMenu = MenuManager.Instance.OpenMenu<LoreScrollMenu>();

			openedMenu.SetScrollText(foundLoreScroll);

			MenuManager.Instance.CloseMenu<PlayerHUDMenu>();

			if (foundLoreScroll.Number <= highestFoundLoreScrollNumber) { return; }

			if (foundLoreScroll.Number != 12) { EventCollection.Instance.OpenPopupEvent.Invoke("You unlocked the next clue", PopupMenu.PopupLevel.Info); }

			newScrollFoundEvent?.Invoke(foundLoreScroll.Number);

			foreach (WorldLoreScroll worldLoreScroll in worldLoreScrolls)
			{
				if (worldLoreScroll.Data.Number != foundLoreScroll.Number + 1) { continue; }

				worldLoreScroll.gameObject.SetActive(true);
			}

			highestFoundLoreScrollNumber = foundLoreScroll.Number;
			ScrollsFound++;

			Transform noteTransform = null;
			if (foundLoreScroll.Number < 11) { noteTransform = worldLoreScrolls.ToList().Find(worldLoreScroll => worldLoreScroll.Data.Number == foundLoreScroll.Number).transform; }


			switch (foundLoreScroll.Number)
			{
				case 1:
					invokingPlayer.Inventory.AddCraftingRecipe(ResourceManager.Instance.GetRecipeByResultName("Torch"));
					break;

				case 3:
					invokingPlayer.Inventory.AddCraftingRecipe(ResourceManager.Instance.GetRecipeByResultName("Datura Potion"));
					break;

				case 7:
					invokingPlayer.Inventory.AddCraftingRecipe(ResourceManager.Instance.GetRecipeByResultName("Spear"));
					break;

				case 8:

					//Enable the altar trigger, so the you can perform the ritual.
					void EnableAltarTrigger()
					{
						noteTransform.parent.GetComponent<Collider>().enabled = true;
						openedMenu.Closed -= EnableAltarTrigger;
					}

					openedMenu.Closed += EnableAltarTrigger;
					break;

				case 10:
					//Enable the collider for the ritual dagger so you can collect the tongue item.
					Transform ritualDagger = noteTransform.parent.Find("Ritual Dagger");
					ritualDagger.GetComponent<Collider>().enabled = true;
					ritualDagger.GetComponent<ParticleSystem>().Play();
					break;

				case 12:
					openedMenu.Closed += () =>
					{
						EnemyManager.Instance.SpawnEnemyHorde(invokingPlayer.transform);
						AudioManager.Instance.Play("Enemy_Chase");
						AudioManager.Instance.Play("Jumpscare_02");
					};
					break;
			}
		}
	}
}