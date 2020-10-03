using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SeasonalSystem : MonoBehaviour
{
    public const float SEASONAL_OFFSET = 12f;
    private Dictionary<Season,List<Seasonal>> mObjectsBySeason = new Dictionary<Season, List<Seasonal>>();

    void Start()
    {
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
        // move seasonal copies to the left or right as the player/camera moves
    }
}
