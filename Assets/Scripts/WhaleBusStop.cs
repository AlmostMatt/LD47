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

    private int mSummoningWhale = 0;
    private float mStageTime = 0f;
    private float mStageTimer = 0f;

    private float mCameraInitialSize;

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
                    break;
                }
            case 3:
                {
                    Instantiate(whale, whaleSpawnPoint.transform.position, whaleSpawnPoint.transform.rotation);
                    ParticleSystem[] pss = whaleSpawnPoint.GetComponentsInChildren<ParticleSystem>();
                    foreach(ParticleSystem ps in pss)
                    {
                        ps.Play();
                    }
                    break;
                }
        }

        if(stageDone)
        {
            ++mSummoningWhale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(mSummoningWhale > 0) return;

        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().PreventMovement(true);
            mCameraInitialSize = playerCam.GetComponent<Camera>().orthographicSize;
            mSummoningWhale = 1;
            mStageTime = cameraZoomOutTime;
        }
    }
}
