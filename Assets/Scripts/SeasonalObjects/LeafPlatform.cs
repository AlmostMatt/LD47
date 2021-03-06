﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafPlatform : MonoBehaviour
{
    public bool canReflect = true;
    public Sprite[] seasonalSprites = new Sprite[4];
    private SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        Season season = gameObject.GetComponent<Seasonal>().Season;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = seasonalSprites[(int)season];

        if(season == Season.WINTER || seasonalSprites[(int)season] == null)
        {
            gameObject.SetActive(false);
        }
        // 50% to reflect horizontally
        if (canReflect && Random.Range(0f, 1f) > 0.5f)
        {
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
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
