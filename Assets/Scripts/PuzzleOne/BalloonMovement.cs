using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMovement : MonoBehaviour
{
    private bool mStarted = false;
    private float mCurrentT = 0f;
    private Vector3 mStartPos;
    private const float VEL = 3f;
    private const float WAV_LEN = 2f;
    private const float WAV_HEIGHT = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mStarted)
        {
            // Copied from whale movement.
            mCurrentT += Time.fixedDeltaTime * VEL;

            float posY = mStartPos.y + mCurrentT;
            float posX = mStartPos.x + WAV_HEIGHT * Mathf.Sin(mCurrentT / WAV_LEN);

            // Slope is dy dx
            // And y = WAV_HEIGHT * Mathf.Sin(mCurrentX / WAV_LEN);
            // so dy dx = WAV_HEIGHT * Mathf.Cos(mCurrentX / WAV_LEN) * 1f/WAV_LEN
            float dydx = WAV_HEIGHT * Mathf.Cos(mCurrentT/ WAV_LEN) * 1f / WAV_LEN;
            float angle = (0f - (Mathf.Atan2(dydx, 1) * Mathf.Rad2Deg));

            transform.localPosition = new Vector3(posX, posY, transform.localPosition.z);
            //transform.localEulerAngles = new Vector3(0f, 0f, angle);
        }
    }

    public void StartMoving()
    {
        mStartPos = transform.position;
        mStarted = true;
        transform.SetParent(null, true); // detach from parent
    }
}
