using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float specificSpeed = 0f;
    [SerializeField] private Vector3 moveDirection;
     [SerializeField] private Vector3 destination;
    private GlobalSpeedController speedController;
    private void Awake()
    {
        speedController = FindObjectOfType<GlobalSpeedController>();
    }

    private void OnEnable()
    {
        if(speedController != null)
        {
            speedController.OnSpeedChanged += SetSpeed;
        }
    }
    private void OnDisable()
    {
        if(speedController != null)
        {
            speedController.OnSpeedChanged -= SetSpeed;
        }
    }
    void Start()
    {
      if(speedController != null)
        {
            SetSpeed(speedController.GetGlobalSpeed());
        }
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed + specificSpeed;
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
