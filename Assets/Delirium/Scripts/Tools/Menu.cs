using UnityEngine;

namespace Delirium.Tools
{
	public abstract class Menu : MonoBehaviour
	{
		public bool IsOpen { get; private set; }

		private Transform content;

		protected virtual void Start()
		{ 
			content = transform.GetChild(0);
			Close();

			MenuManager.Instance.RegisterMenu(this);
		}

		public abstract bool CanBeOpened();
		public abstract bool CanBeClosed();

		public virtual void Open()
		{
			if (!CanBeOpened())
			{
				Debug.LogWarning($"{GetType().Name} does not meet the opening criteria");
				return;
			}

			content.gameObject.SetActive(true);
			IsOpen = true;
		}

		public virtual void Close()
		{
			if (!CanBeClosed())
			{
				Debug.LogWarning($"{GetType().Name} does not meet the closing criteria");
				return;
			}

			content.gameObject.SetActive(false);
			IsOpen = false;
		}
	}
}