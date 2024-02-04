using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float scoreEffect;
    [SerializeField] float speedEffect;
    [SerializeField] float speedEffectDuration;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.gameObject.tag == "Player")
        {
            GlobalSpeedController globalSpeedController = FindObjectOfType<GlobalSpeedController>();
            if (globalSpeedController != null)
            {
            
                globalSpeedController.ChangeGlobalSpeed(speedEffect, speedEffectDuration);
            }

            ScoreController scoreController = FindObjectOfType<ScoreController>();
            if(scoreController != null)
            {
                scoreController.SetScore(scoreEffect);
            }
            Destroy(gameObject);
        }
    }
}
