﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject mPlayerObj;
    private const float MOV_SPEED = 4f;
    private const float SMOOTH_TIME = 0.3f;

    private const float MIN_Y = -1.2f;
    private Vector3 mCamVelocity = Vector3.zero;

    private bool mFollowPlayer = true;

    void Update()
    {
    }

    void FixedUpdate()
    {
        if(!mFollowPlayer) return;

        MoveCamera();
    }

    void MoveCamera()
    {
        if (mPlayerObj == null)
        {
            mPlayerObj = GameObject.FindObjectOfType<Player>().gameObject;
        }
        else
        {
            //Vector3 playerPos = mPlayerObj.transform.localPosition;
            Vector3 playerPos = mPlayerObj.transform.position;
            Vector3 targetPos = new Vector3(playerPos.x, Mathf.Max(playerPos.y, MIN_Y), transform.localPosition.z);

            transform.localPosition = targetPos;
            // TODO: use smooth damp and fix the jitter
            //transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPos, ref mCamVelocity, SMOOTH_TIME);
            // transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * MOV_SPEED);
        }
    }

    public void StopFollowingPlayer()
    {
        mFollowPlayer = false;
        // This happens at the end of the game. Shortly after this we want to fade to black.
        FadeIn fadeIn = GameObject.FindObjectOfType<FadeIn>();
        if (fadeIn != null)
        {
            fadeIn.StartFadeOut();
        }
    }
}
