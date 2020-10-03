using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneTree : MonoBehaviour
{
    public Sprite sapling;
    public Sprite deadSapling;
    public Sprite summerSapling;

    private GameObject mSapling;
    private GameObject mTree;

    // Start is called before the first frame update
    void Start()
    {
        mSapling = transform.Find("Sapling").gameObject;
        mTree = transform.Find("GrownTree").gameObject;

        UpdatePuzzleState(false);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdatePuzzleState(bool good)
    {
        Season season = GetComponent<Seasonal>().Season;
        if(season == Season.SPRING)
        {
            mSapling.SetActive(true);
            mTree.SetActive(false);
        }
        else if(season == Season.SUMMER)
        {
            mTree.SetActive(good);
            if(good)
            {
                mSapling.SetActive(false);
            }
            else
            {
                mSapling.GetComponent<SpriteRenderer>().sprite = deadSapling;
            }
        }
        else
        {
            mSapling.SetActive(false);
            mTree.SetActive(good);
        }
    }

    public void PositionChanged(GameObject waypoint)
    {
        Debug.Log("tree position changed! " + waypoint.name);
        UpdatePuzzleState(waypoint.name.Contains("GoodSquirrelWaypoint"));
    }
}
