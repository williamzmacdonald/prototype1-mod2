﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
   
    Vector3 movement;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;
    bool sprint;
    private void Awake()
    {
        sprint = false;
        floorMask = LayerMask.GetMask("Floor");
        playerRigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprint = true;
        }
        else
        {
            sprint = false;
        }
        Move(h, v);
        Turning();
    }
    private void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        if(sprint)
        {
            movement = movement.normalized * speed * Time.deltaTime * 1.5f;
        }
        else
        {
            movement = movement.normalized * speed * Time.deltaTime;
        }

        playerRigidbody.MovePosition(transform.position + movement);
    }
    private void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if(Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }
    
}