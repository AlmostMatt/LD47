using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleMovement : MonoBehaviour
{
    private GameObject mCameraObj;
    private float mCurrentX = 0f;
    private float mLastStageX = 0f;
    private float mLastStageY = 0f;
    private const float VEL_X = 5f;
    private const float WAV_LEN = 6f;
    private const float WAV_HEIGHT = 3f;
    private Vector3 mStartPos;

    private const float BREACH_WAV_LEN = 12f;
    private const float BREACH_WAV_HEIGHT = 12f;

    private float mAscendOverTime = 0f;
    private float mStageTime = 0f;
    private int mPathStage = 0;

    private void Start()
    {
        mCameraObj = GameObject.FindObjectOfType<CameraMovement>().gameObject;
        mStartPos = transform.position;
    }

    void Update()
    {
        // Move in FixedUpdate because that's what other objects do (thanks to physics)
        Move(Time.fixedDeltaTime);
    }
    private void Move(float deltaTime)
    {
        float posX, posY, angle;
        mStageTime += deltaTime;

        if(mPathStage == 0)
        {
            // big arc to breach the clouds to start...
            mCurrentX += deltaTime * VEL_X;
            posX = mStartPos.x + mCurrentX;
            posY = mStartPos.y + BREACH_WAV_HEIGHT * Mathf.Sin(mCurrentX / BREACH_WAV_LEN);
            
            // Slope is dy dx
            // And y = WAV_HEIGHT * Mathf.Sin(mCurrentX / WAV_LEN);
            // so dy dx = WAV_HEIGHT * Mathf.Cos(mCurrentX / WAV_LEN) * 1f/WAV_LEN
            float dydx = BREACH_WAV_HEIGHT * Mathf.Cos(mCurrentX / BREACH_WAV_LEN) * 1f / BREACH_WAV_LEN;
            angle = Mathf.Atan2(dydx, 1) * Mathf.Rad2Deg;

            if(mStageTime >= 1.7f)
            {
                mLastStageX = posX; // mCurrentX;
                mCurrentX = 0;
                mLastStageY = posY;
                ++mPathStage;
            }
        }
        else
        {
            mAscendOverTime += deltaTime;
            mCurrentX += deltaTime * VEL_X;
            posX = mLastStageX + mCurrentX;
            posY = mLastStageY + WAV_HEIGHT * Mathf.Sin(mCurrentX / WAV_LEN) + mAscendOverTime;

            // Slope is dy dx
            // And y = WAV_HEIGHT * Mathf.Sin(mCurrentX / WAV_LEN);
            // so dy dx = WAV_HEIGHT * Mathf.Cos(mCurrentX / WAV_LEN) * 1f/WAV_LEN
            float dydx = WAV_HEIGHT * Mathf.Cos(mCurrentX / WAV_LEN) * 1f / WAV_LEN;
            angle = Mathf.Atan2(dydx, 1) * Mathf.Rad2Deg;
        }

         // Also, screen wrap if too far from the camera
        float fourSeasons = 4f * SeasonalSystem.SEASONAL_OFFSET;
        float dx = posX - mCameraObj.transform.position.x;
        while (dx > 2.5f * SeasonalSystem.SEASONAL_OFFSET)
        {
            dx -= fourSeasons;
            posX -= fourSeasons;
        }
        while (dx < -2.5f * SeasonalSystem.SEASONAL_OFFSET)
        {
            dx += fourSeasons;
            posX += fourSeasons;
        }

        transform.localPosition = new Vector3(posX, posY, transform.localPosition.z);
        transform.localEulerAngles = new Vector3(0f, 0f, angle);

    }
}
