using UnityEngine.UI;
using Delirium.Tools;
using UnityEngine;
using System;
using TMPro;

public class PopupMenu : Menu
{
	public enum PopupLevel { Info, Waring }

	[SerializeField] private GameObject popupPrefab;
	private Transform grid;

	protected override void Start()
	{
		IsHUD = true;
		grid = GetComponentInChildren<GridLayoutGroup>().transform;
		base.Start();
	}

	public void ShowPopup(string message, PopupLevel level)
	{
		GameObject popupGameObject = Instantiate(popupPrefab, grid);

		var textMeshProUGUI = popupGameObject.GetComponentInChildren<TextMeshProUGUI>();
		textMeshProUGUI.SetText(message);
		switch (level)
		{
			case PopupLevel.Info:
				textMeshProUGUI.color = Color.white;
				break;
			case PopupLevel.Waring: 
				textMeshProUGUI.color = Color.yellow;
				break;
			default: throw new ArgumentOutOfRangeException(nameof(level), level, null);
		}
		
		Destroy(popupGameObject, 5.0f);
	}

	public override bool CanBeOpened() => true;
	public override bool CanBeClosed() => true;
}