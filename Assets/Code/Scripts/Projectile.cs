using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float projectileSpeed = 4f;
    private PlayerMovement player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += transform.right * Time.deltaTime * projectileSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
