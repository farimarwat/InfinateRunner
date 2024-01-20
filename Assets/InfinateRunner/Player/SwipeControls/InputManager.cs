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

    public delegate void TouchEndAction(Vector2 position, float time);
    public event TouchEndAction OnTouchEnd;
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
        playercontrols.Touch.PrimaryContact.canceled += TouchEnded;
    }

   

    private void TouchStarted(InputAction.CallbackContext obj)
    {
        if (OnTouchStart != null) OnTouchStart(Utils.ScreenToWorld(mainCamera,
            playercontrols.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)obj.startTime);
    }
    private void TouchEnded(InputAction.CallbackContext obj)
    {
        if (OnTouchEnd != null) OnTouchEnd(Utils.ScreenToWorld(mainCamera,
            playercontrols.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)obj.startTime);
    }
}
