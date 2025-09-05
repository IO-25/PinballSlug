using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgroundappend : MonoBehaviour
{
    [Header("Required Objects")]
    public GameObject backgroundPrefab;
    public Transform playerTransform;

    [Header("Manual Setup")]
    public float backgroundWidth; 
    public float startXPosition = 0f;

    private float lastSpawnPositionX;
    private List<GameObject> backgroundPieces = new List<GameObject>();
    private int backgroundPieceCount = 0;

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform이 인스펙터에 연결되지 않았습니다!");
            return;
        }

        if (backgroundPrefab == null)
        {
            Debug.LogError("Background Prefab이 할당되지 않았습니다");
            return;
        }

        lastSpawnPositionX = startXPosition - backgroundWidth;
        
        for (int i = 0; i < 3; i++)
        {
            SpawnNewBackgroundPiece();
        }
    }

    void Update()
    {
        if (playerTransform == null)
        {
            return; 
        }

        if (backgroundPieces.Count > 0 && playerTransform.position.x > backgroundPieces[0].transform.position.x + backgroundWidth)
        {
            RecycleFirstPiece();
        }
    }

    private void SpawnNewBackgroundPiece()
    {
        Vector3 spawnPosition = new Vector3(lastSpawnPositionX + backgroundWidth, 0, 0);
        GameObject newPiece = Instantiate(backgroundPrefab, spawnPosition, Quaternion.identity, transform);
        backgroundPieces.Add(newPiece);
        
        backgroundPieceCount++;
        if (backgroundPieceCount % 2 == 0)
        {
            Vector3 newScale = newPiece.transform.localScale;
            newScale.x = -Mathf.Abs(newScale.x);
            newPiece.transform.localScale = newScale;
        }

        lastSpawnPositionX = newPiece.transform.position.x;
    }

    private void RecycleFirstPiece()
    {
        GameObject pieceToRecycle = backgroundPieces[0];
        backgroundPieces.RemoveAt(0);
        
        float lastPieceX = backgroundPieces[backgroundPieces.Count - 1].transform.position.x;
        Vector3 newPosition = new Vector3(lastPieceX + backgroundWidth, pieceToRecycle.transform.position.y, pieceToRecycle.transform.position.z);

        pieceToRecycle.transform.position = newPosition;
        backgroundPieces.Add(pieceToRecycle);
        
        backgroundPieceCount++;
        if (backgroundPieceCount % 2 == 0)
        {
            Vector3 newScale = pieceToRecycle.transform.localScale;
            newScale.x = -Mathf.Abs(newScale.x);
            pieceToRecycle.transform.localScale = newScale;
        }
        else
        {
            Vector3 newScale = pieceToRecycle.transform.localScale;
            newScale.x = Mathf.Abs(newScale.x);
            pieceToRecycle.transform.localScale = newScale;
        }

        lastSpawnPositionX = pieceToRecycle.transform.position.x;
    }
}