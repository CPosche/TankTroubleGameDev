using System.Collections;
using Photon.Pun;
using UnityEngine;

public class LandmineSpawner : MonoBehaviourPunCallbacks
{
    public GameObject landminePrefab;
    public float spawnInterval = 10f;

    private Transform[] spawnPoints;
    private int landmineCount = 0; // keep track of landmines spawned

    private void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        StartCoroutine(SpawnLandmine());
    }

    private IEnumerator SpawnLandmine()
    {
        while (true)
        {
            // Wait for spawn interval time before spawning a new landmine
            yield return new WaitForSeconds(spawnInterval);

            // Spawn only if maximum count is not reached
            if (landmineCount < 5)
            {
                // Choose a random spawn point
                int spawnIndex = Random.Range(1, spawnPoints.Length);
                Transform spawnPoint = spawnPoints[spawnIndex];

                // Spawn the landmine at the chosen spawn point
                PhotonNetwork.Instantiate(landminePrefab.name, spawnPoint.position, spawnPoint.rotation);

                // Increment the landmine count
                landmineCount++;
            }
        }
    }
}
