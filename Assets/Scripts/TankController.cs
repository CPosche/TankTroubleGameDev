using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TankController : MonoBehaviour
{
    
    [SerializeField] private GameObject muzzle;
    public int numberOfBullets = 5;
    // private readonly GameObject[] _bullets = new GameObject[5];
    private List<GameObject> _bullets = new List<GameObject>();
    public GameObject bulletPrefab;
    public float moveSpeed = 3f;
    public float rotationSpeed = 200f;
    public GameObject explosionPrefab;

    private Transform _tankTransform;
    private TankController _tankController;
    private PhotonView _photonView;
    private GameObject _tankHouse;
    private Animator _tankAnimator;
    [SerializeField] private TMP_Text bulletText;

    // Start is called before the first frame update
    void Start()
    {
        _tankAnimator = GetComponent<Animator>();
        _tankHouse = transform.GetChild(0).gameObject;
        _photonView = GetComponent<PhotonView>();
        _tankTransform = GetComponent<Transform>();
        _tankController = GetComponent<TankController>();
        bulletText.text = numberOfBullets + "/" + numberOfBullets;
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
        
        // check if object in the list is destroyed
        for (int i = 0; i < _bullets.Count; i++)
        {
            if (_bullets[i] == null)
            {
                _bullets.RemoveAt(i);
                bulletText.text = numberOfBullets - _bullets.Count + "/" + numberOfBullets;
            }
        }
    }

    private void Shoot()
    {
        if (_bullets.Count >= numberOfBullets) return;
        var bullet = Instantiate(bulletPrefab, muzzle.transform.position, muzzle.transform.rotation);
        _bullets.Add(bullet);
        bulletText.text = numberOfBullets - _bullets.Count + "/" + numberOfBullets;
    }
    
    // on destroy play animation and destroy the tank
    private void OnDestroy()
    {
        var explosion = Instantiate(explosionPrefab, _tankTransform.position, _tankTransform.rotation);
        explosion.gameObject.GetComponent<Animator>().Play("tank_explosion");
        Destroy(explosion, 1f);
    }
}
