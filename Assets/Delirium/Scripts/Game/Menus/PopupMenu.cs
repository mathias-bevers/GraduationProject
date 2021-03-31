using System;
using Delirium.Events;
using Delirium.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupMenu : Menu
{
	public enum PopupLevel { Info, Waring, Error }

	[SerializeField] private GameObject popupPrefab;
	private Transform grid;

	private void Awake() { IsHUD = true; }

	protected override void Start()
	{
		EventCollection.Instance.OpenPopupEvent.AddListener(ShowPopup);

		grid = GetComponentInChildren<GridLayoutGroup>().transform;
		base.Start();
	}

	private void ShowPopup(string message, PopupLevel level)
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
			case PopupLevel.Error:
				textMeshProUGUI.color = Color.red;
				break;
			default: throw new ArgumentOutOfRangeException(nameof(level), level, null);
		}

		Destroy(popupGameObject, 5.0f);
	}

	public override bool CanBeOpened() => true;
	public override bool CanBeClosed() => true;
}