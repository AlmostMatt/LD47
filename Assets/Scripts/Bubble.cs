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
    private float mSquashFactor = 1f;
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
            transform.localScale = new Vector3(scale, mSquashFactor * scale, scale);
        } else
        {
            transform.localScale = new Vector3(1, mSquashFactor, 1);
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

        if(transform.position.y > 60)
        {
            Pop();
            return;
        }

        // Visual effect of wobbling or spinning over time
        float wobbleSpeed = 1.5f;
        float wobbleAmount = 30f;
        transform.GetChild(0).localEulerAngles = new Vector3(0f,0f, wobbleAmount * Mathf.Sin(wobbleSpeed * Time.time));
        // Visual effect of popping soon
        float popAnimationDuration = 1f;
        if (mPopTimer > 0f && mPopTimer < popAnimationDuration)
        {
            float popTimerFraction = mPopTimer / popAnimationDuration;
            Vector3 popCol = new Vector3(212f / 255f, 155f / 255f, 199f / 255f);
            Vector3 normalCol = new Vector3(1f,1f,1f);
            float alpha = 0.3f + (0.7f * popTimerFraction);
            Vector3 currentColor = (1f - popTimerFraction) * popCol + (popTimerFraction) * normalCol;
            GetComponentInChildren<SpriteRenderer>().color = new Color(currentColor.x, currentColor.y, currentColor.z, alpha);
            mSquashFactor = 0.9f + (0.1f * popTimerFraction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(mPopTimer < 0f)
            {
                mPopTimer = 1f;
            }
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
