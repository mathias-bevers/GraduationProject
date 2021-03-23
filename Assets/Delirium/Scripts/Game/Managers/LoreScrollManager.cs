using Delirium.Events;
using Delirium.Tools;
using UnityEngine;

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
				if (worldLoreScroll.Data.Number == 1) { continue;}
				worldLoreScroll.gameObject.SetActive(false);
			}
		}

		private void OnLoreScrollFound(LoreScrollData foundLoreScroll)
		{
			var openedMenu = MenuManager.Instance.OpenMenu<LoreScrollMenu>();
			if (!openedMenu.IsOpen) { return; }

			openedMenu.SetScrollText(foundLoreScroll);
			MenuManager.Instance.CloseMenu<GeneralHudMenu>();

			if (foundLoreScroll.Number <= highestFoundLoreScrollNumber) { return; }

			foreach (WorldLoreScroll worldLoreScroll in worldLoreScrolls)
			{
				if (worldLoreScroll.Data.Number != foundLoreScroll.Number + 1) { continue; }

				worldLoreScroll.gameObject.SetActive(true);
			}

			highestFoundLoreScrollNumber = foundLoreScroll.Number;
		}
	}
}