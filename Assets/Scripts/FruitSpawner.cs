using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject fruit;

    private GameObject mSpawnedFruit;
    // Start is called before the first frame update
    void Start()
    {
        SpawnFruit();
    }

    // Update is called once per frame
    void Update()
    {
        if(mSpawnedFruit == null)
        {
            // TODO: only spawn fruit once, at the beginning of entering a season
//            SpawnFruit();
        }
    }

    void SpawnFruit()
    {
        mSpawnedFruit = Instantiate(fruit, transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(mSpawnedFruit != null)
        {
            mSpawnedFruit.GetComponent<Fruit>().DropFromTree();
        }
    }
}
