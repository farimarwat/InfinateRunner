using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSpeedController : MonoBehaviour
{
    [SerializeField] private float GlobalSpeed;
    public delegate void ActionSpeedChanged(float speed);
    public event ActionSpeedChanged OnSpeedChanged;

    public void SetGlobalSpeed(float speed)
    {
        GlobalSpeed = speed;
        OnSpeedChanged?.Invoke(GlobalSpeed);
    }
    public float GetGlobalSpeed()
    {
        return GlobalSpeed;
    }
}
