using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TrailRenderer))]
public class Projectile : MonoBehaviour, IReseteable
{
    private Rigidbody myRigidbody;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void Shoot(Vector3 direction, float force) 
    {
        myRigidbody.velocity = new Vector3(direction.x, direction.y, direction.z) * force;
    }

    void OnCollisionEnter(Collision collision)
    {
        Reset();
    }

    public void Reset()
    {
        trailRenderer.Clear();
        myRigidbody.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
