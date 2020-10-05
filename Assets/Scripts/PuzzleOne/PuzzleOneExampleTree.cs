using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneExampleTree : MonoBehaviour
{
    private GameObject mWinterPlant;
    private GameObject mSapling;
    private GameObject mTree;
    private GameObject mFirewood;

    // Start is called before the first frame update
    void Start()
    {
        mWinterPlant = transform.Find("WinterPlant").gameObject;
        mSapling = transform.Find("Sapling").gameObject;
        mTree = transform.Find("GrownTree").gameObject;
        mFirewood = transform.Find("Firewood").gameObject;

        UpdatePuzzleState();        
    }

    private void UpdatePuzzleState()
    {
        Season season = GetComponent<Seasonal>().Season;
        mSapling.SetActive(season == Season.SPRING);
        mTree.SetActive(season == Season.SUMMER);
        mFirewood.SetActive(season == Season.FALL);
        mWinterPlant.SetActive(season == Season.WINTER);
    }
}
