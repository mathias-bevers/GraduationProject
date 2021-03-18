using System;

namespace Delirium.Exceptions
{
	public class AddingCraftingRecipeFailed : Exception
	{
		public AddingCraftingRecipeFailed(string message) : base(message) { }
	}
}