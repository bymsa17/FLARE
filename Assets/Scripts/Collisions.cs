using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Collisions : MonoBehaviour
{
    #region Variables
    public EnemyBehaviour currentEnemy;
    [Header("Physics")]
    private Rigidbody2D _rb2D;
    private Collider2D _coll2D;
    private float gravityMagnitude = 1.50f;
    [Header("Permissions")]
    public bool checkGround = true;
    public bool checkCeiling = true;
    public bool checkWall = true;
    public bool checkAttack = true;
    [Header("State")]
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isTouchingCeiling;
    [HideInInspector] public bool isTouchingWall;
    public bool isTouchingEnemy;
    [HideInInspector] public bool isFalling;
    [HideInInspector] public bool wasGroundedLastFrame;
    [HideInInspector] public bool wasTouchingCeilingLastFrame;
    [HideInInspector] public bool wasTouchingWallLastFrame;
    [HideInInspector] public bool justGotGrounded;
    [HideInInspector] public bool justNotGrounded;
    [HideInInspector] public bool justTouchWall;
    [HideInInspector] public bool justTouchCeiling;
    [Header("Ground Filter")]
    public ContactFilter2D groundFilter;
    public ContactFilter2D ceilingFilter;
    public ContactFilter2D wallFilter;
    public ContactFilter2D attackFilter;
    public int maxGroundHits;
    private LayerMask _groundMaskSave;
    [Header("Ground Box")]
    public Vector2 bottomBoxSize;
    public Vector2 bottomBoxPos;    
    [Header("Ceiling Box")]
    public Vector2 topBoxSize;
    public Vector2 topBoxPos;
    [Header("Wall Box")]
    public Vector2 sideBoxSize;
    public Vector2 sideBoxPos;
    [Header("Attack Box")]
    public Vector2 attackBoxSize;
    public Vector2 attackBoxPos;
    #endregion

    public void MyStart()
    {
        _groundMaskSave = groundFilter.layerMask;

        _rb2D = GetComponent<Rigidbody2D>();
        _coll2D = GetComponent<Collider2D>();

        _rb2D.gravityScale = gravityMagnitude;
    }

    public void MyFixedUpdate()
    {
        ResetState();

        if(checkGround) GroundCollision();
        if(checkCeiling) CeilingCollision();
        if(checkWall) WallCollision();
        if(checkAttack) AttackCollisions();
    }

    private void ResetState()
    {
        wasGroundedLastFrame = isGrounded;
        wasTouchingCeilingLastFrame = isTouchingCeiling;
        wasTouchingWallLastFrame = isTouchingWall;

        isGrounded = false;
        isTouchingWall = false;
        isTouchingCeiling = false;
        isTouchingEnemy = false;
        currentEnemy = null;

        justGotGrounded = false;
        justNotGrounded = false;
        justTouchCeiling = false;
        justTouchWall = false;

        isFalling = true;
    }
    private void GroundCollision()
    {
        Collider2D[] results = new Collider2D[maxGroundHits];
        Vector2 pos = this.transform.position;
        int hits = Physics2D.OverlapBox(pos + bottomBoxPos, bottomBoxSize, 0, groundFilter, results);

        if(hits > 0)
        {
            isGrounded = true;
        }

        if(!wasGroundedLastFrame && isGrounded) justGotGrounded = true;
        if(wasGroundedLastFrame && !isGrounded) justNotGrounded = true;
        if(isGrounded) isFalling = false;
    }
    private void CeilingCollision()
    {
        Collider2D[] results = new Collider2D[maxGroundHits];
        Vector2 pos = this.transform.position;
        int hits = Physics2D.OverlapBox(pos + topBoxPos, topBoxSize, 0, ceilingFilter, results);

        if(hits > 0)
        {
            isTouchingCeiling = true;
        }

        if(!wasTouchingCeilingLastFrame && isTouchingCeiling) justTouchCeiling = true;
    }
    private void WallCollision()
    {
        Collider2D[] results = new Collider2D[maxGroundHits];
        Vector2 pos = this.transform.position;
        int hits = Physics2D.OverlapBox(pos + sideBoxPos, sideBoxSize, 0, wallFilter, results);

        if(hits > 0)
        {
            isTouchingWall = true;
        }

        if(!wasTouchingWallLastFrame && isTouchingWall) justTouchWall = true;
    }

    private void AttackCollisions()
    {
        Collider2D[] result = new Collider2D[maxGroundHits];
        Vector2 pos = this.transform.position;
        int hits = Physics2D.OverlapBox(pos + attackBoxPos, attackBoxSize, 0, attackFilter, result);

        if(hits > 0)
        {
            currentEnemy = result[0].GetComponent<EnemyBehaviour>();
            isTouchingEnemy = true;
        }
    }

    public void Flip()
    {
        sideBoxPos.x *= -1;
        attackBoxPos.x *= -1;
    }   

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 pos = this.transform.position;
        Gizmos.DrawWireCube(pos + bottomBoxPos, bottomBoxSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos + topBoxPos, topBoxSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos + sideBoxPos, sideBoxSize);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(pos + attackBoxPos, attackBoxSize);
    }

}
