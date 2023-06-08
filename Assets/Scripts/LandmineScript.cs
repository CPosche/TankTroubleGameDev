using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LandmineScript : MonoBehaviourPun
{
 private float lifespan = 7f; // the lifespan of the health pack in seconds

    void Start()
    {

        Invoke("Explode", lifespan); // schedule the health pack to be removed after the lifespan has elapsed
    }


    void Explode()
    {
            PhotonNetwork.Destroy(gameObject.transform.parent.gameObject); // remove the landmine from the network for all players
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<TankController>().TakeDamage(5);
        }

        Explode();
    }
}
