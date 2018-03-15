using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    public EnemyBehaviour enemy;

    public enum State { Default, Dead, God }
    public State state = State.Default;

    [Header("State")]
    public bool life = true;
    public bool canMove = true;
    public bool canJump = true;
    public bool isFacingRight = true;
    public bool isJumping = false;
    public bool isRunning = false;
    public bool isLaddering = false;
    public bool attack = false;
    public bool crouch = false;
    public bool isLookingUp = false;
    public bool isLookingDown = false;
    [Header("Physics")]
    public Rigidbody2D rb;
    public Collisions collisions;
    private float gravity;
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
    public float jumpVelocity;
    [Header("Graphics")]
    public SpriteRenderer rend;
    // Use this for initialization
    void Start()
    {
        collisions = GetComponent<Collisions>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBehaviour>();
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
                DeadUpdate();
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

        transform.position += new Vector3(horizontalSpeed * Time.deltaTime, jumpVelocity * Time.deltaTime, 0);
    }

    protected virtual void DefaultUpdate()
    {
        HorizontalMovement();

        if(isLaddering)
        {
            VerticalMovement();
        }
    }

    protected virtual void DeadUpdate()
    {
        /*if (collisions.isTouchingPlayer)
        {
            Debug.Log("PlayerDead");
            life = false;
        }*/
    }

    void HorizontalMovement()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (collisions.isTouchingWall)
        {
            if (isFacingRight && axis.x > 0.1f)
            {
                if (horizontalSpeed > 0.1f) horizontalSpeed = 0;
            }

            if (!isFacingRight && axis.x < -0.1f)
            {
                if (horizontalSpeed < -0.1f) horizontalSpeed = 0;
            }
        }

        if (!canMove) horizontalSpeed = 0;

        if (isFacingRight && axis.x < -0.1f) Flip();
        if (!isFacingRight && axis.x > 0.1f) Flip();

        if (isRunning) movementSpeed = runSpeed;
        else movementSpeed = walkSpeed;

        if (collisions.isGrounded) horizontalSpeed = movementSpeed * axis.x;
        if (collisions.justGotGrounded) jumpVelocity = 0;

        if(isLaddering) jumpVelocity = axis.y;

        if (!collisions.isGrounded)
        {
            jumpVelocity -= 0.2f;
            if ((axis.x < 0.1f) || (axis.x > -0.1f)) horizontalSpeed += movementSpeed * axis.x / 20;
            if (horizontalSpeed > movementSpeed && isFacingRight) horizontalSpeed = movementSpeed;
            if (horizontalSpeed < movementSpeed * -1 && !isFacingRight) horizontalSpeed = movementSpeed * -1;

        }
    }

    void VerticalMovement()
    {
        if((axis.y > 0.1f) || (axis.y < 0.1f))
        {
            this.transform.position += new Vector3(0, axis.y * 0.05f, 0);
        }
    }

    public void Attack()
    {
        if(collisions.isTouchingEnemy)
        {
            Debug.Log("PlayerAttack");
            enemy.Dead();
        }
    }

    void Flip()
    {
        rend.flipX = !rend.flipX;
        isFacingRight = !isFacingRight;
        collisions.Flip();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Ladder")
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            isLaddering = true;
            canJump = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Ladder")
        {
            rb.gravityScale = gravity;
            isLaddering = false;
            canJump = true;
        }
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
            jumpVelocity = jumpForce;
        }
        else if ((collisions.isTouchingWall) && ((axis.x > 0.5) || (axis.x < -0.5)))
        {
            if (isRunning) jumpForce = jumpRunForce;
            else jumpForce = jumpWalkForce;
            jumpVelocity = 2;
            if (axis.x > 0.5f)
            {
                horizontalSpeed = -7;
            }
            if (axis.x < -0.5f)
            {
                horizontalSpeed = 7;
            }
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
