using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
public class MazeManagerScript : MonoBehaviour
{
    public GameObject maze1;
    public GameObject maze2;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Awake() {
         int randomIndex = Random.Range(0, 2);
        Debug.Log(randomIndex);
        if (randomIndex == 0)
        {
            Instantiate(maze1, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Instantiate(maze2, Vector3.zero, Quaternion.identity);
        }
    }
}
