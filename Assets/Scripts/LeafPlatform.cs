using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafPlatform : MonoBehaviour
{
    public Sprite[] seasonalSprites = new Sprite[4];
    
    // Start is called before the first frame update
    void Start()
    {
        Season season = gameObject.GetComponent<Seasonal>().Season;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(gameObject.name + " collision with " + collision.gameObject.name);
        ParticleSystem leaves = GetComponentInChildren<ParticleSystem>();
        if(leaves != null)
        {
            leaves.Play();
        }
    }
}
