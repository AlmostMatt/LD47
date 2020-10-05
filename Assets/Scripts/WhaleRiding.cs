using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleRiding : MonoBehaviour
{
    private bool mActive = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!mActive) return;

        if(collision.CompareTag("Player"))
        {
            Debug.Log("ride the whale");
            collision.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            collision.transform.SetParent(transform.parent, true);
            mActive = false;
        }
    }
}
