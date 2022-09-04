using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject pacMan;

    CharacterController _charControl;

    public float speed = 5;

    public int dotCount = 0;

    private void Start()
    {
        //get character controller component
        _charControl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Snack")) 
        {
            dotCount += 1;
            Destroy(other.gameObject);
            Debug.Log(dotCount);
        }
    }
}
