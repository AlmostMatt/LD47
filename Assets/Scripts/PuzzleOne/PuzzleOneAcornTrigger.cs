using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneAcornTrigger : MonoBehaviour
{
    public GameObject resetTrigger;
    public GameObject acornSpawner;
    public GameObject leafPlatform;

    private GameObject mResetTrigger;
    private GameObject mAcornSpawner;
    private GameObject mLeafPlatform;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Seasonal>().Season != Season.FALL)
        {
            gameObject.SetActive(false);
            return;
        }

        // a little annoying, but these are references to the SPRING version.
        // need to find the fall versions.
        SeasonalSystem seasonalSystem = SeasonalSystem.GetSingleton();
        mResetTrigger = seasonalSystem.GetSeasonalVariants(resetTrigger.GetComponent<Seasonal>())[(int)Season.SUMMER].gameObject;
        mAcornSpawner = seasonalSystem.GetSeasonalVariants(acornSpawner.GetComponent<Seasonal>())[(int)Season.FALL].gameObject;
        mLeafPlatform = seasonalSystem.GetSeasonalVariants(leafPlatform.GetComponent<Seasonal>())[(int)Season.FALL].gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        mResetTrigger.SetActive(true);
        mAcornSpawner.GetComponent<FruitSpawner>().DropFruit();
        mLeafPlatform.GetComponentInChildren<ParticleSystem>().Play();
        
        gameObject.SetActive(false);
    }
}
