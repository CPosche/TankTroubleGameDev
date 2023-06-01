using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        Invoke("JoinLobbyWithDelay", 3f); // Call JoinLobbyWithDelay method after 5 seconds
    }

    private void JoinLobbyWithDelay()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
