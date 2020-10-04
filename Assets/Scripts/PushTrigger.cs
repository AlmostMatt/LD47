using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushTrigger : MonoBehaviour
{
    public Vector2 force = new Vector2(0, 1);
    public float riseSpeed = 2f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        collider.attachedRigidbody.AddForce(force);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // other.attachedRigidbody.AddForce(force);

        other.attachedRigidbody.velocity = new Vector2(other.attachedRigidbody.velocity.x, Mathf.Max(riseSpeed, other.attachedRigidbody.velocity.y));
    }
}
