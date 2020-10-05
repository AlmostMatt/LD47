using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneTree : MonoBehaviour
{
    public Sprite buriedAcorn;
    public Sprite sapling;
    public Sprite deadSapling;

    private GameObject mSapling;
    private GameObject mTree;

    private float mSaplingAlpha = 1f;
    private float mSaplingTargetAlpha = 1f;
    private float mTreeAlpha = 0.0001f;
    private float mTreeTargetAlpha = 0f;
    private float mFadeSpeed = 2f;

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
        if(mTreeAlpha != mTreeTargetAlpha)
        {
            mTreeAlpha += Time.deltaTime * (mTreeTargetAlpha > mTreeAlpha ? 1f : -1f) * mFadeSpeed;
            mTreeAlpha = Mathf.Clamp(mTreeAlpha, 0, 1);
            
            SpriteRenderer[] sprs = mTree.GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer sr in sprs)
            {
                Color c = sr.color;
                c.a = mTreeAlpha;
                sr.color = c;
            }
                    
            if(mTreeAlpha == 0f)
            {
                mTree.SetActive(false);
            }
        }

        if(mSaplingAlpha != mSaplingTargetAlpha)
        {
            mSaplingAlpha += Time.deltaTime * (mSaplingTargetAlpha > mSaplingAlpha ? 1f : -1f) * mFadeSpeed;
            mSaplingAlpha = Mathf.Clamp(mSaplingAlpha, 0, 1);
            
            SpriteRenderer[] sprs = mSapling.GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer sr in sprs)
            {
                Color c = sr.color;
                c.a = mSaplingTargetAlpha;
                sr.color = c;
            }
            
            if(mSaplingAlpha == 0f)
            {
                mSapling.SetActive(false);
            }
        }
    }

    private void ActivateSapling(bool active)
    {
        mSaplingTargetAlpha = active ? 1f : 0f;
        if(active)
            mSapling.SetActive(true);
    }
    private void ActivateTree(bool active)
    {
        mTreeTargetAlpha = active ? 1f : 0f;
        if(active)
            mTree.SetActive(true);
    }

    private void UpdatePuzzleState(bool good)
    {
        Season season = GetComponent<Seasonal>().Season;
        if(season == Season.SPRING)
        {
            ActivateTree(false);
            ActivateSapling(true);
            mSapling.GetComponentInChildren<SpriteRenderer>().sprite = sapling;
        }
        else if(season == Season.SUMMER)
        {
            ActivateTree(good);
            ActivateSapling(!good);
            if(!good)
            {
                mSapling.GetComponentInChildren<SpriteRenderer>().sprite = deadSapling;
            }
        }
        else if(season == Season.WINTER)
        {
            ActivateTree(false);
            ActivateSapling(true);
            mSapling.GetComponentInChildren<SpriteRenderer>().sprite = buriedAcorn;
        }
        else
        {
            ActivateTree(false);
            ActivateSapling(false);
        }
    }

    public void PositionChanged(GameObject waypoint)
    {
        Debug.Log("tree position changed! " + waypoint.name);
        // the tree can only really grow in one spot, so there's no need to move it...
        // it's just the saplings that matter. but the tree still needs to be moved initially
        mSapling.transform.position = waypoint.gameObject.transform.position;
        bool good = waypoint.name.Contains("GoodSquirrelWaypoint");
        if(good)
        {
            mTree.transform.position = waypoint.gameObject.transform.position;
        }
        UpdatePuzzleState(good);
    }
}
