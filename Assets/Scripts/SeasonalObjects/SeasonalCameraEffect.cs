using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonalCameraEffect : MonoBehaviour
{
    private Season mSeason = Season.SUMMER;

    // Update is called once per frame
    void Update()
    {
        float xOffsetForMinIntensity = -2f;
        float xOffsetForMaxIntensity = 3f;
        float xOffsetDiff = xOffsetForMaxIntensity - xOffsetForMinIntensity;
        float camPos = transform.position.x;
        float seasonX1 = SeasonalSystem.GetSingleton().GetSeasonX1(mSeason);
        float seasonX2 = SeasonalSystem.GetSingleton().GetSeasonX2(mSeason);
        float intensityRelativeToX1 = (Mathf.Clamp(camPos - seasonX1, xOffsetForMinIntensity, xOffsetForMaxIntensity) - xOffsetForMinIntensity) / xOffsetDiff;
        float intensityRelativeToX2 = (Mathf.Clamp(seasonX2 - camPos, xOffsetForMinIntensity, xOffsetForMaxIntensity) - xOffsetForMinIntensity) / xOffsetDiff;
        float intensity = Mathf.Min(intensityRelativeToX1, intensityRelativeToX2);

        //GetComponent<ParticleSystem>().main..startColor = new Color(1f, 1f, 1f, 0.45f * intensity);
        //var main = GetComponent<ParticleSystem>().main;
        //main.startColor = new Color(1f, 1f, 1f, 0.45f * intensity);

        var colOverTime = GetComponent<ParticleSystem>().colorOverLifetime;
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] {
            new GradientColorKey(Color.white, 0.0f),
            new GradientColorKey(Color.white, 1.0f) },
        new GradientAlphaKey[] {
            new GradientAlphaKey(0.0f, 0.0f),
            new GradientAlphaKey(intensity, 0.12f),
            new GradientAlphaKey(intensity, 0.88f),
            new GradientAlphaKey(0.0f, 1.0f)
        });
        colOverTime.color = grad;
    }
}
