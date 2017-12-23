using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    //public Vector2 position;
    public Transform position;
    private bool life;
    public float velocity;
    public bool direction;

	// Use this for initialization
	void Start ()
    {
        position = transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //this.transform.position = new Vector2(position.x += velocity * Time.deltaTime, this.transform.position.y);
        position.x += velocity * Time.deltaTime;
	}
}
