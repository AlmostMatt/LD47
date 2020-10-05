using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float expandTime = 1f;
    public GameObject popEffect;

    private float mPopTimer = -1f;
    private const int PHYS_LAYER_BUBBLE_POP = 16;
    private float mExpandTimer;
    Rigidbody2D mRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
        mExpandTimer = expandTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(mExpandTimer > 0f)
        {
            mExpandTimer -= Time.deltaTime;
            float scale = Mathf.Lerp(0.1f, 1, (expandTime - mExpandTimer) / expandTime);
            transform.localScale = new Vector3(scale, scale, scale);
        }

        if(mRigidbody.velocity.y < 0f)
        {
            Pop();
            return;
        }

        if(mPopTimer > 0f)
        {
            mPopTimer -= Time.deltaTime;
            if(mPopTimer <= 0f)
            {
                Pop();
                return;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            mPopTimer = 1f;
            return;
        }
            
        if(collision.gameObject.layer == PHYS_LAYER_BUBBLE_POP)
        {
            mPopTimer = 0.01f;
        }
    }

    private void Pop()
    {
        if(popEffect != null)
        {
            Instantiate(popEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
