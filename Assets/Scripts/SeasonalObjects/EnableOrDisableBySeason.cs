using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOrDisableBySeason : MonoBehaviour
{
    public bool[] EnabledBySeason = new bool[4];
    
    void Start()
    {
        Season season = GetComponent<Seasonal>().Season;
        gameObject.SetActive(EnabledBySeason[(int)season]);
    }
}
