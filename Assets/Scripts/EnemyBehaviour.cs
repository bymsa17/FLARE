using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public enum State { Default, Dead }
    public State state = State.Default;
    public CharacterBehaviour player;

    [Header("State")]
    public bool isPaused = false;
    public bool isDead = false;
    public bool isFacingRight = true;
    public bool isJumping = false;
    [Header("Physics")]
    public Rigidbody2D rb;
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
    }

    // Update is called once per frame
    void Update()
    {
        isPaused = player.pause;
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
        Debug.Log("Flip?");
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