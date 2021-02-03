using System;

namespace Delirium.Exceptions
{
	public class AddingItemFailed : Exception
	{
		public AddingItemFailed(string message) : base(message) { }
	}
}