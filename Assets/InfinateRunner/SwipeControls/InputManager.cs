using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region
    public delegate void TouchStartAction(Vector2 position, float time);
    public event TouchStartAction OnTouchStart;


    public delegate void TouchPerformAction(Vector2 position, float time);
    public event TouchPerformAction OnTouchPerform;
    #endregion

    
    private PlayerControls playercontrols;
    [SerializeField]
    private Camera mainCamera;

    private void Awake()
    {
        playercontrols = new PlayerControls();
    }
    private void OnEnable()
    {
        playercontrols.Enable();
    }
    private void OnDisable()
    {
        playercontrols.Disable();
    }
    private void Start()
    {
        playercontrols.Touch.PrimaryContact.started += TouchStarted;
        playercontrols.Touch.PrimaryContact.performed += TouchPerformed;
    }

    private void TouchPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Touch Performed");
        if (OnTouchPerform != null) OnTouchPerform(Utils.ScreenToWorld(mainCamera,
             playercontrols.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)obj.startTime);
    }

    private void TouchStarted(InputAction.CallbackContext obj)
    {
        Debug.Log("Touch Started");
        if (OnTouchStart != null) OnTouchStart(Utils.ScreenToWorld(mainCamera,
            playercontrols.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)obj.startTime);
    }
  
}
