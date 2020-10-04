using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneSquirrel : MonoBehaviour
{
    public GameObject growTree;

    GameObject mTargetObject;

    private float mFleeSpeed = 8f;
    private Rigidbody2D mRigidbody;
    private Season mSeason;
    private float mScaleX;

    private bool mGoingForAcorn = false;

    void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
        mSeason = GetComponent<Seasonal>().Season;
        //if(mSeason == Season.SPRING || mSeason == Season.SUMMER)
        if(mSeason != Season.FALL)
        {
            gameObject.SetActive(false);
        }

        mScaleX = Mathf.Abs(transform.localScale.x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            mRigidbody.AddForce(new Vector2(2, 20));
            mGoingForAcorn = false;
            SelectWaypoint(collision.gameObject);
        }
    }

    private void Update()
    {        
        if(!mGoingForAcorn)
        {
            bool hasAcorn = transform.childCount > 2;
            if(!hasAcorn)
            {
                GameObject acorn = GameObject.FindGameObjectWithTag("Acorn");
                if(acorn != null && acorn.transform.position.y <= -3 && !acorn.GetComponent<Rigidbody2D>().simulated)
                {
                    mGoingForAcorn = true;
                    Debug.Log("going for acorn");
                    if(transform.position.y > -3)
                    {
                        // assume the squirrel is sitting on the logs
                        mRigidbody.AddRelativeForce(new Vector2(-90, -10));
                    }
                    mTargetObject = acorn;                    
                }
            }
        }
    
        if(mTargetObject != null)
        {
            Vector3 toTarget = mTargetObject.transform.position - transform.position;
            if(Mathf.Abs(toTarget.x) <= 0.1f)
            {
                mRigidbody.velocity = new Vector2(0, mRigidbody.velocity.y);

                if(mTargetObject.CompareTag("Acorn"))
                {
                    mTargetObject.transform.SetParent(transform);
                    mTargetObject.transform.localPosition = new Vector3(0, 0, 0);
                    SelectWaypoint(null);
                    Debug.Log("picked up acorn");
                    mGoingForAcorn = false;
                }
                else
                {
                    if(transform.childCount > 2) // hasAcorn
                    {
                        // move all versions of the growing tree to match the position of the waypoint
                        if(growTree != null)
                        {
                            SeasonalSystem seasonalSystem = SeasonalSystem.GetSingleton();
                            List<Seasonal> waypoints = seasonalSystem.GetSeasonalVariants(mTargetObject.GetComponent<Seasonal>());
                            List<Seasonal> trees = seasonalSystem.GetSeasonalVariants(growTree.GetComponent<Seasonal>());
                            for(int i = 0; i < 4; ++i)
                            {
                                trees[i].BroadcastMessage("PositionChanged", waypoints[i].gameObject);
                            }
                        }
                    }

                    mTargetObject = null;
                }
            }
            else
            {
                float towardsSign = Mathf.Sign(toTarget.x);
                //GetComponentInChildren<SpriteRenderer>().flipX = towardsSign < 0;
                transform.localScale = new Vector3(mScaleX * towardsSign, transform.localScale.y, transform.localScale.z);
                if(transform.position.y <= -3)
                {
                    mRigidbody.velocity = new Vector2(towardsSign * mFleeSpeed, mRigidbody.velocity.y);
                }
            }
        }
    }

    private void SelectWaypoint(GameObject player)
    {
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("SquirrelWaypoint");
        float best = 999999;
        foreach(GameObject waypoint in waypoints)
        {
            if(waypoint.GetComponent<Seasonal>().Season != mSeason) continue;

            Vector3 waypointPos = waypoint.transform.position;
            float distToPlayer = 1000;
            if(player != null)
            {
                distToPlayer = Mathf.Abs(player.transform.position.x - waypointPos.x);
            }
            if(distToPlayer < 1) continue;

            float distToSquirrel = Mathf.Abs(transform.position.x - waypointPos.x);
            if(distToSquirrel < 1) continue;

            float cost = distToSquirrel + (distToSquirrel > distToPlayer ? 10000 : 0);

            if(cost < best)
            {
                best = cost;
                mTargetObject = waypoint;
            }
        }
    }
}
