using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float dropItemLiveDuration = 20.0f;
    private DropItemData dropItemData;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(DropItemData newData)
    {
        // 무기 타입과 스프라이트 설정
        dropItemData = newData;
        spriteRenderer.sprite = dropItemData.dropItemSprite;

        // 랜덤한 방향으로 떠오르는 힘을 가함
        float randomAngle = Random.Range(0f, 360f);
        Vector2 randomDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.right;
        rb.velocity = randomDirection * dropItemData.floatStrength;

        // 일정 시간 후 아이템 제거
        Destroy(gameObject, dropItemLiveDuration);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어가 아이템을 획득했을 때 무기 교체
            Player player = StageManager.Instance.player;
            if (player == null) return;

            if (dropItemData.weaponType == WeaponType.Bomb)
            {
                player.PlayerAttack.EquipBomb();
            }
            else
            {
                player.PlayerAttack.EquipWeapon(dropItemData.weaponType);
            }

            Destroy(gameObject);
        }
    }
}
