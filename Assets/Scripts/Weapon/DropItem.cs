using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    private DropItemData dropItemData;

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void Initialize(DropItemData newData)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        // ���� Ÿ�԰� ��������Ʈ ����
        dropItemData = newData;
        spriteRenderer.sprite = dropItemData.dropItemSprite;

        // ������ �������� �������� ���� ����
        float randomAngle = Random.Range(0f, 360f);
        Vector2 randomDirection = new(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        rb.velocity = randomDirection * dropItemData.floatStrength;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾ �������� ȹ������ �� ���� ��ü
            Player player = StageManager.Instance.player;
            if (player != null)
            {
                player.PlayerAttack.EquipWeapon(dropItemData.weaponType);
                Destroy(gameObject);
            }
        }
    }
}
