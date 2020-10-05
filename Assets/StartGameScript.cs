using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    AsyncOperation mAsync;

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
        if (Input.anyKeyDown)
        {
            ActivateScene();
        }
    }
}
