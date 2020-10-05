using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private const float FADE_IN_TIME = 1f;
    private const float FADE_OUT_TIME = 3f;
    private float mFadeInTimer = FADE_IN_TIME;
    private bool readyToFadeOut = false;
    private float mFadeOutDelay;
    private float mFadeOutTimer; 

    // Start is called before the first frame update
    void Start()
    {
        SetColor(1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!readyToFadeOut)
        {
            mFadeInTimer -= Time.deltaTime;
            float alpha = Mathf.Clamp(mFadeInTimer / FADE_IN_TIME, 0f, 1f);
            SetColor(alpha);
        } else if (mFadeOutDelay > 0f)
        {
            mFadeOutDelay -= Time.deltaTime;
        }
        else
        {
            mFadeOutTimer -= Time.deltaTime;
            float alpha = 1f - Mathf.Clamp(mFadeOutTimer / FADE_OUT_TIME, 0f, 1f);
            SetColor(alpha);
        }
    }

    void SetColor(float alpha)
    {
        GetComponent<Image>().color = new Color(0f, 0f, 0f, alpha);
    }

    public void StartFadeOut() // called by CameraMovement stopfollowingplayer
    {
        readyToFadeOut = true;
        // BallonMovement has a VEL of around 3
        // and camera has a half-height of around 3
        // after that the whale flies by
        mFadeOutDelay = 8f;
        mFadeOutTimer = FADE_OUT_TIME;
    }
}
