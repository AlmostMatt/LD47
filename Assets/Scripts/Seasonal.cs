using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Seasonal : MonoBehaviour
{
    private Season mSeason;
    public Season Season
    {
        get { return mSeason; }
        set { mSeason = value; }
    }

}

// Draws a gizmo in the editor to visualize the seasonal copies
public class SeasonalGizmoDrawer
{
    [DrawGizmo(GizmoType.Selected | GizmoType.Active | GizmoType.NotInSelectionHierarchy)]
    static void DrawGizmoForMyScript(Seasonal scr, GizmoType gizmoType)
    {
        foreach(Renderer rend in scr.gameObject.GetComponentsInChildren<Renderer>())
        {
            if(rend != null)
            {
                DrawRectangle(Vector3.zero, rend.bounds);
                DrawRectangle(new Vector3(+SeasonalSystem.SEASONAL_OFFSET, 0f, 0f), rend.bounds);
                DrawRectangle(new Vector3(-SeasonalSystem.SEASONAL_OFFSET, 0f, 0f), rend.bounds);
            }
        }
    }

    static void DrawRectangle(Vector3 offset, Bounds bounds)
    {
        Vector3 botLeft = offset + bounds.min;
        Vector3 topRight = offset + bounds.max;
        Vector3 topLeft = new Vector3(botLeft.x, topRight.y, botLeft.z);
        Vector3 botRight = new Vector3(topRight.x, botLeft.y, botLeft.z);
        Gizmos.DrawLine(botLeft, topLeft);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, botRight);
        Gizmos.DrawLine(botRight, botLeft);
    }
}
