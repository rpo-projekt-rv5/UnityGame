using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private const float movementSpeed = 2f;
    private const float sightRadius = 5f;
    private PlayerMovement player;
    private Rigidbody2D enemy;
    private BoxCollider2D boxCollider;
    
    private const float attackTriggerRadius = 0.8f;
    private const float knockbackLength = 2f;
    private const float knockbackSpeed = 2f;
    private int attackDamage = 30;
    private SpriteRenderer sprite;
    private Animator animator;
    [SerializeField]
    private float attackRadius = 0.5f;
    [SerializeField]
    private Transform attackPosition;
    [SerializeField]
    private LayerMask playerLayer;

    private PlayerCombat playerCombat;
    [SerializeField]
    private int maxHealth = 100;
    private int currentHealth;
    private const float attackCooldown = 1.5f;
    private float lastAttack = Mathf.NegativeInfinity;
    private bool isAttacking;
    private bool isHurt;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerCombat = GameObject.Find("Player").GetComponent<PlayerCombat>();
        boxCollider = GetComponent<BoxCollider2D>();
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
        // x in y v vektorju sta 2, ker je skeleton za 2x 'scale'-an
        // �e gleda desno
        if(player.GetX() < enemy.position.x && IsPlayerInSight())
        {
            enemy.transform.localScale = new Vector2(2, 2);
        }
        else if(player.GetX() > enemy.position.x && IsPlayerInSight())
        {
            enemy.transform.localScale = new Vector2(-2, 2);
        }
        if(!isAttacking && !isHurt)
        {
            Move();
        }
    }

    private void Move()
    {
         if(IsPlayerInSight() && !playerCombat.IsDead())
         {
              // se premika v desno proti igralcu
              if(player.GetX() > enemy.position.x)
              {
                   enemy.velocity = new Vector2(movementSpeed, enemy.velocity.y);
              }
              // se premika v levo proti igralcu
              else if(player.GetX() < enemy.position.x)
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
        if(!isAttacking && lastAttack <= Time.time - attackCooldown && IsPlayerInRange() && !playerCombat.IsDead())
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

    private void CheckMeleeHitBox()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, playerLayer);
        
        if(hits.Length > 0)
        {
            playerCombat.TakeDamage(attackDamage);
            Knockback();
        }
    }

    private void Knockback()
    {
        // �e je nasprotnik desno od igralca
        if(enemy.position.x > player.GetX())
        {
            player.GetKnockbacked(-knockbackLength, knockbackSpeed);
        }
        else
        {
            player.GetKnockbacked(knockbackLength, knockbackSpeed);
        }
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
        if(currentHealth <= 0)
        {
            DeathAnimation();
            return;
        }
        HurtAnimation();
    }

    private void HurtAnimation()
    {
        animator.SetTrigger("isHurt");
        animator.SetFloat("speed", 0f);
        animator.SetBool("isAttacking", false);
        isAttacking = false;
        isHurt = true;
    }

    private void HurtAnimationFinish()
    {
        isHurt = false;
    }
    
    private void DeathAnimation()
    {
        animator.SetTrigger("isDead");
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public float GetX()
    {
        return enemy.position.x;
    }

    public void GetKnockbacked(float knockbackLength, float knockbackSpeed)
    {
        enemy.velocity = new Vector2(knockbackLength, knockbackSpeed);
    }
        
    public float GetKnockbackLength()
    {
        return knockbackLength;
    }

    public float GetKnockbackSpeed()
    {
        return knockbackSpeed;
    }
}
