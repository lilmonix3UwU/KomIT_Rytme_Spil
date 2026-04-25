using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public bool playerDetected;
    [SerializeField] BEEG BEEG;

    private void Update()
    {
        if (BEEG.enabled)
        {
            playerDetected = BEEG.ye;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!BEEG.enabled)
            {
                playerDetected = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!BEEG.enabled)
            {
                playerDetected = false;
            }
        }
    }

    public void BigBox(bool on)
    {
        BEEG.enabled = on;
    }

}
