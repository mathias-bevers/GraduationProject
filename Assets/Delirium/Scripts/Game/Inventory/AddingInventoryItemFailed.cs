using System;

namespace Delirium.Exceptions
{
	public class AddingInventoryItemFailed : Exception
	{
		public AddingInventoryItemFailed(string message) : base(message) { }
	}
}