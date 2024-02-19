using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] float spawnGap = 50;
    private float lastSpawnPos = 53;
    private GameObject player;

    void Start()
    {
        player = EventManager.getPlayer?.Invoke().gameObject;
        lastSpawnPos += spawnGap;
        SpawnPath();
    }

    private void SpawnPath()
    {
        GameObject spawnedPath = EventManager.callObjectFromPool?.Invoke($"Path " + Random.Range(1, 5).ToString());
        spawnedPath.transform.position = new Vector3(0, 0, lastSpawnPos);
        lastSpawnPos += spawnGap;
    }

    private void SpawnObstacle()
    {

    }

    void Update()
    {
        if (player.transform.position.z >= lastSpawnPos - 900)
        {
            SpawnPath();
        }
    }
}
