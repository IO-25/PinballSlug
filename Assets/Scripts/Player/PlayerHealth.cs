using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action OnDie;
    public event Action<int> OnHealthChanged;
    private Player player;
    [SerializeField] private float invincibilityDuration = 2f; // 무적 시간
    [SerializeField] private int maxHealth = 3; // 최대 체력

    private int currentHealth;
    private float nextDamageTime;

    private void Awake()
    {
        player = GetComponent<Player>();
        currentHealth = maxHealth; // 체력 초기화
    }

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        nextDamageTime = Time.time + invincibilityDuration; // 초기화 시 무적 시간 적용
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (enabled == false) return;
        if (Time.time < nextDamageTime)
        {
            Debug.Log($"남은 무적시간 {(nextDamageTime - Time.time).ToString("F2")}");
            return; // 무적 시간 동안은 무시
        }
        
        player.StartDeathSequence(currentHealth == 0);
        currentHealth = Mathf.Max(currentHealth - 1, 0);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0) Die();
    }

    private void Die() => OnDie?.Invoke();
}