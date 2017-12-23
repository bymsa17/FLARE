using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject player;
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;
    public Vector3 offset;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset.z = -10;
    }

    void FixedUpdate()
    {
        if(player.GetComponent<CharacterBehaviour>().isFacingRight) offset.x = Mathf.Abs(offset.x);
        else offset.x = -Mathf.Abs(offset.x);

        Vector3 newPosition = player.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }
}
