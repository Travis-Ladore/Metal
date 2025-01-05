using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodable : MonoBehaviour
{

    public GameObject explosionEffect; // Assign a particle system prefab for the explosion
    public float explosionRadius = 5f; // Radius of the explosion
    public float explosionForce = 700f; // Force applied to nearby objects

    private bool hasBeenThrown = false; // Tracks if the object has been thrown

    public AudioClip collisionSound;
    public AudioClip explosionSound;
    private AudioSource audioSource;
    private Collider objectCollider; // Reference to the object's collider
    private Rigidbody objectRigidbody;
    private MeshRenderer objectMeshRenderer;

    // This method is called by the throwing script
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        objectCollider = GetComponent<Collider>(); // Get the collider
        objectRigidbody = GetComponent<Rigidbody>(); // Get the Rigidbody
        objectMeshRenderer = GetComponent<MeshRenderer>();// Ensure the object has an AudioSource component
    }

    public void MarkAsThrown()
    {
        hasBeenThrown = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasBeenThrown)
        {
            if (collisionSound != null)
            {
                audioSource.PlayOneShot(collisionSound);
            }
            Explode();
        }
    }

    void Explode()
    {
        // Instantiate explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Disable the collider and Rigidbody to prevent bouncing
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }

        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = true;
        }
        if (objectMeshRenderer != null)
        {
            objectMeshRenderer.enabled = false;
        }

        // Play explosion sound
        if (explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // Apply force to nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // Delay destruction until the explosion sound finishes
        float destructionDelay = explosionSound != null ? explosionSound.length : 0f;
        Destroy(gameObject, destructionDelay);
    }
}
