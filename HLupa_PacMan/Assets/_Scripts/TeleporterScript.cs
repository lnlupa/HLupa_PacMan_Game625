using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterScript : MonoBehaviour
{
    //object that sets the location for when pac-man teleports
    public GameObject Locator;

    //when something enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        //if it's pac-man, he is moved to the locator
        if (other.gameObject.CompareTag("Player")) 
        {
            other.transform.position = Locator.transform.position;
        }
    }
}
