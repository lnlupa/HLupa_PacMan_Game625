using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GhostScript : MonoBehaviour
{
    //variable for the player
    public GameObject pacMan;

    //the navigation meshes of the ghosts
    NavMeshAgent ghostNav;

    //whether the ghosts fear Pac-Man
    public bool FearMode = false;

    //how close the ghosts need to be to kill Pac-Man/be killed by Pac-Man
    public float killDistance = 0.5f;

    //object that sets the point for the ghosts to respawn when killed
    public GameObject respawnPoint;

    //locations for the ghosts to flee to
    public List<Transform> EscapePoints;
    private int currentPoint = 0;

    //amount of time left before the ghosts become dangerous again
    private float TimeRemaining = 10;

    //messages ghost is sending to other scripts
    public delegate void Message();
    //message sent to the UI to say that PacMan has lost a life
    public static event Message PacKilled;
    //message sent to the UI to say that PacMan has killed a ghost
    public static event Message GhostKilled;

    //light that turns on to indicate that they are in fear mode
    private Light GhostLight;

    private void Start()
    {
        //get the ghosts' navmesh and the player object
        ghostNav = GetComponent<NavMeshAgent>();
        pacMan = GameObject.FindGameObjectWithTag("Player");

        //subscribe to Pac-Man's messages about eating powerups
        PlayerScript.EatPowerup += FearModeOn;
        PlayerScript.PacReset += Reset;

        //get the ghost's light and turn it off until they enter fear mode
        GhostLight = GetComponent<Light>();
        GhostLight.enabled = false;
    }

    private void Update()
    {
        //if the ghosts fear Pac-Man, they run from him
        if (FearMode)
        {
            //turn on the fear mode light
            GhostLight.enabled = true;
            Debug.Log("ghosts in fear mode");
            //Debug.LogError(currentPoint);
            RunFrom();
            //check the fear mode timer
            if (TimeRemaining > 0)
            {
                TimeRemaining -= Time.deltaTime;
            }
            else 
            {
                FearMode = false;
            }
            //Debug.LogWarning(TimeRemaining);
        }

        //otherwise they run toward Pac-Man with lights off and fear mode reset
        else 
        {
            RunTo();
            TimeRemaining = 10;
            GhostLight.enabled = false;
        }
    }

    //what happens when the ghosts hit Pac-Man
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            //if in fear mode, they respawn
            if (FearMode)
            {
                Respawn();
            }
            //otherwise they kill Pac-Man
            else
            {
                KillPacMan();
            }
        }
    }

    //the ghosts' destination is Pac-Man
    private void RunTo() 
    {
        ghostNav.SetDestination(pacMan.transform.position);
    }

    //the ghosts' destination is away from Pac-Man
    private void RunFrom() 
    {
        Transform destination = EscapePoints[currentPoint];

        //if Pac-Man gets too close they flee to the next point
        if (Vector3.Distance(pacMan.transform.position, destination.position) <= 5)
        {
            currentPoint = (currentPoint + 1) % EscapePoints.Count;
        }
        else 
        {
            ghostNav.SetDestination(destination.position);
        }
        
    }

    //how the ghosts kill Pac-Man
    private void KillPacMan() 
    {
        //Debug.Log("kill pacman");
        pacMan.SetActive(false);
        if (PacKilled != null) 
        {
            PacKilled();
        }
    }

    //how the ghosts are killed by Pac-Man
    private void Respawn() 
    {
        //Debug.Log("kill ghost");
        transform.position = respawnPoint.transform.position;
        FearMode = false;
        if (GhostKilled != null) 
        {
            GhostKilled();
        }
    }

    //switch that flips fear mode on when Pac-Man eats a powerup
    public void FearModeOn() 
    {
        FearMode = true;
    }

    //function that resets their location to the respawn point when Pac-Man dies
    public void Reset()
    {
        transform.position = respawnPoint.transform.position;
    }

    //unsubscribe from the events on quit just in case
    private void OnApplicationQuit()
    {
        PlayerScript.EatPowerup -= FearModeOn;
        PlayerScript.PacReset -= Reset;
    }
}
