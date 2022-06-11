using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private GameObject otherPlayer;


    private void OnTriggerEnter(Collider other)
    {
    Debug.Log(other.transform.gameObject.tag);
        if (other.transform.gameObject.tag == "Player")
        { 
            otherPlayer = other.gameObject;
            otherPlayer.GetComponent<HealthController>().hasEscaped = true;
        }
        
    }
}
