using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGameManager : Singleton<MapGameManager>
{
    public ScoreDisplayManager scoreDisplay;
    public ScoreDisplayManager ammoDisplay;
    public ScoreDisplayManager bombDisplay;
    private int currentScore = 0;


    /*
    void Update()
    {
        // 'Space' 키를 누르면 점수 1점 추가
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddScore(1);
        }
    }
    */

    public void DisplayAmmo(int ammo, bool useAmmo)
    {
        if (ammoDisplay == null) return;
        ammoDisplay.UpdateScore(ammo);
        ammoDisplay.SetInfinity(!useAmmo);
    }

    public void DisplayBomb(int bomb)
    {
        if (bombDisplay == null) return;
        bombDisplay.UpdateScore(bomb);
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        if (scoreDisplay != null)
        {
            scoreDisplay.UpdateScore(currentScore);
        }
    }

    public void ReduceScore(int amount)
    {
        currentScore -= amount;
        if (scoreDisplay != null)
        {
            scoreDisplay.UpdateScore(currentScore);
        }
    }
}
