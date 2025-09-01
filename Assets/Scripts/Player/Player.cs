using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("�̵� ����")]
    public float moveSpeed = 5f;

    [Header("���� ����")]
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;    // ������ �� ���ӵ�
    public float lowJumpMultiplier = 2f;   // ���� ��ư ���� �� �� ���ӵ�
    public Transform groundCheck; // �ٴ� üũ�� Ʈ������
    public LayerMask groundLayerMask; // �ٴ� ���̾� ����ũ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // �¿� �̵�
        float moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // ���� ����
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // === �߷� ���� ===
        if (rb.velocity.y < 0) // �ϰ� ��
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // ��� ���ε� ��ư�� �� ���
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayerMask);
    }
}
