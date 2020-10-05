using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudLayer : MonoBehaviour
{
    public static float CLOUD_HEIGHT = 65f;
    public void Start()
    {
        CLOUD_HEIGHT = transform.position.y;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.GetComponent<Player>();
        if (p != null)
        {
            p.EnterCloud();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Player p = collision.GetComponent<Player>();
        if (p != null)
        {
            p.StayInCloud();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player p = collision.GetComponent<Player>();
        if (p != null)
        {
            p.ExitCloud();
        }
    }
}
