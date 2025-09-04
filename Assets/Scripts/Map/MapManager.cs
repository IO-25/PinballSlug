using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    // === UI와 게임 상태 관리 ===
    public GameObject settingsPanel;
    private bool isPaused = false;
    
    // === 배경 관리 ===
    public GameObject backgroundPrefab;
    public Transform playerTransform;

    public int maxPieces = 3;
    public float destroyPointX = -20f;
    
    private List<GameObject> activePieces = new List<GameObject>();
    private float backgroundWidth;
    private bool isNextFlipped = false;
    
    void Start()
    {
        InitializeManagers();
        InitializeBackground();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        UpdateBackgroundSpawnAndDestroy();
    }
    
    private void InitializeManagers()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (backgroundPrefab != null)
        {
            backgroundWidth = backgroundPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x * backgroundPrefab.transform.localScale.x;
        }
        else
        {
            Debug.LogError("Background Prefab이 할당되지 않았습니다.");
        }

        if (playerTransform == null)
        {
            Debug.LogError("Player Transform이 할당되지 않았습니다.");
        }
    }

    private void InitializeBackground()
    {
        for (int i = 0; i < maxPieces; i++)
        {
            SpawnNextBackgroundPiece();
        }
    }
    // esc 일시정지 재개
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    // 배경 관리
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
            spawnPosition = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
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
