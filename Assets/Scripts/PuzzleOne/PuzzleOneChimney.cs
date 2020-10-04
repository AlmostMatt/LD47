using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneChimney : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(GetComponent<Seasonal>().Season == Season.WINTER);
    }
}
