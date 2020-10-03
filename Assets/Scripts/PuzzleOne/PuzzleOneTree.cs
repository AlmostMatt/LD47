using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneTree : MonoBehaviour
{
    public Sprite sapling;
    public Sprite deadSapling;

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
            mTree.SetActive(false);
            mSapling.SetActive(true);
            mSapling.GetComponent<SpriteRenderer>().sprite = sapling;
        }
        else if(season == Season.SUMMER)
        {
            mTree.SetActive(good);
            mSapling.SetActive(!good);
            if(!good)
            {
                mSapling.GetComponent<SpriteRenderer>().sprite = deadSapling;
            }
        }
        else if(season == Season.FALL)
        {
            mTree.SetActive(false);
            mSapling.SetActive(false);
        }
        else
        {
            mSapling.SetActive(false);
            mTree.SetActive(false);
        }
    }

    public void PositionChanged(GameObject waypoint)
    {
        Debug.Log("tree position changed! " + waypoint.name);
        UpdatePuzzleState(waypoint.name.Contains("GoodSquirrelWaypoint"));
    }
}
