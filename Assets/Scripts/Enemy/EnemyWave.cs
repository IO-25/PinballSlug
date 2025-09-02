using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public static int LANECOUNT = 8;

    public Enemy[] enemy = new Enemy[LANECOUNT];
    private void Awake()
    {
        for (int i = 0; i < LANECOUNT; i++)
        {
            enemy[i] = transform.GetChild(i).GetComponent<Enemy>();
        }
    }

    public void SetEnemy(int index, EnemyData data)
    {
        if (data == null)
        {
            enemy[index].gameObject.SetActive(false);
        }
        else
        {
            enemy[index].Init(data);
            enemy[index].transform.localPosition += Vector3.up * (data.enemySize.y - 1);
        }
    }
}
