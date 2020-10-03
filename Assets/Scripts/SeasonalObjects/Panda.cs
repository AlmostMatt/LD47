using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panda : MonoBehaviour
{
    private int mFacing = -1;
    private float mSpeed = 4f;

    void FixedUpdate()
    {
        SeasonalSystem seasonSystem = SeasonalSystem.GetSingleton();
        Season season = GetComponent<Seasonal>().Season;
        if (transform.localPosition.x < seasonSystem.GetSeasonX1(season))
        {
            SetFacingDirection(1);
        }
        if (transform.localPosition.x > seasonSystem.GetSeasonX2(season))
        {
            SetFacingDirection(-1);
        }

        // Accelerate to desired velocity
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float desiredV = mFacing * mSpeed;
        float deltaV = desiredV - rb.velocity.x;
        float desiredAccel = deltaV / Time.fixedDeltaTime;
        float maxAccel = 30f;
        float actualAccel = Mathf.Min(Mathf.Abs(desiredAccel), maxAccel) * Mathf.Sign(desiredAccel);
        rb.AddForce(new Vector2(actualAccel, 0f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float normalX = collision.contacts[0].normal.x;
        if (Mathf.Abs(normalX) > 0.2f)
        {
            SetFacingDirection((int)Mathf.Sign(normalX));
        }
    }

    private void SetFacingDirection(int facingDirection)
    {
        mFacing = facingDirection;
        transform.localScale = new Vector3(-mFacing * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
