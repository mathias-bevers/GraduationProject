using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Delirium.Tools
{
	public class MenuManager : Singleton<MenuManager>
	{
		private readonly List<Menu> menus = new List<Menu>();

		public int OpenMenus => menus.Count(menu => menu.IsOpen);
		
		public void RegisterMenu(Menu menu)
		{
			if (menus.Contains(menu)) { return; }

			menus.Add(menu);
			Debug.Log($"Registered {menu.GetType().Name}");
		}

		public T OpenMenu<T>() where T : Menu
		{
			T[] menusOfTypeT = menus.OfType<T>().ToArray();

			if (menusOfTypeT.Length == 0) { return null; }

			foreach (T menuOfTypeT in menusOfTypeT)
			{
				if (menuOfTypeT.IsOpen) { continue; }

				menuOfTypeT.Open();
				return menuOfTypeT;
			}

			return null;
		}

		public T CloseMenu<T>() where T : Menu
		{
			T[] menusOfTypeT = menus.OfType<T>().ToArray();

			if (menusOfTypeT.Length == 0) { return null; }

			foreach (T menuOfTypeT in menusOfTypeT)
			{
				if (!menuOfTypeT.IsOpen) { continue; }

				menuOfTypeT.Close();
				return menuOfTypeT;
			}

			return null;
		}
	}
}