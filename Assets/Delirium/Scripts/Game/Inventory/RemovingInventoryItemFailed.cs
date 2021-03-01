using System;

namespace Delirium.Exceptions
{
	public class RemovingInventoryItemFailed : Exception
	{
		public RemovingInventoryItemFailed(string message) : base(message) { }
	}
}