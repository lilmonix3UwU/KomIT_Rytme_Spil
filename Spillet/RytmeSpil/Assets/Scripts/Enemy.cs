using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float HP;

    void Start()
    {
        
    }

    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
