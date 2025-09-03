using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGameManager : MonoBehaviour
{
    public ScoreDisplayManager scoreDisplayManager;
    private int currentScore = 0;

    void Update()
    {
        // 'Space' 키를 누르면 점수 10점 추가
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddScore(1);
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        if (scoreDisplayManager != null)
        {
            scoreDisplayManager.UpdateScore(currentScore);
        }
    }

    public void reduceScore(int amount)
    {
        currentScore -= amount;
        if (scoreDisplayManager != null)
        {
            scoreDisplayManager.UpdateScore(currentScore);
        }
    }
}
