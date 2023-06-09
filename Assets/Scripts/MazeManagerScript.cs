using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MazeManagerScript : MonoBehaviourPun
{
    public GameObject maze1;
    public GameObject maze2;

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Loading");
 
            return;
        }

        PhotonNetwork.ConnectUsingSettings();
    }
    private void Awake()
    {
        // only instantiate a maze if we are the master client
        if (PhotonNetwork.IsMasterClient)
        {
            CreateMaze();
        }
    }
    
    private void CreateMaze()
    {
        // choose a random number between 1 and 2
        int mazeNumber = Random.Range(0, 2);
        // instantiate the chosen maze
        if (mazeNumber == 0)
        {
            PhotonNetwork.Instantiate(maze1.name, Vector3.zero, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(maze2.name, Vector3.zero, Quaternion.identity);
        }
    }
}
