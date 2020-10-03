using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

public class SeasonalSystem : MonoBehaviour
{
    public const float SEASONAL_OFFSET = 12f;
    private Dictionary<Season, List<Seasonal>> mRootObjectsBySeason = new Dictionary<Season, List<Seasonal>>();
    private Dictionary<Season, List<Seasonal>> mObjectsBySeason = new Dictionary<Season, List<Seasonal>>();
    private Dictionary<Seasonal, List<Seasonal>> mObjectToSeasonalVariants = new Dictionary<Seasonal, List<Seasonal>>();
    private GameObject mCameraObj;

    void Start()
    {
        mCameraObj = GameObject.FindObjectOfType<CameraMovement>().gameObject;

        // Find all seasonal objects in the scene
        Seasonal[] seasonalObjs = (Seasonal[])GameObject.FindObjectsOfType(typeof(Seasonal));
        foreach (Season season in Enum.GetValues(typeof(Season)))
        {
            mRootObjectsBySeason.Add(season, new List<Seasonal>());
            mObjectsBySeason.Add(season, new List<Seasonal>());
        }
        // Create a copy of each (root) seasonal object for each season
        foreach (Seasonal seasonal in seasonalObjs)
        {
            // Only copy root objects
            if (seasonal.transform.parent != null)
            {
                continue;
            }
            List<Seasonal> seasonalVariants = new List<Seasonal>();
            foreach (Season season in Enum.GetValues(typeof(Season))) {
                Seasonal seasonalObj = seasonal;
                if (season != Season.SPRING)
                {
                    seasonalObj = GameObject.Instantiate(seasonal.gameObject).GetComponent<Seasonal>();
                }
                seasonalObj.Season = season;
                Vector3 pos = seasonalObj.transform.position;
                seasonalObj.transform.position = new Vector3(pos.x + (SEASONAL_OFFSET*(int)season), pos.y, pos.z);
                mRootObjectsBySeason[season].Add(seasonalObj);
                mObjectsBySeason[season].Add(seasonalObj);
                seasonalVariants.Add(seasonalObj);
            }
            Dictionary<Season, Seasonal[]> seasonalChildrenPerVariant = new Dictionary<Season, Seasonal[]>();
            foreach (Seasonal seasonalObj in seasonalVariants)
            {
                mObjectToSeasonalVariants.Add(seasonalObj, seasonalVariants);
                seasonalChildrenPerVariant.Add(seasonalObj.Season, seasonalObj.GetComponentsInChildren<Seasonal>());
            }
            // In case a root seasonal object had seasonal children, find and update those as well.
            // it should set season for the child and it should make a list of the equivalent children in seasonal variations
            // This relies on GetComponentsInChildren returning children in a predictable order
            for (int i=0; i< seasonalChildrenPerVariant[Season.SPRING].Length; i++)
            {
                // For this function, ignore the roots, they were already handled. just look at the children.
                if (seasonalChildrenPerVariant[Season.SPRING][i].transform.parent == null)
                {
                    continue;
                }
                List<Seasonal> seasonalVariantsForTheChild = new List<Seasonal>();
                foreach (Season season in Enum.GetValues(typeof(Season)))
                {
                    Seasonal[] seasonalChildrenArray = seasonalChildrenPerVariant[season];
                    Seasonal seasonalChildObj = seasonalChildrenArray[i];
                    seasonalChildObj.Season = season;
                    seasonalVariantsForTheChild.Add(seasonalChildObj);
                    mObjectToSeasonalVariants.Add(seasonalChildObj, seasonalVariantsForTheChild);
                    mObjectsBySeason[season].Add(seasonalChildObj);
                }
            }
        }
    }
    
    void Update()
    {
        // TODO: see if there are any issues with moving static objects
        if (mCameraObj != null)
        {
            float totalWidth = 4 * SEASONAL_OFFSET;
            foreach (List<Seasonal> seasonalObjList in mRootObjectsBySeason.Values)
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

// Draws a gizmo in the editor to visualize the seasonal copies
public class SeasonalSystemGizmoDrawer
{
    [DrawGizmo(GizmoType.Selected | GizmoType.Active | GizmoType.NotInSelectionHierarchy)]
    static void DrawGizmoForMyScript(Seasonal scr, GizmoType gizmoType)
    {
        Gizmos.color = Color.yellow;
        float y1 = -6f;
        float y2 = 20f;
        foreach (float x in new float[]{
            SeasonalSystem.SEASONAL_OFFSET * -1f,
            SeasonalSystem.SEASONAL_OFFSET * 0f,
            SeasonalSystem.SEASONAL_OFFSET * 1f,
            SeasonalSystem.SEASONAL_OFFSET * 2f,
        })
        {
            Gizmos.DrawLine(new Vector3(x, y1, 0), new Vector3(x, y2, 0));
        }
    }
}
