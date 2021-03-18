using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private Button startButton;
	[SerializeField] private Button quitButton;
	[SerializeField] private GameObject loadingBar;

	private void Start()
	{
		startButton.onClick.AddListener(() => StartCoroutine(LoadSceneAsync()));
		quitButton.onClick.AddListener(Application.Quit);
	}

	private IEnumerator LoadSceneAsync()
	{
		AsyncOperation loadingSceneOperation = SceneManager.LoadSceneAsync(1);

		loadingBar.SetActive(true);

		while (!loadingSceneOperation.isDone)
		{
			float progress = Mathf.Clamp01(loadingSceneOperation.progress / .9f);

			loadingBar.GetComponentInChildren<Image>().fillAmount = progress;

			yield return null;
		}
	}
}