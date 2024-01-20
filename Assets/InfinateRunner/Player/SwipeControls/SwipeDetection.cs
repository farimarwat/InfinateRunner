using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] InputManager inputmanager;

    [SerializeField]
    private float minDistance = .03f;
    [SerializeField]
    private float maxTime = 1f;
    [SerializeField, Range(0f,1f)]
    private float directionThreshold = .9f;

    private Vector2 startPosition;
    private float startTime;

    private Vector2 endPosition;
    private float endTime;


    #region Events
    public delegate void SwipeLeft();
    public event SwipeLeft OnLeft;

    public delegate void SwipeRight();
    public event SwipeRight OnRight;

    public delegate void SwipeUp();
    public event SwipeUp OnUp;

    public delegate void SwipeDown();
    public event SwipeDown OnDown;
    #endregion

    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        inputmanager.OnTouchStart += SwipeStarted;
        inputmanager.OnTouchEnd += SwipeEnded;
    }
    private void OnDisable()
    {
        inputmanager.OnTouchStart -= SwipeStarted;
        inputmanager.OnTouchEnd -= SwipeEnded;
    }


    private void SwipeStarted(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
    }
    private void SwipeEnded(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if(Vector3.Distance(startPosition,endPosition) >= minDistance &&
            (endTime - startTime) <= maxTime)
        {
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2d = new Vector2(direction.x, direction.y).normalized;
            DetectDirection(direction2d);
        }
    }

    private void DetectDirection(Vector2 direction2d)
    {
        if(Vector2.Dot(Vector2.up,direction2d) > directionThreshold)
        {
            OnUp();
        }
        if (Vector2.Dot(Vector2.down, direction2d) > directionThreshold)
        {
            OnDown();
        }
        if (Vector2.Dot(Vector2.left, direction2d) > directionThreshold)
        {
            OnLeft();
        }
        if (Vector2.Dot(Vector2.right, direction2d) > directionThreshold)
        {
            OnRight();
        }

    }
}
