using Delirium.Events;
using Delirium.Tools;

namespace Delirium
{
	public class LoreScrollManager : Singleton<LoreScrollManager>
	{
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
			if (!openedMenu.IsOpen) { return; }

			openedMenu.SetScrollText(foundLoreScroll);
			MenuManager.Instance.CloseMenu<GeneralHudMenu>();

			if (foundLoreScroll.Number <= highestFoundLoreScrollNumber) { return; }

			EventCollection.Instance.OpenPopupEvent.Invoke("You unlocked the next clue", PopupMenu.PopupLevel.Info);

			foreach (WorldLoreScroll worldLoreScroll in worldLoreScrolls)
			{
				if (worldLoreScroll.Data.Number != foundLoreScroll.Number + 1) { continue; }

				worldLoreScroll.gameObject.SetActive(true);
			}

			highestFoundLoreScrollNumber = foundLoreScroll.Number;

			switch (foundLoreScroll.Number)
			{
				case 1:
					invokingPlayer.Inventory.AddCraftingRecipe(ResourceManager.Instance.GetRecipeByResultName("Torch"));
					break;

				case 3:
					invokingPlayer.Inventory.AddCraftingRecipe(ResourceManager.Instance.GetRecipeByResultName("Datura Potion"));
					break;
				
				case 5: 
					//TODO: Add replacement for torch.
					break;
				case 7: 
					invokingPlayer.Inventory.AddCraftingRecipe(ResourceManager.Instance.GetRecipeByResultName("Spear"));
					break;
			}
		}
	}
}