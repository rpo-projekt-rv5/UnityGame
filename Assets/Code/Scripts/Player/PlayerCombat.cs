using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private int maxHealth = 120;
    private int currentHealth;

    [SerializeField]
    private float inputTimer, meleeAttackRadius = 0.5f;
    [SerializeField]
    private int meleeAttackDamage = 20;
    [SerializeField]
    private Transform meleeAttackPosition;  
    [SerializeField]
    private LayerMask enemyLayers;

    private const float meleeAttackCooldown = 0.4f;
    private const float knockbackLength = 2f;
    private const float knockbackSpeed = 2f;
    private float meleeLastInput = Mathf.NegativeInfinity;
    private bool isAttacking;
    private bool isHurt;
    private bool isDead;
    private Animator animator;
    private PlayerMovement player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        CheckCombatInput();
    }

    // function checks combat input
    private void CheckCombatInput()
    {
        if(player.IsGrounded() && !player.IsCrouching())
        {
            MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(!isAttacking && meleeLastInput <= Time.time - meleeAttackCooldown)
            {
                meleeLastInput = Time.time;
                isAttacking = true;
                animator.SetBool("isAttacking", true);
            }
        }
    }

    private void CheckMeleeAttackHitBox()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(meleeAttackPosition.position, meleeAttackRadius, enemyLayers);

        foreach (Collider2D enemy in hits)
        {
            enemy.GetComponent<Enemy>().TakeDamage(meleeAttackDamage);
            Knockback(enemy);
        }
    }

    private void Knockback(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        // če je player desno od nasprotnika
        if(player.GetX() > enemy.GetX())
        {
            enemy.GetKnockbacked(-knockbackLength, knockbackSpeed);
        }
        else
        {
            enemy.GetKnockbacked(knockbackLength, knockbackSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackRadius);
    }

    // event, ki se izvede na zadnjem frame-u animacije
    // za končanje animacije
    private void MeleeAttackAnimation()
    {
        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        HurtAnimation();
        if(currentHealth <= 0)
        {
            DeathAnimation();
        }
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
        isDead = true;
        animator.SetTrigger("isDead");
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public bool IsHurt()
    {
        return isHurt;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
