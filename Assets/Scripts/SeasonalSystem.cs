using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SeasonalSystem : MonoBehaviour
{
    public const float SEASONAL_OFFSET = 12f;
    private Dictionary<Season,List<Seasonal>> mObjectsBySeason = new Dictionary<Season, List<Seasonal>>();
    private GameObject mCameraObj;

    void Start()
    {
        mCameraObj = GameObject.FindObjectOfType<CameraMovement>().gameObject;

        // Find all seasonal objects in the scene
        Seasonal[] seasonalObjs = (Seasonal[])GameObject.FindObjectsOfType(typeof(Seasonal));
        // Create a copy of each seasonal object for each season
        foreach (Season season in Enum.GetValues(typeof(Season)))
        {
            mObjectsBySeason.Add(season, new List<Seasonal>());
            foreach (Seasonal seasonal in seasonalObjs)
            {
                Seasonal seasonalObj = seasonal;
                if (season != Season.SPRING)
                {
                    seasonalObj = GameObject.Instantiate(seasonal.gameObject).GetComponent<Seasonal>();
                }
                seasonalObj.Season = season;
                Vector3 pos = seasonalObj.transform.localPosition;
                seasonalObj.transform.localPosition = new Vector3(pos.x + (SEASONAL_OFFSET*(int)season), pos.y, pos.z);
                mObjectsBySeason[season].Add(seasonalObj);
            }
        }
    }
    
    void Update()
    {
        // TODO: see if there are any issues with moving static objects
        if (mCameraObj != null)
        {
            float totalWidth = 4 * SEASONAL_OFFSET;
            foreach (List<Seasonal> seasonalObjList in mObjectsBySeason.Values)
            {
                foreach (Seasonal seasonalObj in seasonalObjList)
                {
                    Vector3 pos = seasonalObj.transform.localPosition;
                    float objDistX = pos.x - mCameraObj.transform.localPosition.x;
                    if (objDistX > totalWidth / 2f)
                    {
                        seasonalObj.transform.localPosition = new Vector3(pos.x - totalWidth, pos.y, pos.z);
                    }
                    if (objDistX < -totalWidth / 2f)
                    {
                        seasonalObj.transform.localPosition = new Vector3(pos.x + totalWidth, pos.y, pos.z);
                    }
                }
            }
        }
    }
}
