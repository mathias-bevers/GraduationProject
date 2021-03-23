using UnityEngine;

namespace Delirium
{
	[CreateAssetMenu(fileName = "Lore scroll", menuName = "Delirium/Lore scroll", order = 0)]
	public class LoreScrollData : ScriptableObject
	{
		[SerializeField, Range(1, 12)] private int number = 1;
		[SerializeField, TextArea] private string text;

		public string Text => text;
		public int Number => number;
	}
}