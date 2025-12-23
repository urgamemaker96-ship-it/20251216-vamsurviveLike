using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    [SerializeField] private float damage;
    [SerializeField] private int per;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        // 원거리 무기라면
        if (per >= 0)
        {
            rigidbody2d.linearVelocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            enemy.OnHit(damage);
            
            // 원거리인 경우에만
            if (per >= 0)
            {
                per--;
                
                if (per < 0)
                {
                    rigidbody2d.linearVelocity = Vector2.zero;
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Area"))
        {
            rigidbody2d.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
