using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject backgroundPrefab;  // 원본 배경 프리팹 하나만 사용
    public Transform playerTransform;   // 플레이어 또는 카메라 트랜스폼

    public int maxPieces = 3;            // 유지할 배경 조각의 최대 개수
    public float destroyPointX = -20f;  // 배경이 파괴될 X 좌표

    private List<GameObject> activePieces = new List<GameObject>();
    private float backgroundWidth;
    private bool isNextFlipped = false;  // 다음 조각이 뒤집힐지 여부

    void Start()
    {
        // 필수 요소 확인
        if (backgroundPrefab == null || playerTransform == null)
        {
            Debug.LogError("프리팹과 플레이어 트랜스폼을 할당하세요!");
            enabled = false;
            return;
        }

        // 배경 조각의 가로 너비 계산
        backgroundWidth = backgroundPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x * backgroundPrefab.transform.localScale.x;

        // 게임 시작 시 초기 배경 조각 생성
        for (int i = 0; i < maxPieces; i++)
        {
            SpawnNextPiece();
        }
    }

    void Update()
    {
        // 첫 번째(가장 왼쪽) 배경 조각이 지정된 파괴 지점을 벗어났는지 확인
        if (activePieces.Count > 0 && activePieces[0].transform.position.x < playerTransform.position.x + destroyPointX)
        {
            DestroyOldestPiece();
            SpawnNextPiece();
        }
    }

    void SpawnNextPiece()
    {
        GameObject newPiece;
        Vector3 spawnPosition;

        // 리스트가 비어 있으면 현재 플레이어 위치에 첫 조각 생성
        if (activePieces.Count == 0)
        {
            spawnPosition = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            // 마지막 조각의 끝에 새로운 조각 생성
            Vector3 lastPiecePosition = activePieces[activePieces.Count - 1].transform.position;
            spawnPosition = new Vector3(lastPiecePosition.x + backgroundWidth, lastPiecePosition.y, lastPiecePosition.z);
        }

        // 원본 프리팹으로 생성
        newPiece = Instantiate(backgroundPrefab, spawnPosition, Quaternion.identity, transform);
        
        // Sprite Renderer 컴포넌트를 가져와서 flipX를 설정
        SpriteRenderer renderer = newPiece.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.flipX = isNextFlipped;
        }

        activePieces.Add(newPiece);
        isNextFlipped = !isNextFlipped; // 다음 생성 시 반전 상태 토글
    }

    void DestroyOldestPiece()
    {
        GameObject oldestPiece = activePieces[0];
        activePieces.RemoveAt(0);
        Destroy(oldestPiece);
    }
}
