using System;
using System.Linq;
using Delirium.AI;
using Delirium.Audio;
using Delirium.Events;
using Delirium.Tools;
using UnityEngine;

namespace Delirium.Lore
{
	/// <summary>
	///     This class is used to keep track of all the loreScrolls. And handles when a lore scroll is found.
	///     <para>Made by: Mathias Bevers</para>
	/// </summary>
	public class LoreScrollManager : Singleton<LoreScrollManager>
	{
		/// <summary>
		///     Get the amount of found lore scrolls.
		/// </summary>
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

		/// <summary>
		///     This event is invoked in the <see cref="OnLoreScrollFound" /> method, when the picked up lore scroll is the latest unlocked scroll.
		/// </summary>
		public event Action<int> NewScrollFoundEvent;

		/// <summary>
		///     Handle the actions that should be taken when a loreScroll is found, when the found lore scroll meets all the criteria to be new invoke <see cref="NewScrollFoundEvent" />.
		/// </summary>
		/// <param name="foundLoreScroll">The data of the found lore scroll</param>
		/// <param name="invokingPlayer">The player that has found the lore scroll.</param>
		private void OnLoreScrollFound(LoreScrollData foundLoreScroll, Player invokingPlayer)
		{
			LoreScrollMenu openedMenu = null;

			if (foundLoreScroll.Number != 11)
			{
				openedMenu = MenuManager.Instance.OpenMenu<LoreScrollMenu>();

				openedMenu.SetScrollText(foundLoreScroll);

				MenuManager.Instance.CloseMenu<PlayerHUDMenu>();
			}

			if (foundLoreScroll.Number <= highestFoundLoreScrollNumber) { return; }

			if (foundLoreScroll.Number != 12) { EventCollection.Instance.OpenPopupEvent.Invoke("You unlocked the next clue", PopupMenu.PopupLevel.Info); }

			NewScrollFoundEvent?.Invoke(foundLoreScroll.Number);

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