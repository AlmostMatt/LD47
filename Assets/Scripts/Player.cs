using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D mRigidbody;
    public float gravity = -9.8f;
    public float jumpSpeed = 5f;
    
    private bool mOnGround = true;
    private float mSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = new Vector2();

        float horiz = Input.GetAxis("Horizontal");
        bool jump = Input.GetButtonDown("Jump");

        if(jump && mOnGround)
        {
            vel.y = jumpSpeed;
        }
        else
        {
            vel.y = mRigidbody.velocity.y;
        }

        if(horiz != 0f)
        {
            vel.x = (Mathf.Sign(horiz) * mSpeed);
        }

        mRigidbody.velocity = vel;
    }
}
