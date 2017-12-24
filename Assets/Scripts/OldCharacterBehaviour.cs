using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCharacterBehaviour : MonoBehaviour 
{
    public enum State { Default, Dead, God }
    public State state = State.Default;

    [Header("State")]
    public bool canMove = true;
    public bool canJump = true;
    public bool isFacingRight = true;
    public bool isJumping = false;
    public bool isRunning = false;
    public bool crouch = false;
    public bool isLookingUp = false;
    public bool isLookingDown = false;
    [Header("Physics")]
    public Rigidbody2D rb;
    public Collisions collisions;
    [Header("Speed")]
    public float walkSpeed;
    public float runSpeed;
    public float movementSpeed;
    public float horizontalSpeed;
    public Vector2 axis;
    [Header("Forces")]
    public float jumpWalkForce;
    public float jumpRunForce;
    public float jumpForce;
    [Header("Graphics")]
    public SpriteRenderer rend;
    // Use this for initialization
    void Start()
    {
        collisions = GetComponent<Collisions>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Default:
                DefaultUpdate();
                break;
            case State.Dead:
                // TODO: DeadUpdate();
                break;
            case State.God:
                // TODO: GodUpdate();
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        collisions.MyFixedUpdate();

        if (isJumping)
        {
            isJumping = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        rb.velocity = new Vector2(horizontalSpeed, rb.velocity.y);
    }

    protected virtual void DefaultUpdate()
    {
        HorizontalMovement();
    }

    void HorizontalMovement()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (!canMove || (-0.1f < axis.x && axis.x < 0.1f))
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            horizontalSpeed = 0;
            return;
        }

        if (isFacingRight && axis.x < -0.1f) Flip();
        if (!isFacingRight && axis.x > 0.1f) Flip();

        if (isRunning) movementSpeed = runSpeed;
        else movementSpeed = walkSpeed;

        if (collisions.isGrounded) horizontalSpeed = movementSpeed * axis.x;
        if (!collisions.isGrounded)
        {
            if ((axis.x < 0.1f) || (axis.x > -0.1f)) horizontalSpeed += movementSpeed * axis.x / 15;
            if (horizontalSpeed > movementSpeed && isFacingRight) horizontalSpeed = movementSpeed;
            if (horizontalSpeed < movementSpeed * -1 && !isFacingRight) horizontalSpeed = movementSpeed * -1;

        }


        if ((collisions.isTouchingWall == true) && (collisions.isGrounded == false))
        {
            //Dash()
            if (isFacingRight) horizontalSpeed = -7;
            else horizontalSpeed = 7;
        }
    }

    void VerticalMovement()
    {
        crouch = false;
        isLookingDown = false;
        isLookingUp = false;
    }
    void Jump()
    {
        isJumping = true;
    }
    void Flip()
    {
        rend.flipX = !rend.flipX;
        isFacingRight = !isFacingRight;
        collisions.Flip();
    }

    #region Public
    public void SetAxis(Vector2 inputAxis)
    {
        axis = inputAxis;
    }

    public void JumpStart() //Decidir como será el salto
    {
        if (!canJump) return;

        if (collisions.isGrounded)
        {
            if (crouch)
            {
                Debug.Log("bajar plataforma");
            }

            if (isRunning) jumpForce = jumpRunForce;
            else jumpForce = jumpWalkForce;
            Jump();
        }


    }
    #endregion

    #region Sets
    public void SetGod()
    {



        state = State.God;
    }
    #endregion


}
