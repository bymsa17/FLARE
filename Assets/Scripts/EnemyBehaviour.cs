using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public enum State { Default, Dead }
    public State state = State.Default;
    public CharacterBehaviour player;

    [Header("State")]
    public bool canJump;
    public bool isPaused = false;
    public bool isPausedLastFrame = false;
    public bool isDead = false;
    public bool isFacingRight = true;
    public bool isJumping = false;
    [Header("Physics")]
    public Rigidbody2D rb;
    public Vector2 rbVelocity;
    public Collisions collisions;
    private float gravity;
    [Header("Speed")]
    public float walkSpeed;
    public float movementSpeed;
    public float horizontalSpeed;
    public Vector2 axis;
    [Header("Forces")]
    public float jumpWalkForce;
    public float jumpRunForce;
    public float jumpForce;
    public float jumpVelocity;
    [Header("Graphics")]
    public SpriteRenderer rend;
    public Animator animator;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehaviour>();
        collisions = GetComponent<Collisions>();
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if(isFacingRight == false)
        {
            collisions.sideBoxPos.x *= -1;
            collisions.attackBoxPos.x *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isPaused = player.pause;
        if(isPaused)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            if(isPausedLastFrame) rb.velocity = rbVelocity;
            rbVelocity = rb.velocity;
            rb.gravityScale = 1;
        }

        if(isPaused) isPausedLastFrame = true;
        else isPausedLastFrame = false;

        if(!isPaused)
        {
            switch(state)
            {
                case State.Default:
                    DefaultUpdate();
                    break;
                case State.Dead:
                    DeadUpdate();
                    break;
                default:
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        collisions.MyFixedUpdate();

        if (!isPaused) transform.position += new Vector3(horizontalSpeed * Time.deltaTime, jumpVelocity * Time.deltaTime, 0);
    }

    protected virtual void DefaultUpdate()
    {
        HorizontalMovement();
    }

    protected virtual void DeadUpdate()
    {
        horizontalSpeed = 0;

        if(!collisions.isGrounded)
        {
            jumpVelocity -= 0.2f;
            if((axis.x < 0.1f) || (axis.x > -0.1f)) horizontalSpeed += movementSpeed * axis.x / 20;
            if(horizontalSpeed > movementSpeed && isFacingRight) horizontalSpeed = movementSpeed;
            if(horizontalSpeed < movementSpeed * -1 && !isFacingRight) horizontalSpeed = movementSpeed * -1;

        }
    }

    void HorizontalMovement()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        if (isFacingRight) horizontalSpeed = walkSpeed;
        else horizontalSpeed = -walkSpeed;

        if(collisions.isTouchingWall)
        {
            Flip();
        }
        
        //if(collisions.isGrounded) horizontalSpeed = movementSpeed * axis.x;
        if(collisions.justGotGrounded) jumpVelocity = 0;

        if(!collisions.isGrounded)
        {
            jumpVelocity -= 0.2f;
            if((axis.x < 0.1f) || (axis.x > -0.1f)) horizontalSpeed += movementSpeed * axis.x / 20;
            if(horizontalSpeed > movementSpeed && isFacingRight) horizontalSpeed = movementSpeed;
            if(horizontalSpeed < movementSpeed * -1 && !isFacingRight) horizontalSpeed = movementSpeed * -1;

        }
    }

    void Flip()
    {
        rend.flipX = !rend.flipX;
        isFacingRight = !isFacingRight;
        collisions.Flip();
    }

    public void Dead()
    {
        if(!isDead)
        {
            state = State.Dead;
            isDead = true;
            animator.SetTrigger("Die");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Jumper")
        {
            if (canJump) rb.velocity = new Vector2(0, 4);
        }
    }

    public void SetAxis(Vector2 inputAxis)
    {
        axis = inputAxis;
    }

    public void JumpStart() //Decidir como será el salto
    {
        if(collisions.isGrounded)
        {
            jumpVelocity = jumpForce;
        }
    }
}