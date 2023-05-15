using UnityEngine;
using Photon.Pun;

public class HealthPack : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    private float lifespan = 8f; // the lifespan of the health pack in seconds

    void Start()
    {

        Invoke("RemoveHealthPack", lifespan); // schedule the health pack to be removed after the lifespan has elapsed
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) // called when the health pack is instantiated by Photon
    {
        if (photonView.IsMine)
        {
            // set up the health pack's position, rotation, etc. here
        }
    }

    void RemoveHealthPack()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject); // remove the health pack from the network for all players
        }
    }
}
