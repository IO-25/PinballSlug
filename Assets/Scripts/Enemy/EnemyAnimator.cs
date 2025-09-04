using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    const string DIEANIMATIONSTRING = "IsDie";
    [SerializeField] Enemy enemy;
    public Animator animator;
    SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        enemy.OnDeadActions += OnDead;
    }

    public void OnDead()
    {
        animator.SetBool(DIEANIMATIONSTRING, true);
    }

    public void OnDieAnimationEnd()
    {
        enemy.parentWave.SetEnemy(enemy.index, null);
        Destroy(enemy.gameObject);
    }

    public IEnumerator DamageFlash()
    {
        spriteRenderer.color = new Color(1.0f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}
