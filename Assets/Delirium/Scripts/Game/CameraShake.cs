using Delirium;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraShake : MonoBehaviour
{
	[SerializeField] private float fadeSpeed;
	[SerializeField] private float vignetteScale;

	public float power;
	public Transform cameraTransform;
	private Player parentPlayer;
	private PostProcessingProfile postProcessingProfile;
	private VignetteModel.Settings vignetteSettings;
	private Vector3 startPosition;
	private VignetteModel vignetteEffect;

	// Start is called before the first frame update
	private void Start()
	{
		postProcessingProfile = GetComponent<PostProcessingBehaviour>().profile;
		parentPlayer = SearchForParentPlayer();

		cameraTransform = Camera.main.transform;
		startPosition = cameraTransform.localPosition;

		vignetteEffect = postProcessingProfile.vignette;
		postProcessingProfile.vignette.enabled = true;

		vignetteSettings = vignetteEffect.settings;
		vignetteSettings.intensity = 0f;
		vignetteEffect.settings = vignetteSettings;
	}

	// Update is called once per frame
	private void Update()
	{
		/*if (!shouldShake) { return; }

		if (duration > 0)
		{
			VignetteModel.Settings vignetteEffectSettings = vignetteEffect.settings;
			vignetteEffectSettings.intensity = Mathf.Lerp(vignetteEffectSettings.intensity, 15, .05f * Time.deltaTime);
			vignetteEffect.settings = vignetteEffectSettings;
			
			cameraTransform.localPosition = startPosition + Random.insideUnitSphere * power;
			duration -= Time.deltaTime * slowDownAmount;
		}
		else
		{
			shouldShake = false;
			duration = initialDuration;
			cameraTransform.localPosition = startPosition;
		}*/

		if (parentPlayer.Sanity.CurrentSanity >= 20)
		{
			vignetteSettings.intensity = Mathf.Lerp(vignetteEffect.settings.intensity, 0.0f, fadeSpeed * 2 * Time.deltaTime);
			vignetteEffect.settings = vignetteSettings;

			cameraTransform.localPosition = startPosition;
			return;
		}


		vignetteSettings.intensity = Mathf.Lerp(vignetteSettings.intensity, vignetteScale, fadeSpeed * Time.deltaTime);
		vignetteEffect.settings = vignetteSettings;

		cameraTransform.localPosition = startPosition + Random.insideUnitSphere * power;
	}

	private Player SearchForParentPlayer()
	{
		Transform t = transform;

		while (t.parent != null)
		{
			var player = t.parent.GetComponent<Player>();

			if (player != null) { return player; }

			t = t.parent;
		}

		Debug.LogError("Could not find parent player");
		return null;
	}
}