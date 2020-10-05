using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetApplesTrigger : MonoBehaviour
{
    public List<GameObject> appleSpawners;

    private List<GameObject> mFallAppleSpawners = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Season season = GetComponent<Seasonal>().Season;
        if(season != Season.FALL && season != Season.SUMMER)
        {
            gameObject.SetActive(false);
            return;
        }

        SeasonalSystem seasonalSystem = SeasonalSystem.GetSingleton();
        foreach(GameObject spawner in appleSpawners)
        {
            GameObject fallSpawner = seasonalSystem.GetSeasonalVariants(spawner.GetComponent<Seasonal>())[(int)Season.FALL].gameObject;
            mFallAppleSpawners.Add(fallSpawner);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            foreach(GameObject spawner in mFallAppleSpawners)
            {
                spawner.GetComponent<FruitSpawner>().SpawnFruit();
            }
        }
    }
}
