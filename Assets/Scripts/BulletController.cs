using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float bulletSpeed = 2f;
    public float bulletLifeTime = 5f;
    private Transform _transform;
    private Vector3 _direction;
    
    private void Awake()
    {
        Destroy(gameObject, bulletLifeTime);
    }
    private void Start()
    {
        _transform = GetComponent<Transform>();
        _direction = transform.right;
    }

    private void Update()
    {
        _transform.position += _direction * (bulletSpeed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hit");
        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log(other.gameObject.name + " was hit by " + name);
            CalculateNewDirection(other);    
        }
    }

    private void CalculateNewDirection(Collider2D surfaceHit)
    {
        Vector2 normal = surfaceHit.transform.right;
        Vector2 direction = transform.right;
        Vector2 reflection = Vector2.Reflect(direction, normal);
        float angle = Mathf.Atan2(reflection.y, reflection.x) * Mathf.Rad2Deg;
        _transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Correct bullet position to avoid getting stuck in the wall
        //var correction = surfaceHit.contacts[0].point - (Vector2)_transform.position;
        //_transform.position += correction;
    }
    
}
