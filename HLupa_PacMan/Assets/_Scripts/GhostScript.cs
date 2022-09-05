using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostScript : MonoBehaviour
{
    public GameObject pacMan;
    NavMeshAgent ghostNav;
    public bool FearMode = false;

    public float killDistance = 0.5f;

    public GameObject respawnPoint;

    private void Start()
    {
        ghostNav = GetComponent<NavMeshAgent>();
        pacMan = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (FearMode)
        {
            RunFrom();
            Respawn();
            FearMode = false;
        }
        else 
        {
            RunTo();
            KillPacMan();
        }
    }


    private void RunTo() 
    {
        ghostNav.SetDestination(pacMan.transform.position);
    }

    private void RunFrom() 
    {

    }

    private void KillPacMan() 
    {
        var dist = Vector3.Distance(pacMan.transform.position, transform.position);

        if (dist <= killDistance) 
        {
            Debug.Log("kill pacman");
            pacMan.SetActive(false);
        }
    }

    private void Respawn() 
    {
        var dist = Vector3.Distance(pacMan.transform.position, transform.position);

        if (dist <= killDistance)
        {
            Debug.Log("kill ghost");
            transform.position = respawnPoint.transform.position;
        }
    }
}
