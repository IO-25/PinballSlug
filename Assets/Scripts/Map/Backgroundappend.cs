using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgroundappend : MonoBehaviour
{
    public GameObject backgroundPrefab;
    public Transform playerTransform;

    public int maxPieces = 3;
    public float destroyPointX = -20f;
    
    private List<GameObject> activePieces = new List<GameObject>();
    private float backgroundWidth;
    private bool isNextFlipped = false;
    
    void Start()
    {
        InitializeBackground();
    }
    
    void Update()
    {
        UpdateBackgroundSpawnAndDestroy();
    }

    private void InitializeBackground()
    {
        backgroundWidth = backgroundPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x * backgroundPrefab.transform.localScale.x;

        for (int i = 0; i < maxPieces; i++)
        {
            SpawnNextBackgroundPiece();
        }
    }

    private void UpdateBackgroundSpawnAndDestroy()
    {
        if (activePieces.Count > 0 && activePieces[0].transform.position.x < playerTransform.position.x + destroyPointX)
        {
            DestroyOldestPiece();
            SpawnNextBackgroundPiece();
        }
    }

    private void SpawnNextBackgroundPiece()
    {
        GameObject newPiece;
        Vector3 spawnPosition;

        if (activePieces.Count == 0)
        {
            spawnPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        }
        else
        {
            Vector3 lastPiecePosition = activePieces[activePieces.Count - 1].transform.position;
            spawnPosition = new Vector3(lastPiecePosition.x + backgroundWidth, lastPiecePosition.y, lastPiecePosition.z);
        }

        newPiece = Instantiate(backgroundPrefab, spawnPosition, Quaternion.identity, transform);
        
        SpriteRenderer renderer = newPiece.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.flipX = isNextFlipped;
        }

        activePieces.Add(newPiece);
        isNextFlipped = !isNextFlipped;
    }

    private void DestroyOldestPiece()
    {
        GameObject oldestPiece = activePieces[0];
        activePieces.RemoveAt(0);
        Destroy(oldestPiece);
    }
}
