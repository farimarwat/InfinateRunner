using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform[] LaneTransforms;
    [SerializeField] float MoveSpeed;
    Vector3 Destination;
    int CurrentIndex;

    [SerializeField]
    private SwipeDetection swipeDetection;
    [SerializeField]
    private float maxJumpHeight = 1.3f;

    //Ground Detection
    [SerializeField] Transform groundCheckPositon;
    [SerializeField, Range(0,1)] float groundCheckRadius = 0.1f;
    [SerializeField] LayerMask groundCheckMask;

    private Animator animator;

    [SerializeField] private Transform mainCamera;
    [SerializeField] private float mainCameraYPosition = 0.5f;

   
   
    void Start()
    {
        animator = GetComponent<Animator>();
        Destination = LaneTransforms[1].position;
        CurrentIndex = 1;
    }

    private void OnEnable()
    {
        swipeDetection.OnLeft += MoveLeft;
        swipeDetection.OnRight += MoveRight;
        swipeDetection.OnUp += Jump;
        swipeDetection.OnDown += MoveDown;
    }

    private void MoveDown()
    {
        
    }

    
    private void MoveRight()
    {
        if (!IsOnGround())
        {
            return;
        }
        if(CurrentIndex == LaneTransforms.Length -1)
        {
            return;
        }
        CurrentIndex++;
        Destination = LaneTransforms[CurrentIndex].position;
    }
    private void MoveLeft()
    {
        if (!IsOnGround())
        {
            return;
        }
        if (CurrentIndex == 0)
        {
            return;
        }
        CurrentIndex--;
        Destination = LaneTransforms[CurrentIndex].position;
    }
    private void Jump()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null)
        {
            float jumpSpeed = Mathf.Sqrt(2 * maxJumpHeight * Physics.gravity.magnitude);
            rb.AddForce(new Vector3(0.0f, jumpSpeed, 0.0f), ForceMode.VelocityChange);
        }
    }
    private bool IsOnGround()
    {

        return Physics.CheckSphere(groundCheckPositon.transform.position, groundCheckRadius,groundCheckMask);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOnGround())
        {
            animator.SetBool("isOnGround", true);
        }
        else
        {
            animator.SetBool("isOnGround", false);
        }
        float TransformX = Mathf.Lerp(transform.position.x, Destination.x, Time.deltaTime * MoveSpeed);
        transform.position = new Vector3(TransformX, transform.position.y, transform.position.z);

        // Smoothly follow the player on X coordinate
        float smoothTime = 0.1f; // You can adjust this value
        float velocity = 0.0f;

        float newPosX = Mathf.SmoothDamp(mainCamera.position.x, Destination.x, ref velocity, smoothTime);
        float newPosY = transform.position.y + mainCameraYPosition;
        float newPosZ = mainCamera.position.z;

        mainCamera.position = new Vector3(newPosX, newPosY, newPosZ);
    }
 

}

