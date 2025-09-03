using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public static int LANECOUNT = 8;
    public Enemy[] enemy = new Enemy[LANECOUNT];
    public bool isInitialized = false;
    public int waveEnemyCount = 0;
    public static Vector3 leftMovement;
    private void Awake()
    {
        for (int i = 0; i < LANECOUNT; i++)
        {
            enemy[i] = transform.GetChild(i).GetComponent<Enemy>();
        }
    }

    private void FixedUpdate()
    {
        if (isInitialized)
        {
            transform.Translate(leftMovement);
            //When Wave moved to the leftmost side of the camera
            if (Camera.main.WorldToViewportPoint(transform.position).x <= 0.0f)
            {
                //Add Game Over Logic Here
                Debug.Log("GameOver");
            }
        }
    }

    public void SetEnemy(int index, EnemyData data)
    {
        if (data == null)
        {
            if (enemy[index] != null && enemy[index].isInitialized)
                waveEnemyCount--;

            if (isInitialized && waveEnemyCount == 0)
            {
                Destroy(gameObject);
            }
            enemy[index].gameObject.SetActive(false);
            enemy[index] = null;
        }
        else
        {
            if (enemy[index] == null || !enemy[index].isInitialized)
                waveEnemyCount++;
            enemy[index].parentWave = this;
            enemy[index].index = index;
            enemy[index].Init(data);
            enemy[index].transform.localPosition += Vector3.up * (data.enemySize.y - 1f) / 2.0f;
        }
    }
}
