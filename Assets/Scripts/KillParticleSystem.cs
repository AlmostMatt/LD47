using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillParticleSystem : MonoBehaviour
{
    ParticleSystem mSystem;
    bool mWasAlive = false;

    // Start is called before the first frame update
    void Start()
    {
        mSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!mSystem.IsAlive() && mWasAlive)
        {
            Destroy(gameObject);
        }
        mWasAlive = mSystem.IsAlive();
    }
}
