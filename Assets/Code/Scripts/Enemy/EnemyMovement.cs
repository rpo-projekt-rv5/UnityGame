using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
     private PlayerMovement player;
     private SpriteRenderer sprite;
     private Rigidbody2D enemy;

     private void Start()
     {
          player = GameObject.Find("Player").GetComponent<PlayerMovement>();
          sprite = GetComponent<SpriteRenderer>();
          enemy = GetComponent<Rigidbody2D>();
     }

     private void FixedUpdate()
     {
          if(player.GetX() > enemy.position.x)
          {
               sprite.flipX = false;
          }
          else if(player.GetX() < enemy.position.x)
          {
               sprite.flipX = true;
          }
     }
}
