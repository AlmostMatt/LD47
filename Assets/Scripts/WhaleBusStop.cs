using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleBusStop : MonoBehaviour
{
    public GameObject whale;
    public GameObject whaleSpawnPoint;
    public GameObject playerCam;
    public float newCameraSize = 6f;
    public float cameraZoomOutTime = 2f;
    public float whaleSpawnDelay = 1f;
    public float whaleRideTime = 10f;
    public Vector2 blowForce = new Vector2(0, 100);

    private GameObject mWhale;

    private int mSummoningWhale = 0;
    private float mStageTime = 0f;
    private float mStageTimer = 0f;

    private float mCameraInitialSize;
    private GameObject mPlayer;

    private void Start()
    {
        if(GetComponentInParent<Seasonal>().Season != Season.FALL)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(mSummoningWhale <= 0) return;

        bool stageDone = false;
        mStageTimer += Time.deltaTime;
        if(mStageTimer >= mStageTime)
        {
            stageDone = true;
        }

        switch(mSummoningWhale)
        {
            case 1:
                {
                    playerCam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(mCameraInitialSize, newCameraSize, mStageTimer / mStageTime);
                    if(stageDone)
                    {
                        mStageTime = whaleSpawnDelay;
                    }
                    break;
                }
            case 2:
                {
                    if(stageDone)
                    {
                        mStageTime = 0f;
                    }
                    break;
                }
            case 3:
                {
                    mWhale = Instantiate(whale, whaleSpawnPoint.transform.position, whaleSpawnPoint.transform.rotation);
                    ParticleSystem[] pss = whaleSpawnPoint.GetComponentsInChildren<ParticleSystem>();
                    foreach(ParticleSystem ps in pss)
                    {
                        ps.Play();
                    }
                    if(stageDone)
                    {
                        mStageTime = 100f;
                    }
                    break;
                }
            case 4:
                {
                    if(mWhale.transform.childCount > 3)
                    {
                        stageDone = true;
                        mStageTime = whaleRideTime;
                    }
                    break;
                }
            case 5:
                {
                    if(stageDone)
                    {
                        mStageTime = 0f;
                    }
                    break;
                }
            case 6:
                {
                    ParticleSystem ps = mWhale.GetComponentInChildren<ParticleSystem>();
                    ps.Play();
                    mPlayer.transform.SetParent(null);
                    mPlayer.GetComponent<Rigidbody2D>().simulated = true;
                    mPlayer.GetComponent<Rigidbody2D>().AddForce(blowForce);
                    mPlayer.GetComponent<Player>().PreventMovement(false);
                    mPlayer.transform.localEulerAngles = Vector3.zero;
                    mSummoningWhale = -1;
                    break;
                }
        }

        if(stageDone)
        {
            ++mSummoningWhale;
            mStageTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(mSummoningWhale > 0) return;

        if(collision.CompareTag("Player"))
        {
            mPlayer = collision.gameObject;
            collision.gameObject.GetComponent<Player>().PreventMovement(true);
            mCameraInitialSize = playerCam.GetComponent<Camera>().orthographicSize;
            mSummoningWhale = 1;
            mStageTime = cameraZoomOutTime;
        }
    }
}
