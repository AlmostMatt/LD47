using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vines : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Season season = GetComponent<Seasonal>().Season;
        if(season != Season.SUMMER)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
