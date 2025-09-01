using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGDestroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 "Platform" 태그를 가지고 있는지 확인합니다.
        if (other.CompareTag("OneWayPlatform"))
        {
            // 오브젝트를 비활성화해서 풀로 반환합니다.
            other.gameObject.SetActive(false);
        }
    }
}
