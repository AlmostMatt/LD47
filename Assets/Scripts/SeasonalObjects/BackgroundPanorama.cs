using UnityEngine;
using System.Collections;

public class BackgroundPanorama : MonoBehaviour
{
    private float mBackgroundWidth;
    private GameObject mCameraObj;

    // Use this for initialization
    void Start()
    {
        mCameraObj = GameObject.FindObjectOfType<CameraMovement>().gameObject;
        Bounds bounds = GetComponent<SpriteRenderer>().bounds;
        mBackgroundWidth = bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        Season season = GetComponent<Seasonal>().Season;
        /**
         * S = width of a season
         * P = the camera's position relative to the season's start
         * B = the width of the background image (in game units)
         * Offset1 = -B*P/S  (left side of the image relative to the left camera's position)
         * Offset2 = B/2 -B*P/S  (center of the image relative to the left camera's position)
         */
        float S = SeasonalSystem.SEASONAL_OFFSET;
        float P = mCameraObj.transform.localPosition.x - SeasonalSystem.GetSingleton().GetSeasonX1(season);
        float B = mBackgroundWidth;
        float offset1 = -B * P / S;
        float offset = (B/2f) -B * P / S;
        Vector3 pos = transform.localPosition;
        // copy the y of the camera
        transform.localPosition = new Vector3(mCameraObj.transform.localPosition.x + offset, mCameraObj.transform.localPosition.y, pos.z);
    }
}
