using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action OnDie;
    public event Action<int> OnHealthChanged;
    public event Action<int> OnTakeDamage;

    [SerializeField] private float invincibilityDuration = 2f; // ���� �ð�
    [SerializeField] private int maxHealth = 3; // �ִ� ü��

    private int currentHealth;
    private float nextDamageTime;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        currentHealth = maxHealth; // ü�� �ʱ�ȭ
        nextDamageTime = Time.time + invincibilityDuration; // �ʱ�ȭ �� ���� �ð� ����
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // �̹� �׾����� ����
        if (Time.time < nextDamageTime)
        {
            Debug.Log($"���� �����ð� {nextDamageTime - Time.time}");
            return; // ���� �ð� ������ ����
        }

        OnTakeDamage?.Invoke(currentHealth);
        currentHealth = Mathf.Max(currentHealth - 1, 0);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0) Die();
    }

    private void Die() => OnDie?.Invoke();
}