using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerAnimationController playerAnimationController;
    private PlayerSpawner playerSpawner;
    private PlayerHealth playerHealth;

    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerAttack PlayerAttack => playerAttack;
    public PlayerAnimationController PlayerAnimationController => playerAnimationController;


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerSpawner = GetComponent<PlayerSpawner>();
        playerHealth = GetComponent<PlayerHealth>();

        playerHealth.OnTakeDamage += OnTakeDamage;
        playerAnimationController.Initialize();
        StageManager.Instance.player = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            DropItemFactory.CreateRandomDropItem(PlayerAttack.CurrentFirePoint);
        }
    }

    public void Initialize()
    {
        playerMovement.enabled = true;
        playerAttack.enabled = true;
        playerHealth.Initialize();
        playerAnimationController.Initialize();
    }
    
    private void OnTakeDamage(int health)
    {
        TakeDamage();
        if (health > 0)
            playerSpawner.StartSpawn();
        else
            Die();
    }


    private void TakeDamage()
    {
        playerMovement.enabled = false;
        playerAttack.enabled = false;
        playerAnimationController.Initialize();
        playerAnimationController.SetBool("IsDying", true);
    }

    private void Die()
    {
        Debug.Log("GameOver");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerHealth.TakeDamage(1);
        }
    }

}
