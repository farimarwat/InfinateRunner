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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public MovementScript GetMovementScript()
    {
        return movementScript;
    }
}
