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
    //message sent when Pac-Man eats a power-up, a dot, or gets reset
    public static event Message EatPowerup;
    public static event Message EatDot;
    public static event Message PacReset;

    //location for Pac-Man's respawn
    public Transform PacSpawn;

    //lists of dots and powerups
    public GameObject[] Dots;
    public GameObject[] Power;

    private void Start()
    {
        //get character controller component
        _charControl = GetComponent<CharacterController>();
        //subscribing to the ghosts' message about when to respawn
        GhostScript.PacKilled += Reset;
        DisplayData.ResetAll += ResetAll;


        //get the dots and powerups in lists so we can reset them at restart
        Dots = GameObject.FindGameObjectsWithTag("Snack");
        Power = GameObject.FindGameObjectsWithTag("Powerup");
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
            GameObject dot = other.gameObject;
            dot.SetActive(false);
            //pac-man's dot count is incremented and the dot is turned off
            dotCount += 1;
            //Debug.Log(dotCount);
            if (EatDot != null) 
            {
                EatDot();
            }
        }

        //if the pick-up is a powerup
        if (other.CompareTag("Powerup")) 
        {
            GameObject power = other.gameObject;
            power.SetActive(false);
            if (EatPowerup != null) 
            {
                //send out the message that the powerup was consumed
                EatPowerup();
            }
        }
    }

    //put Pac-Man back at his reset point when he is killed
    private void Reset()
    {
        transform.position = PacSpawn.position;
        pacMan.SetActive(true);
        if (PacReset != null) 
        {
            PacReset();
        }
    }

    //reset Pacman and the dots between games
    private void ResetAll() 
    {
        transform.position = PacSpawn.position;
        pacMan.SetActive(true);
        foreach (GameObject Dot in Dots) 
        {
            Dot.SetActive(true);
        }
        foreach (GameObject Pow in Power) 
        {
            Pow.SetActive(true);
        }
    }

    //unsubscribe from messages just in case
    private void OnApplicationQuit()
    {
        GhostScript.PacKilled -= Reset;
        DisplayData.ResetAll -= ResetAll;
    }
}
