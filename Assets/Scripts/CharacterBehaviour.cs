using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBehaviour : MonoBehaviour
{
    public enum State { Prepare, Default, Dead, Pause }
    public State state = State.Prepare;

    [Header("State")]
    public bool isDead = false;
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
    [Header("Scores")]
    public float score;
    public int hiScore;
    public Text scoreText;
    public Text scorePauseText;
    public Text hiScoreText;
    public Text hiScorePauseText;
    public bool pause = false;
    private float startTime = 5;
    public Animator canvasAnimator;
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
        switch(state)
        {
            case State.Prepare:
                PrepareUpdate();
                break;
            case State.Default:
                DefaultUpdate();
                break;
            case State.Dead:
                DeadUpdate();
                break;
            case State.Pause:
                // TODO: PauseUpdate();
                break;
            default:
                break;
        }


        //SCORE
        if(Mathf.RoundToInt(score) < 10) scoreText.text = "00" + Mathf.RoundToInt(score).ToString();
        else if(Mathf.RoundToInt(score) < 100) scoreText.text = "0" + Mathf.RoundToInt(score).ToString();
        else scoreText.text = Mathf.RoundToInt(score).ToString();

        if(Mathf.RoundToInt(hiScore) < 10) hiScoreText.text = "00" + Mathf.RoundToInt(hiScore).ToString();
        else if(Mathf.RoundToInt(hiScore) < 100) hiScoreText.text = "0" + Mathf.RoundToInt(hiScore).ToString();
        else hiScoreText.text = Mathf.RoundToInt(score).ToString();
        
        if(score >= hiScore) hiScore = Mathf.RoundToInt(score);

        hiScorePauseText.text = hiScoreText.text;
        scorePauseText.text = scoreText.text;

        if(Mathf.RoundToInt(hiScore) < 10) hiScoreText.text = "High Score 00" + Mathf.RoundToInt(hiScore).ToString();
        else if(Mathf.RoundToInt(hiScore) < 100) hiScoreText.text = "High Score 0" + Mathf.RoundToInt(hiScore).ToString();
        else hiScoreText.text = "High Score " + Mathf.RoundToInt(score).ToString();
    }

    private void FixedUpdate()
    {
        collisions.MyFixedUpdate();

        transform.position += new Vector3(horizontalSpeed * Time.deltaTime, jumpVelocity * Time.deltaTime, 0);
    }

    protected virtual void DefaultUpdate()
    {
        HorizontalMovement();
        score += Time.deltaTime;
        if(isLaddering)
        {
            VerticalMovement();
        }
    }

    protected virtual void DeadUpdate()
    {
        horizontalSpeed = 0;
        canJump = false;
    }

    protected virtual void PrepareUpdate()
    {
        startTime -= Time.deltaTime;
        if(startTime <= 0) state = State.Default;
        HorizontalMovement();
        horizontalSpeed = 0;
    }

    void HorizontalMovement()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if(collisions.isTouchingWall)
        {
            if(isFacingRight && axis.x > 0.1f)
            {
                if(horizontalSpeed > 0.1f) horizontalSpeed = 0;
            }

            if(!isFacingRight && axis.x < -0.1f)
            {
                if(horizontalSpeed < -0.1f) horizontalSpeed = 0;
            }
        }

        if(!canMove) horizontalSpeed = 0;

        if(isFacingRight && axis.x < -0.1f) Flip();
        if(!isFacingRight && axis.x > 0.1f) Flip();

        if(isRunning) movementSpeed = runSpeed;
        else movementSpeed = walkSpeed;

        if(collisions.isGrounded) horizontalSpeed = movementSpeed * axis.x;
        if(collisions.justGotGrounded) jumpVelocity = 0;

        if(isLaddering) jumpVelocity = axis.y;

        if(!collisions.isGrounded)
        {
            jumpVelocity -= 0.2f;
            if((axis.x < 0.1f) || (axis.x > -0.1f)) horizontalSpeed += movementSpeed * axis.x / 20;
            if(horizontalSpeed > movementSpeed && isFacingRight) horizontalSpeed = movementSpeed;
            if(horizontalSpeed < movementSpeed * -1 && !isFacingRight) horizontalSpeed = movementSpeed * -1;

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
            collisions.currentEnemy.Dead();
            //enemy.Dead();
            score += 5;
        }
    }

    public void Pause()
    {
        pause = !pause;
        if(pause)
        {
            state = State.Pause;
            canvasAnimator.SetTrigger("Pause");
        }
        else
        {
            state = State.Default;
            canvasAnimator.SetTrigger("Unpause");
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
        if(!isDead)
        {
            if(other.tag == "Ladder")
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
                isLaddering = true;
                canJump = false;
            }

            if(other.tag == "Enemy")
            {
                if(other.GetComponent<EnemyBehaviour>().isDead == false)
                {
                    Debug.Log("TAS MORIO PRIMOGAO");
                    state = State.Dead;
                    canvasAnimator.SetTrigger("Die");
                    isDead = true;
                }
            }

            if (other.tag == "Coin")
            {
                score += 50;
                other.gameObject.SetActive(false);
            }
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
        if(!canJump) return;

        if(collisions.isGrounded)
        {
            if(crouch)
            {
                Debug.Log("bajar plataforma");
            }

            if(isRunning) jumpForce = jumpRunForce;
            else jumpForce = jumpWalkForce;
            jumpVelocity = jumpForce;
        }
        else if((collisions.isTouchingWall) && ((axis.x > 0.5) || (axis.x < -0.5)))
        {
            if(isRunning) jumpForce = jumpRunForce;
            else jumpForce = jumpWalkForce;
            jumpVelocity = 2;
            if(axis.x > 0.5f)
            {
                horizontalSpeed = -7;
            }
            if(axis.x < -0.5f)
            {
                horizontalSpeed = 7;
            }
        }


    }
    #endregion
}
