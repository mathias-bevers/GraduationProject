using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraShake : MonoBehaviour
{
    
    public PostProcessProfile volume;
    public float power = 0.5f;
    public float duration = 1.0f;
    public Transform camera;
    public float slowDownAmount = 1.0f;
    public bool shouldShake = false;

    Vector3 startPosition;
    float initialDuration;

    private Vignette vignetteEffect;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        startPosition = camera.localPosition;
        initialDuration = duration;
        volume.TryGetSettings(out vignetteEffect);
        vignetteEffect.intensity.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldShake)
        {
            if (duration > 0)
            {
                //vignetteEffect.intensity.value = Mathf.Lerp(vignetteEffect.intensity.value, 15, .05f * Time.deltaTime);
                camera.localPosition = startPosition + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount;
            }
            else
            {
                shouldShake = false;
                duration = initialDuration;
                camera.localPosition = startPosition;
            }
        }
    }
}
