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

    [Header("Ragdoll")]
    [SerializeField] Rigidbody[] ragdollParts;
    [Header("Stiffness")]
    [SerializeField] float jointSpring = 800f;
    [SerializeField] float jointDamper = 80f;
    [Header("Torque")]
    [SerializeField] float jointBreakTorque = 500f;

    [SerializeField] float swingLimit = 30f;

    [Header("Life")]
    [SerializeField] float lifetime = 6f;


    Rigidbody rb;
    bool exploded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SetupRagdollJoints();
    }

    void SetupRagdollJoints()
    {
        if (ragdollParts == null) return;

        SoftJointLimitSpring spring = new SoftJointLimitSpring
        {
            spring = jointSpring,
            damper = jointDamper
        };

        foreach (Rigidbody part in ragdollParts)
        {
            if (part == null || part == rb) continue;

            CharacterJoint joint = part.GetComponent<CharacterJoint>();
            if (joint == null)
                joint = part.gameObject.AddComponent<CharacterJoint>();

            joint.connectedBody     = rb;
            joint.swingLimitSpring  = spring;
            joint.twistLimitSpring  = spring;
            joint.highTwistLimit    = new SoftJointLimit { limit =  swingLimit };
            joint.lowTwistLimit     = new SoftJointLimit { limit = -swingLimit };
            joint.swing1Limit       = new SoftJointLimit { limit =  swingLimit };
            joint.swing2Limit       = new SoftJointLimit { limit =  swingLimit };
            joint.breakTorque       = jointBreakTorque;

            Collider partCol = part.GetComponent<Collider>();
            Collider rootCol = rb.GetComponent<Collider>();
            if (partCol != null && rootCol != null)
                Physics.IgnoreCollision(partCol, rootCol);
        }
    }

    public void Launch(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);

        if (ragdollParts != null)
            foreach (Rigidbody part in ragdollParts)
                if (part != null)
                    part.AddForce(force, ForceMode.Impulse);

        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        if (exploded) return;
        if (rb.linearVelocity.sqrMagnitude > 0.1f)
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity.normalized);
    }

    void OnCollisionEnter(Collision col)
    {
        if (exploded) return;
        exploded = true;

        float impactSpeed = col.relativeVelocity.magnitude;
        float forceMult = velocityScaling
            ? Mathf.Clamp(impactSpeed / 10f, 0.5f, maxVelocityMultiplier)
            : 1f;
        float finalForce = baseExplosionForce * forceMult;

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            Rigidbody hitRb = hit.attachedRigidbody;
            if (hitRb != null && hitRb != rb)
                hitRb.AddExplosionForce(finalForce, transform.position, explosionRadius,
                                        upwardModifier, ForceMode.Impulse);
        }

        if (ragdollParts != null)
            foreach (Rigidbody part in ragdollParts)
                if (part != null)
                    part.AddExplosionForce(finalForce * 0.5f, transform.position,
                                           explosionRadius, upwardModifier, ForceMode.Impulse);

        Destroy(gameObject, 1.5f);
    }
}
