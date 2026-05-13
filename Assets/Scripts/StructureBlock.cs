using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class StructureBlock : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] float density = 2400f;

    [Header("Joints")]
    [SerializeField] float connectionRadius = 0.7f;
    [SerializeField] float jointBreakForce  = 6000f;
    [SerializeField] float jointBreakTorque = 6000f;

    [Header("Demolition")]
    [SerializeField] bool  autoThreshold      = true;
    [SerializeField] float demolishThreshold  = 0.5f;
    [SerializeField] float thresholdMultiplier = 0.4f;

    public Rigidbody Rb { get; private set; }

    Vector3 originPos;
    bool demolished;
    float computedThreshold;
    Renderer rend;
    Color originalColor;

    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    void Start()
    {
        originPos = transform.position;

        Vector3 size = GetComponent<Collider>().bounds.size;
        float volume = size.x * size.y * size.z;
        Rb.mass = Mathf.Max(0.1f, volume * density);

        computedThreshold = autoThreshold
            ? Mathf.Max(size.x, size.y, size.z) * thresholdMultiplier
            : demolishThreshold;

        DemolitionTracker.Instance.Register(this);

        StartCoroutine(ConnectNextFrame());
    }

    IEnumerator ConnectNextFrame()
    {
        yield return new WaitForEndOfFrame();

        Collider[] hits = Physics.OverlapSphere(transform.position, connectionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            StructureBlock neighbour = hit.GetComponent<StructureBlock>();
            if (neighbour == null) continue;

            if (AlreadyConnectedTo(neighbour)) continue;

            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody   = neighbour.Rb;
            joint.breakForce      = jointBreakForce;
            joint.breakTorque     = jointBreakTorque;
            joint.enableCollision = false;
        }
    }

    bool AlreadyConnectedTo(StructureBlock other)
    {
        foreach (FixedJoint j in GetComponents<FixedJoint>())
            if (j.connectedBody == other.Rb)
                return true;
        return false;
    }

    void FixedUpdate()
    {
        if (demolished) return;

        float dist = Vector3.Distance(transform.position, originPos);

        if (rend != null)
        {
            float t = Mathf.Clamp01(dist / computedThreshold);
            rend.material.color = Color.Lerp(originalColor, Color.red, t * 0.6f);
        }

        if (dist > computedThreshold)
        {
            demolished = true;
            DemolitionTracker.Instance.OnBlockDemolished();
        }
    }

    void OnDestroy()
    {
        if (!demolished)
            DemolitionTracker.Instance?.Unregister(this);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, connectionRadius);
    }
}
