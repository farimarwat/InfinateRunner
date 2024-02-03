using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Threat : MonoBehaviour
{
    [SerializeField] private float _SpawnInterval = 2f;
    [SerializeField] private MovementScript movementScript;


    public float SpawnInterval { 
        get { return _SpawnInterval; }
    }
 
    public MovementScript GetMovementScript()
    {
        return movementScript;
    }
}
