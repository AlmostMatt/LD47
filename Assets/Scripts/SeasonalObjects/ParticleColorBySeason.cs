using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColorBySeason : MonoBehaviour
{
    public Gradient[] ParticleColorGradientBySeason = new Gradient[4];

    // Start is called before the first frame update
    void Start()
    {
        Season season = GetComponent<Seasonal>().Season;
        var main = GetComponent<ParticleSystem>().main;
        main.startColor = ParticleColorGradientBySeason[(int)season];
    }
}
