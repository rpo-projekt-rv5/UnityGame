using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
     private const float movementSpeed = 2f;
     private const float sightRadius = 2f;
     private const float attackTriggerRadius = 0.8f;
     private int attackDamage = 10;
     private float attackRadius;
     private PlayerMovement player;
     private Transform target;
     private SpriteRenderer sprite;
     private Rigidbody2D enemy;
     private Animator animator;
    [SerializeField]
     private Transform attackPosition;
    [SerializeField]
     private LayerMask playerLayer;

     private PlayerCombat playerCombat;
     private int maxHealth = 100;
     private int currentHealth;
     private const float attackCooldown = 1.2f;
     private float lastAttack = Mathf.NegativeInfinity;
     private bool isAttacking;

     private void Start()
     {
          player = GameObject.Find("Player").GetComponent<PlayerMovement>();
          playerCombat = GameObject.Find("Player").GetComponent<PlayerCombat>();
          sprite = GetComponent<SpriteRenderer>();
          enemy = GetComponent<Rigidbody2D>();
          animator = GetComponent<Animator>();
          currentHealth = maxHealth;
          isAttacking = false;
     }

     private void Update()
     {
          Attack();
     }

     private void FixedUpdate()
     {
          if(player.GetX() > enemy.position.x && IsPlayerInSight())
          {
               sprite.flipX = false;
          }
          else if(player.GetX() < enemy.position.x && IsPlayerInSight())
          {
               sprite.flipX = true;
          }
          if(!isAttacking)
          {
               Move();
          }
     }

     private void Move()
     {
          if(IsPlayerInSight())
          {
               // se premika v desno proti igralcu
               if(player.GetX() > enemy.position.x)
               {
                    enemy.velocity = new Vector2(movementSpeed, enemy.velocity.y);
               }
               // se premika v levo proti igralcu
               else
               {
                    enemy.velocity = new Vector2(-movementSpeed, enemy.velocity.y);
               }
               animator.SetBool("playerInSight", true);
               animator.SetFloat("speed", movementSpeed);
          }
          else
          {
               animator.SetBool("playerInSight", false);
               animator.SetFloat("speed", 0f);
          }
     }

     private bool IsPlayerInSight()
     {
          if(Mathf.Abs(player.GetX() - enemy.position.x) > sightRadius)
          {
               return false;
          }
          return true;
     }

    private void Attack()
    {
        if(!isAttacking && lastAttack <= Time.time - attackCooldown && IsPlayerInRange())
        {
            lastAttack = Time.time;
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            animator.SetFloat("speed", 0f);
        }
    }

    private void AttackAnimation()
    {
        animator.SetBool("isAttacking", false);
        animator.SetFloat("speed", movementSpeed);
        isAttacking = false;
    }

    private bool IsPlayerInRange()
    {
        if(Mathf.Abs(player.GetX() - enemy.position.x) > attackTriggerRadius)
        {
            return false;
        }
        return true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("I AM DEAD");
        currentHealth = maxHealth;
    }
}
