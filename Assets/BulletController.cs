using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float bulletSpeed = 2f;
    public float bulletLifeTime = 5f;
    private Rigidbody2D _rigidbody2D;
    private Vector3 _direction;
    
    void Awake()
    {
        Destroy(gameObject, bulletLifeTime);
    }
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _direction = transform.right;
    }

    void Update()
    {
        _rigidbody2D.velocity = _direction * bulletSpeed;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            
        }
    }
}
