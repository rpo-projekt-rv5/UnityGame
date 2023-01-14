using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vert;
    private float speed = 8f;
    private bool isLadder;
    private bool isClimbing;

    [SerializeField] private Rigidbody2D rigBody;
    // Update is called once per frame
    void Update()
    {
        vert = Input.GetAxis("Vertical");

        if(isLadder && Mathf.Abs(vert) > 0f)
        {
            isClimbing = true;
        }
        
    }

    private void FixedUpdate()
    {
        if(isClimbing)
        {
            rigBody.gravityScale = 0f;
            rigBody.velocity = new Vector2(rigBody.velocity.x, vert * speed);
        }
        else
        {
            rigBody.gravityScale = 4f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
        
    }
}


