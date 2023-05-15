using System.Collections;
using Photon.Pun;
using UnityEngine;

public class HealthPackSpawner : MonoBehaviourPunCallbacks
{
    public GameObject healthPackPrefab;
    public float spawnInterval = 10f;

    private Transform[] spawnPoints;
    private int healthPackCount = 0; // keep track of health packs spawned

    private void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        StartCoroutine(SpawnHealthPack());
    }

    private IEnumerator SpawnHealthPack()
    {
        while (true)
        {
            // Wait for spawn interval time before spawning a new health pack
            yield return new WaitForSeconds(spawnInterval);

            // Spawn only if maximum count is not reached
            if (healthPackCount < 5)
            {
                // Choose a random spawn point
                int spawnIndex = Random.Range(1, spawnPoints.Length);
                Transform spawnPoint = spawnPoints[spawnIndex];

                // Spawn the health pack at the chosen spawn point
                PhotonNetwork.Instantiate(healthPackPrefab.name, spawnPoint.position, spawnPoint.rotation);

                // Increment the health pack count
                healthPackCount++;
            }
        }
    }
}
