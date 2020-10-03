using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneWeather : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Season season = GetComponent<Seasonal>().Season;
        foreach (Season s in System.Enum.GetValues(typeof(Season)))
        {
            Transform child = transform.Find(s.ToString());
            if(child != null)
            {
                child.gameObject.SetActive(s == season);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
