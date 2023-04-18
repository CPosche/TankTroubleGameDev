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
    private Animation _animation;

    private void Awake()
    {
        StartCoroutine(FadeOut(bulletLifeTime));
    }
    private void Start()
    {
        _transform = GetComponent<Transform>();
        _animation = GetComponent<Animation>();
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
        } else if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name + " was hit by " + name);
            Destroy(other.gameObject);
            Destroy(gameObject);
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

    private IEnumerator FadeOut(float time)
    {
        yield return new WaitForSeconds(time);
        _animation.Play("FadeOut");
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
    }
    
}
