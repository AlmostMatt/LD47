using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafPlatform : MonoBehaviour
{
    public Sprite[] seasonalSprites = new Sprite[(int)Season.NUM_SEASONS];
    
    // Start is called before the first frame update
    void Start()
    {
        Season season = GetComponent<Seasonal>().Season;
        GetComponent<SpriteRenderer>().sprite = seasonalSprites[(int)season];

        if(season == Season.WINTER)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
