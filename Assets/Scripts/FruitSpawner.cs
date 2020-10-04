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
        if(GetComponent<Seasonal>().Season != Season.FALL)
        {
            gameObject.SetActive(false);
            return;
        }

        SpawnFruit();
    }

    // Update is called once per frame
    void Update()
    {        
    }

    public void SpawnFruit()
    {
        mSpawnedFruit = Instantiate(fruit, transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            DropFruit();
        }
    }

    public void DropFruit()
    {
        if(mSpawnedFruit != null)
        {
            mSpawnedFruit.GetComponent<Fruit>().DropFromTree();
        }
    }
}
