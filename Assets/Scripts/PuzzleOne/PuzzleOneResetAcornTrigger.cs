using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneResetAcornTrigger : MonoBehaviour
{
    public GameObject fallSquirrelStartPoint;
    public GameObject squirrel;
    public GameObject dropAcornTrigger;
    public GameObject acornSpawner;

    private GameObject mFallSquirrelStartPoint;
    private GameObject mFallSquirrel;
    private GameObject mFallDropAcornTrigger;
    private GameObject mFallAcornSpawner;

    private void Start()
    {        
        gameObject.SetActive(false);
        
        SeasonalSystem seasonalSystem = SeasonalSystem.GetSingleton();

        mFallSquirrelStartPoint = seasonalSystem.GetSeasonalVariants(fallSquirrelStartPoint.GetComponent<Seasonal>())[(int)Season.FALL].gameObject;
        mFallSquirrel = seasonalSystem.GetSeasonalVariants(squirrel.GetComponent<Seasonal>())[(int)Season.FALL].gameObject;
        mFallDropAcornTrigger = seasonalSystem.GetSeasonalVariants(dropAcornTrigger.GetComponent<Seasonal>())[(int)Season.FALL].gameObject;
        mFallAcornSpawner = seasonalSystem.GetSeasonalVariants(acornSpawner.GetComponent<Seasonal>())[(int)Season.FALL].gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player")) return;

        if(mFallSquirrel.transform.childCount > 2)
            Destroy(mFallSquirrel.transform.GetChild(2).gameObject);

        mFallSquirrel.transform.position = mFallSquirrelStartPoint.transform.position;
        mFallSquirrel.transform.localScale = mFallSquirrelStartPoint.transform.localScale;
        mFallAcornSpawner.GetComponent<FruitSpawner>().SpawnFruit();
        mFallDropAcornTrigger.SetActive(true);

        gameObject.SetActive(false);
    }
}
