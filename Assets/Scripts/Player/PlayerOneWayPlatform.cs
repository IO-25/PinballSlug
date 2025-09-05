using System.Collections;
using UnityEngine;

public class PlayerOneWayPlatform : MonoBehaviour
{
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private float dropTime = 0.2f;
    private Collider2D currentPlatform;
    private bool isDropping = false;

    public bool IsDropping => isDropping;

    private void Awake()
    {
        if(playerCollider == null)
            playerCollider = GetComponentInChildren<Collider2D>();
    }

    public void Drop()
    {
        if (!isDropping)
            StartCoroutine(DisablePlatform(currentPlatform));
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentPlatform = collision.collider;
            // transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentPlatform = null;
            // transform.SetParent(null);
        }
    }

    IEnumerator DisablePlatform(Collider2D platformCollider)
    {
        if(platformCollider == null) yield break;

        isDropping = true;
        Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
        yield return new WaitForSeconds(dropTime);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);

        isDropping = false;
    }


}
