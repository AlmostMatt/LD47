using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBalloonTrigger : MonoBehaviour
{
    private bool mTriggered = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !mTriggered)
        {
            mTriggered = true;
            GetComponentInChildren<BalloonMovement>().StartMoving();
        }
    }
}
