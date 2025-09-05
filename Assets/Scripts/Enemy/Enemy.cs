using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    //Base Components
    SpriteRenderer boxSpriteRenderer;
    [SerializeField] EnemyAnimator enemyAnimator;
    BoxCollider2D enemyCollider;
    EnemySoundEmitter soundEmitter;

    [Header("적 웨이브 정보")]
    public EnemyWave parentWave = null;
    public int index;

    [Header("적 정보")]
    [HideInInspector] public Action OnDeadActions;
    public int maxHealth = 0;
    public int curHealth = 0;
    public bool isInitialized = false;

    public EnemyBehaviour[] behaviours;

    [Header("드랍 정보")]
    DropItemData[] dropItemDatas;
    float[] dropRate;
    [SerializeField] DropItem dropItemPrefab;


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
        enemyAnimator.animator.runtimeAnimatorController = referenceEnemy.EnemyAnimatorController;
        boxSpriteRenderer.size = referenceEnemy.enemySize;
        enemyCollider.size = referenceEnemy.enemySize;
        behaviours = referenceEnemy.behaviours;
        isInitialized = true;

        dropItemDatas = referenceEnemy.dropItemDatas;
        dropRate = referenceEnemy.dropRate;

        if (dropItemDatas.Length != dropRate.Length)
            throw new System.Exception("DropItem And Drop Rate Size is Different");

        for (int i = 0; i < behaviours.Length; i++)
            StartCoroutine(behaviours[i].ActionCorutine(this.transform));
        soundEmitter = EnemySoundEmitter.Instance;
    }

    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        soundEmitter.Onhit();
        if (curHealth <= 0)
            OnDead();
        StartCoroutine(enemyAnimator.DamageFlash());
    }

    private void OnDead()
    {
        OnDeadActions?.Invoke();
        soundEmitter.OnDead();
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
