using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleMovement : MonoBehaviour
{
    private GameObject mCameraObj;
    private float mCurrentX = 0f;
    private const float VEL_X = 5f;
    private const float WAV_LEN = 6f;
    private const float WAV_HEIGHT = 3f;
    private Vector3 mStartPos;

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
        mCurrentX += deltaTime * VEL_X;
        float posX = mStartPos.x + mCurrentX;
        float posY = mStartPos.y + WAV_HEIGHT * Mathf.Sin(mCurrentX / WAV_LEN);
        // Also, screen wrap if too far from the camera
        float fourSeasons = 4 * SeasonalSystem.SEASONAL_OFFSET;
        float dx = posX - mCameraObj.transform.position.x;
        while (dx > fourSeasons)
        {
            dx -= fourSeasons;
            posX -= fourSeasons;
        }
        while (dx < -fourSeasons)
        {
            dx += fourSeasons;
            posX += fourSeasons;
        }
        transform.localPosition = new Vector3(posX, posY, transform.localPosition.z);

        // Slope is dy dx
        // And y = WAV_HEIGHT * Mathf.Sin(mCurrentX / WAV_LEN);
        // so dy dx = WAV_HEIGHT * Mathf.Cos(mCurrentX / WAV_LEN) * 1f/WAV_LEN
        float dydx = WAV_HEIGHT * Mathf.Cos(mCurrentX / WAV_LEN) * 1f / WAV_LEN;
        float angle = Mathf.Atan2(dydx, 1) * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(0f, 0f, angle);
    }
}
