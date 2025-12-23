using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCleaner : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            enemy.OnHit(10000f);
        }
    }
}
