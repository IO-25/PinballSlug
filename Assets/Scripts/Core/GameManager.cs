using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action OnGameOver;

    private Player player;
    public Player Player { get => player; set => player = value; }

    public void GameOver()
    {
        Debug.Log("GameOver");
        OnGameOver?.Invoke();
    }

}
