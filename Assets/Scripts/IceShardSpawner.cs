using UnityEngine;

public class IceShardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject iceShardPrefab;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float spawnWidth = 10f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnShard();
            timer = 0f;
        }
    }

    void SpawnShard()
    {
        float randomX = Random.Range(-spawnWidth / 2, spawnWidth / 2);
        Vector3 spawnPos = transform.position + new Vector3(randomX, 0, 0);
        Instantiate(iceShardPrefab, spawnPos, Quaternion.identity);
    }
}