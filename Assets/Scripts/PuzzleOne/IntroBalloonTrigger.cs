using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBalloonTrigger : MonoBehaviour
{
    public bool isEnding = false;
    public Vector3 holdOffset;
    public GameObject playerCam;

    private bool mTriggered = false;

    private float mStopCameraTimer = -1f;

    private void Update()
    {
        if(mStopCameraTimer > 0f)
        {
            mStopCameraTimer -= Time.deltaTime;
            if(mStopCameraTimer <= 0f)
            {
                playerCam.GetComponent<CameraMovement>().StopFollowingPlayer();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !mTriggered)
        {
            if(isEnding)
            {
                Transform balloon = transform.GetChild(0);
                balloon.position = collision.transform.position + holdOffset;
                collision.transform.SetParent(balloon);
                collision.GetComponent<Rigidbody2D>().simulated = false;
                collision.GetComponent<Player>().PreventMovement(true);
                collision.GetComponent<Player>().HoldBalloon();

                mStopCameraTimer = 3f;
            }

            mTriggered = true;
            GetComponentInChildren<BalloonMovement>().StartMoving();
        }
    }
}
