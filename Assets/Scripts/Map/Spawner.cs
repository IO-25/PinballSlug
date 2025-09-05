using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("생성할 오브젝트 설정")]
    public GameObject platformPrefab; 
    public int poolAmount = 20; 
    private List<GameObject> pooledObjects;

    [Header("발판 생성 기준점")]
    public Transform spawnPoint;

    [Header("생성 확률")]
    [Tooltip("Element 뒤 숫자가 생성 할 플랫폼의 개수 - 각 확률 값 입력")]
    public float[] spawnProbability = new float[4];

    [Header("생성될 범위 설정")]
    [Tooltip("x:중심 X, y:중심 Y, z:너비, w:높이")]
    public List<Vector4> spawnRanges; 

    [Header("생성 타이밍")]
    public float timeBetweenSpawns = 2f; 
    private float nextSpawnTime;

    void Start()
    {
        // 오브젝트 풀링 초기화
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = Instantiate(platformPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        
        nextSpawnTime = Time.time;
    }

    void Update()
    {
        // 시간에 맞게 발판 그룹 생성
        if (Time.time >= nextSpawnTime)
        {
            SpawnAllPlatformsAtOnce();
            
            // 다음 생성 시간을 현재 시간에 시간 간격을 더하여 설정
            nextSpawnTime = Time.time + timeBetweenSpawns;
        }
    }

    // 모든 범위에서 발판을 한 번에 하나씩 생성
    void SpawnAllPlatformsAtOnce()
    {
        if (spawnRanges == null || spawnRanges.Count == 0)
        {
            Debug.LogError("Spawn Ranges가 설정되지 않았습니다. Spawner를 확인해주세요.");
            return;
        }

        int spawnnumber = RandomManager.RandomPicker(spawnProbability);
        float spawnRate = spawnnumber / spawnRanges.Count;
        for (int i = 0; i < spawnRanges.Count; i++)
        {
            //No more Platform will spawn
            if (spawnnumber <= 0)
                break;
            //Based on Platform count, flip coin
            if (spawnnumber > spawnRanges.Count - i && !RandomManager.FlipCoin(spawnRate))
                continue;
            spawnnumber--;
            spawnRate = spawnnumber / spawnRanges.Count;
            SpawnPlatformFromRange(spawnRanges[i]);
        }
    }

    // 단일 발판 생성
    void SpawnPlatformFromRange(Vector4 selectedRange)
    {
        float centerX = selectedRange.x;
        float centerY = selectedRange.y;
        float width = selectedRange.z;
        float height = selectedRange.w;

        float randomX = Random.Range(centerX - width / 2f, centerX + width / 2f);
        float randomY = Random.Range(centerY - height / 2f, centerY + height / 2f);

        GameObject newPlatform = GetPooledObject();
        
        if (newPlatform != null)
        {
            Vector3 spawnPosition = new Vector3(
                spawnPoint.position.x + randomX,
                spawnPoint.position.y + randomY,
                0
            );

            newPlatform.transform.position = spawnPosition;
            newPlatform.SetActive(true);
        }
    }
    
    private GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    //범위 설정
    void OnDrawGizmosSelected()
    {
        if (spawnPoint != null && spawnRanges != null)
        {
            Gizmos.color = Color.yellow;
            
            foreach (Vector4 range in spawnRanges)
            {
                Vector3 center = new Vector3(
                    spawnPoint.position.x + range.x,
                    spawnPoint.position.y + range.y,
                    0
                );
                Vector3 size = new Vector3(
                    range.z, 
                    range.w, 
                    0.1f
                );

                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}