using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private float score = 0f;

    public delegate void ActionScoreChanged(float score);
    public ActionScoreChanged OnScoreChanged;
    
    public void SetScore(float point)
    {
        score += point;
        OnScoreChanged?.Invoke(score);
    }
}
