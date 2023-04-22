using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Photon.Pun;

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
            PhotonNetwork.Destroy(other.gameObject);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void CalculateNewDirection([NotNull] Collider2D other)
    {
        // var normal = surfaceHit.transform.right;
        // var direction = _transform.right;
        // var reflection = Vector2.Reflect(direction, normal) * -1;
        // var angle = Mathf.Atan2(reflection.y, reflection.x) * Mathf.Rad2Deg;
        // _transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        if (other == null) throw new ArgumentNullException(nameof(other));
        if (other.CompareTag("Wall")) {
            Vector2 collisionPoint = other.ClosestPoint(_transform.position);

            // Calculate the normal vector based on the edge of the box if the collision point is close enough
            float maxDistance = Mathf.Min(other.transform.lossyScale.x, other.transform.lossyScale.y, other.transform.lossyScale.z) / 2f;
            float edgeThreshold = maxDistance * 0.2f; // Use 10% of the maxDistance as the edge threshold
            bool hitEdge = false;
            Vector2 normal = Vector2.zero;
            Vector2[] edges = GetEdges(other.gameObject);
            for (int i = 0; i < edges.Length; i++) {
                Vector2 edgeStart = edges[i];
                Vector2 edgeEnd = edges[(i + 1) % edges.Length];
                float distance = DistanceToLineSegment(edgeStart, edgeEnd, collisionPoint);
                if (distance < edgeThreshold) {
                    hitEdge = true;
                    normal = (edgeEnd - edgeStart).normalized;
                    break;
                }
            }

            if (!hitEdge) {
                normal = (collisionPoint - (Vector2)other.transform.position).normalized;
            }

            // Calculate the reflection vector and rotate the bullet
            Vector2 direction = _transform.right;
            Vector2 reflection = Vector2.Reflect(direction, normal) * -1;
            float angle = Mathf.Atan2(reflection.y, reflection.x) * Mathf.Rad2Deg;
            _transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    
    // Helper function to get the edges of a box collider
    Vector2[] GetEdges(GameObject gameObject) {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        Vector2 center = collider.offset;
        Vector2 size = collider.size;
        float halfWidth = size.x / 2f;
        float halfHeight = size.y / 2f;
        Vector2[] edges = new Vector2[4];
        edges[0] = center + new Vector2(-halfWidth, -halfHeight);
        edges[1] = center + new Vector2(halfWidth, -halfHeight);
        edges[2] = center + new Vector2(halfWidth, halfHeight);
        edges[3] = center + new Vector2(-halfWidth, halfHeight);
        for (int i = 0; i < edges.Length; i++) {
            edges[i] = gameObject.transform.TransformPoint(edges[i]);
        }
        return edges;
    }

// Helper function to calculate the distance between a point and a line segment
    float DistanceToLineSegment(Vector2 start, Vector2 end, Vector2 point) {
        Vector2 line = end - start;
        Vector2 pointStart = point - start;
        float dot = Vector2.Dot(pointStart, line);
        if (dot <= 0f) {
            return pointStart.magnitude;
        }
        Vector2 pointEnd = point - end;
        if (Vector2.Dot(pointEnd, line) >= 0f) {
            return pointEnd.magnitude;
        }
        Vector2 projection = start + line * dot / line.sqrMagnitude;
        return (point - projection).magnitude;
    }

    private IEnumerator FadeOut(float time)
    {
        yield return new WaitForSeconds(time);
        _animation.Play("FadeOut");
        yield return new WaitForSeconds(0.6f);
        PhotonNetwork.Destroy(gameObject);
    }
    
}
