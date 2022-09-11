using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //variable for the player object
    public GameObject pacMan;

    //player's controller
    CharacterController _charControl;

    //pac-man's speed
    public float speed = 5;

    //how many dots pac-man has eaten so far
    public int dotCount = 0;

    //messages pac-man is sending to other scripts
    public delegate void Message();
    //message sent to ghosts to let them know that pac-man is in powerup mode
    public static event Message EatPowerup;
    public static event Message EatDot;
    public static event Message PacReset;

    public Transform PacSpawn;

    private void Start()
    {
        //get character controller component
        _charControl = GetComponent<CharacterController>();
        GhostScript.PacKilled += Reset;
    }

    // Update is called once per frame
    void Update()
    {
        //always allow for character movement
        MoveChar();
    }

    void MoveChar() 
    {
        //creating a vector3 for Pac-Man to move along on the X and Z axes
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //applying those values to Pac-Man's character controller
        _charControl.Move(move * (Time.deltaTime * speed));

        if (move != Vector3.zero) 
        {
            pacMan.transform.forward = move;
        }
    }
    
    //what happens when pac-man collides with the various pick-up items. which all have trigger colliders
    private void OnTriggerEnter(Collider other)
    {
        //if the pick-up item is a basic dot/snack
        if (other.CompareTag("Snack")) 
        {
            //pac-man's dot count is incremented and the dot is destroyed
            dotCount += 1;
            Destroy(other.gameObject);
            Debug.Log(dotCount);
            if (EatDot != null) 
            {
                EatDot();
            }
        }

        //if the pick-up is a powerup
        if (other.CompareTag("Powerup")) 
        {
            if (EatPowerup != null) 
            {
                //destroy the powerup and send out the message that the powerup was consumed
                Destroy(other.gameObject);
                EatPowerup();
            }
        }
    }

    private void Reset()
    {
        transform.position = PacSpawn.position;
        pacMan.SetActive(true);
        if (PacReset != null) 
        {
            PacReset();
        }
    }
}
