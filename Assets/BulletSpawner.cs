using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class BulletSpawner : MonoBehaviourPun
{
    public GameObject bulletPrefab;
    
    // a function that get each player there is on the photonnetwork and instantiate and instantiate 5 bullets for each player
    public void SpawnBullets()
    {
        // get all the players in the photonnetwork
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // instantiante 5 bullets as children of the bullet spawner
            for (int i = 0; i < 5; i++)
            {
                var bullet = PhotonNetwork.Instantiate(bulletPrefab.name, transform.position, Quaternion.identity);
                // disable the bullet so it doesn't move
                bullet.GetComponent<Transform>().gameObject.SetActive(false);
                bullet.transform.SetParent(this.gameObject.transform);
            }
        }
    }

    private void Awake()
    {
        SpawnBullets();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
