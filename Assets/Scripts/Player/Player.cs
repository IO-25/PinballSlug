using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("�̵� ����")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("���� ����")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float fallMultiplier = 2.5f;    // ������ �� ���ӵ�
    [SerializeField] private float lowJumpMultiplier = 2f;   // ���� ��ư ���� �� �� ���ӵ�
    [SerializeField] private Transform groundCheck; // �ٴ� üũ�� Ʈ������
    [SerializeField] private LayerMask groundLayerMask; // �ٴ� ���̾� ����ũ
    [SerializeField] private float groundCheckDistance; // �ٴ� üũ �Ÿ�

    private Rigidbody2D rb;
    private PlayerOneWayPlatform oneWayPlatform;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        oneWayPlatform = GetComponent<PlayerOneWayPlatform>();
    }

    void Update()
    {
        // �¿� �̵�
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            // �پ����
            if(Input.GetKey(KeyCode.S))
                oneWayPlatform.Drop();
            // ����
            else
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
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayerMask);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }
    }
}
