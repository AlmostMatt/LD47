using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private const float FADE_TIME = 1f;
    private float mFadeTimer = FADE_TIME;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mFadeTimer -= Time.deltaTime;
        float alpha = Mathf.Clamp(mFadeTimer / FADE_TIME, 0f, 1f);
        GetComponent<Image>().color = new Color(0f, 0f, 0f, alpha);
    }
}
