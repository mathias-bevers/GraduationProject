using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private Button startButton;
	[SerializeField] private Button quitButton;
	[SerializeField] private Button splashContinueButton;
	[SerializeField] private GameObject loadingSplash;
	[SerializeField] private Image loadingBarFill;

	private void Start()
	{
		startButton.onClick.AddListener(() => StartCoroutine(LoadSceneAsync()));
		quitButton.onClick.AddListener(Application.Quit);
	}

	private IEnumerator LoadSceneAsync()
	{
		AsyncOperation async = SceneManager.LoadSceneAsync(1);
		async.allowSceneActivation = false;
		splashContinueButton.onClick.AddListener(() => async.allowSceneActivation = true);

		loadingSplash.SetActive(true);

		while (!async.isDone)
		{
			loadingBarFill.fillAmount = Mathf.Clamp01(async.progress / .9f);

			if (async.progress >= 0.9f) { splashContinueButton.gameObject.SetActive(true); }

			yield return null;
		}
	}
}