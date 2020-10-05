using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    AsyncOperation mAsync;
    private float mCountdown = 4f; // Auto start after this amount of time

    private void Start()
    {
        mAsync = SceneManager.LoadSceneAsync("PuzzleOne", LoadSceneMode.Single);
        mAsync.allowSceneActivation = false;
    }

    public void ActivateScene()
    {
        mAsync.allowSceneActivation = true;
    }

    void Update()
    {
        if (mCountdown > 0f)
        {
            mCountdown -= Time.deltaTime;
        }
        if (Input.anyKeyDown || mCountdown < 0f)
        {
            ActivateScene();
        }
    }
}
