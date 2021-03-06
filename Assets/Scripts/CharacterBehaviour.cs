﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBehaviour : MonoBehaviour
{
    public enum State { Prepare, Default, Dead, Pause }
    public State state = State.Prepare;

    public Animator animPlayer;
    private AudioPlayer audioPlayer;

    [Header("State")]
    public bool isDead = false;
    public bool canMove = true;
    public bool canJump = false;
    public bool isFacingRight = true;
    public bool isJumping = false;
    public bool isRunning = false;
    public bool isLaddering = false;
    public bool touchLadder = false;
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
    private float startTime = 4;
    public Animator canvasAnimator;
    [Header("Graphics")]
    public SpriteRenderer rend;
    public SpriteRenderer sword;

    // Use this for initialization

    void Start()
    {
        collisions = GetComponent<Collisions>();
        rb = GetComponent<Rigidbody2D>();
        GameData.LoadGame(1);
        hiScore = GameData.gameState.score;
        animPlayer = GetComponent<Animator>();
        audioPlayer = GetComponentInChildren<AudioPlayer>();
        audioPlayer.PlayMusic(0);
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
        else hiScoreText.text = Mathf.RoundToInt(hiScore).ToString();
        
        if(score >= hiScore) hiScore = Mathf.RoundToInt(score);

        hiScorePauseText.text = hiScoreText.text;
        scorePauseText.text = scoreText.text;

        if(Mathf.RoundToInt(hiScore) < 10) hiScoreText.text = "High Score 00" + Mathf.RoundToInt(hiScore).ToString();
        else if(Mathf.RoundToInt(hiScore) < 100) hiScoreText.text = "High Score 0" + Mathf.RoundToInt(hiScore).ToString();
        else hiScoreText.text = "High Score " + Mathf.RoundToInt(hiScore).ToString();
        
    }

    private void FixedUpdate()
    {
        collisions.MyFixedUpdate();

        if (!pause) transform.position += new Vector3(horizontalSpeed * Time.deltaTime, jumpVelocity * Time.deltaTime, 0);
    }

    protected virtual void DefaultUpdate()
    {
        HorizontalMovement();
        score += Time.deltaTime;

        if (touchLadder)
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                isLaddering = true;
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                isLaddering = false;
            }
        }

        if(isLaddering)
        {
            VerticalMovement();
        }
    }

    protected virtual void DeadUpdate()
    {
        horizontalSpeed = 0;
        canJump = false;

        if(!collisions.isGrounded)
        {
            jumpVelocity -= 0.2f;
            if((axis.x < 0.1f) || (axis.x > -0.1f)) horizontalSpeed += movementSpeed * axis.x / 20;
            if(horizontalSpeed > movementSpeed && isFacingRight) horizontalSpeed = movementSpeed;
            if(horizontalSpeed < movementSpeed * -1 && !isFacingRight) horizontalSpeed = movementSpeed * -1;

        }
    }

    protected virtual void PrepareUpdate()
    {
        startTime -= Time.deltaTime;
        canJump = false;
        if(startTime <= 0)
        {
            state = State.Default;
            canJump = true;
        }
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

        if(isRunning)
        {
            movementSpeed = runSpeed;
            //animPlayer.SetBool("run", true);
        }
        else
        {
            movementSpeed = walkSpeed;
            //animPlayer.SetBool("run", false);
        }

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
        rb.velocity = new Vector2 (0, 0);
        if((axis.y > 0.1f) || (axis.y < 0.1f))
        {
            this.transform.position += new Vector3(0, axis.y * 0.05f, 0);
        }
    }

    public void Attack()
    {
        if(state == State.Default)
        {
            if (isFacingRight) animPlayer.SetTrigger("Attack");
            else animPlayer.SetTrigger("AttackFlip");

            audioPlayer.PlaySFX(2, 1, Random.Range(0.9f, 1.1f));

            if(collisions.isTouchingEnemy)
            {
                Debug.Log("PlayerAttack");
                if(!collisions.currentEnemy.isDead) score += 10;
                collisions.currentEnemy.Dead();
                Destroy(collisions.currentEnemy.gameObject, 9);
            }
        }
    }

    public void Pause()
    {
        pause = !pause;
        if(pause)
        {
            state = State.Pause;
            canvasAnimator.SetTrigger("Pause");
            canJump = false;
        }
        else
        {
            state = State.Default;
            canvasAnimator.SetTrigger("Unpause");
            canJump = true;
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
                touchLadder = true;
            }

            if(other.tag == "Enemy")
            {
                if(other.GetComponent<EnemyBehaviour>().isDead == false)
                {
                    Die();
                }
            }

            if (other.tag == "Coin")
            {
                if(other.GetComponent<CoinBehaviour>().grabbable)
                {
                    score += 50;
                    audioPlayer.PlaySFX(1, 1, Random.Range(0.9f, 1.1f));
                    other.GetComponent<CoinBehaviour>().ResetCoin();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Ladder")
        {
            touchLadder = false;
            isLaddering = false;
        }
    }

    #region Public
    public void SetAxis(Vector2 inputAxis)
    {
        axis = inputAxis;
    }

    public void Die()
    {
        audioPlayer.PlaySFX(3, 1, Random.Range(0.9f, 1.1f));
        Debug.Log("TAS MORIO PRIMOGAO");
        state = State.Dead;
        canvasAnimator.SetTrigger("Die");
        animPlayer.SetTrigger("Die");
        isDead = true;
        canJump = false;
        GameData.gameState.score = hiScore;
        GameData.SaveGame(1);
    }

    public void JumpStart() //Decidir como será el salto
    {
        if(!canJump) return;
        audioPlayer.PlaySFX(0, 1, Random.Range(0.9f, 1.1f));
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
