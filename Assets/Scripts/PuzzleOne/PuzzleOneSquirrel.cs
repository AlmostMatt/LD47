using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneSquirrel : MonoBehaviour
{
    public GameObject growTree;

    private bool mHasTargetPosition = false;
    GameObject mTargetWaypoint;
    Vector3 mTargetPosition;

    private float mFleeSpeed = 8f;
    private Rigidbody2D mRigidbody;
    private Season mSeason;

    void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
        mSeason = GetComponent<Seasonal>().Season;
        //if(mSeason == Season.SPRING || mSeason == Season.SUMMER)
        if(mSeason != Season.FALL)
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
            float best = 999999;
            foreach(GameObject waypoint in waypoints)
            {
                if(waypoint.GetComponent<Seasonal>().Season != mSeason) continue;

                Vector3 waypointPos = waypoint.transform.position;
                Vector3 waypointToPlayer = collision.gameObject.transform.position - waypointPos;
                waypointToPlayer.z = 0;
                float distToPlayer = waypointToPlayer.sqrMagnitude;
                if(distToPlayer < 1) continue;

                Vector3 waypointToSquirrel = transform.position - waypointPos;
                waypointToSquirrel.z = 0;
                float distToSquirrel = waypointToSquirrel.sqrMagnitude;
                if(distToSquirrel < 1) continue;

                float cost = distToSquirrel + (distToSquirrel > distToPlayer ? 10000 : 0);

                if(cost < best)
                {
                    best = cost;
                    mHasTargetPosition = true;
                    mTargetPosition = waypoint.transform.position;
                    mTargetWaypoint = waypoint;
                }
            }
        }
    }

    private void Update()
    {
        if(mHasTargetPosition)
        {
            Vector3 toTarget = mTargetWaypoint.transform.position - transform.position;
            if(Mathf.Abs(toTarget.x) <= 0.1f)
            {
                mHasTargetPosition = false;
                mRigidbody.velocity = new Vector2(0, mRigidbody.velocity.y);

                // move all versions of the growing tree to match the position of the waypoint
                if(growTree != null)
                {
                    SeasonalSystem seasonalSystem = SeasonalSystem.GetSingleton();
                    List<Seasonal> waypoints = seasonalSystem.GetSeasonalVariants(mTargetWaypoint.GetComponent<Seasonal>());
                    List<Seasonal> trees = seasonalSystem.GetSeasonalVariants(growTree.GetComponent<Seasonal>());
                    for(int i = 0; i < 4; ++i)
                    {
                        trees[i].gameObject.transform.position = waypoints[i].gameObject.transform.position;
                        trees[i].BroadcastMessage("PositionChanged", waypoints[i].gameObject);
                    }
                }
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
