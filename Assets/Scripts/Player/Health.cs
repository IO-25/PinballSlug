using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    public event Action OnDie;
    public event Action<int> OnHealthChanged;
    public event Action<int> OnTakeDamage;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize(){
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        OnTakeDamage?.Invoke(currentHealth);
        currentHealth = Mathf.Max(currentHealth - 1, 0);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0) Die();
    }

    private void Die() => OnDie?.Invoke();
}