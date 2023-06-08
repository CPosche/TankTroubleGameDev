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
    private void Awake() {
         int randomIndex = Random.Range(0, 2);
        Debug.Log(randomIndex);
        if (randomIndex == 0)
        {
            PhotonNetwork.Instantiate(maze1.name, Vector3.zero, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(maze2.name, Vector3.zero, Quaternion.identity);
        }
    }
}
