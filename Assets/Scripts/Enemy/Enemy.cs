using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyWave parentWave = null;
    public int index;

    public Action OnDeadActions;

    SpriteRenderer boxSpriteRenderer;
    [SerializeField] EnemyAnimator enemyAnimator;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip deadSound;
    BoxCollider2D enemyCollider;
    AudioSource enemyAudioSource;

    [Header("Àû Á¤º¸")]
    public int maxHealth = 0;
    public int curHealth = 0;
    public bool isInitialized = false;

    public EnemyBehaviour[] behaviours;


    private void Awake()
    {
        boxSpriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<BoxCollider2D>();
        enemyAudioSource = GetComponent<AudioSource>();
    }

    public void Init(EnemyData referenceEnemy)
    {
        if (isInitialized)
            return;
        maxHealth = referenceEnemy.InitialHealth;
        curHealth = referenceEnemy.InitialHealth;
        enemyAnimator.animator.runtimeAnimatorController = referenceEnemy.EnemyAnimatorController;
        boxSpriteRenderer.size = referenceEnemy.enemySize;
        enemyCollider.size = referenceEnemy.enemySize;
        behaviours = referenceEnemy.behaviours;
        enemyAudioSource.clip = hitSound;
        isInitialized = true;

        for (int i = 0; i < behaviours.Length; i++)
            StartCoroutine(behaviours[i].ActionCorutine(this.transform));
    }

    public void TakeDamage(int damage)
    {
        enemyAudioSource.Play();
        curHealth -= damage;
        if (curHealth <= 0)
            OnDead();
        StartCoroutine(enemyAnimator.DamageFlash());
    }

    private void OnDead()
    {
        enemyAudioSource.Stop();
        enemyAudioSource.clip = deadSound;
        enemyAudioSource.Play();
        OnDeadActions?.Invoke();
        StopAllCoroutines();
    }

}
