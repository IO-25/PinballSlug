using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    //Base Components
    SpriteRenderer boxSpriteRenderer;
    [SerializeField] EnemyAnimator enemyAnimator;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip deadSound;
    BoxCollider2D enemyCollider;
    AudioSource enemyAudioSource;

    [Header("�� ���̺� ����")]
    public EnemyWave parentWave = null;
    public int index;

    [Header("�� ����")]
    [HideInInspector] public Action OnDeadActions;
    public int maxHealth = 0;
    public int curHealth = 0;
    public bool isInitialized = false;

    public EnemyBehaviour[] behaviours;

    [Header("��� ����")]
    DropItemData[] dropItemDatas;
    float[] dropRate;
    [SerializeField] DropItem dropItemPrefab;


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

        dropItemDatas = referenceEnemy.dropItemDatas;
        dropRate = referenceEnemy.dropRate;

        if (dropItemDatas.Length != dropRate.Length)
            throw new System.Exception("DropItem And Drop Rate Size is Different");

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

    public void DropItemOnDead()
    {
        int index = RandomManager.RandomPicker(dropRate);
        if (dropItemDatas[index] != null)
        {
            DropItem drop = Instantiate(dropItemPrefab);
            drop.transform.position = this.transform.position;
            drop.Initialize(dropItemDatas[index]);
        }
    }
}
