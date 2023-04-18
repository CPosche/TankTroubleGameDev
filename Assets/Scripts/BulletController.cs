using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float bulletSpeed = 2f;
    public float bulletLifeTime = 5f;
    private Transform _transform;

    private void Awake()
    {
        Destroy(gameObject, bulletLifeTime);
    }
    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        _transform.position += _transform.right * (bulletSpeed * Time.deltaTime);
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

    private void CalculateNewDirection([NotNull] Collider2D surfaceHit)
    {
        if (surfaceHit == null) throw new ArgumentNullException(nameof(surfaceHit));
        var normal = surfaceHit.transform.right;
        var direction = _transform.right;
        var reflection = Vector2.Reflect(direction, normal) * -1;
        var angle = Mathf.Atan2(reflection.y, reflection.x) * Mathf.Rad2Deg;
        _transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
}
