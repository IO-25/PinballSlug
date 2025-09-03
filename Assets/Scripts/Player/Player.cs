using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerAnimationController playerAnimationController;
    private PlayerSpawner playerSpawner;
    private Health health;

    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerAttack PlayerAttack => playerAttack;
    public PlayerAnimationController PlayerAnimationController => playerAnimationController;


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerSpawner = GetComponent<PlayerSpawner>();
        health = GetComponent<Health>();

        health.OnTakeDamage += OnTakeDamage;
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
        playerAnimationController.SetBool("IsDying", false);
        playerAnimationController.SetBool("IsMoving", false);
        playerAnimationController.SetFloat_Upper("Y", 0f);
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
            health.TakeDamage(1);
        }
    }

}
