using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Vector2 spawnPoint;
    [SerializeField] private float spawnDelay = 2f;

    public void StartSpawn()
        => StartCoroutine(SpawnPlayer());

    IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);
        player.transform.position = spawnPoint;
        player.Initialize();
    }
}
