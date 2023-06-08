using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TankController : MonoBehaviourPun
{
    [SerializeField] private GameObject muzzle;
    [SerializeField] private GameObject smoke;
    public int numberOfBullets = 5;
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
    public Image[] bulletImages;
    public Sprite[] ammoSprites;
    private SpriteRenderer _tankHouseSpriteRenderer;
    public AudioSource[] audioSources;
    private Rigidbody2D _rigidbody2D;
    public int maxHealth = 5;
    private int _currentHealth;
    public Image healthImage;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _tankAnimator = GetComponent<Animator>();
        _tankHouse = transform.GetChild(0).gameObject;
        _tankHouseSpriteRenderer = _tankHouse.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        _photonView = GetComponent<PhotonView>();
        _tankTransform = GetComponent<Transform>();
        _tankController = GetComponent<TankController>();
        bulletText.text = numberOfBullets + "/" + numberOfBullets;
        _currentHealth = maxHealth;
        healthImage.fillAmount = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_photonView.IsMine) return;
        
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        
        // use rigidbpdy2d to move the tank
        // _rigidbody2D.velocity = _tankTransform.right * (vertical * moveSpeed);
        // _rigidbody2D.angularVelocity = -horizontal * rotationSpeed;

        // var horizontal = Input.GetAxis("Horizontal");
        // var vertical = Input.GetAxis("Vertical");
        
        var movement = _tankTransform.right * (vertical * moveSpeed * Time.deltaTime);
        _tankTransform.position += movement;
        
        var rotation = -horizontal * rotationSpeed * Time.deltaTime;
        _tankTransform.Rotate(0, 0, rotation);
        
        // if tank is moving then play the animation
        if (horizontal != 0 || vertical != 0)
        {
            _tankAnimator.SetBool("isDriving", true);
            if (audioSources[1].isPlaying == false)
            {
                audioSources[1].Play();
            }
        }
        else
        {
            _tankAnimator.SetBool("isDriving", false);
            if (audioSources[1].isPlaying)
            {
                audioSources[1].Stop();
            }
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
                bulletImages[(numberOfBullets - _bullets.Count) - 1].enabled = true;
            }
        }

        _tankHouseSpriteRenderer.sprite = _bullets.Count switch
        {
            // check if bullets left is 0 then change sprite
            0 => ammoSprites[0],
            > 0 when _bullets.Count < numberOfBullets => ammoSprites[1],
            _ => ammoSprites[2]
        };
    }
    
    private void Shoot()
    {
        if (_bullets.Count >= numberOfBullets) return;
        audioSources[0].Play();
        StartCoroutine(Recoil());
        var smokeEffect = PhotonNetwork.Instantiate(smoke.name, muzzle.transform.position, muzzle.transform.rotation);
        StartCoroutine(DestroySmoke(smokeEffect));
        var bullet = PhotonNetwork.Instantiate(bulletPrefab.name, muzzle.transform.position, muzzle.transform.rotation);
        _bullets.Add(bullet);
        _photonView.RPC("UpdateTankUi", RpcTarget.All);
    }
    
    // on destroy play animation and destroy the tank
    private void OnDestroy()
    {
        var explosion = PhotonNetwork.Instantiate(explosionPrefab.name, _tankTransform.position, _tankTransform.rotation);
        explosion.gameObject.GetComponent<Animator>().Play("tank_explosion");
        Destroy(explosion, 1f);
    }

    // move the tankhouse back a little to simulate recoil
    private IEnumerator Recoil(){
        _tankHouse.transform.position += _tankHouse.transform.right * -0.05f;
        yield return new WaitForSeconds(0.1f);
        _tankHouse.transform.position += _tankHouse.transform.right * 0.05f;
    }
    
    private IEnumerator DestroySmoke(GameObject smoke)
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.Destroy(smoke);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
         if (other.CompareTag("HealthPack"))
        {
            // Restore the player's health to maximum
            _currentHealth = maxHealth;

            // Destroy the health pack object
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Wall"))
        {
            // make the tank bounce back a little using transform depending if its driving backwards or forwards
            
            
            if (Input.GetAxis("Vertical") > 0)
            {
                _tankTransform.position += _tankTransform.right * -0.5f;
            }
            else
            {
                _tankTransform.position += _tankTransform.right * 0.5f;
            }
            

        }
    }

    [PunRPC]
    private void UpdateTankUi()
    {
        bulletText.text = numberOfBullets - _bullets.Count + "/" + numberOfBullets;
        bulletImages[numberOfBullets - _bullets.Count].enabled = false;
    }

    public void TakeDamage(int damageToTake)
    {
        _currentHealth -= damageToTake;
        healthImage.fillAmount = _currentHealth / (float)maxHealth;
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
