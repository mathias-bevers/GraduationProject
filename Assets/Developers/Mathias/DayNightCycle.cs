using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
	[SerializeField] private Light sun;
	[SerializeField] private Light moon;
	[SerializeField] private float dayDurationInSeconds = 60f;

	public float TimeMultiplier { get; set; } = 1.0f;

	private float currentTimeOfDay;
	private float sunInitialIntensity;

	private void Start() { sunInitialIntensity = sun.intensity; }

	private void Update()
	{
		currentTimeOfDay += Time.deltaTime / dayDurationInSeconds * TimeMultiplier;
		if (currentTimeOfDay >= 1) { currentTimeOfDay = 0; }

		UpdateSun();
		moon.intensity = currentTimeOfDay <= 0.25f || currentTimeOfDay >= 0.72f ? 0.5f : 0.0f;
	}

	private void UpdateSun()
	{
		sun.transform.localRotation = Quaternion.Euler(currentTimeOfDay * 360f - 90, 90, 0);

		var intensityPercent = 1.0f;
		if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f) { intensityPercent = 0.0f; }
		else if (currentTimeOfDay <= 0.25f) { intensityPercent = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f)); }
		else if (currentTimeOfDay >= 0.73f) { intensityPercent = Mathf.Clamp01(1 - (currentTimeOfDay - 0.73f) * (1 / 0.02f)); }

		sun.intensity = sunInitialIntensity * intensityPercent;
	}
}