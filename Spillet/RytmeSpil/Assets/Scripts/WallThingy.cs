using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallThingy : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("EnemyWall") || collision.CompareTag("Enemy"))
        {
            enemy.hitWall = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("EnemyWall") || collision.CompareTag("Enemy"))
        {
            enemy.hitWall = false;
        }
    }
}
