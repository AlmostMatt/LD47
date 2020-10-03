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
        Gizmos.color = new Color(1f,1f,1f,0.5f);
        foreach (Renderer rend in scr.gameObject.GetComponentsInChildren<Renderer>())
        {
            //DrawRectangle(Vector3.zero, rend.bounds);
            DrawRectangle(new Vector3(+SeasonalSystem.SEASONAL_OFFSET, 0f, 0f), rend.bounds);
            DrawRectangle(new Vector3(-SeasonalSystem.SEASONAL_OFFSET, 0f, 0f), rend.bounds);
        }
        Gizmos.color = Color.green;
        foreach (Collider2D collider in scr.gameObject.GetComponentsInChildren<Collider2D>())
        {
            //DrawRectangle(Vector3.zero, collider.bounds);
            DrawRectangle(new Vector3(+SeasonalSystem.SEASONAL_OFFSET, 0f, 0f), collider.bounds);
            DrawRectangle(new Vector3(-SeasonalSystem.SEASONAL_OFFSET, 0f, 0f), collider.bounds);
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
