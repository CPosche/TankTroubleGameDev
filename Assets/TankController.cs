using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    
    [SerializeField] private GameObject muzzle;
    public int numberOfBullets = 5;
    public float bulletSpeed = 10f;
    public float bulletLifeTime = 10f;
    private GameObject[] _bullets = new GameObject[5];
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            if (_bullets[i] != null)
            {
                _bullets[i].GetComponent<Transform>().Translate(Vector2.right * (bulletSpeed * Time.deltaTime));
            }
        }
    }
    
    public void Shoot()
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            if (_bullets[i] == null)
            {
                _bullets[i] = Instantiate(bulletPrefab, muzzle.transform.position, muzzle.transform.rotation);
                _bullets[i].GetComponent<Transform>().Translate(Vector2.up * (bulletSpeed * Time.deltaTime));
                Destroy(_bullets[i], bulletLifeTime);
                break;
            }
        }
    }
}
