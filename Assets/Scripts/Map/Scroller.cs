using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float scrollSpeed = 5f;

    void Update()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
    }
}
