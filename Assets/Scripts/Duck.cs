using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Duck : MonoBehaviour
{
    [SerializeField] float explosionForce = 600f;
    [SerializeField] float explosionRadius = 4f;
    [SerializeField] float lifetime = 6f;

    Rigidbody rb;
    bool exploded;

    void Awake() => rb = GetComponent<Rigidbody>();

    public void Launch(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision col)
    {
        if (exploded) return;
        exploded = true;

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            Rigidbody hitRb = hit.attachedRigidbody;
            if (hitRb != null && hitRb != rb)
                hitRb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.5f, ForceMode.Impulse);
        }
    }
}
