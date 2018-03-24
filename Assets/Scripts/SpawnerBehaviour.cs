using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    public Vector3 startPos;

    public GameObject rightWalkEnemy;
    public GameObject leftWalkEnemy;
    public GameObject rightFlyEnemy;
    public GameObject leftFlyEnemy;

    public float timeToRightWalk;
    public float timeToLeftWalk;
    public float timeToRightFly;
    public float timeToLeftFly;

    public float maxTimeToRightWalk;
    public float maxTimeToLeftWalk;
    public float maxTimeToRightFly;
    public float maxTimeToLeftFly;

    private void Start()
    {
        startPos = this.transform.position;
    }

    void Update ()
    {
        timeToRightWalk -= Time.deltaTime;
        timeToLeftWalk -= Time.deltaTime;
        timeToRightFly -= Time.deltaTime;
        timeToLeftFly -= Time.deltaTime;

        if(timeToRightWalk <= 0)
        {
            timeToRightWalk = maxTimeToRightWalk;
            Instantiate(rightWalkEnemy, startPos, new Quaternion(0, 0, 0, 0));
        }
        if(timeToLeftWalk <= 0)
        {
            timeToLeftWalk = maxTimeToLeftWalk;
            Instantiate(leftWalkEnemy, startPos, new Quaternion(0, 0, 0, 0));
        }
        if(timeToRightFly <= 0)
        {
            timeToRightFly = maxTimeToRightFly;
            Instantiate(rightFlyEnemy, startPos, new Quaternion(0, 0, 0, 0));
        }
        if(timeToLeftFly <= 0)
        {
            timeToLeftFly = maxTimeToLeftFly;
            Instantiate(leftFlyEnemy, startPos, new Quaternion(0, 0, 0, 0));
        }
    }
}
