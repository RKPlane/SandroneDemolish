using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Duck : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] float baseExplosionForce = 35f;
    [SerializeField] float explosionRadius = 2.5f;
    [SerializeField] float upwardModifier = 0.8f;

    [SerializeField] bool velocityScaling = true;
    [SerializeField] float maxVelocityMultiplier = 2.5f;

    [Header("Vida")]
    [SerializeField] float lifetime = 6f;

    [Header("Efectos")]
    [SerializeField] GameObject impactVFXPrefab;

    Rigidbody rb;
    bool exploded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {

        if (!exploded && rb.linearVelocity.sqrMagnitude > 0.1f)
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity.normalized);
    }

    void OnCollisionEnter(Collision col)
    {
        if (exploded) return;
        exploded = true;

        float impactSpeed = col.relativeVelocity.magnitude;
        float forceMult = velocityScaling ? Mathf.Clamp(impactSpeed / 10f, 0.5f, maxVelocityMultiplier) : 1f;
        float finalForce = baseExplosionForce * forceMult;

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            Rigidbody hitRb = hit.attachedRigidbody;
            if (hitRb != null && hitRb != rb)
                hitRb.AddExplosionForce(finalForce, transform.position, explosionRadius,
                                        upwardModifier, ForceMode.Impulse);
        }

        if (impactVFXPrefab != null)
            Instantiate(impactVFXPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
