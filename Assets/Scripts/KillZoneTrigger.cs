using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("KIllZone");
        if(other.tag == "Player") other.GetComponent<CharacterBehaviour>().Die();
        else Destroy(other.gameObject);
    }
}
