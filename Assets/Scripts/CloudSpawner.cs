using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    private float mBaseFrequency; 
    // Start is called before the first frame update
    void Start()
    {
        var main = GetComponent<ParticleSystem>().main;
        var emission = GetComponent<ParticleSystem>().emission;
        mBaseFrequency = emission.rateOverTime.constant;  // roughly 0.25 per second with duration of 20, 
    }

    // Update is called once per frame
    void Update()
    {
        // Set the emission frequency based on the altitude
        // Note that this is attached to the camera
        float minIntensity = 0.2f;
        float maxIntensity = 2f;
        // 0 clouds while player is below minCloudHeight
        // minIntensity clouds at minCloudHeight
        // 1 intensity clouds at maxCloudHeight
        float minCloudHeight = 5f;
        float maxCloudHeight = CloudLayer.CLOUD_HEIGHT; // the height at which intensity is maxed
        float intensity = 0f;
        if (transform.position.y > minCloudHeight)
        {
            intensity = minIntensity + (maxIntensity - minIntensity) * Mathf.Clamp((transform.position.y - minCloudHeight) / (maxCloudHeight - minCloudHeight), 0f, 1f);//
        }
        var emission = GetComponent<ParticleSystem>().emission;
        emission.rateOverTime = mBaseFrequency * intensity;

        // TODO: maybe have a bunch of these at different y values so clouds will pre-exist
    }
}
