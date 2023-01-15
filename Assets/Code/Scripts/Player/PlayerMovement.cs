using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D player;
    private PlayerCombat playerCombat;
    private SpriteRenderer sprite;
    private Animator animator;

    private float deltaX;
    private bool isFlipped;
    // se lahko kasneje spremeni, glede na potrebo oziroma ustreznost
    private const float movementSpeed = 4f;
    private const float jumpSpeed = 4f;
    private const float crouchSpeed = 2.5f;
    // dodaj prosim to, da igra preverja, �e je igralec na tleh; �e je, lahko sko�i, �e ne, ne more sko�iti,
    // ker druga�e lahko igralec v nedogled ska�e
    // lahko �e proba� dodat tud double jump, wall slide, pa kako dashanje :D
    // - Gal
    private bool isGrounded;
    private bool isCrouching;
    // za double jump
    // če je jumpCount > 1, potem ne moremo več skočiti
    private int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        // potrebno za fiziko - za zaznavanje trkov z NPC-ji, okoljem,...
        boxCollider = GetComponent<BoxCollider2D>();
        player = GetComponent<Rigidbody2D>();
        playerCombat = GetComponent<PlayerCombat>();
        // to je za renderanje sprite-a in animacijo
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isGrounded = true;
        isCrouching = false;
        isFlipped = false;
        jumpCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // premik igralca na X os, glede na input
        deltaX = Input.GetAxisRaw("Horizontal");
        // skok
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            animator.SetBool("isJumping", !isGrounded);
        }
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Crouch();
            animator.SetBool("isCrouching", isCrouching);
        }
        // sproži animacijo za tekanje
        animator.SetFloat("speed", Mathf.Abs(player.velocity.x));
        // premik igralca
        if(!playerCombat.IsAttacking())
        {
            Move();
            Flip();
        }
        // �e igralec napade, potem se ustavi na nekem mestu
        else
        {
            AttackMotion();
        }
    }

    private void Flip()
    {
        if(deltaX < 0)
        {
            player.transform.localScale = new Vector2(-1, 1);
            isFlipped = true;
        }
        else if(deltaX > 0)
        {
            player.transform.localScale = Vector2.one;
            isFlipped = false;
        }
    }

    private void Jump()
    {
        jumpCount++;
        if(jumpCount > 2)
        {
            return;
        }
        player.velocity = new Vector2(player.velocity.x, jumpSpeed);
    }

    private void Crouch()
    {
        if(!isGrounded)
        {
            return;
        }
        isCrouching = !isCrouching;
    }

    private void Move()
    {
        if(!playerCombat.IsHurt())
        {
            if(isCrouching)
            {
                player.velocity = new Vector2(deltaX * crouchSpeed, player.velocity.y);
            }
            else
            { 
                player.velocity = new Vector2(deltaX * movementSpeed, player.velocity.y);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.GetType());
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            // če je igralec desno od nasprotnika
            if (player.position.x > enemy.GetX())
            {
                GetKnockbacked(enemy.GetKnockbackLength(), enemy.GetKnockbackSpeed());
            }
            else
            {
                GetKnockbacked(-enemy.GetKnockbackLength(), enemy.GetKnockbackSpeed());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Grid"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Grid"))
        {
            isGrounded = false;
        }
    }

    private void AttackMotion()
    {
        player.velocity = new Vector2(0, player.velocity.y);
    }

    public float GetX()
    {
        return player.position.x;
    }

    public bool IsFacingRight()
    {
        return !isFlipped;
    }

    public Collider2D GetCollider()
    {
        return boxCollider;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsCrouching()
    {
        return isCrouching;
    }

    public void GetKnockbacked(float knockbackLength, float knockbackSpeed)
    {
        player.velocity = new Vector2(knockbackLength, knockbackSpeed);
    }
}
