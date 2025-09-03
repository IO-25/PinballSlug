using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float floatStrength = 1f;
    [SerializeField] private WeaponType weaponType;

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void Initialize(WeaponType weaponType, Sprite sprite)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        // 무기 타입과 스프라이트 설정
        this.weaponType = weaponType;
        spriteRenderer.sprite = sprite;

        // 랜덤한 방향으로 떠오르는 힘을 가함
        float randomAngle = Random.Range(0f, 360f);
        Vector2 randomDirection = new(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        rb.AddForce(randomDirection * floatStrength, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어가 아이템을 획득했을 때 무기 교체
            Player player = GameManager.Instance.Player;
            if (player != null)
            {
                player.PlayerAttack.EquipWeapon(weaponType);
                Destroy(gameObject);
            }
        }
    }
}
