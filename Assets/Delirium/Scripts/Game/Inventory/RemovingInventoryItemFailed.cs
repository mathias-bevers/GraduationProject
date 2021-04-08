using System;

namespace Delirium.Exceptions
{
	/// <summary>
	///     This Exception is thrown in the <see cref="Inventory" /> class, when a item could not be removed.
	///     <para>Made by: Mathias Bevers</para>
	/// </summary>
	public class RemovingInventoryItemFailed : Exception
	{
		/// <summary>
		///     TThis Exception is thrown in the <see cref="Inventory" /> class, when a item could not be removed.
		///     The exception is mostly used for in game notifications, the message is converted to the notification text.
		/// </summary>
		/// <param name="message">This will be displayed as a in game notification</param>
		public RemovingInventoryItemFailed(string message) : base(message) { }
	}
}