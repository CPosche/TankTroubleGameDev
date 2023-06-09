using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivePrefabCounter : MonoBehaviourPunCallbacks
{
    private int activePrefabCount = 0;
    private bool hasLoadedScene = false;
    private bool isFirstPlayerJoined = false;

    private void Start()
    {
        CountActivePrefabs();
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        base.OnDisable(); // Call the base class implementation
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (!isFirstPlayerJoined)
        {
            isFirstPlayerJoined = true;
        }
        else
        {
            CountActivePrefabs();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        CountActivePrefabs();
    }

    private void Update()
    {
        if (isFirstPlayerJoined && activePrefabCount == 1 && !hasLoadedScene)
        {
            hasLoadedScene = true;

            if (PhotonNetwork.IsMasterClient)
            {
                SceneManager.LoadScene("GameOverScene");
            }
            else
            {
                SceneManager.LoadScene("VictoryScene");
            }
        }
    }

    private void CountActivePrefabs()
    {
        activePrefabCount = PhotonNetwork.CurrentRoom.PlayerCount;
    }
}
