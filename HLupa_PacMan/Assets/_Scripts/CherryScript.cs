using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryScript : MonoBehaviour
{
    //bool that indicates whether the cherry will give points or not
    private bool CherryOn = false;

    //int that resets the timer
    private int TimerCap = 300;

    //int that counts down the timer
    private int CherryTimer = 300;

    //mesh renderer so the cherry's visibility can be turned on and off
    private MeshRenderer CherrySkin;

    public delegate void Message();
    //message sent to the UI to say that the cherry got ate
    public static event Message CherryAte;

    private void Start()
    {
        //on start the cherry's mesh renderer is found
        CherrySkin = GetComponent<MeshRenderer>();
        //the cherry's points are turned off
        CherryOn = false;
        //and the cherry is turned invisible
        CherrySkin.enabled = false;
    }

    //every so often check on the cherry
    private void FixedUpdate()
    {
        //if the cherry timer is at 0, flip the switch
        if (CherryTimer <= 0)
        {
            //if the cherry is off, turn it on
            if (CherryOn == false)
            {
                CherryOn = true;
                CherrySkin.enabled = true;
                CherryTimer = TimerCap;
            }
            //if the cherry is on, turn it off
            else
            {
                CherryOn = false;
                CherrySkin.enabled = false;
                CherryTimer = TimerCap;
            }
        }
        //or keep counting down
        else 
        {
            CherryTimer -= 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if the player hits the cherry while it's on
        if (CherryOn == true && other.CompareTag("Player")) 
        {
            //send the CherryAte message and turn the cherry off
            if (CherryAte != null) 
            {
                CherryAte();
                CherryOn = false;
                CherrySkin.enabled = false;
                CherryTimer = TimerCap;
            }
        }
    }
}
