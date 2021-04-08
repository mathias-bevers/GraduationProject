using UnityEngine;

namespace Delirium.Lore
{
	/// <summary>
	///     This scriptable object contains all the information of a lore scroll, its story and number.
	/// </summary>
	[CreateAssetMenu(fileName = "Lore scroll", menuName = "Delirium/Lore scroll", order = 0)]
	public class LoreScrollData : ScriptableObject
	{
		[SerializeField, Range(1, 12)] private int number = 1;
		[SerializeField, TextArea(10, 100)] private string text;

		/// <summary>
		///     Get the text that is written o the scroll, which is set in the inspector.
		/// </summary>
		public string Text => text;

		/// <summary>
		///     Get the Number of the lore scroll, which is set in the editor.
		/// </summary>
		public int Number => number;
	}
}