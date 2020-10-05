using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public float collisionDelay = 1f;
    private float mCollisionDelay = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(mCollisionDelay > 0f)
        {
            mCollisionDelay -= Time.deltaTime;
            if(mCollisionDelay <= 0f)
            {
                // gameObject.layer = 0;
            }
        }
    }

    public void DropFromTree()
    {
        transform.SetParent(null);
        GetComponent<Rigidbody2D>().simulated = true;
        mCollisionDelay = collisionDelay;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(mCollisionDelay <= 0f)
        {
            GetComponent<Rigidbody2D>().simulated = false;
        }
    }
}
