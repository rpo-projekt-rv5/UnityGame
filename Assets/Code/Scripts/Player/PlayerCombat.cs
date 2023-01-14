using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private int maxHealth = 120;
    private int currentHealth;

    [SerializeField]
    private float inputTimer, meleeAttackRadius;
    [SerializeField]
    private int meleeAttackDamage = 20;
    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private LayerMask enemyLayers;

    public Transform launchOffset;
    public Projectile crescentProjectile;
    private const float meleeAttackCooldown = 0.4f;
    private const float crescentAttackCooldown = 1f;
    private float meleeLastInput = Mathf.NegativeInfinity;
    private float crescentLastInput = Mathf.NegativeInfinity;
    private bool isAttacking;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        CheckCombatInput();
    }

    private void CheckCombatInput()
    {
        MeleeAttack();
        CrescentAttack();
    }

    private void MeleeAttack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(!isAttacking && meleeLastInput <= Time.time - meleeAttackCooldown)
            {
                meleeLastInput = Time.time;
                isAttacking = true;
                animator.SetBool("meleeAttack", true);
                animator.SetBool("isAttacking", true);
            }
        }
    }

    private void CrescentAttack()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if(!isAttacking && crescentLastInput <= Time.time - crescentAttackCooldown)
            {
                crescentLastInput = Time.time;
                isAttacking = true;
                animator.SetBool("crescentAttack", true);
                animator.SetBool("isAttacking", true);
            }
        }
    }

    private void CheckMeleeAttackHitBox()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(meleeAttackPosition.position, meleeAttackRadius, enemyLayers);

        foreach (Collider2D enemy in hits)
        {
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(meleeAttackDamage);
        }
    }

    private void Fire()
    {
        Instantiate(crescentProjectile, launchOffset.position, transform.rotation);
    }

    // event, ki se izvede na zadnjem frame-u animacije
    // za končanje animacije
    private void MeleeAttackAnimation()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("meleeAttack", false);
        isAttacking = false;
    }

    // event, ki se izvede na zadnjem frame-u animacije
    // za končanje animacije
    private void CrescentAttackAnimation()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("crescentAttack", false);
        isAttacking = false;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Debug.Log(currentHealth);
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("I AM DEAD");
        currentHealth = maxHealth;
    }
}
