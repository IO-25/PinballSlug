using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyWave parentWave = null;
    public int index;

    public Action OnDeadActions;

    const string DIEANIMATIONSTRING = "IsDie";
    SpriteRenderer boxSpriteRenderer;
    [SerializeField] Animator enemyAnimator;
    BoxCollider2D enemyCollider;

    [Header("�� ����")]
    public int maxHealth = 0;
    public int curHealth = 0;
    public bool isInitialized = false;

    public EnemyBehaviour[] behaviours;


    private void Awake()
    {
        boxSpriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<BoxCollider2D>();
    }

    public void Init(EnemyData referenceEnemy)
    {
        if (isInitialized)
            return;
        maxHealth = referenceEnemy.InitialHealth;
        curHealth = referenceEnemy.InitialHealth;
        enemyAnimator.runtimeAnimatorController = referenceEnemy.EnemyAnimatorController;
        boxSpriteRenderer.size = referenceEnemy.enemySize;
        enemyCollider.size = referenceEnemy.enemySize;
        behaviours = referenceEnemy.behaviours;
        isInitialized = true;

        for (int i = 0; i < behaviours.Length; i++)
            StartCoroutine(behaviours[i].ActionCorutine(this.transform));
    }

    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        if (curHealth <= 0)
            OnDead();
    }

    private void OnDead()
    {
        OnDeadActions();
        enemyAnimator.SetBool(DIEANIMATIONSTRING, true);
        StopAllCoroutines();
    }

}
