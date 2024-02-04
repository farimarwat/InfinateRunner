using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUiController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private ScoreController scoreController;


    private void Awake()
    {
        scoreController = FindObjectOfType<ScoreController>();
    }
    private void OnEnable()
    {
        if(scoreController!= null)
        {
            scoreController.OnScoreChanged += UpDateScore;
        }
    }
    private void OnDisable()
    {
        if (scoreController != null)
        {
            scoreController.OnScoreChanged -= UpDateScore;
        }
    }

    private void UpDateScore(float score)
    {
        Debug.Log("Score: " + score);
        scoreText.SetText($"Score: {score}");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
