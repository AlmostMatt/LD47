﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D mRigidbody;
    public float jumpSpeed = 5f;
    
    private bool mOnGround = true;
    private float mJumpTimer = 0f;
    private float mJumpGraceTimeTimer = 0f;
    private float mSpeed = 5;

    private const float GROUND_CHECK_DIST = 0.015f;
    private const int PLATFORM_PHYS_LAYER = 1 << 8;
    private const float JUMP_GRACE_TIME = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 vel = new Vector2();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        float horiz = Input.GetAxis("Horizontal");
        bool sideBlocked = false;
        if(horiz != 0f)
        {
            vel.x = (Mathf.Sign(horiz) * mSpeed);
            RaycastHit2D[] sideHits = Physics2D.BoxCastAll(transform.position, collider.size, 0, vel, GROUND_CHECK_DIST, PLATFORM_PHYS_LAYER);
            foreach(RaycastHit2D hit in sideHits)
            {   
                if(hit.collider != null && hit.normal.x != 0f && hit.distance <= GROUND_CHECK_DIST && hit.collider != collider)
                {
                    sideBlocked = true;
                    break;
                }
            }

            if(sideBlocked)
            {
                Debug.Log("side blocked");
                vel.x = 0;
            }
        }

        // jumping
        bool onGround = false;
        RaycastHit2D[] groundHits = Physics2D.BoxCastAll(transform.position, collider.size, 0, new Vector2(0, -1), GROUND_CHECK_DIST, PLATFORM_PHYS_LAYER);
        foreach(RaycastHit2D hit in groundHits)
        {   
            if(hit.collider != null && hit.normal.y > 0 && hit.distance <= GROUND_CHECK_DIST && hit.collider != collider)
            {
                onGround = true;
                break;
            }
        }

        if(mJumpTimer > 0f)
        {
            mJumpTimer -= Time.fixedDeltaTime;
        }

        if(mJumpGraceTimeTimer > 0f)
        {
            mJumpGraceTimeTimer -= Time.fixedDeltaTime;
        }

        if(mOnGround && !onGround)
        {
            // just left the ground
            mJumpGraceTimeTimer = JUMP_GRACE_TIME;
        }

        bool jump = Input.GetButton("Jump");
        if(jump && (onGround || mJumpGraceTimeTimer > 0f) && mJumpTimer <= 0f)
        {
            vel.y = jumpSpeed;
            mOnGround = false;
            mJumpTimer = 0.2f;
        }
        else
        {
            vel.y = mRigidbody.velocity.y;
        }

        mOnGround = onGround;

        mRigidbody.velocity = vel;
    }
}
