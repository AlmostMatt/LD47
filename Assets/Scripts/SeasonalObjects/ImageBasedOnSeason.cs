using UnityEngine;
using System.Collections;

public class ImageBasedOnSeason : MonoBehaviour
{
    public Sprite[] seasonalSprites = new Sprite[4];
    // Use this for initialization
    void Start()
    {
        Season season = gameObject.GetComponent<Seasonal>().Season;
        GetComponent<SpriteRenderer>().sprite = seasonalSprites[(int)season];
    }
}
