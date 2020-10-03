﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public GameObject mainCamera;

    private AudioSource source;
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
                if(s == season)
                {
                    source = child.GetComponent<AudioSource>();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(mainCamera != null)
        {
            float dist = Mathf.Abs(mainCamera.transform.position.x - transform.position.x);
            source.volume = Mathf.Clamp((20 - dist)/20, 0, 1);
        }
    }
}
