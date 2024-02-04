using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArraySpawner : MonoBehaviour
{
    [SerializeField] private float amount = 10;
    [SerializeField] private float gap = 1f;
    void Start()
    {
        for(int i = 0; i <= amount; i++)
        {
            Vector3 spawnPosition = transform.position + transform.forward * gap * i;
            GameObject nextCoin = Instantiate(gameObject, spawnPosition, Quaternion.identity);
            nextCoin.GetComponent<ArraySpawner>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
