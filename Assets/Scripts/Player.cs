using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D mRigidbody;
    public float jumpSpeed = 5f;

    private int mFacing = 1; // 1 or -1
    private bool mOnGround = true;
    private bool mJumping = true;
    private float mJumpTimer = 0f;
    private float mJumpGraceTimeTimer = 0f;
    private float mSpeed = 5;
    private float mClimbSpeed = 3;
    private bool mClimbing = false;
    private bool mStayClimbing = false;
    private float mOldGravityScale;
    private float mJumpedFromClimbTimer = 0f;
    private const float JUMP_GRACE_TIME = 0.15f;
    private const float GROUND_CHECK_DIST = 0.015f;

    private bool mPreventMovement = false;

    // fields related to in-cloud effect
    private int mNumCloudsTouching = 0;

    // phys layers
    // Note, this could also be done with LayerMask.GetMask("UserLayerA", "UserLayerB")) using the names of layers
    private const int PHYS_LAYER_DEFAULT = 0;
    private const int PHYS_LAYER_PLATFORM = 8;
    private const int PHYS_LAYER_CLIMBABLE = 9;
    private const int PHYS_LAYER_CLIMBING = 10;
    // 11 = AnimalNoCollide
    // 12 = FallingFruit
    private const int RAIN_BLOCKING_PLATFORM_PHYS_LAYER = 13;
    // 14 = Rain particles
    private const int PHYS_LAYER_BLOCKING_ENV = 15;
    private const int PHYS_LAYER_PLATFORM_FRUIT_PASS = 17;
    private const int GROUND_LAYER_MASK = 1 << PHYS_LAYER_PLATFORM | 1 << RAIN_BLOCKING_PLATFORM_PHYS_LAYER | 1 << PHYS_LAYER_PLATFORM_FRUIT_PASS;

    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
        mOldGravityScale = mRigidbody.gravityScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Most of the following is collision checks and user input
        Vector2 vel = new Vector2();
        Collider2D collider = GetComponent<Collider2D>();
        float horiz = mPreventMovement ? 0f : Input.GetAxis("Horizontal");
        bool sideBlocked = false;
        if(horiz != 0f && !mStayClimbing)
        {
            float direction = Mathf.Sign(horiz);
            vel.x = (direction * mSpeed);
            RaycastHit2D[] sideHits = Physics2D.BoxCastAll(transform.position + new Vector3(direction * 0.95f * collider.bounds.extents.x, 0, 0), collider.bounds.size, 0, vel, GROUND_CHECK_DIST, 1 << PHYS_LAYER_BLOCKING_ENV);
            foreach(RaycastHit2D hit in sideHits)
            {
                if(hit.collider != null && hit.normal.x != 0f && hit.distance <= GROUND_CHECK_DIST && hit.collider != collider)
                {
                    sideBlocked = true;
                    break;
                }
            }
            
            if(sideBlocked)
            {
                Debug.Log("side blocked");
                vel.x = 0;
            }
        }

        float vert = mPreventMovement ? 0f : Input.GetAxis("Vertical");        
        {
            Collider2D climbable = Physics2D.OverlapBox(transform.position, collider.bounds.size, 0, 1 << PHYS_LAYER_CLIMBABLE);
            if(climbable != null)
            {
                Vector3 zAlignedPos = new Vector3(transform.position.x, transform.position.y, climbable.bounds.center.z);
                if(!mClimbing)
                {
                    bool initiateClimbing = (mJumpedFromClimbTimer <= 0f) && (climbable.bounds.Contains(zAlignedPos));
                    if(initiateClimbing)
                    {
                        mClimbing = true;
                        mStayClimbing = true;
                        mJumping = false;
                        gameObject.layer = PHYS_LAYER_CLIMBING;
                        mRigidbody.gravityScale = 0;
                    }
                }
                
                if(mClimbing)
                {
                    vel.y = vert == 0f ? 0f : Mathf.Sign(vert) * mClimbSpeed;
                }
            }
            else
            {
                mClimbing = false;
                gameObject.layer = PHYS_LAYER_DEFAULT;
                mRigidbody.gravityScale = mOldGravityScale;
            }
        }
        
        // jumping
        bool onGround = false;
        if (!mJumping) // Don't even bother with an on-ground check if the player is moving up a because they jumped
        {
            RaycastHit2D[] groundHits = Physics2D.BoxCastAll(transform.position, collider.bounds.size, 0, new Vector2(0, -1), GROUND_CHECK_DIST, GROUND_LAYER_MASK);
            foreach (RaycastHit2D hit in groundHits)
            {
                if (hit.collider != null && hit.normal.y > 0 && hit.distance <= GROUND_CHECK_DIST && hit.collider != collider)
                {
                    onGround = true;
                    break;
                }
            }
        }

        if(mJumpTimer > 0f)
        {
            mJumpTimer -= Time.fixedDeltaTime;
        }

        if(mJumpGraceTimeTimer > 0f)
        {
            mJumpGraceTimeTimer -= Time.fixedDeltaTime;
        }

        if(mOnGround && !onGround)
        {
            // just left the ground
            mJumpGraceTimeTimer = JUMP_GRACE_TIME;
        }

        bool jumpInput = mPreventMovement ? false : Input.GetButton("Jump");
        bool jump = jumpInput || vert > 0f;
        if(jump && ((mNumCloudsTouching > 0) || (mClimbing && horiz != 0f) || onGround || mJumpGraceTimeTimer > 0f) && mJumpTimer <= 0f)
        {
            mJumping = true;
            vel.y = jumpSpeed;
            mOnGround = false;
            mJumpTimer = 0.2f;

            if(mClimbing)
            {
                mJumpedFromClimbTimer = 0.1f;
                mClimbing = false;
                mRigidbody.gravityScale = mOldGravityScale;
            }
        }
        else if(!mClimbing)
        {
            vel.y = mRigidbody.velocity.y;
        }

        mOnGround = onGround;

        // Modified gravity and fall-speed while in clouds.
        // I want to slow down when I first "collide" with the clouds, but gravity will still make me slowly fall over time. 
        // (Don't mess with gravity while climbing)
        if (mRigidbody.gravityScale != 0f)
        {
            mRigidbody.gravityScale = (mNumCloudsTouching > 0 ? 0.2f : 1f) * mOldGravityScale;
            if (mNumCloudsTouching > 0)
            {
                float maxCloudFallSpeed = 4f;
                if (vel.y < -maxCloudFallSpeed)
                {
                    vel.y = -maxCloudFallSpeed;
                }
            }
        }

        mRigidbody.velocity = vel;
        if (mJumping) { mJumping = (mRigidbody.velocity.y > 0f); } // Stay in "jumping" state until velocity is <= 0f
    }

    private void Update()
    {
        if(mJumpedFromClimbTimer > 0f)
            mJumpedFromClimbTimer -= Time.deltaTime;
        
        // allow the player to keep holding a direction as they approach a climbable.
        // only register an attempt to leave the climbable if the player releases and then re-presses
        if(mStayClimbing && ((mPreventMovement ? 0f : Input.GetAxis("Horizontal")) == 0f || !mClimbing))
        {
            mStayClimbing = false;
        }
        
        // Update animator or sprite renderer
        Animator anim = GetComponentInChildren<Animator>();
        float stopThreshold = 0.1f;
        if (mRigidbody.velocity.x > stopThreshold)
        {
            SetFacingDirection(1);
        } else if (mRigidbody.velocity.x < -stopThreshold)
        {
            SetFacingDirection(-1);
        }
        
        anim.SetFloat("Speed", Mathf.Abs(mRigidbody.velocity.x));
        // TODO: if climbing, jumping, falling, etc can have multiple true, set the one that is most relevant to animation
        anim.SetBool("Climbing", mClimbing);
        anim.SetBool("Jumping", mJumping);
        anim.SetBool("Falling", !mOnGround && !mJumping && !mClimbing);

        // This is relevant to whether climbing animation should play or be paused.
        bool movingVertically = Mathf.Abs(mRigidbody.velocity.y) > 0.1f;
        anim.SetFloat("ClimbSpeed", movingVertically ? 1f : 0f);

        anim.SetBool("HoldingBalloon", false);
    }

    private void SetFacingDirection(int facingDirection)
    {
        mFacing = facingDirection;
        transform.localScale = new Vector3(mFacing * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    public void EnterCloud()
    {
        mNumCloudsTouching++;
    }

    public void StayInCloud()
    {

    }
    public void ExitCloud()
    {
        mNumCloudsTouching--;
    }

    public void PreventMovement(bool prevent)
    {
        mPreventMovement = prevent;
    }
}
