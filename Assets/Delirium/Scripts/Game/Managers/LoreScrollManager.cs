using System.Linq;
using Delirium.AI;
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

		private void OnLoreScrollFound(LoreScrollData foundLoreScroll, Player invokingPlayer)
		{
			var openedMenu = MenuManager.Instance.OpenMenu<LoreScrollMenu>();

			openedMenu.SetScrollText(foundLoreScroll);

			MenuManager.Instance.CloseMenu<PlayerHUDMenu>();

			if (foundLoreScroll.Number <= highestFoundLoreScrollNumber) { return; }

			EventCollection.Instance.OpenPopupEvent.Invoke("You unlocked the next clue", PopupMenu.PopupLevel.Info);

			foreach (WorldLoreScroll worldLoreScroll in worldLoreScrolls)
			{
				if (worldLoreScroll.Data.Number != foundLoreScroll.Number + 1) { continue; }

				worldLoreScroll.gameObject.SetActive(true);
			}

			highestFoundLoreScrollNumber = foundLoreScroll.Number;
			ScrollsFound++;

			if (foundLoreScroll.Number > 10) { return; }

			Transform noteTransform = worldLoreScrolls.ToList().Find(worldLoreScroll => worldLoreScroll.Data.Number == foundLoreScroll.Number).transform;

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
					noteTransform.parent.GetComponent<Collider>().enabled = true;
					break;
				
				case 10:
					noteTransform.parent.Find("Ritual Dagger").GetComponent<Collider>().enabled = true;
					break;
			}
		}
	}
}