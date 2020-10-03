using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneSquirrel : MonoBehaviour
{
    private bool mHasTargetPosition = false;
    Vector3 mTargetPosition;

    private float mFleeSpeed = 8f;
    private Rigidbody2D mRigidbody;
    private Season mSeason;

    void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
        mSeason = GetComponent<Seasonal>().Season;
        //if(mSeason == Season.SPRING || mSeason == Season.SUMMER)
        if(false)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("AHH a player!");
            mRigidbody.AddForce(new Vector2(2, 20));

            GameObject[] waypoints = GameObject.FindGameObjectsWithTag("SquirrelWaypoint");
            float best = 9999;
            foreach(GameObject waypoint in waypoints)
            {
                Vector3 waypointPos = waypoint.transform.position;
                Vector3 waypointToPlayer = collision.gameObject.transform.position - waypointPos;
                waypointToPlayer.z = 0;
                float distToPlayer = waypointToPlayer.sqrMagnitude;

                Vector3 waypointToSquirrel = transform.position - waypointPos;
                waypointToSquirrel.z = 0;
                float distToSquirrel = waypointToSquirrel.sqrMagnitude;

                float cost = distToSquirrel - (distToPlayer * 1000);

                if(cost < best)
                {
                    best = cost;
                    mHasTargetPosition = true;
                    mTargetPosition = waypoint.transform.position;
                }
            }
        }
    }

    private void Update()
    {
        if(mHasTargetPosition)
        {
            Vector3 toTarget = mTargetPosition - transform.position;
            if(Mathf.Abs(toTarget.x) <= 0.1f)
            {
                mHasTargetPosition = false;
                mRigidbody.velocity = new Vector2(0, mRigidbody.velocity.y);
                
            }
            else
            {
                float towardsSign = Mathf.Sign(toTarget.x);
                GetComponent<SpriteRenderer>().flipX = towardsSign < 0;
                mRigidbody.velocity = new Vector2(towardsSign * mFleeSpeed, mRigidbody.velocity.y);
            }
        }
    }

}
