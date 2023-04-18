using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TankController : MonoBehaviour
{
    
    [SerializeField] private GameObject muzzle;
    public int numberOfBullets = 5;
    private readonly GameObject[] _bullets = new GameObject[5];
    public GameObject bulletPrefab;
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    private Transform _tankTransform;
    private TankController _tankController;
    private PhotonView _photonView;
    private GameObject _tankHouse;
    private Animator _tankAnimator;

    // Start is called before the first frame update
    void Start()
    {
        _tankAnimator = GetComponent<Animator>();
        _tankHouse = transform.GetChild(0).gameObject;
        _photonView = GetComponent<PhotonView>();
        _tankTransform = GetComponent<Transform>();
        _tankController = GetComponent<TankController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_photonView.IsMine) return;
        
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var movement = _tankTransform.right * (vertical * moveSpeed * Time.deltaTime);
        _tankTransform.position += movement;

        var rotation = -horizontal * rotationSpeed * Time.deltaTime;
        _tankTransform.Rotate(0, 0, rotation);
        
        // if tank is moving then play the animation
        if (horizontal != 0 || vertical != 0)
        {
            _tankAnimator.SetBool("isDriving", true);
        }
        else
        {
            _tankAnimator.SetBool("isDriving", false);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _tankController.Shoot();
        }
        
        // rotate the tank house with keyboard
        if (Input.GetKey(KeyCode.J))
        {
            _tankHouse.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.K))
        {
            _tankHouse.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.I))
        {
            _tankHouse.transform.rotation = Quaternion.Euler(0, 0, _tankTransform.rotation.eulerAngles.z);
        }
    }

    private void Shoot()
    {
        for (var i = 0; i < numberOfBullets; i++)
        {
            if (_bullets[i] != null) continue;
            _bullets[i] = Instantiate(bulletPrefab, muzzle.transform.position, muzzle.transform.rotation);
            break;
        }
    }
}
