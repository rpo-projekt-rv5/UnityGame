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
    // se lahko kasneje spremeni, glede na potrebo oziroma ustreznost
    private const float movementSpeed = 4f;
    private const float jumpSpeed = 5f;
    // dodaj prosim to, da igra preverja, �e je igralec na tleh; �e je, lahko sko�i, �e ne, ne more sko�iti,
    // ker druga�e lahko igralec v nedogled ska�e
    // lahko �e proba� dodat tud double jump, wall slide, pa kako dashanje :D
    // - Gal

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
    }

    // Update is called once per frame
    void Update()
    {
        // premik igralca na X os, glede na input
        deltaX = Input.GetAxisRaw("Horizontal");
        // skok
        if(Input.GetKeyDown(KeyCode.Space) && !playerCombat.IsAttacking())
        {
            Jump();
        }

        // sproži animacijo za tekanje
        animator.SetFloat("velocity", Mathf.Abs(player.velocity.x));
    }

    private void FixedUpdate()
    {
        // premik igralca
        if(!playerCombat.IsAttacking())
        {
            Move();
        }
        // �e igralec napade, potem se ustavi na nekem mestu
        else
        {
            AttackMotion();
        }
        // �e se premika v levo, se sprite prezrcali
        // treba je dodati, �e igralec napade, da se ne more zasukati
        if(deltaX < 0 && !playerCombat.IsAttacking())
        {
            sprite.flipX = true;
        }
        else if(deltaX > 0 && !playerCombat.IsAttacking())
        {
            sprite.flipX = false;
        }
    }

    private void Jump()
    {
        player.velocity = new Vector2(player.velocity.x, jumpSpeed);
    }

    private void Move()
    {
        player.velocity = new Vector2(deltaX * movementSpeed, player.velocity.y);
    }

    private void AttackMotion()
    {
        // tu se igralec ustavi na x osi haha
        player.velocity = new Vector2(0, player.velocity.y);
    }

    public float GetX()
    {
        return player.position.x;
    }

    public bool IsFacingRight()
    {
        return !sprite.flipX;
    }

    public Collider2D GetCollider()
    {
        return boxCollider;
    }
}
