using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panda : MonoBehaviour
{
    public Sprite sleepingSprite;
    public GameObject bubble;
    public float bubbleTime = 2f;

    private Transform mBubbleSpawnPoint;
    private int mFacing = -1;
    private float mSpeed = 1.5f;
    private bool mSleeping = false;

    private float mBubbleTimer = 0f;

    private void Start()
    {
        mBubbleSpawnPoint = transform.Find("BubbleSpawnPoint");

        Season season = GetComponent<Seasonal>().Season;
        if(season == Season.WINTER)
        {
            GetComponent<SpriteRenderer>().sprite = sleepingSprite;
            GetComponent<Animator>().SetBool("sleeping", true);
            mSleeping = true;
        }
    }

    void FixedUpdate()
    {
        if(!mSleeping)
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
            float actualAccel = Mathf.Min(Mathf.Abs(desiredAccel), maxAccel) * Mathf.Sign(desiredAccel) * 1000;
            rb.AddForce(new Vector2(actualAccel, 0f));
        }
    }

    private void Update()
    {
        if(mSleeping)
        {
            if(bubble != null)
            {
                if(mBubbleTimer >= 0f)
                {
                    mBubbleTimer -= Time.deltaTime;
                    if(mBubbleTimer < 0f)
                    {
//                        Vector3 spawnOffset = new Vector3(bubbleSpawnOffset.x * transform.localScale.x, transform.localScale.y * bubbleSpawnOffset.y, 0f);
                        GameObject b = Instantiate(bubble, mBubbleSpawnPoint.position, Quaternion.identity);
                        b.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1);
                        mBubbleTimer = bubbleTime;
                    }
                }
            }
        }
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
