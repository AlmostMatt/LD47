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

    private GameObject mTargetFruit;
    private bool mEating = false;

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
        else
        {
            mEating = true;
            GetComponent<Animator>().SetBool("eating", mEating);
        }
    }

    void FixedUpdate()
    {
        if(!mSleeping)
        {
            if(mTargetFruit == null)
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
            }
            else
            {
                float distToTarget = mTargetFruit.transform.position.x - transform.position.x;
                if(Mathf.Abs(distToTarget) <= 0.1f)
                {
                    // stop moving (unless we get another fruit)
                    mEating = true;
                    GetComponent<Animator>().SetBool("eating", mEating);
                    Destroy(mTargetFruit);
                    mTargetFruit = null;

                    // sync all pandas with this one
                    SeasonalSystem seasonalSystem = SeasonalSystem.GetSingleton();
                    Seasonal seasonal = GetComponent<Seasonal>();
                    Season season = seasonal.Season;
                    List<Seasonal> pandas = seasonalSystem.GetSeasonalVariants(seasonal);
                    float relativeX = transform.position.x - seasonalSystem.GetSeasonX1(season);
                    for(int i = 0; i < 4; ++i)
                    {
                        if(i != (int)Season.WINTER) continue; // I know the loop is unnecessary now, but it's here because of design churn, and maybe it'll be needed again...
                        //if(i == (int)season || i == (int)Season.SPRING || i == ) continue;

                        Season s = (Season)i;
                        float seasonX = seasonalSystem.GetSeasonX1(s);
                        Transform panda = pandas[i].transform;
                        panda.position = new Vector3(seasonX + relativeX, transform.position.y, transform.position.z);
                    }
                    
                }
                else
                {
                    SetFacingDirection(distToTarget > 0 ? 1 : -1);                    
                }
            }
            
            if(mTargetFruit && !mEating)
            {
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
                        GameObject b = Instantiate(bubble, mBubbleSpawnPoint.position, Quaternion.identity);
                        b.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1);
                        mBubbleTimer = bubbleTime;
                    }
                }
            }
        }
        else
        {
            if(mTargetFruit == null)
            {
                SeasonalSystem seasonSystem = SeasonalSystem.GetSingleton();
                Season season = GetComponent<Seasonal>().Season;
                float seasonLeftBound = seasonSystem.GetSeasonX1(season);
                float seasonRightBound = seasonSystem.GetSeasonX2(season);
                
                GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
                float best = 9999;
                foreach(GameObject fruit in fruits)
                {
                    float fruitX = fruit.transform.position.x;
                    if(fruitX < seasonLeftBound || fruitX > seasonRightBound) continue;
                    if(fruit.transform.position.y > transform.position.y) continue;

                    float dist = fruitX - transform.position.x;
                    if(dist < best)
                    {
                        best = dist;
                        mTargetFruit = fruit;
                        mEating = false;
                        GetComponent<Animator>().SetBool("eating", mEating);
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) return;
        
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
