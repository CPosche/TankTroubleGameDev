using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    private Transform _tankTransform;
    private TankController _tankController;
    void Start()
    {
        _tankTransform = GetComponent<Transform>();
        _tankController = GetComponent<TankController>();
    }
    
    void Update()
    {
        
        if (!GetComponent<PhotonView>().IsMine) return;
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = _tankTransform.right * vertical * moveSpeed * Time.deltaTime;
        _tankTransform.position += movement;

        float rotation = -horizontal * rotationSpeed * Time.deltaTime;
        _tankTransform.Rotate(0, 0, rotation);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _tankController.Shoot();
        }
        
    }
}
