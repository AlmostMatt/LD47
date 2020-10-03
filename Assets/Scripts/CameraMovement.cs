using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject mPlayerObj;
    private const float MAX_DX = 3f;
    private const float MAX_DY = 3f;
    private const float MOV_SPEED = 4f;

    void Update()
    {
        if (mPlayerObj == null)
        {
            mPlayerObj = GameObject.FindObjectOfType<Player>().gameObject;
        }
        else
        {
            Vector3 playerPos = mPlayerObj.transform.localPosition;
            Vector3 targetPos = new Vector3(playerPos.x, playerPos.y, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * MOV_SPEED);
            // transform.localPosition = new Vector3(targetPos.x, targetPos.y, transform.localPosition.z);
        }
    }
}
