using System;

namespace Delirium.Exceptions
{
	/// <summary>
	///     This Exception is thrown in the <see cref="Inventory" /> class, when an item could not be crafted.
	///     <para>Made by: Mathias Bevers</para>
	/// </summary>
	public class CraftingFailedException : Exception
	{
		/// <summary>
		///     This Exception is thrown in the <see cref="Inventory" /> class, when an item could not be crafted.
		///     The exception is mostly used for in game notifications, the message is converted to the notification text.
		/// </summary>
		/// <param name="message">This will be displayed as a in game notification</param>
		public CraftingFailedException(string message) : base(message) { }
	}
}