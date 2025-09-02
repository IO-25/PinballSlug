using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerAnimationController playerAnimationController;

    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerAttack PlayerAttack => playerAttack;
    public PlayerAnimationController PlayerAnimationController => playerAnimationController;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    public void Die()
    {
        playerMovement.enabled = false;
        playerAttack.enabled = false;
        playerAnimationController.SetBool("IsDying", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

}
