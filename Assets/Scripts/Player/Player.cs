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

        playerAnimationController.Initialize();
        playerAnimationController.SetTrigger("IsSpawning");
        StageManager.Instance.player = this;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            DropItemFactory.CreateRandomDropItem(PlayerAttack.CurrentFirePoint);
        }
    }
#endif

    public void Initialize()
    {
        playerAnimationController.Initialize();
        playerAnimationController.SetTrigger("IsSpawning");
        playerMovement.enabled = true;
        playerAttack.enabled = true;
        playerHealth.enabled = true;
    }

    public void StartDeathSequence(bool isDead = false)
    {
        playerMovement.enabled = false;
        playerAttack.enabled = false;
        playerHealth.enabled = false;
        playerAnimationController.Initialize();
        playerAnimationController.SetBool("IsDying", true);

        if (isDead) Invoke(nameof(Die), 3f);
        else playerSpawner.StartSpawn();
    }

    private void Die()
    {
        SceneChanger.GoGameOverScene();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerHealth.TakeDamage(1);
        }
    }

}
