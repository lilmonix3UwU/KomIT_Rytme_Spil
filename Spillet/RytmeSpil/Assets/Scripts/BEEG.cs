using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BEEG : MonoBehaviour
{
    public bool ye = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ye = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ye = false;
        }
    }
}
