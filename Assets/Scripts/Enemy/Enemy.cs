using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Sprite enemySprite;
    BoxCollider2D enemyCollider;

    [Header("Àû Á¤º¸")]
    public int curHealth = 0;
    bool isInitialized = false;


    private void Awake()
    {
        enemySprite = GetComponent<Sprite>();
        enemyCollider = GetComponent<BoxCollider2D>();
    }

    public void Init(EnemyData referenceEnemy)
    {
        if (isInitialized)
            return;
        curHealth = referenceEnemy.InitialHealth;
        enemySprite = referenceEnemy.enemySprite;
        enemyCollider.size = referenceEnemy.enemySize;
        isInitialized = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if collision is End of the Map
            //GameOver

        //if collision is Player
            //Kill Player

        //if collision is Player Projectile
            //Apply Damage
    }

    private void OnDamageApplied()
    {
        curHealth--;
        if (curHealth <= 0)
            OnDead();
    }

    private void OnDead()
    {
        Destroy(this);
    }

}
