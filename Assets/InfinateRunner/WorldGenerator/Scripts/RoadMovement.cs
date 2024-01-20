using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMovement : MonoBehaviour
{
    private float moveSpeed;
    private Vector3 moveDirection;
    private Vector3 destination;

    void Start()
    {
       
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }
    public void SetDestination(Vector3 dest)
    {
        destination = dest;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        if(Vector3.Dot((destination - transform.position).normalized,moveDirection) < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction;
    }
}
