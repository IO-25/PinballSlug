using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action OnDie;
    public static event Action<int> OnHealthChanged;
    private Player player;
    [SerializeField] private float invincibilityDuration = 2f; // ���� �ð�
    [SerializeField] private int maxHealth = 3; // �ִ� ü��
    [SerializeField] private AudioClip deathSFX; // ��� �Ҹ�
    private int currentHealth;
    private float nextDamageTime;
    private AudioSource audioSource;


    private void Awake()
    {
        player = GetComponent<Player>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth; // ü�� �ʱ�ȭ
    }

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        nextDamageTime = Time.time + invincibilityDuration; // �ʱ�ȭ �� ���� �ð� ����
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (enabled == false) return;
        if (Time.time < nextDamageTime)
        {
            Debug.Log($"���� �����ð� {(nextDamageTime - Time.time).ToString("F2")}");
            return; // ���� �ð� ������ ����
        }
        
        player.StartDeathSequence(currentHealth == 0);
        audioSource.PlayOneShot(deathSFX);
        currentHealth = Mathf.Max(currentHealth - 1, 0);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0) Die();
    }

    private void Die() => OnDie?.Invoke();
}