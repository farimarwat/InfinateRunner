using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSpeedController : MonoBehaviour
{
    [SerializeField] private float GlobalSpeed;
    public delegate void ActionSpeedChanged(float speed);
    public event ActionSpeedChanged OnSpeedChanged;

    public float GetGlobalSpeed()
    {
        return GlobalSpeed;
    }
    public void ChangeGlobalSpeed(float speedchange, float duration)
    {
        GlobalSpeed += speedchange;
        OnSpeedChanged?.Invoke(GlobalSpeed);
        StartCoroutine(RemoveSpeedChange(speedchange,duration));
    }

    IEnumerator RemoveSpeedChange(float speedchange, float waitfortime)
    {
        yield return new WaitForSeconds(waitfortime);
        GlobalSpeed -= speedchange;
        OnSpeedChanged?.Invoke(GlobalSpeed);
    }
}
