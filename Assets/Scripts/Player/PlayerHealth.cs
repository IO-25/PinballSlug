using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action OnDie;
    public static event Action<int> OnHealthChanged;
    private Player player;
    [SerializeField] private float invincibilityDuration = 2f; //부활 후 무적 시간
    [SerializeField] private int maxHealth = 3; //목숨
    [SerializeField] private AudioClip deathSFX; //사망 사운드
    private int currentHealth;
    private float nextDamageTime;
    private AudioSource audioSource;


    private void Awake()
    {
        player = GetComponent<Player>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth; //현제 체력을 최대체력으로 맞추기
    }

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        nextDamageTime = Time.time + invincibilityDuration; // 다음 피해 받기 가능한 시간
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (enabled == false) return;
        //무적 시간
        if (Time.time < nextDamageTime)
        {
            return; 
        }
        
        player.StartDeathSequence(currentHealth == 0);
        audioSource.PlayOneShot(deathSFX);
        currentHealth = Mathf.Max(currentHealth - 1, 0);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0) Die();
    }

    private void Die() => OnDie?.Invoke();
}