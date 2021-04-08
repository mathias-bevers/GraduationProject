using System;

namespace Delirium.Exceptions
{
	/// <summary>
	///     This Exception is thrown in the <see cref="Inventory" /> class, when the item could not be added to the Inventory.
	///     <para>Made by: Mathias Bevers</para>
	/// </summary>
	public class AddingInventoryItemFailed : Exception
	{
		/// <summary>
		///     This Exception is thrown in the <see cref="Inventory" /> class, when the item could not be added to the Inventory.
		///     The exception is mostly used for in game notifications, the message is converted to the notification text.
		/// </summary>
		/// <param name="message">This will be displayed as a in game notification</param>
		public AddingInventoryItemFailed(string message) : base(message) { }
	}
}