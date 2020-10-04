using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float expandTime = 1f;

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
            Destroy(gameObject);
        }
    }
}
