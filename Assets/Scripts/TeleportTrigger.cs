using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    private Vector3 destiny;
    //public Transform destiny;

    private void Start()
    {
        destiny = gameObject.GetComponentsInChildren<Transform>()[1].position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Teleport");
        other.transform.position = destiny;
        //other.transform.position = destiny.position;
    }
}
