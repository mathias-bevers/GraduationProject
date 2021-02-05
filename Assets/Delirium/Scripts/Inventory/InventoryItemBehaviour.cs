using Delirium.Interfaces;
using UnityEngine;

namespace Delirium
{
	public class InventoryItemBehaviour : MonoBehaviour, IHighlightable
	{
		private static readonly int _outline = Shader.PropertyToID("_Outline");

		[SerializeField] private InventoryItemData data;
		public InventoryItemData Data => data;

		private Material material;

		private void Awake()
		{
			material = GetComponent<Renderer>().material; 
			material.SetFloat("_Shininess", 0.0f);
		}

		public void Highlight() { material.SetFloat(_outline, 0.075f); }

		public void EndHighlight() { material.SetFloat(_outline, 0.0f); }
	}
}